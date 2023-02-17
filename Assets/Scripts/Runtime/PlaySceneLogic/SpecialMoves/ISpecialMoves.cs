namespace Runtime.PlaySceneLogic.SpecialMoves
{
    using System.Collections.Generic;
    using UnityEngine;

    public interface ISpecialMoves
    {
        void Execute(Vector2Int currentPieceIndex, Vector2Int targetPieceIndex);
    }
}