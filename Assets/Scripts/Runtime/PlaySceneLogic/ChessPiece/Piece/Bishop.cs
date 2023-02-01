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
            // Check top right diagonal
            this.CheckTopRightDiagonal(runtimePieces, availableMoves);

            // Check top left diagonal
            this.CheckTopLeftDiagonal(runtimePieces, availableMoves);

            // Check bot right diagonal
            this.CheckBotRightDiagonal(runtimePieces, availableMoves);

            // Check bot left diagonal
            this.CheckBotLeftDiagonal(runtimePieces, availableMoves);
            return availableMoves;
        }

        private void CheckBotLeftDiagonal(BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (this.row - i < 0 || this.col - i < 0) continue;
                var botLeftDiagonalPiece = runtimePieces[this.row - i, this.col - i];
                if (botLeftDiagonalPiece != null)
                {
                    availableMoves.Add(new Vector2Int(this.row - 1, this.col - 1));
                    break;
                }

                availableMoves.Add(new Vector2Int(this.row - i, this.col - i));
            }
        }

        private void CheckBotRightDiagonal(BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (this.row + i > 7 || this.col - i < 0) continue;
                var botRightDiagonalPiece = runtimePieces[this.row + i, this.col - i];
                if (botRightDiagonalPiece != null)
                {
                    availableMoves.Add(new Vector2Int(this.row + 1, this.col - 1));
                    break;
                }

                availableMoves.Add(new Vector2Int(this.row + i, this.col - i));
            }
        }

        private void CheckTopLeftDiagonal(BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (this.row - i < 0 || this.col + i > 7) continue;
                var topLeftDiagonalPiece = runtimePieces[this.row - i, this.col + i];
                if (topLeftDiagonalPiece != null)
                {
                    availableMoves.Add(new Vector2Int(this.row - 1, this.col + 1));
                    break;
                }

                availableMoves.Add(new Vector2Int(this.row - i, this.col + i));
            }
        }

        private void CheckTopRightDiagonal(BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (this.row + i > 7 || this.col + i > 7) continue;
                var topRightDiagonalPiece = runtimePieces[this.row + i, this.col + i];
                if (topRightDiagonalPiece != null)
                {
                    availableMoves.Add(new Vector2Int(this.row + 1, this.col + 1));
                    break;
                }

                availableMoves.Add(new Vector2Int(this.row + i, this.col + i));
            }
        }

        public override void Attack(BaseChessPiece targetPiece)  { }

        public override void PreMove(BaseChessPiece targetPiece)
        {
        }
    }
}