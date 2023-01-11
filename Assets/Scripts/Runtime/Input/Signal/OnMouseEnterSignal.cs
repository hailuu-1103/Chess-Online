namespace Runtime.Input.Signal
{
    using UnityEngine;

    public class OnMouseEnterSignal
    {
        public GameObject CurrentTileHover { get; }

        public OnMouseEnterSignal(GameObject currentTileHover) { this.CurrentTileHover = currentTileHover; }
    }
}