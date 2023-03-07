namespace GameInstaller
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.UIModule.Utilities;
    using Runtime.Input;
    using Runtime.Input.Signal;
    using Runtime.PlaySceneLogic;
    using Runtime.PlaySceneLogic.ChessPiece;
    using Runtime.PlaySceneLogic.ChessTile;
    using Runtime.PlaySceneLogic.SpecialMoves;
    using Runtime.UI;
    using UnityEngine;
    using Zenject;

    public class PlaySceneInstaller : BaseSceneInstaller
    {
        [SerializeField] private PlaySceneCamera playSceneCamera;
        [SerializeField] private Transform       pieceHolder;

        public override void InstallBindings()
        {
            base.InstallBindings();
            this.Container.Bind<Transform>().FromInstance(this.pieceHolder).AsCached();
            this.Container.Bind<PlaySceneCamera>().FromInstance(this.playSceneCamera).AsCached();
            this.Container.Bind<FileManager>().FromComponentInHierarchy().AsCached();
            this.Container.Bind<TileSpawnerService>().AsCached().NonLazy();
            this.Container.Bind<TileHighlighterService>().AsCached().NonLazy();
            this.Container.Bind<PieceSpawnerService>().AsCached().NonLazy();
            this.Container.Bind<PieceRegularMoveHelper>().AsCached().NonLazy();

            this.Container.Bind<GameInput>().FromComponentsInHierarchy().AsCached();
            this.Container.Bind<BoardController>().FromComponentInHierarchy().AsCached();
            this.Container.Rebind<ISpecialMoves>().To<CastlingMove>().AsSingle();
            this.Container.Rebind<ISpecialMoves>().To<EnPassantMove>().AsSingle();
            this.Container.Rebind<ISpecialMoves>().To<PromotionMove>().AsSingle();
            this.InitSignal(this.Container);

            this.Container.InitScreenManually<GameScreenPresenter>();
        }

        private void InitSignal(DiContainer diContainer)
        {
            diContainer.DeclareSignal<OnMouseEnterSignal>();
            diContainer.DeclareSignal<OnMouseReleaseSignal>();
            diContainer.DeclareSignal<OutOfTimeSignal>();
        }
    }
}