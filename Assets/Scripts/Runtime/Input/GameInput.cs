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
        private SignalBus signalBus;
        private BoardController boardController;

        #endregion

        public TextMeshPro playerWhiteTimeText;
        public TextMeshPro playerBlackTimeText;
        public bool IsBlockInput = false;

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
            if (Physics.Raycast(ray, out var hitInfo, 100, LayerMask.GetMask("Tile")) && !IsBlockInput)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    this.signalBus.Fire(new OnMouseEnterSignal(this.boardController.GetTileIndex(hitInfo.transform.gameObject)));
                }
            }

            if (this.boardController.isWhiteTurn.Value)
            {
                this.boardController.playerWhiteTimeRemaining.Value -= Time.deltaTime;
                if (this.boardController.playerWhiteTimeRemaining.Value <= 0)
                {
                    this.signalBus.Fire(new OutOfTimeSignal(true));
                }
            }
            else
            {
                this.boardController.playerBlackTimeRemaining.Value -= Time.deltaTime;
                if (this.boardController.playerWhiteTimeRemaining.Value <= 0)
                {
                    this.signalBus.Fire(new OutOfTimeSignal(false));
                }
            }
        }
    }
}