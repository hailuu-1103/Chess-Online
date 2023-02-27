namespace MultiplayerNetwork.Controller
{
    using MultiplayerNetwork.Configs;
    using Zenject;

    public class MultiplayerNetworkInstaller : Installer<MultiplayerNetworkInstaller>
    {
        public override void InstallBindings()
        {
            this.Container.Bind<MultiplayerNetworkConfig>().FromScriptableObjectResource(nameof(MultiplayerNetworkConfig)).AsCached();
            this.Container.Bind<MapLoader>().FromNewComponentOnNewGameObject().AsCached().NonLazy();

        }
    }
}