namespace RPSLS_Game.Domain.Models
{
    /// <summary>
    /// Represents a choice.
    /// </summary>
    public class Choice
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Choice(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
