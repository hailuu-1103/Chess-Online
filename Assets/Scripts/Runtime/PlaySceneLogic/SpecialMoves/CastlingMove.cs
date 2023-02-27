namespace Runtime.PlaySceneLogic.SpecialMoves
{
    using System;
    using DG.Tweening;
    using UnityEngine;

    public class CastlingMove : ISpecialMoves
    {
        private readonly BoardController boardController;

        public CastlingMove(BoardController boardController) { this.boardController = boardController; }

        public void Execute(Vector2Int currentPieceIndex, Vector2Int targetPieceIndex)
        {
            var runtimePieces = this.boardController.RuntimePieces;
            var kingY         = currentPieceIndex.y;
            var rookX         = targetPieceIndex.x == 2 ? 0 : 7;
            var newKingX      = targetPieceIndex.x == 2 ? 2 : 6;
            var newRookX      = targetPieceIndex.x == 2 ? 3 : 5;

            var rookPiece = runtimePieces[rookX, kingY];
            var kingPiece = runtimePieces[currentPieceIndex.x, kingY];

            if (rookPiece == null || kingPiece == null)
            {
                throw new Exception("Invalid castling");
            }

            runtimePieces[rookX, kingY]               = null;
            runtimePieces[newKingX, kingY]            = kingPiece;
            runtimePieces[newRookX, kingY]            = rookPiece;
            runtimePieces[currentPieceIndex.x, kingY] = null;

            rookPiece.ReplaceData(newRookX, kingY);
            kingPiece.ReplaceData(newKingX, kingY);

            var rookTargetIndex = new Vector2Int(newRookX, kingY);
            var kingTargetIndex = new Vector2Int(newKingX, kingY);

            rookPiece.transform.DOMove(this.boardController.GetTileByIndex(rookTargetIndex).transform.position, GameStaticValue.MoveDuration);
            kingPiece.transform.DOMove(this.boardController.GetTileByIndex(kingTargetIndex).transform.position, GameStaticValue.MoveDuration);
        }

    }
}