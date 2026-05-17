using MediatR;
using System.Text.Json;
using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Enums;

namespace Ticketing.Application.UseCases.Reservation.Commands.CancelExpiredReservations
{
    public class CancelExpiredReservationsHandler : IRequestHandler<CancelExpiredReservationsCommand>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelExpiredReservationsHandler(
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

        public async Task Handle(CancelExpiredReservationsCommand request, CancellationToken cancellationToken)
        {
            var expiredReservations = await _reservationRepository.GetExpiredPendingReservationsAsync(DateTime.UtcNow);

            if (!expiredReservations.Any())
            {
                return;
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                foreach (var reservation in expiredReservations)
                {
                    reservation.Status = ReservationStatus.Expired;
                    if (reservation.Seat != null)
                    {
                        reservation.Seat.Status = SeatStatus.Available;
                        reservation.Seat.Version++;
                        _seatRepository.Update(reservation.Seat);
                    }


                    await _auditLogRepository.EnqueueAuditLogAsync(new AuditLog
                    {
                        Id = Guid.NewGuid(),
                        UserId = null,
                        Action = "RESERVATION_EXPIRED",
                        EntityType = "Reservation",
                        EntityId = reservation.Id.ToString(),
                        Details = JsonSerializer.Serialize(new
                        {
                            ReservationId = reservation.Id,
                            SeatId = reservation.SeatId,
                            SeatNumber = reservation.Seat?.SeatNumber,
                            PreviousSeatStatus = SeatStatus.Reserved.ToString(),
                            NewSeatStatus = SeatStatus.Available.ToString(),
                            PreviousSeatVersion = reservation.Seat?.Version - 1,
                            NewSeatVersion = reservation.Seat?.Version,
                            ExpiredAt = reservation.ExpireAt
                        }),
                        CreatedAt = DateTime.UtcNow
                    });
                }
                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
