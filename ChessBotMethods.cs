﻿using Chess.Classes;
using Chess.Types;

namespace ChessBotNamespace
{
    internal static class ChessBotMethods
    {
        public static int EvaluatedActionsCount = 0;

        public static int EvaluateActionPriority(Action action, Piece[][] board)
        {
            var evaluation = 0;

            if (action.ActionType == ActionType.Capture)
                evaluation += 100 + board[action.Square.X][action.Square.Y].PieceValue;

            return evaluation;

        }


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

                    if (piece == null || piece.Name == PieceName.King) continue;

                    score += piece.TeamColour == TeamColour.White ? piece.PieceValue : -piece.PieceValue;
                }
            }
            EvaluatedActionsCount++;
            return score;
        }

        public static int Minimax(this Gameboard gameboard, int depth, bool isBotTurn, TeamColour botTeamColour, int alpha, int beta)
        {
            if (depth == 0 || gameboard.CheckmateTeamColour != null)
            {
                return gameboard.MaterialEvaluation();
            }

            TeamColour playerTeamColour = botTeamColour.GetOppositeTeam();

            if (isBotTurn) // (maximising bot score)
            {
                int maxEval = int.MinValue;

                foreach (var action in gameboard.CalculateTeamActions(botTeamColour))
                {
                    Gameboard simulatedBoard = new(gameboard);
                    Action simulatedAction = new(action);
                    simulatedBoard.PerformAction(simulatedAction);
                    int eval = Minimax(simulatedBoard, depth - 1, false, botTeamColour, alpha, beta);
                    maxEval = Math.Max(maxEval, eval);
                    alpha = Math.Max(alpha, eval);

                    if (beta <= alpha)
                        break;
                }
                return maxEval;
            }
            else // player's turn (minimising bot score)
            {
                int minEval = int.MaxValue;
                foreach (var action in gameboard.CalculateTeamActions(playerTeamColour))
                {
                    Gameboard simulatedBoard = new(gameboard);
                    Action simulatedAction = new(action);
                    simulatedBoard.PerformAction(simulatedAction);
                    int eval = Minimax(simulatedBoard, depth - 1, true, botTeamColour, alpha, beta);
                    minEval = Math.Min(minEval, eval);
                    beta = Math.Min(beta, eval);

                    if (beta <= alpha)
                        break;
                }
                return minEval;
            }
        }
    }
}
