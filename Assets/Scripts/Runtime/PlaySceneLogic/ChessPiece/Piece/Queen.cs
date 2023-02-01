namespace Runtime.PlaySceneLogic.ChessPiece.Piece
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Queen : BaseChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves()
        {
            var runtimePieces  = this.boardController.runtimePieces;
            var availableMoves = new List<Vector2Int>();
            
            this.pieceRegularMoveHelper.CheckTopColumn(this.row, this.col, runtimePieces, availableMoves);
            
            this.pieceRegularMoveHelper.CheckBotColumn(this.row, this.col, runtimePieces, availableMoves);
            
            this.pieceRegularMoveHelper.CheckLeftRow(this.row, this.col, runtimePieces, availableMoves);
            
            this.pieceRegularMoveHelper.CheckRightRow(this.row, this.col, runtimePieces, availableMoves);
            
            this.pieceRegularMoveHelper.CheckTopRightDiagonal(this.row, this.col, runtimePieces, availableMoves);
            
            this.pieceRegularMoveHelper.CheckTopLeftDiagonal(this.row, this.col, runtimePieces, availableMoves);
            
            this.pieceRegularMoveHelper.CheckBotRightDiagonal(this.row, this.col, runtimePieces, availableMoves);
            
            this.pieceRegularMoveHelper.CheckBotLeftDiagonal(this.row, this.col, runtimePieces, availableMoves);
            
            return availableMoves;
        }
        public override void Attack(BaseChessPiece targetPiece)
        {
            
        }

        public override void PreMove(BaseChessPiece targetPiece)
        {
            
        }
    }
}