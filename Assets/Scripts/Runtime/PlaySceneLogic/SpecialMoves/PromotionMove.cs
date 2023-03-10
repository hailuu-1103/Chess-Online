namespace Runtime.PlaySceneLogic.SpecialMoves
{
    using DG.Tweening;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.PlaySceneLogic.ChessPiece;
    using Runtime.UI;
    using UnityEngine;
    using Zenject;

    public class PromotionMove : ISpecialMoves
    {
        #region inject

        private readonly IScreenManager      screenManager;
        private readonly DiContainer         diContainer;
        private readonly BoardController     boardController;
        private readonly PieceSpawnerService pieceSpawnerService;
        private readonly ObjectPoolManager   objectPoolManager;
        private          Transform           pieceHolder;

        #endregion

        private Vector2Int currentPieceIndex;
        private Vector2Int targetPieceIndex;

        public PromotionMove(IScreenManager screenManager, Transform pieceHolder, BoardController boardController, PieceSpawnerService pieceSpawnerService)
        {
            this.screenManager       = screenManager;
            this.pieceHolder         = pieceHolder;
            this.boardController     = boardController;
            this.pieceSpawnerService = pieceSpawnerService;
        }

        public async void Execute(Vector2Int currentPieceIndex, Vector2Int targetPieceIndex)
        {
            this.currentPieceIndex = currentPieceIndex;
            this.targetPieceIndex  = targetPieceIndex;
            var currentPiece = this.boardController.GetPieceByIndex(this.currentPieceIndex);
            var targetTile   = this.boardController.GetTileByIndex(this.targetPieceIndex);
            currentPiece.transform.DOMove(targetTile.transform.position, GameStaticValue.MoveDuration);
            var targetPiece = this.boardController.GetPieceByIndex(this.targetPieceIndex);
            if (currentPiece != null) currentPiece.Recycle();
            if (targetPiece != null) targetPiece.Recycle();
            var currentTeam = this.boardController.GetPieceByIndex(this.currentPieceIndex).team;
            await this.screenManager.OpenScreen<PromotionPopUpPresenter, PromotionPopUpModel>(new PromotionPopUpModel(this.SpawnPromotionPiece, currentTeam));
            this.boardController.MoveList.Add(new[]
            {
                new Vector2Int(currentPieceIndex.x, currentPieceIndex.y),
                new Vector2Int(targetPieceIndex.x, targetPieceIndex.y)
            });
            this.boardController.ChessMoveList.Add((currentPiece.team, PieceType.Pawn));
        }

        private async void SpawnPromotionPiece(PieceTeam pieceTeam, PieceType pieceType)
        {
            var chessPieces = await this.pieceSpawnerService.SpawnSinglePiece(this.pieceHolder, pieceType, pieceTeam, this.targetPieceIndex.y, this.targetPieceIndex.x);
            this.boardController.RuntimePieces[this.targetPieceIndex.x, this.targetPieceIndex.y] = chessPieces;
        }
    }
}