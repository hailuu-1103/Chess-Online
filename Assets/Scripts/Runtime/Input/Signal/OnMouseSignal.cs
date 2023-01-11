namespace Runtime.Input.Signal
{
    using UnityEngine;

    public class OnMouseSignal
    {
        public GameObject CurrentPieceHover { get; }

        public OnMouseSignal(GameObject currentPieceHover) { this.CurrentPieceHover = currentPieceHover; }
    }
}