namespace Runtime.Input.Signal
{
    using UnityEngine;

    public class OnMouseEnterSignal
    {
        public Vector2Int CurrentTileIndex { get; }

        public OnMouseEnterSignal(Vector2Int currentTileIndex) { this.CurrentTileIndex = currentTileIndex; }
    }
}