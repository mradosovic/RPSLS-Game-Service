namespace RPSLS_Game.Domain.Models
{
    /// <summary>
    /// Represents a play request.
    /// </summary>
    public class PlayRequest
    {
        public int PlayerChoiceId { get; set; }

        public PlayRequest(int playerChoiceId)
        {
            PlayerChoiceId = playerChoiceId;
        }
    }
}
