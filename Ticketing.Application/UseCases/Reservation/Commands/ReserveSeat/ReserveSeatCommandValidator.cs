using FluentValidation;

namespace Ticketing.Application.UseCases.Reservation.Commands.ReserveSeat
{
    public class ReserveSeatCommandValidator : AbstractValidator<ReserveSeatCommand>
    {
        public ReserveSeatCommandValidator()
        {
            RuleFor(x => x.SeatId)
                .NotEmpty().WithMessage("El SeatId no puede estar vacío.")
                .NotEqual(Guid.Empty).WithMessage("El SeatId debe ser un GUID válido.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("El UserId debe ser un identificador positivo.");
        }
    }
}
