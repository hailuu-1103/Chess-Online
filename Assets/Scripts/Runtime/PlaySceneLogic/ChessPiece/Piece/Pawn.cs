namespace Runtime.PlaySceneLogic.ChessPiece.Piece
{
    using System;
    using System.Collections.Generic;
    using Runtime.PlaySceneLogic.SpecialMoves;
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

        public override SpecialMoveType GetSpecialMoveType(BaseChessPiece currentPiece, ref List<Vector2Int> availableMoves, Vector2Int targetTileIndex)
        {
            var specialMoveType   = SpecialMoveType.None;
            var moveList          = this.boardController.MoveList;
            var direction         = this.team == PieceTeam.White ? 1 : -1;
            var currentPieceIndex = new Vector2Int(this.row, this.col);

            if (moveList.Count > 0 && this.boardController.RuntimePieces[moveList[^1][1].x, moveList[^1][1].y].type == PieceType.Pawn)
            {
                var lastMove   = moveList[^1];
                var enemyPiece = this.boardController.RuntimePieces[lastMove[1].x, lastMove[1].y];

                if (Math.Abs(lastMove[0].y - lastMove[1].y) == 2 && enemyPiece.team != this.team && lastMove[1].y == this.col)
                {
                    if (lastMove[1].x == this.row - 1 && targetTileIndex.x == this.row - 1)
                    {
                        availableMoves.Add(new Vector2Int(this.row - 1, this.col + direction));
                        this.specialMoves = new EnPassantMove(this.boardController);
                        specialMoveType   = SpecialMoveType.EnPassant;
                    }

                    if (lastMove[1].x == this.row + 1 && targetTileIndex.x == this.row + 1)
                    {
                        availableMoves.Add(new Vector2Int(this.row + 1, this.col + direction));
                        this.specialMoves = new EnPassantMove(this.boardController);
                        specialMoveType   = SpecialMoveType.EnPassant;
                    }
                }
            }

            if ((this.team == PieceTeam.White && currentPieceIndex.y == 6 && targetTileIndex.y == 7) ||
                (this.team == PieceTeam.Black && currentPieceIndex.y == 1 && targetTileIndex.y == 0))
            {
                availableMoves.Add(new Vector2Int(currentPieceIndex.x, this.team == PieceTeam.White ? 7 : 0));
                this.specialMoves = new PromotionMove(this.screenManager, this.boardController, this.pieceSpawnerService);
                specialMoveType   = SpecialMoveType.Promotion;
            }

            this.logService.LogWithColor($"Special Move Type: {specialMoveType}", Color.cyan);
            return specialMoveType;
        }

        public override void PerformSpecialMove(Vector2Int currentPieceIndex, Vector2Int targetPieceIndex)
        {
            if (this.specialMoves is PromotionMove)
            {
            }            
            base.PerformSpecialMove(currentPieceIndex, targetPieceIndex);
        }
    }
}