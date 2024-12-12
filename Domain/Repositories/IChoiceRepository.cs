using Domain.Entities;

namespace Domain.Repositories
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
