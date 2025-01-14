using Application.Commands;
using Application.Handlers.CommandHandlers;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace RPSLS_Game.Api.Tests;

public class PlayHandlerTests
{
    private readonly Mock<ILogger<IChoiceRepository>> _repositoryLoggerMock; 
    private readonly Mock<ILogger<PlayHandler>> _playHandlerLoggerMock;
    private readonly PlayHandler _playHandler;
    private readonly IChoiceRepository _repository;

    public PlayHandlerTests()
    {
        _repositoryLoggerMock = new Mock<ILogger<IChoiceRepository>>();
        _playHandlerLoggerMock = new Mock<ILogger<PlayHandler>>();
        _playHandler = new PlayHandler(_repository, _playHandlerLoggerMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsGameResult()
    {
        // Arrange
        var playerChoiceId = (int)ChoiceType.Rock;
        var playCommand = new PlayCommand(playerChoiceId);
        var expectedResult = new GameResult(GameResultType.Win, playerChoiceId, (int)ChoiceType.Scissors);

        // Act
        var result = await _playHandler.Handle(playCommand, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<GameResult>(result);
    }

}
