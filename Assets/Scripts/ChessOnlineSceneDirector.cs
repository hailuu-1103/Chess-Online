using Cysharp.Threading.Tasks;
using GameFoundation.Scripts.AssetLibrary;
using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
using Zenject;

public class ChessOnlineSceneDirector : SceneDirector
{
    public ChessOnlineSceneDirector(SignalBus signalBus, IGameAssets gameAssets) : base(signalBus, gameAssets)
    {
    }
    
    public async UniTask LoadLoadingScene() => await this.LoadSingleSceneAsync(SceneName.Loading);
    public async UniTask LoadMainScene()    => await this.LoadSingleSceneAsync(SceneName.Main);
    public async UniTask LoadPlayScene()    => await this.LoadSingleSceneAsync(SceneName.PlayScene);

    private static class SceneName
    {
        public const string Loading   = "1.LoadingScene";
        public const string Main      = "2.MainScene";
        public const string PlayScene = "3.PlayScene";
    }
}