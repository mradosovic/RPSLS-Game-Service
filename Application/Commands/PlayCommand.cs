using Domain.Entities;
using MediatR;

namespace Application.Commands
{
    public class PlayCommand : IRequest<GameResult>
    {
        public int PlayerChoiceId { get; set; }
        public PlayCommand(int playerChoiceId) => PlayerChoiceId = playerChoiceId;
    }
}
