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

        public override List<Vector2Int> GetCheckMovesIndex(List<Vector2Int> availableMovesIndex, Vector2Int kingPieceIndex)
        {
            var currentPieceIndex = new Vector2Int(this.row, this.col);
            if (kingPieceIndex.x < this.row && kingPieceIndex.y < this.col)
            {
                return this.pieceRegularMoveHelper.GetCheckMovesIndexBotLeftDiagonal(currentPieceIndex, availableMovesIndex, kingPieceIndex);
            }

            if (kingPieceIndex.x > this.row && kingPieceIndex.y < this.col)
            {
                return this.pieceRegularMoveHelper.GetCheckMovesIndexBotRightDiagonal(currentPieceIndex, availableMovesIndex, kingPieceIndex);
            }

            if (kingPieceIndex.x > this.row && kingPieceIndex.y > this.col)
            {
                return this.pieceRegularMoveHelper.GetCheckMovesIndexTopRightDiagonal(currentPieceIndex, availableMovesIndex, kingPieceIndex);
            }

            if (kingPieceIndex.x < this.row && kingPieceIndex.y > this.col)
            {
                return this.pieceRegularMoveHelper.GetCheckMovesIndexTopLeftDiagonal(currentPieceIndex, availableMovesIndex, kingPieceIndex);
            }
            
            if (kingPieceIndex.x < this.row && kingPieceIndex.y == this.col)
            {
                return this.pieceRegularMoveHelper.GetCheckMovesIndexLeftRow(currentPieceIndex, availableMovesIndex, kingPieceIndex);
            }

            if (kingPieceIndex.x > this.row && kingPieceIndex.y == this.col)
            {
                return this.pieceRegularMoveHelper.GetCheckMovesIndexRightRow(currentPieceIndex, availableMovesIndex, kingPieceIndex);
            }

            if (kingPieceIndex.x == this.row && kingPieceIndex.y > this.col)
            {
                return this.pieceRegularMoveHelper.GetCheckMovesIndexTopColumn(currentPieceIndex, availableMovesIndex, kingPieceIndex);
            }

            if (kingPieceIndex.x == this.row && kingPieceIndex.y < this.col)
            {
                return this.pieceRegularMoveHelper.GetCheckMovesIndexBotColumn(currentPieceIndex, availableMovesIndex, kingPieceIndex);
            }
            
            return null;
            
        }
    }
}