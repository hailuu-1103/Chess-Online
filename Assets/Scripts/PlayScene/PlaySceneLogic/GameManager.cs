namespace PlayScene.PlaySceneLogic
{
    using UnityEngine;
    using Zenject;

    public class GameManager : MonoBehaviour
    {
        private BoardController boardController;

        [Inject]
        private void Init(BoardController boardController)
        {
            this.boardController = boardController;
            this.InitGame();
        }

        private async void InitGame()
        {
            await this.boardController.InitPiece(this.transform.GetChild(0));
        }
    }
}