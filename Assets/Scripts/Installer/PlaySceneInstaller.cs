namespace Installer
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.UIModule.Utilities;
    using Runtime.Input.Signal;
    using Runtime.PlaySceneLogic;
    using Runtime.PlaySceneLogic.ChessPiece;
    using Runtime.PlaySceneLogic.ChessTile;
    using Runtime.PlaySceneLogic.Signal;
    using Runtime.UI;
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
            this.Container.Bind<PieceRegularMoveHelper>().AsCached().NonLazy();
            
            this.Container.Bind<BoardController>().FromComponentInHierarchy().AsCached();

            this.InitSignal(this.Container);
            
            this.Container.InitScreenManually<MainSceneScreenPresenter>();
        }

        private void InitSignal(DiContainer diContainer)
        {
            diContainer.DeclareSignal<OnMouseEnterSignal>();
            diContainer.DeclareSignal<OnMouseReleaseSignal>();
            diContainer.DeclareSignal<OnCheckSignal>();
        }
    }
}