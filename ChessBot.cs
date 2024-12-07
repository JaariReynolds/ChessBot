using Chess.Classes;
using Chess.Types;

namespace ChessBotNamespace
{
    public class ChessBot
    {
        protected readonly Gameboard _gameboard;

        public ChessBot(Gameboard gameboard)
        {
            _gameboard = gameboard;
        }

        public Action CalculateBestAction(int depth)
        {
            if (depth == 0)
                throw new ArgumentOutOfRangeException("Depth of 0 is not a valid Minimax depth.");

            ChessBotMethods.EvaluatedActionsCount = 0;

            var botTeamColour = _gameboard.CurrentTeamColour;
            bool isMaximisingWhite = botTeamColour == TeamColour.White;
            int bestScore = isMaximisingWhite ? int.MinValue : int.MaxValue;
            Action bestAction = null!;
            int alpha = int.MinValue;
            int beta = int.MaxValue;

            var prioritisedActions = _gameboard.CalculateTeamActions(botTeamColour)
                .OrderByDescending(action => ChessBotMethods.EvaluateActionPriority(action, _gameboard.Board))
                .ToList();


            Console.WriteLine($"Total number of available moves: {prioritisedActions.Count}");
            foreach (var action in prioritisedActions)
            {
                Gameboard simulatedBoard = new(_gameboard);
                Action simulatedAction = new(action);
                simulatedBoard.PerformAction(simulatedAction);
                int moveScore = ChessBotMethods.Minimax(
                    simulatedBoard,
                    depth - 1,
                    isBotTurn: false,
                    botTeamColour,
                    alpha,
                    beta
                );

                // when the bot is white, the best score is the one with the most positive moveScore
                if (isMaximisingWhite)
                {
                    if (moveScore > bestScore)
                    {
                        bestScore = moveScore;
                        bestAction = action;
                    }
                    alpha = Math.Max(alpha, bestScore);

                }
                // when the bot is black, the best score is the one with the most negative moveScore
                else
                {
                    if (moveScore < bestScore)
                    {
                        bestScore = moveScore;
                        bestAction = action;
                    }
                    beta = Math.Min(beta, bestScore);
                }

                if (beta <= alpha)
                    break; // prune branches that won't affect the outcome
            }

            Console.WriteLine($"best score: {bestScore}");

            Console.WriteLine($"selected action: {bestAction}");
            Console.WriteLine($"actions evaluated: {ChessBotMethods.EvaluatedActionsCount}");
            return bestAction!;
        }
    }
}