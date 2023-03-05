namespace Runtime.PlaySceneLogic
{
    using System.Collections.Generic;
    using Runtime.PlaySceneLogic.ChessPiece;
    using UnityEngine;

    public class PieceRegularMoveHelper
    {
        public List<Vector2Int> GetCheckMovesIndexBotRightDiagonal(Vector2Int currentPieceIndex, List<Vector2Int> availableMovesIndex, Vector2Int kingPieceIndex)
        {
            if (!availableMovesIndex.Contains(kingPieceIndex)) return null;
            var checkMovesIndex = new List<Vector2Int>();
            var distance        = Mathf.Abs(currentPieceIndex.y - kingPieceIndex.y);
            for (var i = 0; i < distance; i++)
            {
                checkMovesIndex.Add(new Vector2Int(currentPieceIndex.x + i, currentPieceIndex.y - i));
            }
            return checkMovesIndex;
        }
        public List<Vector2Int> GetCheckMovesIndexBotLeftDiagonal(Vector2Int currentPieceIndex, List<Vector2Int> availableMovesIndex, Vector2Int kingPieceIndex)
        {
            if (!availableMovesIndex.Contains(kingPieceIndex)) return null;
            var checkMovesIndex = new List<Vector2Int>();
            var distance        = Mathf.Abs(currentPieceIndex.y - kingPieceIndex.y);
            for (var i = 0; i < distance; i++)
            {
                checkMovesIndex.Add(new Vector2Int(currentPieceIndex.x - i, currentPieceIndex.y - i));
            }
            return checkMovesIndex;
        }
        public List<Vector2Int> GetCheckMovesIndexTopRightDiagonal(Vector2Int currentPieceIndex,List<Vector2Int> availableMovesIndex, Vector2Int kingPieceIndex)
        {
            if (!availableMovesIndex.Contains(kingPieceIndex)) return null;
            var checkMovesIndex = new List<Vector2Int>();
            var distance        = Mathf.Abs(currentPieceIndex.y - kingPieceIndex.y);
            for (var i = 0; i < distance; i++)
            {
                checkMovesIndex.Add(new Vector2Int(currentPieceIndex.x + i, currentPieceIndex.y + i));
            }
            return checkMovesIndex;
        }
        public List<Vector2Int> GetCheckMovesIndexTopLeftDiagonal(Vector2Int currentPieceIndex, List<Vector2Int> availableMovesIndex, Vector2Int kingPieceIndex)
        {
            if (!availableMovesIndex.Contains(kingPieceIndex)) return null;
            var checkMovesIndex = new List<Vector2Int>();
            var distance        = Mathf.Abs(currentPieceIndex.y - kingPieceIndex.y);
            for (var i = 0; i < distance; i++)
            {
                checkMovesIndex.Add(new Vector2Int(currentPieceIndex.x - i, currentPieceIndex.y + i));
            }
            return checkMovesIndex;
        }
        public List<Vector2Int> GetCheckMovesIndexTopColumn(Vector2Int currentPieceIndex, List<Vector2Int> availableMovesIndex, Vector2Int kingPieceIndex)
        {
            if (!availableMovesIndex.Contains(kingPieceIndex)) return null;
            var checkMovesIndex = new List<Vector2Int>();
            var distance        = Mathf.Abs(currentPieceIndex.y - kingPieceIndex.y);
            for (var i = 0; i < distance; i++)
            {
                checkMovesIndex.Add(new Vector2Int(currentPieceIndex.x, currentPieceIndex.y + i));
            }
            return checkMovesIndex;
        }
        public List<Vector2Int> GetCheckMovesIndexBotColumn(Vector2Int currentPieceIndex, List<Vector2Int> availableMovesIndex, Vector2Int kingPieceIndex)
        {
            if (!availableMovesIndex.Contains(kingPieceIndex)) return null;
            var checkMovesIndex = new List<Vector2Int>();
            var distance        = Mathf.Abs(currentPieceIndex.y - kingPieceIndex.y);
            for (var i = 0; i < distance; i++)
            {
                checkMovesIndex.Add(new Vector2Int(currentPieceIndex.x, currentPieceIndex.y - i));
            }
            return checkMovesIndex;
        }
        public List<Vector2Int> GetCheckMovesIndexLeftRow(Vector2Int currentPieceIndex,List<Vector2Int> availableMovesIndex, Vector2Int kingPieceIndex)
        {
            if (!availableMovesIndex.Contains(kingPieceIndex)) return null;
            var checkMovesIndex = new List<Vector2Int>();
            var distance        = Mathf.Abs(currentPieceIndex.x - kingPieceIndex.x);
            for (var i = 0; i < distance; i++)
            {
                checkMovesIndex.Add(new Vector2Int(currentPieceIndex.x - i, currentPieceIndex.y));
            }
            return checkMovesIndex;
        }
        public List<Vector2Int> GetCheckMovesIndexRightRow(Vector2Int currentPieceIndex, List<Vector2Int> availableMovesIndex, Vector2Int kingPieceIndex)
        {
            if (!availableMovesIndex.Contains(kingPieceIndex)) return null;
            var checkMovesIndex = new List<Vector2Int>();
            var distance        = Mathf.Abs(currentPieceIndex.x - kingPieceIndex.x);
            for (var i = 0; i < distance; i++)
            {
                checkMovesIndex.Add(new Vector2Int(currentPieceIndex.x + i, currentPieceIndex.y));
            }
            return checkMovesIndex;
        }
        public void CheckBotLeftDiagonal(int row, int col, BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (row - i < 0 || col - i < 0) continue;
                var botLeftDiagonalPiece = runtimePieces[row - i, col - i];
                if (botLeftDiagonalPiece != null && botLeftDiagonalPiece.team == runtimePieces[row, col].team) break;
                availableMoves.Add(new Vector2Int(row - i, col - i));
            }
        }
        public void CheckBotRightDiagonal(int row, int col, BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (row + i > 7 || col - i < 0) continue;
                var botRightDiagonalPiece = runtimePieces[row + i, col - i];
                if (botRightDiagonalPiece != null && botRightDiagonalPiece.team == runtimePieces[row, col].team) break;
                availableMoves.Add(new Vector2Int(row + i, col - i));
            }
        }
        public void CheckTopLeftDiagonal(int row, int col, BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (row - i < 0 || col + i > 7) continue;
                var topLeftDiagonalPiece = runtimePieces[row - i, col + i];
                if (topLeftDiagonalPiece != null && topLeftDiagonalPiece.team == runtimePieces[row, col].team) break;
                availableMoves.Add(new Vector2Int(row - i, col + i));
            }
        }
        public void CheckTopRightDiagonal(int row, int col, BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (row + i > 7 || col + i > 7) continue;
                var topRightDiagonalPiece = runtimePieces[row + i, col + i];
                if (topRightDiagonalPiece != null && topRightDiagonalPiece.team == runtimePieces[row, col].team) break;
                availableMoves.Add(new Vector2Int(row + i, col + i));
            }
        }
        public void CheckRightRow(int row, int col, BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (row + i > 7) continue;
                var topColumnPiece = runtimePieces[row + i, col];
                if (topColumnPiece != null && topColumnPiece.team == runtimePieces[row, col].team) break;
                availableMoves.Add(new Vector2Int(row + i, col));
            }
        }
        public void CheckLeftRow(int row, int col, BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (row - i < 0) continue;
                var topColumnPiece = runtimePieces[row - i, col];
                if (topColumnPiece != null && topColumnPiece.team == runtimePieces[row, col].team) break;
                availableMoves.Add(new Vector2Int(row - i, col));
            }
        }
        public void CheckBotColumn(int row, int col, BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (col - i < 0) continue;
                var topColumnPiece = runtimePieces[row, col - i];
                if (topColumnPiece != null && topColumnPiece.team == runtimePieces[row, col].team) break;
                availableMoves.Add(new Vector2Int(row, col - i));
            }
        }
        public void CheckTopColumn(int row, int col, BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (col + i > 7) continue;
                var topColumnPiece = runtimePieces[row, col + i];
                if (topColumnPiece != null && topColumnPiece.team == runtimePieces[row, col].team) break;
                availableMoves.Add(new Vector2Int(row, col + i));
            }
        }
    }
}