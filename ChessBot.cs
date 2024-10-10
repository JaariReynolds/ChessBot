using Chess.Classes;

namespace ChessBot
{
    public enum BotDifficulty
    {
        Easy
    }

    public class ChessBot
    {
        protected readonly Gameboard _gameboard;

        public ChessBot(BotDifficulty botDifficulty, Gameboard gameboard)
        {
            _gameboard = gameboard;
        }

        public Action CalculateBestAction()
        {

            var teamActions = _gameboard.CalculateTeamActions(_gameboard.CurrentTeamColour);

            var rnd = new Random();
            var randomPiece = teamActions.ElementAt(rnd.Next(0, teamActions.Count));
            var pieceActions = teamActions[randomPiece.Key];

            return pieceActions[0];
        }
    }
}