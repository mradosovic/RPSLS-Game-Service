using Domain;
using MediatR;
using RPSLS_Game.Application.Interfaces;

public class GetChoicesQuery : IRequest<IEnumerable<ChoiceType>> { }

/// <summary>
/// Habdles choices query.
/// </summary>
public class GetChoicesHandler : IRequestHandler<GetChoicesQuery, IEnumerable<ChoiceType>>
{
    private readonly IGameService _gameService;

    public GetChoicesHandler(IGameService gameService)
    {
        _gameService = gameService;
    }

    public async Task<IEnumerable<ChoiceType>> Handle(GetChoicesQuery request, CancellationToken cancellationToken)
    {
        var choices = await _gameService.GetChoicesAsync();
        return choices.Select(c => (ChoiceType)c.Id); 
    }

}
