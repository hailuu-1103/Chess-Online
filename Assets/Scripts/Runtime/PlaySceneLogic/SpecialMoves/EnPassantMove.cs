namespace Runtime.PlaySceneLogic.SpecialMoves
{
    using DG.Tweening;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.PlaySceneLogic.ChessPiece;
    using UnityEngine;

    public class EnPassantMove : ISpecialMoves
    {
        private readonly BoardController boardController;
        public EnPassantMove(BoardController boardController) { this.boardController = boardController; }

        public void Execute(Vector2Int currentPieceIndex, Vector2Int targetPieceIndex)
        {
            var targetTile   = this.boardController.GetTileByIndex(targetPieceIndex);
            var currentPiece = this.boardController.GetPieceByIndex(currentPieceIndex);
            var direction    = currentPiece.team == PieceTeam.White ? 1 : -1;
            var targetPawn   = this.boardController.GetPieceByIndex(new Vector2Int(targetPieceIndex.x, targetPieceIndex.y - direction));
            currentPiece.transform.DOMove(targetTile.transform.position, GameStaticValue.MoveDuration);
            currentPiece.ReplaceData(targetPieceIndex.x, targetPieceIndex.y);
            this.boardController.RuntimePieces[targetPawn.row, targetPawn.col] = null;

            this.boardController.MoveList.Add(new[]{ 
                new Vector2Int(currentPieceIndex.x, currentPieceIndex.y), 
                new Vector2Int(this.boardController.GetTileIndex(targetTile).x, this.boardController.GetTileIndex(targetTile).y) 
            });
            this.boardController.ChessMoveList.Add((currentPiece.team, PieceType.Pawn));

            if (targetPawn != null)
            {
                targetPawn.Recycle();
            }
        }
    }
}