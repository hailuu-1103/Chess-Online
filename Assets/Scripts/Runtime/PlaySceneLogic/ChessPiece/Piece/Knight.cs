namespace Runtime.PlaySceneLogic.ChessPiece.Piece
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Knight : BaseChessPiece
    {
        private int[,] moves = {{2,1}, {1,2}, {-1,2}, {-2,1}, {-2,-1}, {-1,-2}, {1,-2}, {2,-1}};
        public override List<Vector2Int> GetAvailableMoves()
        {
            var runtimePieces  = this.boardController.runtimePieces;
            var availableMoves = new List<Vector2Int>();
            for (var i = 0; i < GameStaticValue.BoardRows; i++)
            {
                var newX = this.Row + this.moves[i, 0];
                var newY = this.Col + this.moves[i, 1];
                if(newX < 0 || newY < 0) continue;
                if (runtimePieces[newX, newY] != null && runtimePieces[newX, newY].Team == this.Team)
                {
                    this.logService.LogWithColor("Implement pre-move here", Color.green);
                    continue;
                }
                availableMoves.Add(new Vector2Int(newX, newY));
            }
            return availableMoves;
        }

        public override void Attack(BaseChessPiece targetPiece)
        {
        }
    }
}