namespace Infrastructure.DTOs
{
    public record GameResultDto
    {
        public string Results { get; set; }
        public int Player { get; set; }
        public int Computer { get; set; }

        public GameResultDto(string result, int playerChoiceId, int computerChoiceId)
        {
            Results = result;
            Player = playerChoiceId;
            Computer = computerChoiceId;
        }
    }
}


