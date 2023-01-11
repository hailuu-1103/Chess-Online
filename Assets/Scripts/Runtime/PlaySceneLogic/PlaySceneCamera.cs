namespace Runtime.PlaySceneLogic
{
    using UnityEngine;

    public class PlaySceneCamera : MonoBehaviour
    {
        private Camera Camera => this.GetComponent<Camera>();

        public Ray GetRayScreenPoint(Vector3 position) => this.Camera.ScreenPointToRay(position);
    }
}