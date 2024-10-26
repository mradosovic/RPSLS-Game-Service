using Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RPSLS_Game.Application.Interfaces;
using RPSLS_Game.Application.Models;
using RPSLS_Game.Application.Settings;
using RPSLS_Game.Domain.Models;

namespace RPSLS_Game.Application.Services
{
    /// <summary>
    /// Game service.
    /// </summary>
    public class GameService : IGameService
    {
        private readonly IChoiceRepository _choiceRepository;
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;
        private readonly ILogger<GameService> _logger;

        public GameService(IChoiceRepository choiceRepository, HttpClient httpClient, IOptions<ApiSettings> apiSettings, ILogger<GameService> logger)
        {
            _choiceRepository = choiceRepository;
            _httpClient = httpClient;
            _apiSettings = apiSettings.Value;
            _logger = logger;
        }

        #region public methods

        public async Task<IEnumerable<Choice>> GetChoicesAsync()
        {
            return await _choiceRepository.GetChoicesAsync();
        }

        public async Task<Choice> GetRandomChoiceAsync()
        {
            Choice randomChoice;

            try
            {
                randomChoice = await _choiceRepository.GetRandomChoiceAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in {MethodName}", nameof(GetRandomChoiceAsync));
                throw new Exception("An unexpected error occurred.");
            }

            return randomChoice;
        }

        public async Task<GameResult> Play(PlayRequest request)
        {
            GameResult gameResult;

            try
            {
                var choices = await _choiceRepository.GetChoicesAsync();
                var playerChoice = choices.FirstOrDefault(c => c.Id == request.PlayerChoiceId);
                var computerChoice = await _choiceRepository.GetRandomChoiceAsync();

                if (playerChoice == null)
                    throw new ArgumentException("Invalid choice");

                string result = DetermineWinner(playerChoice, computerChoice);

                gameResult = new GameResult(result, playerChoice.Id, computerChoice.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in {MethodName}", nameof(Play));
                throw new Exception("An unexpected error occurred.");
            }

            return gameResult;
        }

        #endregion public methods

        #region private methods

        private string DetermineWinner(Choice player, Choice computer)
        {
            if (player.Id == computer.Id) return "tie";

            var playerChoice = (ChoiceType)player.Id;
            var computerChoice = (ChoiceType)computer.Id;

            if ((playerChoice == ChoiceType.Rock && (computerChoice == ChoiceType.Scissors || computerChoice == ChoiceType.Lizard)) ||
                (playerChoice == ChoiceType.Paper && (computerChoice == ChoiceType.Rock || computerChoice == ChoiceType.Spock)) ||
                (playerChoice == ChoiceType.Scissors && (computerChoice == ChoiceType.Paper || computerChoice == ChoiceType.Lizard)) ||
                (playerChoice == ChoiceType.Lizard && (computerChoice == ChoiceType.Spock || computerChoice == ChoiceType.Paper)) ||
                (playerChoice == ChoiceType.Spock && (computerChoice == ChoiceType.Scissors || computerChoice == ChoiceType.Rock)))
            {
                return "win";
            }

            return "lose";
        }

        #endregion private methods
    }
}
