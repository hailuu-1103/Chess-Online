namespace MultiplayerNetwork.Controller
{
    using System.Collections;
    using Cysharp.Threading.Tasks;
    using Fusion;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Zenject;

    public class MapLoader : NetworkSceneManagerBase
    {
        private ChessOnlineSceneDirector sceneDirector;
        [Inject]
        private void Inject(ChessOnlineSceneDirector chessOnlineSceneDirector)
        {
            this.sceneDirector = chessOnlineSceneDirector;
        }
        protected override IEnumerator SwitchScene(SceneRef prevScene, SceneRef newScene, FinishedLoadingDelegate finished)
        {
            Debug.Log($"Switching Scene from {prevScene} to {newScene}");

            yield return this.sceneDirector.LoadPlayScene().ToCoroutine();
            var loadedScene = SceneManager.GetSceneByPath(ChessOnlineSceneDirector.SceneName.PlayScene);
            Debug.Log($"Loaded scene {ChessOnlineSceneDirector.SceneName.PlayScene}: {loadedScene}");
            var sceneObjects = this.FindNetworkObjects(loadedScene, disable: false);

            // Delay one frame
            yield return null;
            finished(sceneObjects);

            Debug.Log($"Switched Scene from {prevScene} to {newScene} - loaded {sceneObjects.Count} scene objects");
        }
    }
}