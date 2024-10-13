using Chess.Classes;
using Chess.Types;

namespace ChessBot
{
    internal static class ChessBotMethods
    {
        /// <summary>
        /// Returns the difference in Material value on the current board, with a positive int being in White's favour
        /// </summary>
        public static int MaterialEvaluation(this Gameboard gameboard)
        {
            int score = 0;

            for (int i = 0; i < gameboard.Board.Length; i++)
            {
                for (int j = 0; j < gameboard.Board[i].Length; j++)
                {
                    var piece = gameboard.Board[i][j];

                    if (piece == null || piece.Name == "King") continue;

                    score += piece.TeamColour == TeamColour.White ? piece.PieceValue : -piece.PieceValue;

                }
            }

            return score;
        }

        public static int Minimax(this Gameboard gameboard, int depth, bool isMaximisingPlayer)
        {
            if (depth == 0 || gameboard.CheckmateTeamColour != CheckStatus.None)
            {
                return gameboard.MaterialEvaluation();
            }

            if (isMaximisingPlayer)
            {
                int maxEval = int.MinValue;

                // Black is always the bot in this case
                foreach (var action in gameboard.CalculateTeamActionsList(TeamColour.Black))
                {
                    Gameboard simulatedBoard = new(gameboard);
                    Action simulatedAction = new(action);
                    simulatedBoard.PerformAction(simulatedAction);

                    int eval = Minimax(simulatedBoard, depth - 1, false);
                    maxEval = Math.Max(maxEval, eval);
                }
                return maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                // White is always the player
                foreach (var action in gameboard.CalculateTeamActionsList(TeamColour.White))
                {
                    Gameboard simulatedBoard = new(gameboard);
                    Action simulatedAction = new(action);
                    simulatedBoard.PerformAction(simulatedAction);

                    int eval = Minimax(simulatedBoard, depth - 1, false);
                    minEval = Math.Min(minEval, eval);
                }
                return minEval;
            }
        }
    }
}
