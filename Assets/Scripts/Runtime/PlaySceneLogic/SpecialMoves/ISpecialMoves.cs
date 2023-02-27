namespace Runtime.PlaySceneLogic.SpecialMoves
{
    using UnityEngine;

    public interface ISpecialMoves
    {
        void Execute(Vector2Int currentPieceIndex, Vector2Int targetPieceIndex);
    }
}