namespace Runtime.PlaySceneLogic.Signal
{
    using System.Collections.Generic;
    using Runtime.PlaySceneLogic.ChessPiece;
    using UnityEngine;

    public class OnCheckSignal
    {
        public BaseChessPiece   CheckPiece      { get; set; }
        public List<Vector2Int> CheckMovesIndex { get; set; }

        public OnCheckSignal(BaseChessPiece checkPiece, List<Vector2Int> checkMovesIndex)
        {
            this.CheckPiece      = checkPiece;
            this.CheckMovesIndex = checkMovesIndex;
        }
    }

    public class CheckPiece
    {
        public string PieceId;
        public int    Row;
        public int    Col;
    }
}