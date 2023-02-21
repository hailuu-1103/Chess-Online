namespace Runtime.PlaySceneLogic.SpecialMoves
{
    using System.Collections;
    using System.Threading.Tasks;
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public interface ISpecialMoves
    {
        void Execute(Vector2Int currentPieceIndex, Vector2Int targetPieceIndex);
    }
}