namespace Runtime.PlaySceneLogic.ChessPiece.Piece
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Bishop : BaseChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves(BaseChessPiece[,] chessboard)
        {
            var availableMoves = new List<Vector2Int>();
            // this.pieceRegularMoveHelper.CheckTopRightDiagonal(this.row, this.col, chessboard, availableMoves);
            //
            // this.pieceRegularMoveHelper.CheckTopLeftDiagonal(this.row, this.col, chessboard, availableMoves);
            //
            // this.pieceRegularMoveHelper.CheckBotRightDiagonal(this.row, this.col, chessboard, availableMoves);
            //
            // this.pieceRegularMoveHelper.CheckBotLeftDiagonal(this.row, this.col, chessboard, availableMoves);
            
            this.CheckDiagonalMoves(this.row, this.col, chessboard, availableMoves);
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

            return null;
        }
        
        private void CheckDiagonalMoves(int row, int col, BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            int[] directions = { -1, 1 };
    
            foreach (int rowDir in directions)
            {
                foreach (int colDir in directions)
                {
                    int r = row + rowDir;
                    int c = col + colDir;
            
                    while (r >= 0 && r < GameStaticValue.BoardRows && c >= 0 && c < GameStaticValue.BoardColumn)
                    {
                        var piece = runtimePieces[r, c];
                        if (piece == null)
                        {
                            availableMoves.Add(new Vector2Int(r, c));
                        }
                        else
                        {
                            if (piece.team != runtimePieces[row, col].team)
                            {
                                availableMoves.Add(new Vector2Int(r, c));
                            }
                            break;
                        }
                        r += rowDir;
                        c += colDir;
                    }
                }
            }
        }
    }
}