using DarkTonic.MasterAudio;
using GameData;
using GameFoundation.Scripts;
using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
using GameFoundation.Scripts.Utilities;
using MultiplayerNetwork.Controller;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class ChessOnlineProjectInstaller : MonoInstaller
{
    [SerializeField] private GameObject playlistController;
    
    public override void InstallBindings()
    {
        GameFoundationInstaller.Install(this.Container);
        this.Container.Rebind<PlaylistController>().FromComponentInNewPrefab(this.playlistController).AsCached().NonLazy();
        this.Container.Bind<UserData>().FromResolveGetter<HandleLocalDataServices>(services => services.Load<UserData>()).AsCached();
        this.Container.Bind<ChessOnlineSceneDirector>().AsCached().NonLazy();
        
        //Multiplayer network
        MultiplayerNetworkInstaller.Install(this.Container);
        
        //Common Event System
        this.Container.Bind<EventSystem>().FromComponentInNewPrefabResource(nameof(EventSystem)).AsSingle().NonLazy();
        
        this.Container.Rebind<SceneDirector>().FromResolveGetter<ChessOnlineSceneDirector>(data => data).AsCached();
    }
}