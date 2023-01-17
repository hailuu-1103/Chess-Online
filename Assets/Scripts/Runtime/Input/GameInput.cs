namespace Runtime.Input
{
    using System;
    using Runtime.Input.Signal;
    using Runtime.PlaySceneLogic;
    using UnityEngine;
    using Zenject;

    public class GameInput : MonoBehaviour
    {
        #region inject

        private PlaySceneCamera playSceneCamera;
        private SignalBus       signalBus;
        private BoardController boardController;

        #endregion

        [Inject]
        private void OnInit(SignalBus signal, PlaySceneCamera playCamera, BoardController controller)
        {
            this.signalBus       = signal;
            this.playSceneCamera = playCamera;
            this.boardController = controller;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var        ray = this.playSceneCamera.GetRayScreenPoint(Input.mousePosition);
                if (Physics.Raycast(ray, out var hitInfo, 100, LayerMask.GetMask("Tile")))
                {
                    this.signalBus.Fire(new OnMouseEnterSignal(this.boardController.GetTileIndex(hitInfo.transform.gameObject)));
                }
            }
        }
    }
}