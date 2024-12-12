using FluentValidation;
using Infrastructure.DTOs;

namespace Infrastructure.Validators
{
    public class PlayRequestDTOValidator : AbstractValidator<PlayRequestDTO>
    {
        public PlayRequestDTOValidator()
        {
            RuleFor(x => x.Player)
                .InclusiveBetween(1, 5)
                .WithMessage("Player's choice Id must be a number between 1 and 5.");
        }
    }
}


