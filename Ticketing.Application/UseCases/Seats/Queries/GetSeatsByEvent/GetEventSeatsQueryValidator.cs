using FluentValidation;

namespace Ticketing.Application.UseCases.Seats.Queries.GetSeatsByEvent
{
    public class GetEventSeatsQueryValidator : AbstractValidator<GetEventSeatsQuery>
    {
        public GetEventSeatsQueryValidator()
        {
            RuleFor(x => x.EventId)
                .GreaterThan(0).WithMessage("El ID del evento debe ser mayor a 0.");

            RuleFor(x => x.SectorId)
                .GreaterThan(0).WithMessage("El ID del sector debe ser mayor a 0.");
        }
    }
}