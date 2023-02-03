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

        public override List<Vector2Int> GetCheckMovesIndex(List<Vector2Int> availableMoves, Vector2Int kingPieceIndex)
        {
            var currentPieceIndex = new Vector2Int(this.row, this.col);
            if (kingPieceIndex.x < this.row && kingPieceIndex.y < this.col)
            {
                return this.pieceRegularMoveHelper.GetCheckMovesIndexBotLeftDiagonal(currentPieceIndex, availableMoves, kingPieceIndex);
            }

            if (kingPieceIndex.x > this.row && kingPieceIndex.y < this.col)
            {
                return this.pieceRegularMoveHelper.GetCheckMovesIndexBotRightDiagonal(currentPieceIndex, availableMoves, kingPieceIndex);
            }

            if (kingPieceIndex.x > this.row && kingPieceIndex.y > this.col)
            {
                return this.pieceRegularMoveHelper.GetCheckMovesIndexTopRightDiagonal(currentPieceIndex, availableMoves, kingPieceIndex);
            }

            if (kingPieceIndex.x < this.row && kingPieceIndex.y > this.col)
            {
                return this.pieceRegularMoveHelper.GetCheckMovesIndexTopLeftDiagonal(currentPieceIndex, availableMoves, kingPieceIndex);
            }

            return null;
        }
    }
}