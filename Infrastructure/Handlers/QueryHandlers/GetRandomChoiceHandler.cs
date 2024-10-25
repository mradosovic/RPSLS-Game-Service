using Domain;
using MediatR;
using RPSLS_Game.Application.Interfaces;

public class GetRandomChoiceQuery : IRequest<ChoiceType> { }

/// <summary>
/// Handles returning posible choices.
/// </summary>
public class GetRandomChoiceHandler : IRequestHandler<GetRandomChoiceQuery, ChoiceType>
{
    private readonly IGameService _gameService;

    public GetRandomChoiceHandler(IGameService gameService)
    {
        _gameService = gameService;
    }

    public async Task<ChoiceType> Handle(GetRandomChoiceQuery request, CancellationToken cancellationToken)
    {
        return await _gameService.GetRandomChoiceAsync();
    }
}
