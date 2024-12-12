
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RPSLS_Game.Application.Interfaces;
using RPSLS_Game.Application.Services;
using RPSLS_Game.Application.Settings;
using RPSLS_Game.Domain.Models;

namespace RPSLS_Game.Tests.Services
{
    public class GameServiceTests
    {
        private readonly Mock<IChoiceRepository> _choiceRepositoryMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly Mock<ILogger<GameService>> _loggerMock;
        private readonly ApiSettings _apiSettings;
        private readonly HttpClient _httpClient;
        private readonly GameService _gameService;

        public GameServiceTests()
        {
            _choiceRepositoryMock = new Mock<IChoiceRepository>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _loggerMock = new Mock<ILogger<GameService>>();
            _apiSettings = new ApiSettings { RandomChoiceApiUrl = "https://codechallenge.boohma.com/random" };
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _gameService = new GameService(_choiceRepositoryMock.Object, _httpClient, Options.Create(_apiSettings), _loggerMock.Object);
        }

        [Fact]
        public async Task GetChoicesAsync_ReturnsAllChoices()
        {
            // Arrange
            var choices = new List<Choice>
                {
                    new Choice(1, "Rock"),
                    new Choice(2, "Paper"),
                    new Choice(3, "Scissors"),
                    new Choice(4, "Lizard"),
                    new Choice(5, "Spock")
                };
            _choiceRepositoryMock.Setup(repo => repo.GetChoicesAsync()).ReturnsAsync(choices);

            // Act
            var result = await _gameService.GetChoicesAsync();

            // Assert
            Assert.Equal(choices, result);
        }

        [Fact]
        public async Task GetRandomChoiceAsync_ReturnsRandomChoice()
        {
            // Arrange
            var expectedChoice = new Choice(2, "Paper"); // Choice ID 2 corresponds to "Paper"
            _choiceRepositoryMock
                .Setup(repo => repo.GetRandomChoiceAsync())
                .ReturnsAsync(expectedChoice);

            // Act
            var result = await _gameService.GetRandomChoiceAsync();

            // Assert
            Assert.Equal(expectedChoice, result);
            Assert.Equal(expectedChoice.Id, result.Id);
            Assert.Equal(expectedChoice.Name, result.Name);
        }


        [Fact]
        public async Task Play_ReturnsGameResult_WhenValidRequest()
        {
            // Arrange
            var playerChoice = new Choice(1, "Rock");
            var computerChoice = new Choice(3, "Scissors");
            var request = new PlayRequest(playerChoice.Id);

            _choiceRepositoryMock.Setup(repo => repo.GetChoicesAsync()).ReturnsAsync(new List<Choice> { playerChoice, computerChoice });
            _choiceRepositoryMock.Setup(repo => repo.GetRandomChoiceAsync()).ReturnsAsync(computerChoice);

            // Act
            var result = await _gameService.Play(request);

            // Assert
            Assert.Equal("win", result.Results);
            Assert.Equal(playerChoice.Id, result.Player);
            Assert.Equal(computerChoice.Id, result.Computer);
        }

        [Fact]
        public async Task Play_ThrowsException_WhenInvalidChoice()
        {
            // Arrange
            var request = new PlayRequest(99); // Invalid choice id

            _choiceRepositoryMock.Setup(repo => repo.GetChoicesAsync()).ReturnsAsync(new List<Choice>());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _gameService.Play(request));
        }
    }
}
