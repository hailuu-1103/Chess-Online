namespace Runtime.PlaySceneLogic.ChessPiece.Piece
{
    using System.Collections.Generic;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using UnityEngine;

    public class Pawn : BaseChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves(BaseChessPiece[,] chessboard)
        {
            var availableMoves = new List<Vector2Int>();

            var direction = this.team == PieceTeam.White ? 1 : -1;

            // One in front
            if (chessboard[this.row, this.col + direction] == null)
            {
                availableMoves.Add(new Vector2Int(this.row, this.col + direction));
                // Two in front
                switch (this.team)
                {
                    // White team
                    case PieceTeam.White when this.col == 1 && chessboard[this.row, this.col + direction * 2] == null:
                    // Black team
                    case PieceTeam.Black when this.col == 6 && chessboard[this.row, this.col + direction * 2] == null:
                        availableMoves.Add(new Vector2Int(this.row, this.col + direction * 2));
                        break;
                }
            }

            // Kill move
            if (this.col != 7 && this.row + 1 <= 7)
            {
                if (chessboard[this.row + 1, this.col + direction] != null && chessboard[this.row + 1, this.col + direction].team != this.team)
                    availableMoves.Add(new Vector2Int(this.row + 1, this.col + direction));
            }

            if (this.col == 0 || this.row - 1 <= 0) return availableMoves;
            if (chessboard[this.row - 1, this.col + direction] != null && chessboard[this.row - 1, this.col + direction].team != this.team)
            {
                availableMoves.Add(new Vector2Int(this.row - 1, this.col + direction));
            }

            return availableMoves;
        }

        public override List<Vector2Int> GetCheckMovesIndex(Vector2Int currentPieceIndex, List<Vector2Int> availableMoves, Vector2Int kingPieceIndex)
        {
            var checkMovesIndex = new List<Vector2Int>();
            if (!availableMoves.Contains(kingPieceIndex)) return null;
            checkMovesIndex.Add(new Vector2Int(this.row, this.col));
            checkMovesIndex.Add(kingPieceIndex);
            return checkMovesIndex;
        }
    }
}