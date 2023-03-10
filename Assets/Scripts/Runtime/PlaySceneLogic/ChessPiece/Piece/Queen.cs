namespace Runtime.PlaySceneLogic.ChessPiece.Piece
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Queen : BaseChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves(BaseChessPiece[,] chessboard)
        {
            var availableMoves = new List<Vector2Int>();

            this.pieceRegularMoveHelper.CheckTopColumn(this.row, this.col, chessboard, availableMoves);

            this.pieceRegularMoveHelper.CheckBotColumn(this.row, this.col, chessboard, availableMoves);

            this.pieceRegularMoveHelper.CheckLeftRow(this.row, this.col, chessboard, availableMoves);

            this.pieceRegularMoveHelper.CheckRightRow(this.row, this.col, chessboard, availableMoves);

            this.pieceRegularMoveHelper.CheckTopRightDiagonal(this.row, this.col, chessboard, availableMoves);

            this.pieceRegularMoveHelper.CheckTopLeftDiagonal(this.row, this.col, chessboard, availableMoves);

            this.pieceRegularMoveHelper.CheckBotRightDiagonal(this.row, this.col, chessboard, availableMoves);

            this.pieceRegularMoveHelper.CheckBotLeftDiagonal(this.row, this.col, chessboard, availableMoves);

            return availableMoves;
        }

        public override List<Vector2Int> GetCheckMovesIndex(Vector2Int currentPieceIndex, List<Vector2Int> availableMoves, Vector2Int kingPieceIndex)
        {
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

            if (kingPieceIndex.x < this.row && kingPieceIndex.y == this.col)
            {
                return this.pieceRegularMoveHelper.GetCheckMovesIndexLeftRow(currentPieceIndex, availableMoves, kingPieceIndex);
            }

            if (kingPieceIndex.x > this.row && kingPieceIndex.y == this.col)
            {
                return this.pieceRegularMoveHelper.GetCheckMovesIndexRightRow(currentPieceIndex, availableMoves, kingPieceIndex);
            }

            if (kingPieceIndex.x == this.row && kingPieceIndex.y > this.col)
            {
                return this.pieceRegularMoveHelper.GetCheckMovesIndexTopColumn(currentPieceIndex, availableMoves, kingPieceIndex);
            }

            if (kingPieceIndex.x == this.row && kingPieceIndex.y < this.col)
            {
                return this.pieceRegularMoveHelper.GetCheckMovesIndexBotColumn(currentPieceIndex, availableMoves, kingPieceIndex);
            }

            return null;
        }
    }
}