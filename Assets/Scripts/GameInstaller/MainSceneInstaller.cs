namespace GameInstaller
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.UIModule.Utilities;
    using Runtime.UI;

    public class MainSceneInstaller : BaseSceneInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            this.Container.InitScreenManually<LobbyScreenPresenter>();
        }
    }
}