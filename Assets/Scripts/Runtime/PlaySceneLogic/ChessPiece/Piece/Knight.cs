namespace Runtime.PlaySceneLogic.ChessPiece.Piece
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Knight : BaseChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves()
        {
            var runtimePieces = this.boardController.runtimePieces;
            return null;
        }

        public override void Attack(BaseChessPiece targetPiece)
        {
        }
    }
}