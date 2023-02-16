namespace Runtime.PlaySceneLogic.ChessPiece.Piece
{
    using System.Collections.Generic;
    using Runtime.PlaySceneLogic.SpecialMoves;
    using UnityEngine;

    public class King : BaseChessPiece
    {
        private readonly int[,] moves = { { 0, 1 }, { 1, 1 }, { 1, 0 }, { 1, -1 }, { 0, -1 }, { -1, 1 }, { -1, 0 }, { -1, -1 } };

        public override void OnInit()
        {
            base.OnInit();
            this.specialMoves = new CastlingMove();
        }

        public override List<Vector2Int> GetAvailableMoves(BaseChessPiece[,] chessboard)
        {
            var availableMoves = new List<Vector2Int>();
            for (var i = 0; i < 8; i++)
            {
                var newX = this.row + this.moves[i, 0];
                var newY = this.col + this.moves[i, 1];
                if (newX < 0 || newY < 0 || newX > 7 || newY > 7) continue;
                availableMoves.Add(new Vector2Int(newX, newY));
            }

            return availableMoves;
        }

        public override List<Vector2Int> GetCheckMovesIndex(Vector2Int currentPieceIndex, List<Vector2Int> availableMoves, Vector2Int kingPieceIndex) { return null; }

        public override SpecialMoveType GetSpecialMoveType(BaseChessPiece currentPiece, ref List<Vector2Int> availableMoves, Vector2Int targetTileIndex)
        {
            var runtimePieces = this.boardController.RuntimePieces;
            var moveList      = this.boardController.MoveList;
            var kingMove      = moveList.Find(move => move[0].x == 4 && move[0].y == (this.team == PieceTeam.White ? 0 : 7));
            var leftCastleMove      = moveList.Find(move => move[0].x == 0 && move[0].y == (this.team == PieceTeam.White ? 0 : 7));
            var rightCastleMove      = moveList.Find(move => move[0].x == 7 && move[0].y == (this.team == PieceTeam.White ? 0 : 7));
            if (this.team == PieceTeam.White)
            {
                switch (kingMove)
                {
                    case null when leftCastleMove == null && runtimePieces[3, 0] == null && runtimePieces[2, 0] == null && runtimePieces[1, 0] == null:
                        availableMoves.Add(new Vector2Int(2, 0));
                        return SpecialMoveType.Castling;
                    case null when rightCastleMove == null && runtimePieces[5, 0] == null && runtimePieces[6, 0] == null:
                        availableMoves.Add(new Vector2Int(6, 0));
                        return SpecialMoveType.Castling;
                }
            }
            else
            {
                switch (kingMove)
                {
                    case null when leftCastleMove == null && runtimePieces[3, 7] == null && runtimePieces[2, 7] == null && runtimePieces[1, 7] == null:
                        availableMoves.Add(new Vector2Int(2, 7));
                        return SpecialMoveType.Castling;
                    case null when rightCastleMove == null && runtimePieces[5, 7] == null && runtimePieces[6, 7] == null:
                        availableMoves.Add(new Vector2Int(6, 7));
                        return SpecialMoveType.Castling;
                }
            }
            
            return SpecialMoveType.None;
        }
    }
}