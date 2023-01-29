namespace Runtime.PlaySceneLogic.ChessPiece
{
    using System.Collections.Generic;
    using Runtime.PlaySceneLogic.ChessTile;
    using UnityEngine;
    using Zenject;

    public abstract class BaseChessPiece : MonoBehaviour
    {
        public PieceTeam Team;
        public int       Row;
        public int       Col;
        public PieceType Type;

        protected BoardController boardController;
        [Inject]
        private void Init(BoardController controller)
        {
            this.boardController = controller;
        }

        public abstract List<Vector2Int> GetAvailableMoves();

        public virtual void ReplaceData(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }
        public virtual void MoveTo(BaseChessPiece targetPiece)
        {
            if(this.Team != targetPiece.Team) this.Attack(targetPiece);
        }

        public virtual void Attack(BaseChessPiece targetPiece)
        {
            
        }

    }
}