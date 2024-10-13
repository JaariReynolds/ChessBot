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
            return _gameboard.GetBestAction(2);
        }
    }
}