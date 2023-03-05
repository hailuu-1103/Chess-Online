namespace Runtime.Input
{
    using Runtime.Input.Signal;
    using Runtime.PlaySceneLogic;
    using TMPro;
    using UnityEngine;
    using Zenject;

    public class GameInput : MonoBehaviour
    {
        #region inject

        private PlaySceneCamera playSceneCamera;
        private SignalBus       signalBus;
        private BoardController boardController;

        #endregion

        public TextMeshPro playerWhiteTimeText;
        public TextMeshPro playerBlackTimeText;

        [Inject]
        private void OnInit(SignalBus signal, PlaySceneCamera playCamera, BoardController controller)
        {
            this.signalBus       = signal;
            this.playSceneCamera = playCamera;
            this.boardController = controller;
        }

        private void Update()
        {
            var ray = this.playSceneCamera.GetRayScreenPoint(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, 100, LayerMask.GetMask("Tile")))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    this.signalBus.Fire(new OnMouseEnterSignal(this.boardController.GetTileIndex(hitInfo.transform.gameObject)));
                }
            }

            if (this.boardController.isWhiteTurn.Value)
            {
                this.boardController.playerWhiteTimeRemaining -= Time.deltaTime;
/*                UpdateTimer(
                    this.boardController.playerWhiteTimeRemaining,
                    playerWhiteTimeText
                    );*/
            }
            else
            {
                this.boardController.playerBlackTimeRemaining -= Time.deltaTime;
/*                UpdateTimer(
                    this.boardController.playerBlackTimeRemaining,
                    playerBlackTimeText
                    );*/
            }
        }

        void UpdateTimer(float timeRemaining, TextMeshPro timerText)
        {
            // Convert the time remaining to minutes and seconds
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);

            // Update the timer text
            timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }

        
    }
}