namespace Runtime.PlaySceneLogic.ChessPiece.Piece
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Castle : BaseChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves()
        {
            var availableMoves = new List<Vector2Int>();
            var runtimePieces  = this.boardController.runtimePieces;
            
            // Check top column
            this.CheckTopColumn(runtimePieces, availableMoves);
            // Check bot column
            this.CheckBotColumn(runtimePieces, availableMoves);
            // Check left row
            this.CheckLeftRow(runtimePieces, availableMoves);
            // Check right row
            this.CheckRightRow(runtimePieces, availableMoves);
            return availableMoves;
        }

        private void CheckRightRow(BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (this.row + i > 7) continue;
                var topColumnPiece = runtimePieces[this.row + i, this.col];
                if (topColumnPiece != null)
                {
                    availableMoves.Add(new Vector2Int(this.row + i, this.col));
                    break;
                }
                availableMoves.Add(new Vector2Int(this.row + i, this.col));
            }
        }


        private void CheckLeftRow(BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (this.row - i < 0) continue;
                var topColumnPiece = runtimePieces[this.row - i, this.col];
                if (topColumnPiece != null)
                {
                    availableMoves.Add(new Vector2Int(this.row - i, this.col));
                    break;
                }
                availableMoves.Add(new Vector2Int(this.row - i, this.col));
            }
        }

        private void CheckBotColumn(BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (this.col - i < 0) continue;
                var topColumnPiece = runtimePieces[this.row, this.col - i];
                if (topColumnPiece != null)
                {
                    availableMoves.Add(new Vector2Int(this.row, this.col - i));
                    break;
                }
                availableMoves.Add(new Vector2Int(this.row, this.col - i));
            }
        }

        private void CheckTopColumn(BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (this.col + i > 7) continue;
                var topColumnPiece = runtimePieces[this.row, this.col + i];
                if (topColumnPiece != null)
                {
                    availableMoves.Add(new Vector2Int(this.row, this.col + i));
                    break;
                }
                availableMoves.Add(new Vector2Int(this.row, this.col + i));
            }
        }

        public override void Attack(BaseChessPiece targetPiece)
        {
        }

        public override void PreMove(BaseChessPiece targetPiece)
        {
            
        }
    }
}