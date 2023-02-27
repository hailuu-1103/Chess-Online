namespace Runtime.Input
{
    using System;
    using Runtime.Input.Signal;
    using UnityEngine;
    using Zenject;

    public class EventSystem : MonoBehaviour
    {
        private SignalBus   signalBus;
        private EventSystem eventSystem;
        [Inject]
        private void Init(SignalBus signal, EventSystem system)
        {
            this.signalBus   = signal;
            this.eventSystem = system;
        }

        private void OnEnable()
        {
            this.signalBus.Subscribe<BlockInputSignal>(this.OnBlockInput);
        }

        private void OnDisable()
        {
            this.signalBus.Unsubscribe<BlockInputSignal>(this.OnBlockInput);
        }

        private void OnBlockInput(BlockInputSignal signal)
        {
            this.eventSystem.enabled = !signal.IsBlockInput;
        }
    }
}