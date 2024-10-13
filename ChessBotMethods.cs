using Chess.Classes;
using Chess.Types;

namespace ChessBot
{
    internal static class ChessBotMethods
    {
        /// <summary>
        /// Returns the difference in Material value on the current board, with a positive int being in White's favour
        /// </summary>
        public static int MaterialEvaluation(this Gameboard gameboard, TeamColour botTeam)
        {
            int score = 0;

            for (int i = 0; i < gameboard.Board.Length; i++)
            {
                for (int j = 0; j < gameboard.Board[i].Length; j++)
                {
                    var piece = gameboard.Board[i][j];

                    if (piece == null || piece.Name == "King") continue;

                    score += piece.TeamColour == botTeam ? piece.PieceValue : -piece.PieceValue;

                }
            }

            return score;
        }

        public static int Minimax(this Gameboard gameboard, int depth, bool isMaximisingPlayer, int alpha, int beta)
        {
            if (depth == 0 || gameboard.CheckmateTeamColour != CheckStatus.None)
            {
                //Console.WriteLine("hit");
                return gameboard.MaterialEvaluation(TeamColour.Black);
            }

            if (isMaximisingPlayer) // bot's turn
            {
                int maxEval = int.MinValue;

                // Black is always the bot in this case
                foreach (var action in gameboard.CalculateTeamActionsList(TeamColour.Black))
                {
                    Gameboard simulatedBoard = new(gameboard);
                    Action simulatedAction = new(action);
                    simulatedBoard.PerformAction(simulatedAction);
                    int eval = Minimax(simulatedBoard, depth - 1, false, alpha, beta);
                    //Console.WriteLine($"Evaluating move {action} with score: {eval}");
                    maxEval = Math.Max(maxEval, eval);
                    alpha = Math.Max(alpha, eval);

                    if (beta <= alpha)
                        break;
                }
                return maxEval;
            }
            else // player's turn
            {
                int minEval = int.MaxValue;
                // White is always the player
                foreach (var action in gameboard.CalculateTeamActionsList(TeamColour.White))
                {
                    Gameboard simulatedBoard = new(gameboard);
                    Action simulatedAction = new(action);
                    simulatedBoard.PerformAction(simulatedAction);

                    int eval = Minimax(simulatedBoard, depth - 1, true, alpha, beta);
                    //Console.WriteLine($"Evaluating move {action} with score: {eval}");

                    minEval = Math.Min(minEval, eval);
                    beta = Math.Min(beta, eval);

                    if (beta <= alpha)
                        break;
                }
                return minEval;
            }
        }

        public static Action GetBestAction(this Gameboard gameboard, int depth)
        {
            int bestScore = int.MinValue;
            Action bestAction = null;

            foreach (var action in gameboard.CalculateTeamActionsList(TeamColour.Black))
            {
                Gameboard simulatedBoard = new(gameboard);
                Action simulatedAction = new(action);
                simulatedBoard.PerformAction(simulatedAction);
                int moveScore = Minimax(simulatedBoard, depth, false, int.MinValue, int.MaxValue);

                Console.WriteLine($"Action: {action}, score: {moveScore}");

                if (moveScore > bestScore)
                {
                    bestScore = moveScore;
                    bestAction = action;
                }
            }

            Console.WriteLine($"selected action: {bestAction}");
            Console.WriteLine("-----------------");
            return bestAction!;
        }
    }
}
