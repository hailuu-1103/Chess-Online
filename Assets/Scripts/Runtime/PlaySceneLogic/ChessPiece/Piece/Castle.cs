namespace Runtime.PlaySceneLogic.ChessPiece.Piece
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Castle : BaseChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves(BaseChessPiece[,] chessboard)
        {
            var availableMoves = new List<Vector2Int>();

            this.pieceRegularMoveHelper.CheckTopColumn(this.row, this.col, chessboard, availableMoves);

            this.pieceRegularMoveHelper.CheckBotColumn(this.row, this.col, chessboard, availableMoves);

            this.pieceRegularMoveHelper.CheckLeftRow(this.row, this.col, chessboard, availableMoves);

            this.pieceRegularMoveHelper.CheckRightRow(this.row, this.col, chessboard, availableMoves);
            return availableMoves;
        }

        public override List<Vector2Int> GetCheckMovesIndex(Vector2Int currentPieceIndex, List<Vector2Int> availableMoves, Vector2Int kingPieceIndex)
        {
            if (kingPieceIndex.y > this.col)
            {
                return this.pieceRegularMoveHelper.GetCheckMovesIndexTopColumn(currentPieceIndex, availableMoves, kingPieceIndex);
            }

            if (kingPieceIndex.y < this.col)
            {
                return this.pieceRegularMoveHelper.GetCheckMovesIndexBotColumn(currentPieceIndex, availableMoves, kingPieceIndex);
            }

            if (kingPieceIndex.x < this.row)
            {
                return this.pieceRegularMoveHelper.GetCheckMovesIndexLeftRow(currentPieceIndex, availableMoves, kingPieceIndex);
            }

            if (kingPieceIndex.x > this.row)
            {
                return this.pieceRegularMoveHelper.GetCheckMovesIndexRightRow(currentPieceIndex, availableMoves, kingPieceIndex);
            }

            return null;
        }
    }
}