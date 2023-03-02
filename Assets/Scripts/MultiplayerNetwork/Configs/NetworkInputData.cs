namespace MultiplayerNetwork.Configs
{
    using Fusion;

    [System.Flags]
    public enum ButtonFlag
    {
        up    = 1 << 0,
        down  = 1 << 1,
        left  = 1 << 2,
        right = 1 << 3,
    }

    public struct NetworkInputData : INetworkInput
    {
        public NetworkButtons Buttons;
    }
}