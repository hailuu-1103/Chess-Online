namespace Runtime.PlaySceneLogic.ChessPiece.Piece
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Bishop : BaseChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves()
        {
            var availableMoves = new List<Vector2Int>();
            var runtimePieces  = this.boardController.runtimePieces;
            this.CheckTopLeftDiagonal(runtimePieces, availableMoves);

            this.CheckTopRightDiagonal(runtimePieces, availableMoves);

            this.CheckBotLeftDiagonal(runtimePieces, availableMoves);

            this.CheckBotRightDiagonal(runtimePieces, availableMoves);
            return availableMoves;
        }

        private void CheckBotRightDiagonal(BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (this.Row - i < 0 || this.Col - i < 0) continue;
                var botLeftDiagonalPiece = runtimePieces[this.Row - i, this.Col - i];
                if (this.IsBlockByAlies(botLeftDiagonalPiece)) break;
                if (botLeftDiagonalPiece == null)
                {
                    availableMoves.Add(new Vector2Int(this.Row - i, this.Col - i));
                }
            }
        }

        private void CheckBotLeftDiagonal(BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (this.Row + i > 7 || this.Col - i < 0) continue;
                var botRightDiagonalPiece = runtimePieces[this.Row + i, this.Col - i];
                if (this.IsBlockByAlies(botRightDiagonalPiece)) break;
                if (botRightDiagonalPiece == null)
                {
                    availableMoves.Add(new Vector2Int(this.Row + i, this.Col - i));
                }
            }
        }

        private void CheckTopRightDiagonal(BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (this.Row - i < 0 || this.Col + i > 7) continue;
                var topLeftDiagonalPiece = runtimePieces[this.Row - i, this.Col + i];
                if (this.IsBlockByAlies(topLeftDiagonalPiece)) break;
                if (topLeftDiagonalPiece == null)
                {
                    availableMoves.Add(new Vector2Int(this.Row - i, this.Col + i));
                }
            }
        }

        private void CheckTopLeftDiagonal(BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (this.Row + i > 7 || this.Col + i > 7) continue;
                var topRightDiagonalPiece = runtimePieces[this.Row + i, this.Col + i];
                if (this.IsBlockByAlies(topRightDiagonalPiece)) break;
                // Blank space or enemy piece
                if (topRightDiagonalPiece == null)
                {
                    availableMoves.Add(new Vector2Int(this.Row + i, this.Col + i));
                }
            }
        }

        public override void Attack(BaseChessPiece targetPiece) { }
    }
}