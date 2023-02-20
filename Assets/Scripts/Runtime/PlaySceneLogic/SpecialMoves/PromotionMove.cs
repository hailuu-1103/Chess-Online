namespace Runtime.PlaySceneLogic.SpecialMoves
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using Runtime.UI;
    using UnityEngine;

    public class PromotionMove : ISpecialMoves
    {
        private readonly IScreenManager  screenManager;
        private readonly BoardController boardController;

        public PromotionMove(IScreenManager screenManager, BoardController boardController)
        {
            this.screenManager   = screenManager;
            this.boardController = boardController;
        }

        public void Execute(Vector2Int currentPieceIndex, Vector2Int targetPieceIndex)
        {
            this.screenManager.OpenScreen<PromotionPopupPresenter>();
            Debug.Log("Implement promotion.!");
        }
    }
}