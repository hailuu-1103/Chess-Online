namespace Runtime.PlaySceneLogic.ChessPiece.Piece
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Pawn : BaseChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves()
        {
            var runtimePieces  = this.boardController.runtimePieces;
            var availableMoves = new List<Vector2Int>();

            var direction = this.Team == PieceTeam.White ? 1 : -1;

            // One in front
            if (runtimePieces[this.Row + direction, this.Col] == null)
            {
                availableMoves.Add(new Vector2Int(this.Row + direction, this.Col));
            }

            // Two in front
            switch (this.Team)
            {
                // White team
                case PieceTeam.White when this.Row == 1 && runtimePieces[this.Row + direction * 2, this.Col] == null:
                // Black team
                case PieceTeam.Black when this.Row == 6 && runtimePieces[this.Row + direction * 2, this.Col] == null:
                    availableMoves.Add(new Vector2Int(this.Row + direction * 2, this.Col));
                    break;
            }

            // Kill move
            if (this.Row != 7 && this.Col + 1 <= 7)
            {
                if (runtimePieces[this.Row + direction, this.Col + 1] != null && runtimePieces[this.Row + direction, this.Col + 1].Team != this.Team)
                    availableMoves.Add(new Vector2Int(this.Row + direction, this.Col + 1));
            }

            if (this.Row == 0 && this.Col - 1 <= 0) return availableMoves;
            if (runtimePieces[this.Row + direction, this.Col - 1] != null && runtimePieces[this.Row + direction, this.Col - 1].Team != this.Team)
            {
                availableMoves.Add(new Vector2Int(this.Row + direction, this.Col - 1));
            }

            return availableMoves;
        }

        public override void MoveTo(GameObject targetTile)
        {
            base.MoveTo(targetTile);
            // var targetPiece = this.boardController.GetPieceByIndex(this.boardController.GetTileIndex(targetTile));
            // if (this.Team == PieceTeam.White && targetPiece.Row == 7)
            // {
            //     this.logService.LogWithColor("Implement white pawn promotion here", Color.yellow);
            // } 
            // if (this.Team == PieceTeam.Black && targetPiece.Row == 0)
            // {
            //     this.logService.LogWithColor("Implement black pawn promotion here", Color.yellow);
            // } 
        }

        public override void Attack(BaseChessPiece targetPiece) { this.logService.LogWithColor($"Attack on {targetPiece.Type}", Color.blue); }
    }
}