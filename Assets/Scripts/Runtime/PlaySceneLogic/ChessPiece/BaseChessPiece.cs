namespace Runtime.PlaySceneLogic.ChessPiece
{
    using System.Collections.Generic;
    using DG.Tweening;
    using GameFoundation.Scripts.Utilities.LogService;
    using UnityEngine;
    using Zenject;

    public abstract class BaseChessPiece : MonoBehaviour
    {
        public PieceTeam Team;
        public int       Row;
        public int       Col;
        public PieceType Type;

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
            this.Row = row;
            this.Col = col;
        }

        public virtual void MoveTo(GameObject targetTile) { this.transform.DOMove(targetTile.transform.position, GameStaticValue.MoveDuration); }

        public virtual bool IsBlockByAlies(BaseChessPiece targetPiece)
        {
            if (targetPiece == null) return false;
            return this.Team == targetPiece.Team;
        }
        public abstract void Attack(BaseChessPiece targetPiece);
        public abstract void PreMove(BaseChessPiece targetPiece);
    }
}