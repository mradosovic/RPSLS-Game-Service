using Domain;
using MediatR;
using RPSLS_Game.Application.Interfaces;
using RPSLS_Game.Domain.Models;

public class GetRandomChoiceQuery : IRequest<Choice> { }

/// <summary>
/// Handles returning posible choices.
/// </summary>
public class GetRandomChoiceHandler : IRequestHandler<GetRandomChoiceQuery, Choice>
{
    private readonly IGameService _gameService;

    public GetRandomChoiceHandler(IGameService gameService)
    {
        _gameService = gameService;
    }

    public async Task<Choice> Handle(GetRandomChoiceQuery request, CancellationToken cancellationToken)
    {
        return await _gameService.GetRandomChoiceAsync();
    }
}
