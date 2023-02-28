namespace Runtime.PlaySceneLogic
{
    using System.Collections.Generic;
    using UnityEngine;

    public class PlaySceneCamera : MonoBehaviour
    {
        [SerializeField] private List<Camera> cameras;

        private Camera currentCamera;

        public Ray GetRayScreenPoint(Vector3 position) => this.currentCamera.ScreenPointToRay(position);

        public void SetMainCamera(bool isWhiteTurn)
        {
            this.DeactivateAllCamera();
            switch (isWhiteTurn)
            {
                case true:
                    this.currentCamera         = this.cameras[0];
                    this.currentCamera.enabled = true;
                    break;
                case false:
                    this.currentCamera         = this.cameras[1];
                    this.currentCamera.enabled = true;
                    break;
            }
        }

        private void DeactivateAllCamera()
        {
            foreach (var cam in this.cameras)
            {
                cam.enabled = false;
            }
        }
    }
}