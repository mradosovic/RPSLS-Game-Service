public class PlayResultDto
{
    /// <summary>
    /// Result of a play.
    /// </summary>
    public string Results { get; set; } 
    public int PlayerChoiceId { get; set; }
    public int ComputerChoiceId { get; set; }

    public PlayResultDto(string result, int playerChoiceId, int computerChoiceId)
    {
        Results = result;
        PlayerChoiceId = playerChoiceId;
        ComputerChoiceId = computerChoiceId;
    }
}
