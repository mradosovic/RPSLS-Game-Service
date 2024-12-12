namespace Domain.Entities
{
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
