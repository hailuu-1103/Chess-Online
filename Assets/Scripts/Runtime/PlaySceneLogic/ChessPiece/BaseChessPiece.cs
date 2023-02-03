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

        public abstract List<Vector2Int> GetAvailableMoves();
        public abstract List<Vector2Int> GetCheckMovesIndex(List<Vector2Int> availableMoves, Vector2Int kingPieceIndex);
        public virtual void ReplaceData(int row, int col)
        {
            this.row = row;
            this.col = col;
        }

        public virtual void MoveTo(BaseChessPiece currentPiece, GameObject targetTile)
        {
            this.transform.DOMove(targetTile.transform.position, GameStaticValue.MoveDuration);
            var targetTileIndex       = this.boardController.GetTileIndex(targetTile);
            currentPiece.ReplaceData(targetTileIndex.x,targetTileIndex.y);
            var availableMoves = currentPiece.GetAvailableMoves();
            if (currentPiece.team == PieceTeam.White)
            {
                this.boardController.CheckPieceToCheckMovesIndex.Clear();
                this.boardController.onCheckMovesIndex.Clear();
                var blackKingIndex  = this.boardController.GetPieceIndex(PieceTeam.Black, PieceType.King);
                var checkMovesIndex = currentPiece.GetCheckMovesIndex(availableMoves, blackKingIndex);
                if(availableMoves.Contains(blackKingIndex))
                {
                    this.signalBus.Fire(new OnCheckSignal(currentPiece, checkMovesIndex));
                    this.logService.LogWithColor("Check!", Color.red);
                }
                this.logService.LogWithColor($"Black king index: {blackKingIndex}", Color.red);
            }
            else
            {
                var whiteKingIndex  = this.boardController.GetPieceIndex(PieceTeam.White, PieceType.King);
                var checkMovesIndex = currentPiece.GetCheckMovesIndex(availableMoves, whiteKingIndex);
                if(availableMoves.Contains(whiteKingIndex))
                {
                    this.signalBus.Fire(new OnCheckSignal(currentPiece, checkMovesIndex));
                    this.logService.LogWithColor("Check!", Color.red);
                }
                this.logService.LogWithColor($"White king index: {whiteKingIndex}", Color.red);
            }
            var targetPiece    = this.boardController.GetPieceByIndex(this.boardController.GetTileIndex(targetTile));
            if (targetPiece != null)
            {
                targetPiece.Recycle();
            }
        }
    }
}