using FluentValidation.TestHelper;
using Infrastructure.DTOs;
using Infrastructure.Validators;

namespace RPSLS_Game.Tests.Validators
{
    public class PlayRequestDTOValidatorTests
    {
        private readonly PlayRequestDTOValidator _validator;

        public PlayRequestDTOValidatorTests()
        {
            _validator = new PlayRequestDTOValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Player_Choice_Is_Less_Than_1()
        {
            // Arrange
            var model = new PlayRequestDTO { Player = 0 };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Player)
                .WithErrorMessage("Player's choice Id must be a number between 1 and 5.");
        }

        [Fact]
        public void Should_Have_Error_When_Player_Choice_Is_Greater_Than_5()
        {
            // Arrange
            var model = new PlayRequestDTO { Player = 6 };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Player)
                .WithErrorMessage("Player's choice Id must be a number between 1 and 5.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Player_Choice_Is_Within_Range()
        {
            // Arrange
            var model = new PlayRequestDTO { Player = 3 };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Player);
        }
    }
}
