using Domain;
using RPSLS_Game.Application.Models;
using RPSLS_Game.Domain.Models;

namespace RPSLS_Game.Application.Interfaces
{
    /// <summary>
    /// Games service interface.
    /// </summary>
    public interface IGameService
    {
        Task<GameResult> Play(PlayRequest request);
        Task<ChoiceType> GetRandomChoiceAsync();  
        Task<IEnumerable<Choice>> GetChoicesAsync(); 
    }
}
