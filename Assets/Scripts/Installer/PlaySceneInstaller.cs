namespace Installer
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using Runtime.Input;
    using Runtime.Input.Signal;
    using Runtime.PlaySceneLogic;
    using Runtime.PlaySceneLogic.ChessPiece;
    using Runtime.PlaySceneLogic.ChessPiece.Piece;
    using Runtime.PlaySceneLogic.ChessTile;
    using UnityEngine;
    using Zenject;

    public class PlaySceneInstaller : BaseSceneInstaller
    {
        [SerializeField] private PlaySceneCamera playSceneCamera;

        public override void InstallBindings()
        {
            base.InstallBindings();

            this.Container.Bind<PlaySceneCamera>().FromInstance(this.playSceneCamera).AsCached();
            
            this.Container.Bind<TileSpawner>().AsCached().NonLazy();
            this.Container.Bind<TileHighlighter>().AsCached().NonLazy();
            this.Container.Bind<PieceSpawner>().AsCached().NonLazy();
            this.Container.Bind<BoardController>().FromComponentInHierarchy().AsCached();
            this.Container.DeclareSignal<OnMouseSignal>();
        }
    }
}