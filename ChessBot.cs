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

        public Action CalculateBestAction(int depth)
        {
            if (depth == 0)
                throw new ArgumentOutOfRangeException("Depth of 0 is not a valid Minimax depth.");

            ChessBotMethods.EvaluatedActionsCount = 0;

            int bestScore = int.MinValue;
            Action bestAction = null!;

            foreach (var action in _gameboard.CalculateTeamActions(_gameboard.CurrentTeamColour))
            {
                Gameboard simulatedBoard = new(_gameboard);
                Action simulatedAction = new(action);
                simulatedBoard.PerformAction(simulatedAction);
                int moveScore = ChessBotMethods.Minimax(simulatedBoard, depth - 1, false, _gameboard.CurrentTeamColour, int.MinValue, int.MaxValue);

                if (moveScore >= bestScore)
                {
                    bestScore = moveScore;
                    bestAction = action;
                }
            }

            Console.WriteLine($"selected action: {bestAction}");
            Console.WriteLine($"actions evaluated: {ChessBotMethods.EvaluatedActionsCount}");
            Console.WriteLine("-----------------");
            return bestAction!;
        }
    }
}