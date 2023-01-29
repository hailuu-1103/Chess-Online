namespace Runtime.PlaySceneLogic.ChessPiece.Piece
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class Pawn : BaseChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves()
        {
            var runtimePieces  = this.boardController.runtimePieces;
            var availableMoves = new List<Vector2Int>();

            if (this.Team == PieceTeam.White)
            {
                if (runtimePieces[this.Row + 1, this.Col] != null || this.Row + 1 > 7) return availableMoves;
                availableMoves.Add(new Vector2Int(this.Row + 1, this.Col));
                if (runtimePieces[this.Row + 2, this.Col] == null && this.Row + 2 <= 7)
                {
                    availableMoves.Add(new Vector2Int(this.Row + 2, this.Col));
                }
            }
            else
            {
                if (runtimePieces[this.Row - 1, this.Col] != null || this.Row - 1 < 0) return availableMoves;
                availableMoves.Add(new Vector2Int(this.Row - 1, this.Col));
                if (runtimePieces[this.Row - 2, this.Col] == null && this.Row - 2 >= 0)
                {
                    availableMoves.Add(new Vector2Int(this.Row - 2, this.Col));
                }
            }
           

            return availableMoves;
        }

        public override void MoveTo(BaseChessPiece targetPiece)
        {
            var availableMoves = this.GetAvailableMoves();
            var tiles          = availableMoves.Select(moveIndex => this.boardController.GetTileByIndex(moveIndex)).ToArray();
        }

        public override void Attack(BaseChessPiece targetPiece) { base.Attack(targetPiece); }
    }
}