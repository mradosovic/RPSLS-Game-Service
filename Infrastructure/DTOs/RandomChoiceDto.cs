
namespace Infrastructure.DTOs
{
    /// <summary>
    /// Random choice DTO.
    /// </summary>
    public class RandomChoiceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public RandomChoiceDto(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

}
