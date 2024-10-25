namespace RPSLS_Game.Application.Models
{
    /// <summary>
    /// Represents a game result.
    /// </summary>
    public class GameResult
    {
        public string Results { get; set; }
        public int Player { get; set; }
        public int Computer { get; set; }

        public GameResult(string result, int playerChoiceId, int computerChoiceId)
        {
            Results = result;
            Player = playerChoiceId;
            Computer = computerChoiceId;
        }
    }
}
