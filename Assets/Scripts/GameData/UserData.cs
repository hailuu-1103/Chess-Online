namespace GameData
{
    using System;
    using PlayScene.PlaySceneLogic.Models;

    public class UserData
    {
        public IGamePlayMode   CurrentModeOnPlay { get; set; }
        public ComputerMode    ComputerMode      { get; set; } = new();
        public MultiplayerMode MultiplayerMode   { get; set; } = new();
    }

    public class ComputerMode : IComputerMode
    {
        public Type          CurrentGamePlayMode => typeof(IGamePlayMode);
        public GameDataState GameDataState       { get; set; } = new();
    }

    public class MultiplayerMode : IMultiplayerMode
    {
        public Type          CurrentGamePlayMode => typeof(IMultiplayerMode);
        public GameDataState GameDataState       { get; set; } = new();
    }
}