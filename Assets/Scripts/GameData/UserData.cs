namespace GameData
{
    using System;

    public class UserData
    {
        public IGamePlayMode   CurrentModeOnPlay { get; set; }
        public ComputerMode    ComputerMode      { get; set; } = new();
        public MultiplayerMode MultiplayerMode   { get; set; } = new();
    }

    public class ComputerMode : IComputerMode
    {
        public Type          CurrentGamePlayMode => typeof(IGamePlayMode);
    }

    public class MultiplayerMode : IMultiplayerMode
    {
        public Type          CurrentGamePlayMode => typeof(IMultiplayerMode);
    }
}