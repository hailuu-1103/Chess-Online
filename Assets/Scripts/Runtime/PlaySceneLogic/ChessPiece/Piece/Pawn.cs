namespace Runtime.PlaySceneLogic.ChessPiece.Piece
{
    using UnityEngine;

    public class Pawn : BaseChessPiece
    {
        public override void MoveTo()
        {
            Debug.Log("Pawn move.");
        }
    }
}