namespace Runtime.PlaySceneLogic.ChessPiece
{
    using UnityEngine;

    public abstract class BaseChessPiece : MonoBehaviour
    {
        public PieceTeam Team;
        public int            Row;
        public int            Col;
        public PieceType Type;

        public abstract void MoveTo();

    }
}