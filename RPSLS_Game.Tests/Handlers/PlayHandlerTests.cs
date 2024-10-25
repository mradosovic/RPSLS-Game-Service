using Domain;
using Moq;
using RPSLS_Game.Application.Interfaces;
using RPSLS_Game.Application.Models;
using RPSLS_Game.Domain.Models;

namespace RPSLS_Game.Api.Tests;

public class PlayHandlerTests
{
    private readonly Mock<IGameService> _gameServiceMock;
    private readonly PlayHandler _playHandler;

    public PlayHandlerTests()
    {
        _gameServiceMock = new Mock<IGameService>();
        _playHandler = new PlayHandler(_gameServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsGameResult()
    {
        // Arrange
        var playerChoiceId = (int)ChoiceType.Rock;
        var playCommand = new PlayCommand(playerChoiceId);
        var expectedResult = new GameResult("Player wins", playerChoiceId, (int)ChoiceType.Scissors);

        _gameServiceMock.Setup(gs => gs.Play(It.IsAny<PlayRequest>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _playHandler.Handle(playCommand, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResult.Results, result.Results);
        Assert.Equal(expectedResult.Player, result.Player);
        Assert.Equal(expectedResult.Computer, result.Computer);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsNull()
    {
        // Arrange
        var playerChoiceId = (int)ChoiceType.Rock;
        var playCommand = new PlayCommand(playerChoiceId);

        _gameServiceMock.Setup(gs => gs.Play(It.IsAny<PlayRequest>()))
            .ReturnsAsync((GameResult)null); // Simulating a scenario where result is null

        // Act
        var result = await _playHandler.Handle(playCommand, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
