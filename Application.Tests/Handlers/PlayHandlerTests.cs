//using Application.Commands;
//using Application.Handlers.CommandHandlers;
//using Domain.Entities;
//using Domain.Repositories;
////using Infrastructure.Repositories; //this is wrong! Application tests shouldn't know about Infrasructure project
////using Infrastructure.Settings;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using Moq;

//namespace RPSLS_Game.Api.Tests;

//public class PlayHandlerTests
//{
//    private readonly Mock<IOptions<ApiSettings>> _apiSettingsMock;  //this is wrong! Application tests shouldn't know about Infrasructure project
//    private readonly Mock<ILogger<IChoiceRepository>> _repositoryLoggerMock;  //this is wrong! Application tests shouldn't know about Repository project
//    private readonly Mock<ILogger<PlayHandler>> _playHandlerLoggerMock;
//    private readonly PlayHandler _playHandler;
//    private readonly IChoiceRepository _repository;

//    public PlayHandlerTests()
//    {
//        _apiSettingsMock = new Mock<IOptions<ApiSettings>>();
//        _apiSettingsMock.Setup(x => x.Value).Returns(new ApiSettings { RandomChoiceApiUrl = "https://codechallenge.boohma.com/random" });
//        _repositoryLoggerMock = new Mock<ILogger<IChoiceRepository>>();
//        _playHandlerLoggerMock = new Mock<ILogger<PlayHandler>>();
//        _repository = new ChoiceRepository(_apiSettingsMock.Object, _repositoryLoggerMock.Object);
//        _playHandler = new PlayHandler(_repository, _playHandlerLoggerMock.Object);
//    }

//    [Fact]
//    public async Task Handle_ValidRequest_ReturnsGameResult()
//    {
//        // Arrange
//        var playerChoiceId = (int)ChoiceType.Rock;
//        var playCommand = new PlayCommand(playerChoiceId);
//        var expectedResult = new GameResult(GameResultType.Win, playerChoiceId, (int)ChoiceType.Scissors);

//        // Act
//        var result = await _playHandler.Handle(playCommand, CancellationToken.None);

//        // Assert
//        Assert.NotNull(result);
//        Assert.IsAssignableFrom<GameResult>(result);
//        //Assert.Equal(expectedResult.Result, result.Result);
//        //Assert.Equal(expectedResult.PlayersChoice, result.PlayersChoice);
//        //Assert.Equal(expectedResult.ComputersChoice, result.ComputersChoice);
//    }

//}
