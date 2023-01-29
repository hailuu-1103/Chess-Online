namespace Runtime.PlaySceneLogic.ChessPiece.Piece
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Knight : BaseChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves()
        {
            return null;
        }

        public override void             MoveTo(BaseChessPiece targetPiece) { throw new System.NotImplementedException(); }
    }
}