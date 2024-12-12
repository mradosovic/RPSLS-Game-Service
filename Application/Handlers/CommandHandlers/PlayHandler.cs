using MediatR;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using Application.Commands;

namespace Application.Handlers.CommandHandlers
{
    public class PlayHandler : IRequestHandler<PlayCommand, GameResult>
    {
        private readonly ILogger<PlayHandler> _logger;
        private readonly IChoiceRepository _choiceRepository;

        public PlayHandler(IChoiceRepository choiceRepository, ILogger<PlayHandler> logger)
        {
            _choiceRepository = choiceRepository;
            _logger = logger;
        }

        public async Task<GameResult> Handle(PlayCommand request, CancellationToken cancellationToken)
        {
            GameResult gameResult;

            try
            {
                var choices = await _choiceRepository.GetChoicesAsync();
                var playerChoice = choices.FirstOrDefault(c => c.Id == request.PlayerChoiceId);
                var computerChoice = await _choiceRepository.GetRandomChoiceAsync();

                if (playerChoice == null)
                    throw new ArgumentException("Invalid choice");

                GameResultType result = DetermineWinner(playerChoice, computerChoice);

                gameResult = new GameResult(result, playerChoice.Id, computerChoice.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in {MethodName}", nameof(Handle));
                throw new Exception("An unexpected error occurred.");
            }

            return gameResult;
        }

        private GameResultType DetermineWinner(Choice playersChoice, Choice computersChoice)
        {
            if (playersChoice.Id == computersChoice.Id) return GameResultType.Tie;

            var playerChoose = (ChoiceType)playersChoice.Id;
            var computerChoose = (ChoiceType)computersChoice.Id;

            if ((playerChoose == ChoiceType.Rock && (computerChoose == ChoiceType.Scissors || computerChoose == ChoiceType.Lizard)) ||
                (playerChoose == ChoiceType.Paper && (computerChoose == ChoiceType.Rock || computerChoose == ChoiceType.Spock)) ||
                (playerChoose == ChoiceType.Scissors && (computerChoose == ChoiceType.Paper || computerChoose == ChoiceType.Lizard)) ||
                (playerChoose == ChoiceType.Lizard && (computerChoose == ChoiceType.Spock || computerChoose == ChoiceType.Paper)) ||
                (playerChoose == ChoiceType.Spock && (computerChoose == ChoiceType.Scissors || computerChoose == ChoiceType.Rock)))
            {
                return GameResultType.Win;
            }

            return GameResultType.Loss;
        }
    }

}


