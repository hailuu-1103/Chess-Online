namespace GameData
{
    using System;

    public interface IGamePlayMode
    {
        Type          CurrentGamePlayMode { get; }
        
    }

    public interface IComputerMode : IGamePlayMode
    {
    }

    public interface IMultiplayerMode : IGamePlayMode
    {
    }
}