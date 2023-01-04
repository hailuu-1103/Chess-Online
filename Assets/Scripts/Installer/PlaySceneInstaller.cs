namespace Installer
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using PlayScene.PlaySceneLogic;
    using PlayScene.ViewElements;

    public class PlaySceneInstaller : BaseSceneInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            this.Container.BindFactory<PieceItemModel, UniTask<PieceItemController>, PieceItemController.Factory>();
            this.Container.Bind<BoardController>().AsCached().NonLazy();
        }
    }
}