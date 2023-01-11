namespace Runtime.PlaySceneLogic.ChessPiece
{
    using System;
    using Zenject;

    public class PieceMovement : IDisposable
    {
        private readonly SignalBus signalBus;
        public PieceMovement(SignalBus signalBus) { this.signalBus = signalBus; }

        public void Dispose()
        {
        }
    }
}