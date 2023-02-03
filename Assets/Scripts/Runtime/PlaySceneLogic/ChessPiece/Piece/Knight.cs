namespace Runtime.PlaySceneLogic.ChessPiece.Piece
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Knight : BaseChessPiece
    {
        private readonly int[,] moves = {{2,1}, {1,2}, {-1,2}, {-2,1}, {-2,-1}, {-1,-2}, {1,-2}, {2,-1}};
        public override List<Vector2Int> GetAvailableMoves()
        {
            var runtimePieces  = this.boardController.runtimePieces;
            var availableMoves = new List<Vector2Int>();
            for (var i = 0; i < GameStaticValue.BoardRows; i++)
            {
                var newX = this.row + this.moves[i, 0];
                var newY = this.col + this.moves[i, 1];
                if(newX < 0 || newY < 0 || newX > 7 || newY > 7) continue;
                availableMoves.Add(new Vector2Int(newX, newY));
            }
            return availableMoves;
        }

        public override List<Vector2Int> GetCheckMovesIndex(List<Vector2Int> availableMoves, Vector2Int kingPieceIndex)
        {
            return null;
        }
    }
}