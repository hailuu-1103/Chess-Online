using GameFoundation.Scripts;
using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
using GameFoundation.Scripts.Utilities;
using UserData;
using Zenject;

public class ChessOnlineProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        GameFoundationInstaller.Install(this.Container);
        this.Container.Bind<UserLocalData>().FromResolveGetter<HandleLocalDataServices>(services => services.Load<UserLocalData>()).AsCached();
        this.Container.Bind<ChessOnlineSceneDirector>().AsCached().NonLazy();
        this.Container.Rebind<SceneDirector>().FromResolveGetter<ChessOnlineSceneDirector>(data => data).AsCached();
    }
}