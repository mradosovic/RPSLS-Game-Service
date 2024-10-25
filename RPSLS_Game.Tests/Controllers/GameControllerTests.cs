﻿using Moq;
using Microsoft.AspNetCore.Mvc;
using RPSLS_Game.Presentation.Controllers;
using MediatR;
using Domain;
using RPSLS_Game.Presentation.DTOs;
using RPSLS_Game.Application.Models;
using Infrastructure.DTOs;

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

            // Create expected choice DTOs
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
            var randomChoice = ChoiceType.Rock;
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetRandomChoiceQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(randomChoice);

            // Act
            var result = await _controller.GetRandomChoice();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<RandomChoiceDto>(okResult.Value);
            Assert.Equal((int)randomChoice, dto.Id);
            Assert.Equal(randomChoice.ToString(), dto.Name);
        }

        [Fact]
        public async Task Play_ReturnsOkResult_WithGameResult()
        {
            // Arrange
            var playRequest = new PlayRequestDTO { Player = (int)ChoiceType.Rock };
            var expectedResult = new GameResult("Player wins", (int)ChoiceType.Rock, (int)ChoiceType.Scissors);
            var expectedPlayResultDto = new PlayResultDto(expectedResult.Results, expectedResult.Player, expectedResult.Computer);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PlayCommand>(), default))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Play(playRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var playResultDto = Assert.IsType<PlayResultDto>(okResult.Value);

            Assert.Equal(expectedPlayResultDto.Results, playResultDto.Results);
            Assert.Equal(expectedPlayResultDto.PlayerChoiceId, playResultDto.PlayerChoiceId);
            Assert.Equal(expectedPlayResultDto.ComputerChoiceId, playResultDto.ComputerChoiceId);
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