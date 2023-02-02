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
            this.pieceRegularMoveHelper.CheckTopRightDiagonal(this.row, this.col, runtimePieces, availableMoves);
            
            this.pieceRegularMoveHelper.CheckTopLeftDiagonal(this.row, this.col, runtimePieces, availableMoves);
            
            this.pieceRegularMoveHelper.CheckBotRightDiagonal(this.row, this.col, runtimePieces, availableMoves);
            
            this.pieceRegularMoveHelper.CheckBotLeftDiagonal(this.row, this.col, runtimePieces, availableMoves);
            return availableMoves;
        }

        public override void PreMove(BaseChessPiece targetPiece) { }
    }
}