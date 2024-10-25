using MediatR;
using RPSLS_Game.Application.Interfaces;
using RPSLS_Game.Application.Models;
using RPSLS_Game.Domain.Models;

public class PlayCommand : IRequest<GameResult>
{
    public int PlayerChoiceId { get; set; }
    public PlayCommand(int playerChoiceId) => PlayerChoiceId = playerChoiceId;
}

/// <summary>
/// Handles play request.
/// </summary>
public class PlayHandler : IRequestHandler<PlayCommand, GameResult>
{
    private readonly IGameService _gameService;

    public PlayHandler(IGameService gameService) => _gameService = gameService;

    public async Task<GameResult> Handle(PlayCommand request, CancellationToken cancellationToken)
    {
        var playRequest = new PlayRequest(request.PlayerChoiceId);
        return await _gameService.Play(playRequest);
    }
}
