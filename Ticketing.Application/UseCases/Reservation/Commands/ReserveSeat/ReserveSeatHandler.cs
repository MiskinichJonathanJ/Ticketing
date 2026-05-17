using AutoMapper;
using FluentValidation;
using MediatR;
using Ticketing.Application.DTOs;
using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Enums;
using System.Text.Json;
using Ticketing.Domain.Exceptions;

namespace Ticketing.Application.UseCases.Reservation.Commands.ReserveSeat
{
    public class ReserveSeatHandler : IRequestHandler<ReserveSeatCommand, ReservationDto>
    {
        private readonly IValidator<ReserveSeatCommand> _validator;
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
            IValidator<ReserveSeatCommand> validator,
            IMapper mapper)
        {
            _seatRepository = seatRepository;
            _reservationRepository = reservationRepository;
            _auditLogRepository = auditLogRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<ReservationDto> Handle(ReserveSeatCommand request, CancellationToken cancellationToken)
        {
            // Log de intento de reserva al inicio
            await LogAuditOutsideTransaction(request, "RESERVE_ATTEMPT", cancellationToken, new {
                request.SeatId,
                request.UserId,
                InitialStatus = "Attempting to reserve"
            });

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                await LogAuditOutsideTransaction(request, "RESERVE_VALIDATION_FAILED", cancellationToken, new {
                    request.SeatId,
                    request.UserId,
                    Errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                });
                throw new ValidationException(validationResult.Errors);
            }

            var seat = await _seatRepository.GetByIdAsync(request.SeatId);

            if (seat == null)
            {
                await LogAuditOutsideTransaction(request, "RESERVE_SEAT_FAILED_NOT_FOUND", cancellationToken, new { request.SeatId, request.UserId });
                throw new KeyNotFoundException($"Asiento {request.SeatId} no encontrado.");
            }

            if (seat.Status != SeatStatus.Available)
            {
                await LogAuditOutsideTransaction(request, "RESERVE_FAILED_NOT_AVAILABLE", cancellationToken, new { request.SeatId, request.UserId, CurrentSeatStatus = seat.Status.ToString() });
                throw new InvalidOperationException("El asiento no está disponible.");
            }

            seat.Status = SeatStatus.Reserved;
            seat.Version++;

            var reservation = new Domain.Entities.Reservation
            {
                Id = Guid.NewGuid(),
                SeatId = request.SeatId,
                UserId = request.UserId,
                Status = ReservationStatus.Pending,
                ReservedAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddMinutes(5)
            };

            // Abrir transacción e intentar persistir
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                _seatRepository.Update(seat);
                await _reservationRepository.AddAsync(reservation);

                await _auditLogRepository.EnqueueAuditLogAsync(new AuditLog
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    Action = "RESERVE_SUCCESS",
                    EntityType = "Seat",
                    EntityId = seat.Id.ToString(),
                    Details = JsonSerializer.Serialize(new {
                        SeatNumber = seat.SeatNumber,
                        PreviousVersion = seat.Version - 1,
                        NewVersion = seat.Version,
                        ReservationId = reservation.Id,
                        ExpiresAt = reservation.ExpireAt
                    }),
                    CreatedAt = DateTime.UtcNow
                });
        
                await _unitOfWork.CommitAsync(cancellationToken);

                return _mapper.Map<ReservationDto>(reservation);
            }
            catch (ConcurrencyConflictException)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                await LogAuditOutsideTransaction(request, "RESERVE_CONFLICT", cancellationToken, new { request.SeatId, request.UserId, ConflictReason = "Seat modified by another user" });
                throw; // ExceptionMiddleware → 409
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }
        }

        private async Task LogAuditOutsideTransaction(ReserveSeatCommand request, string action, CancellationToken cancellationToken, object? details = null)
        {
            try
            {
                await _auditLogRepository.AddAuditLogAsync(new AuditLog
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    Action = action,
                    EntityType = "Seat",
                    EntityId = request.SeatId.ToString(),
                    Details = details != null ? JsonSerializer.Serialize(details) : $"Intento sobre asiento {request.SeatId} — acción: {action}",
                    CreatedAt = DateTime.UtcNow
                });
            }
            catch
            {
                // El log nunca debe romper el flujo principal
            }
        }
    }
}