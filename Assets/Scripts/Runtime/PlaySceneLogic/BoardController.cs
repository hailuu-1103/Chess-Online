namespace Runtime.PlaySceneLogic
{
    using UnityEngine;
    using Zenject;

    public class BoardController : MonoBehaviour
    {
        #region inject

        private TileSpawner tileSpawner;

        #endregion

        [SerializeField] private Transform tileHolder;

        public GameObject[,] runtimeTiles = new GameObject[GameStaticValue.BoardRows, GameStaticValue.BoardColumn];

        [Inject]
        private void OnInit(TileSpawner spawner) { this.tileSpawner = spawner; }

        private async void Start()
        {
            this.runtimeTiles = await this.tileSpawner.GenerateAllTiles(GameStaticValue.BoardRows, GameStaticValue.BoardColumn, this.tileHolder);
        }
    }
}