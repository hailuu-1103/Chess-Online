namespace Runtime.PlaySceneLogic.ChessPiece
{
    using System.Collections.Generic;
    using DG.Tweening;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.PlaySceneLogic.SpecialMoves;
    using UnityEngine;
    using Zenject;

    public abstract class BaseChessPiece : MonoBehaviour
    {
        public PieceTeam team;
        public int       row;
        public int       col;
        public PieceType type;

        #region inject

        protected SignalBus              signalBus;
        protected BoardController        boardController;
        protected PieceRegularMoveHelper pieceRegularMoveHelper;
        protected ILogService            logService;
        protected ISpecialMoves          specialMoves;
        protected IScreenManager         screenManager;
        protected PieceSpawnerService    pieceSpawnerService;

        #endregion

        [Inject]
        private void Init(SignalBus signal, BoardController controller, ILogService service, ISpecialMoves special, IScreenManager screen, PieceRegularMoveHelper helper,
            PieceSpawnerService pieceSpawner)
        {
            this.signalBus              = signal;
            this.boardController        = controller;
            this.logService             = service;
            this.pieceRegularMoveHelper = helper;
            this.specialMoves           = special;
            this.screenManager          = screen;
            this.pieceSpawnerService    = pieceSpawner;
            this.OnInit();
        }

        public virtual  void             OnInit() { }
        public abstract List<Vector2Int> GetAvailableMoves(BaseChessPiece[,] chessboard);
        public abstract List<Vector2Int> GetCheckMovesIndex(Vector2Int currentPieceIndex, List<Vector2Int> availableMoves, Vector2Int kingPieceIndex);

        public virtual void ReplaceData(int row, int col)
        {
            this.row = row;
            this.col = col;
        }

        public virtual void PerformNormalMove(Vector2Int currentPieceIndex, GameObject targetTile)
        {
            this.transform.DOMove(targetTile.transform.position, GameStaticValue.MoveDuration);
            this.logService.LogWithColor("Play move sound here", Color.yellow);
            var targetTileIndex = this.boardController.GetTileIndex(targetTile);
            this.ReplaceData(targetTileIndex.x, targetTileIndex.y);
            var targetPiece = this.boardController.GetPieceByIndex(this.boardController.GetTileIndex(targetTile));

            // Kill move
            if (targetPiece != null)
            {
                this.logService.LogWithColor("Play kill sound here", Color.yellow);
                targetPiece.Recycle();
            }

            this.boardController.MoveList.Add(new[]
                { new Vector2Int(currentPieceIndex.x, currentPieceIndex.y), new Vector2Int(this.boardController.GetTileIndex(targetTile).x, this.boardController.GetTileIndex(targetTile).y) });
            this.boardController.ChessMoveList.Add((team, type));
        }

        public virtual SpecialMoveType GetSpecialMoveType(BaseChessPiece currentPiece, ref List<Vector2Int> availableMoves, Vector2Int targetTileIndex) { return SpecialMoveType.None; }

        public virtual void PerformSpecialMove(Vector2Int currentPieceIndex, Vector2Int targetPieceIndex)
        {
            this.specialMoves.Execute(currentPieceIndex, targetPieceIndex);
        }
    }
}