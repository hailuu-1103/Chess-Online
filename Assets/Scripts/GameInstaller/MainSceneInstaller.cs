namespace GameInstaller
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.UIModule.Utilities;
    using Runtime.UI.LobbyFlow;
    using Runtime.UI.Screen;

    public class MainSceneInstaller : BaseSceneInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            
            this.Container.BindInterfacesAndSelfTo<LobbyFlowHandler>().AsCached();
            this.Container.InitScreenManually<MainScreenPresenter>();
        }
    }
}