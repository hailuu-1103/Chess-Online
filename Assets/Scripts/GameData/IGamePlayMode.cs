namespace GameData
{
    using System;
    using PlayScene.PlaySceneLogic.Models;

    public interface IGamePlayMode
    {
        Type          CurrentGamePlayMode { get; }
        GameDataState GameDataState       { get; set; }
    }

    public interface IComputerMode : IGamePlayMode
    {
    }

    public interface IMultiplayerMode : IGamePlayMode
    {
    }
}