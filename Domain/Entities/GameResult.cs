namespace Domain.Entities
{
    public class GameResult
    {
        public GameResultType Result { get; set; }
        public int PlayersChoice { get; set; }
        public int ComputersChoice { get; set; }

        public GameResult(GameResultType result, int playerChoiceId, int computerChoiceId)
        {
            Result = result;
            PlayersChoice = playerChoiceId;
            ComputersChoice = computerChoiceId;
        }
    }
}
