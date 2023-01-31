namespace Runtime.PlaySceneLogic.ChessPiece.Piece
{
    using System.Collections.Generic;
    using DG.Tweening;
    using UnityEngine;

    public class Bishop : BaseChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves()
        {
            return null;
        }

        public override void MoveTo(GameObject targetTile)
        {
            base.MoveTo(targetTile);
        }

        public override void Attack(BaseChessPiece targetPiece)
        {
            
        }
    }
}