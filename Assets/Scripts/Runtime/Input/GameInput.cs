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

        #endregion

        [Inject]
        private void OnInit(SignalBus signal, PlaySceneCamera playCamera)
        {
            this.signalBus       = signal;
            this.playSceneCamera = playCamera;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var        ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hitInfo, 100, LayerMask.GetMask("Piece", "Hover")))
                {
                    this.signalBus.Fire(new OnMouseSignal(hitInfo.transform.gameObject));
                }
            }
        }
    }
}