namespace Runtime.PlaySceneLogic.ChessPiece
{
    using System.Collections.Generic;
    using DG.Tweening;
    using GameFoundation.Scripts.Utilities.LogService;
    using UnityEngine;
    using Zenject;

    public abstract class BaseChessPiece : MonoBehaviour
    {
        public PieceTeam team;
        public int       row;
        public int       col;
        public PieceType type;

        protected BoardController boardController;
        protected ILogService     logService;

        [Inject]
        private void Init(BoardController controller, ILogService service)
        {
            this.boardController = controller;
            this.logService      = service;
        }

        public abstract List<Vector2Int> GetAvailableMoves();

        public virtual void ReplaceData(int row, int col)
        {
            this.row = row;
            this.col = col;
        }

        public virtual void MoveTo(GameObject targetTile) { this.transform.DOMove(targetTile.transform.position, GameStaticValue.MoveDuration); }
        
        public abstract void Attack(BaseChessPiece targetPiece);
        public abstract void PreMove(BaseChessPiece targetPiece);
    }
}