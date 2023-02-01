namespace Runtime.PlaySceneLogic
{
    using System.Collections.Generic;
    using Runtime.PlaySceneLogic.ChessPiece;
    using UnityEngine;

    public class PieceRegularMoveHelper
    {
        public void CheckBotLeftDiagonal(int row, int col, BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (row - i < 0 || col - i < 0) continue;
                var botLeftDiagonalPiece = runtimePieces[row - i, col - i];
                if (botLeftDiagonalPiece != null)
                {
                    availableMoves.Add(new Vector2Int(row - i, col - i));
                    break;
                }

                availableMoves.Add(new Vector2Int(row - i, col - i));
            }
        }
        
        public void CheckBotRightDiagonal(int row, int col, BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (row + i > 7 || col - i < 0) continue;
                var botRightDiagonalPiece = runtimePieces[row + i, col - i];
                if (botRightDiagonalPiece != null)
                {
                    availableMoves.Add(new Vector2Int(row + i, col - i));
                    break;
                }

                availableMoves.Add(new Vector2Int(row + i, col - i));
            }
        }

        public void CheckTopLeftDiagonal(int row, int col, BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (row - i < 0 || col + i > 7) continue;
                var topLeftDiagonalPiece = runtimePieces[row - i, col + i];
                if (topLeftDiagonalPiece != null)
                {
                    availableMoves.Add(new Vector2Int(row - i, col + i));
                    break;
                }

                availableMoves.Add(new Vector2Int(row - i, col + i));
            }
        }

        public void CheckTopRightDiagonal(int row, int col, BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (row + i > 7 || col + i > 7) continue;
                var topRightDiagonalPiece = runtimePieces[row + i, col + i];
                if (topRightDiagonalPiece != null)
                {
                    availableMoves.Add(new Vector2Int(row + i, col + i));
                    break;
                }

                availableMoves.Add(new Vector2Int(row + i, col + i));
            }
        }

        public void CheckRightRow(int row, int col, BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (row + i > 7) continue;
                var topColumnPiece = runtimePieces[row + i, col];
                if (topColumnPiece != null)
                {
                    availableMoves.Add(new Vector2Int(row + i, col));
                    break;
                }

                availableMoves.Add(new Vector2Int(row + i, col));
            }
        }

        public void CheckLeftRow(int row, int col, BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (row - i < 0) continue;
                var topColumnPiece = runtimePieces[row - i, col];
                if (topColumnPiece != null)
                {
                    availableMoves.Add(new Vector2Int(row - i, col));
                    break;
                }

                availableMoves.Add(new Vector2Int(row - i, col));
            }
        }

        public void CheckBotColumn(int row, int col, BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (col - i < 0) continue;
                var topColumnPiece = runtimePieces[row, col - i];
                if (topColumnPiece != null)
                {
                    availableMoves.Add(new Vector2Int(row, col - i));
                    break;
                }

                availableMoves.Add(new Vector2Int(row, col - i));
            }
        }

        public void CheckTopColumn(int row, int col, BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (col + i > 7) continue;
                var topColumnPiece = runtimePieces[row, col + i];
                if (topColumnPiece != null)
                {
                    availableMoves.Add(new Vector2Int(row, col + i));
                    break;
                }

                availableMoves.Add(new Vector2Int(row, col + i));
            }
        }
    }
}