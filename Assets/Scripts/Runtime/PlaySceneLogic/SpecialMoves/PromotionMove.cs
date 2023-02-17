namespace Runtime.PlaySceneLogic.SpecialMoves
{
    using System.Collections.Generic;
    using UnityEngine;

    public class PromotionMove : ISpecialMoves
    {
        public void Execute(Vector2Int currentPieceIndex, Vector2Int targetPieceIndex) { Debug.Log("Implement promotion.!"); }
    }
}