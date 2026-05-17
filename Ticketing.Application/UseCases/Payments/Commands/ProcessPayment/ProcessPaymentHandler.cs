using MediatR;
using Ticketing.Application.DTOs;
using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Enums;

namespace Ticketing.Application.UseCases.Payments.Commands.ProcessPayment
{
    public class ProcessPaymentHandler : IRequestHandler<ProcessPaymentCommand, PaymentDto>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProcessPaymentHandler(
            IReservationRepository reservationRepository,
            ISeatRepository seatRepository,
            IAuditLogRepository auditLogRepository,
            IUnitOfWork unitOfWork)
        {
            _reservationRepository = reservationRepository;
            _seatRepository = seatRepository;
            _auditLogRepository = auditLogRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaymentDto> Handle(
            ProcessPaymentCommand request,
            CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                // 1. Verificamos que la reserva existe
                var reservation = await _reservationRepository.GetByIdAsync(request.ReservationId);
                if (reservation is null)
                    throw new KeyNotFoundException($"Reserva {request.ReservationId} no encontrada.");

                // 2. Verificamos que no fue pagada ya (idempotencia)
                if (reservation.Status == ReservationStatus.Paid)
                    throw new InvalidOperationException("Esta reserva ya fue pagada.");

                // 3. Verificamos que está pendiente
                if (reservation.Status != ReservationStatus.Pending)
                    throw new InvalidOperationException("La reserva no está en estado pendiente.");

                // 4. Verificamos que no expiró
                if (reservation.ExpireAt < DateTime.UtcNow)
                    throw new InvalidOperationException("La reserva ha expirado.");

                // 5. Verificamos que la butaca existe
                var seat = await _seatRepository.GetByIdAsync(reservation.SeatId);
                if (seat is null)
                    throw new KeyNotFoundException($"Butaca {reservation.SeatId} no encontrada.");

                // 6. Validamos que el asiento no esté ya vendido
                if (seat.Status == SeatStatus.Sold)
                    throw new InvalidOperationException("El asiento ya fue vendido.");

                // Usamos un solo DateTime para consistencia
                var now = DateTime.UtcNow;

                // 7. Cambiamos estado de la butaca a Sold
                seat.Status = SeatStatus.Sold;
                _seatRepository.Update(seat);

                // 8. Cambiamos estado de la reserva a Paid
                reservation.Status = ReservationStatus.Paid;
                _reservationRepository.Update(reservation);

                // 9. Registramos en auditoría
                await _auditLogRepository.AddAuditLogAsync(new AuditLog
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    Action = "PAYMENT_SUCCESS",
                    EntityType = "Reservation",
                    EntityId = reservation.Id.ToString(),
                    Details = $"Pago exitoso para butaca {seat.SeatNumber}",
                    CreatedAt = now
                });

                // 10. Commit — SaveChanges + transacción
                await _unitOfWork.CommitAsync(cancellationToken);

                return new PaymentDto
                {
                    ReservationId = reservation.Id,
                    SeatId = seat.Id,
                    SeatStatus = seat.Status.ToString(),
                    ReservationStatus = reservation.Status.ToString(),
                    PaidAt = now,
                    Message = "Pago procesado exitosamente"
                };
            }
            catch
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
