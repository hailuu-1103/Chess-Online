namespace Runtime.PlaySceneLogic.SpecialMoves
{
    using System.Collections.Generic;
    using UnityEngine;

    public class EnPassantMove : ISpecialMoves
    {
        public void Execute(Vector2Int currentPieceIndex, Vector2Int targetPieceIndex)
        {
            Debug.Log("Implement En Passant.!");

        }
    }
}