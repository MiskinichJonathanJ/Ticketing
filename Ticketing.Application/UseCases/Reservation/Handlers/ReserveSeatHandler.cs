using AutoMapper;
using MediatR;
using Ticketing.Application.DTOs;
using Ticketing.Application.Interfaces;
using Ticketing.Application.UseCases.Reservation.Commands;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Enums;

namespace Ticketing.Application.UseCases.Reservation.Handlers
{
    public class ReserveSeatHandler : IRequestHandler<ReserveSeatCommand, ReservationDto>
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReserveSeatHandler(
            ISeatRepository seatRepository,
            IReservationRepository reservationRepository,
            IAuditLogRepository auditLogRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _seatRepository = seatRepository;
            _reservationRepository = reservationRepository;
            _auditLogRepository = auditLogRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReservationDto> Handle(ReserveSeatCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            var seat = await _seatRepository.GetByIdAsync(request.SeatId);

            if (seat == null)
            {
                await LogAudit(request, "RESERVE_SEAT_FAILED_NOT_FOUND", cancellationToken);
                throw new Exception("Seat not found"); // Podrías usar una excepción personalizada
            }

            if (seat.Status != SeatStatus.Available)
            {
                await LogAudit(request, "RESERVE_FAILED_NOT_AVAILABLE", cancellationToken);
                throw new InvalidOperationException("Seat is not available");
            }

            var reservation = new Domain.Entities.Reservation
            {
                Id = Guid.NewGuid(),
                SeatId = request.SeatId,
                UserId = request.UserId,
                Status = ReservationStatus.Pending,
                ReservedAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddMinutes(5)
            };

            seat.Status = SeatStatus.Reserved;

            try
            {
                _seatRepository.Update(seat);
                await _reservationRepository.AddAsync(reservation);

                await _auditLogRepository.AddAuditLogAsync(new AuditLog
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    Action = "RESERVE_SUCCESS",
                    EntityType = "Seat",
                    EntityId = seat.Id.ToString(),
                    Details = $"Seat {seat.SeatNumber} reserved",
                    CreatedAt = DateTime.UtcNow
                });

                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch
            {
                await _unitOfWork.RollbackAsync(cancellationToken);

                await LogAudit(request, "RESERVE_FAILED_EXCEPTION", cancellationToken);
                throw;
            }

            return _mapper.Map<ReservationDto>(reservation);
        }

        private async Task LogAudit(ReserveSeatCommand request, string action, CancellationToken cancellationToken)
        {
            var auditLog = new AuditLog
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Action = action,
                EntityType = "Seat",
                EntityId = request.SeatId.ToString(),
                Details = $"Attempt on seat {request.SeatId}",
                CreatedAt = DateTime.UtcNow
            };

            await _auditLogRepository.AddAuditLogAsync(auditLog);
        }
    }
}
