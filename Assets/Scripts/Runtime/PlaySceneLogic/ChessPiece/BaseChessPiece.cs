namespace Runtime.PlaySceneLogic.ChessPiece
{
    using System.Collections.Generic;
    using System.Linq;
    using DG.Tweening;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.PlaySceneLogic.Signal;
    using UnityEngine;
    using Zenject;

    public abstract class BaseChessPiece : MonoBehaviour
    {
        public PieceTeam team;
        public int       row;
        public int       col;
        public PieceType type;

        protected SignalBus              signalBus;
        protected BoardController        boardController;
        protected PieceRegularMoveHelper pieceRegularMoveHelper;
        protected ILogService            logService;
        
        [Inject]
        private void Init(SignalBus signal, BoardController controller, ILogService service, PieceRegularMoveHelper helper)
        {
            this.signalBus              = signal;
            this.boardController        = controller;
            this.logService             = service;
            this.pieceRegularMoveHelper = helper;
        }

        public abstract List<Vector2Int> GetAvailableMoves(BaseChessPiece[,] chessboard);
        public abstract List<Vector2Int> GetCheckMovesIndex(Vector2Int currentPieceIndex, List<Vector2Int> availableMoves, Vector2Int kingPieceIndex);
        public virtual void ReplaceData(int row, int col)
        {
            this.row = row;
            this.col = col;
        }

        public virtual void MoveTo(BaseChessPiece currentPiece, GameObject targetTile)
        {
            this.transform.DOMove(targetTile.transform.position, GameStaticValue.MoveDuration);
            this.logService.LogWithColor("Play move sound here", Color.yellow);
            var targetPiece    = this.boardController.GetPieceByIndex(this.boardController.GetTileIndex(targetTile));
            
            // Kill move
            if (targetPiece != null)
            {
                this.logService.LogWithColor("Play kill sound here", Color.yellow);
                targetPiece.Recycle();
            }
        }
    }
}