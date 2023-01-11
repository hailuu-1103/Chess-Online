namespace Runtime.Input.Signal
{
    using UnityEngine;

    public class OnMouseSignal
    {
        public GameObject CurrentTileHover { get; }

        public OnMouseSignal(GameObject currentTileHover) { this.CurrentTileHover = currentTileHover; }
    }
}