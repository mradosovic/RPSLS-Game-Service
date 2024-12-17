using Moq;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Infrastructure.DTOs;
using API.Controllers;
using Domain.Entities;
using Application.Queries;
using Application.Commands;

namespace RPSLS_Game.Api.Tests
{
    public class GameControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly GameController _controller;

        public GameControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new GameController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetChoices_ReturnsOkResult_WithChoices()
        {
            // Arrange
            var expectedChoices = new List<ChoiceType> { ChoiceType.Rock, ChoiceType.Paper };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetChoicesQuery>(), default))
                .ReturnsAsync(expectedChoices);

            // Act
            var result = await _controller.GetChoices();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualChoiceDtos = Assert.IsAssignableFrom<IEnumerable<ChoiceDto>>(okResult.Value);

            var expectedChoiceDtos = expectedChoices.Select(c => new ChoiceDto
            {
                Id = (int)c,
                Name = c.ToString()
            }).ToList();

            Assert.Equal(expectedChoiceDtos.Count, actualChoiceDtos.Count());
            for (int i = 0; i < expectedChoiceDtos.Count; i++)
            {
                Assert.Equal(expectedChoiceDtos[i].Id, actualChoiceDtos.ElementAt(i).Id);
                Assert.Equal(expectedChoiceDtos[i].Name, actualChoiceDtos.ElementAt(i).Name);
            }
        }

        [Fact]
        public async Task GetRandomChoice_ReturnsOkWithRandomChoiceDto()
        {
            // Arrange
            var randomChoice = new Choice(1, "Rock");
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetRandomChoiceQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(randomChoice);

            // Act
            var result = await _controller.GetRandomChoice();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<RandomChoiceDto>(okResult.Value);
            Assert.Equal(randomChoice.Id, dto.Id);
            Assert.Equal(randomChoice.Name, dto.Name);
        }

        [Fact]
        public async Task Play_ReturnsOkResult_WithGameResult()
        {
            // Arrange
            var playRequest = new PlayRequestDTO { Player = (int)ChoiceType.Rock };
            var expectedResult = new GameResult(GameResultType.Win, (int)ChoiceType.Rock, (int)ChoiceType.Scissors);
            var expectedPlayResultDto = new GameResultDto(expectedResult.Result.ToString(), expectedResult.PlayersChoice, expectedResult.ComputersChoice);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PlayCommand>(), default))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Play(playRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var playResultDto = Assert.IsType<GameResultDto>(okResult.Value);

            Assert.Equal(expectedPlayResultDto.Results, playResultDto.Results);
            Assert.Equal(expectedPlayResultDto.Player, playResultDto.Player);
            Assert.Equal(expectedPlayResultDto.Computer, playResultDto.Computer);
        }


        [Fact]
        public async Task GetChoices_ReturnsInternalServerError_OnException()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetChoicesQuery>(), default))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetChoices();

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
            Assert.Equal("An error occurred while retrieving choices.", statusResult.Value);
        }

        [Fact]
        public async Task GetRandomChoice_ReturnsInternalServerError_OnException()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetRandomChoiceQuery>(), default))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetRandomChoice();

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
            Assert.Equal("An error occurred while retrieving a random choice.", statusResult.Value);
        }

        [Fact]
        public async Task Play_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var playRequest = new PlayRequestDTO { Player = (int)ChoiceType.Rock };
            _mediatorMock.Setup(m => m.Send(It.IsAny<PlayCommand>(), default))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.Play(playRequest);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
            Assert.Equal("An error occurred while playing the game.", statusResult.Value);
        }
    }
}
