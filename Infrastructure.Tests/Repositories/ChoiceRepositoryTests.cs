using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Infrastructure.Settings;
using System.Net;
using System.Text;
using Domain.Entities;

namespace Infrastructure.Tests.Repositories
{
    public class ChoiceRepositoryTests
    {
        private readonly Mock<IOptions<ApiSettings>> _apiSettingsMock;
        private readonly Mock<ILogger<ChoiceRepository>> _loggerMock;
        private readonly ChoiceRepository _repository;

        public ChoiceRepositoryTests()
        {
            _apiSettingsMock = new Mock<IOptions<ApiSettings>>();
            _apiSettingsMock.Setup(x => x.Value).Returns(new ApiSettings { RandomChoiceApiUrl = "https://codechallenge.boohma.com/random" });
            _loggerMock = new Mock<ILogger<ChoiceRepository>>();
            _repository = new ChoiceRepository(_apiSettingsMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetChoicesAsync_ReturnsAllChoices()
        {
            // Act
            var result = await _repository.GetChoicesAsync();

            // Assert
            var choices = Assert.IsAssignableFrom<IEnumerable<Choice>>(result);
            Assert.Equal(5, choices.Count());
            Assert.Contains(choices, c => c.Id == 1 && c.Name == "Rock");
            Assert.Contains(choices, c => c.Id == 2 && c.Name == "Paper");
            Assert.Contains(choices, c => c.Id == 3 && c.Name == "Scissors");
            Assert.Contains(choices, c => c.Id == 4 && c.Name == "Lizard");
            Assert.Contains(choices, c => c.Id == 5 && c.Name == "Spock");
        }

        [Fact]
        public async Task GetRandomChoiceAsync_ReturnsValidChoice()
        {
            // Arrange
            var randomResponse = new RandomNumberResponse { random_number = 7 }; 
            var jsonResponse = JsonConvert.SerializeObject(randomResponse);
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            var repository = new ChoiceRepository(_apiSettingsMock.Object, _loggerMock.Object) { };

            // Act
            var result = await repository.GetRandomChoiceAsync();

            // Assert
            Assert.NotNull(result);
        }

        internal class RandomNumberResponse
        {
            public int random_number { get; set; }
        }

    }
}
