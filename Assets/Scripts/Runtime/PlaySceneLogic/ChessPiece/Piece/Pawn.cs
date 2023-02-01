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

            var direction = this.team == PieceTeam.White ? 1 : -1;

            // One in front
            if (runtimePieces[this.row, this.col + direction] == null)
            {
                availableMoves.Add(new Vector2Int(this.row, this.col + direction));
            }

            // Two in front
            switch (this.team)
            {
                // White team
                case PieceTeam.White when this.col == 1 && runtimePieces[this.row, this.col + direction * 2] == null:
                // Black team
                case PieceTeam.Black when this.col == 6 && runtimePieces[this.row, this.col + direction * 2] == null:
                    availableMoves.Add(new Vector2Int(this.row, this.col + direction * 2));
                    break;
            }

            // Kill move
            if (this.col != 7 && this.row + 1 <= 7)
            {
                if (runtimePieces[this.row + 1, this.col + direction] != null && runtimePieces[this.row + 1, this.col + direction].team != this.team)
                    availableMoves.Add(new Vector2Int(this.row + 1, this.col + direction));
            }

            if (this.col == 0 || this.row - 1 <= 0) return availableMoves;
            if (runtimePieces[this.row - 1, this.col + direction] != null && runtimePieces[this.row - 1, this.col + direction].team != this.team)
            {
                availableMoves.Add(new Vector2Int(this.row - 1, this.col + direction));
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

        public override void Attack(BaseChessPiece targetPiece)  { this.logService.LogWithColor($"Attack on {targetPiece.type}", Color.blue); }

        public override void PreMove(BaseChessPiece targetPiece)
        {
            
        }
    }
}