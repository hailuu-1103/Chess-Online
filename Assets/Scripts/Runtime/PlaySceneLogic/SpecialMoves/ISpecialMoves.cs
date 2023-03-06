namespace Runtime.PlaySceneLogic.SpecialMoves
{
    using Runtime.PlaySceneLogic.ChessPiece;
    using UnityEngine;

    public interface ISpecialMoves
    {
        void Execute(Vector2Int currentPieceIndex, Vector2Int targetPieceIndex);
    }
}