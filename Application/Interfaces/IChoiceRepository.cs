using RPSLS_Game.Domain.Models;

namespace RPSLS_Game.Application.Interfaces
{
    /// <summary>
    /// Choice repository interface.
    /// </summary>
    public interface IChoiceRepository
    {
        Task<IEnumerable<Choice>> GetChoicesAsync();
        Task<Choice> GetRandomChoiceAsync();
    }
}
