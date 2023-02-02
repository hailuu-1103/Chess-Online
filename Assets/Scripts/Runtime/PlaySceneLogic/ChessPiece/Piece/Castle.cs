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
            
            this.pieceRegularMoveHelper.CheckTopColumn(this.row, this.col, runtimePieces, availableMoves);
            
            this.pieceRegularMoveHelper.CheckBotColumn(this.row, this.col, runtimePieces, availableMoves);
            
            this.pieceRegularMoveHelper.CheckLeftRow(this.row, this.col, runtimePieces, availableMoves);
            
            this.pieceRegularMoveHelper.CheckRightRow(this.row, this.col, runtimePieces, availableMoves);
            return availableMoves;
        }
        
        public override void PreMove(BaseChessPiece targetPiece)
        {
            
        }
    }
}