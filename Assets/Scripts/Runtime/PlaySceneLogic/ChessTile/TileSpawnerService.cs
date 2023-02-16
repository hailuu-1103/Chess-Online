namespace Runtime.PlaySceneLogic.ChessTile
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using UnityEngine;

    public class TileSpawnerService
    {
        private readonly IGameAssets       gameAssets;
        private readonly ObjectPoolManager objectPoolManager;

        public TileSpawnerService(IGameAssets gameAssets, ObjectPoolManager objectPoolManager)
        {
            this.gameAssets        = gameAssets;
            this.objectPoolManager = objectPoolManager;
        }

        public async UniTask<GameObject[,]> GenerateAllTiles(int boardRows, int boardColumn, Transform parent)
        {
            var tiles = new GameObject[boardRows, boardColumn];
            for (var i = 0; i < boardRows; i++)
            {
                for (var j = 0; j < boardColumn; j++)
                {
                    tiles[j, i] = await this.GenerateSingleTiles(i, j, parent);
                }
            }

            return tiles;
        }

        private async UniTask<GameObject> GenerateSingleTiles(int x, int y, Transform parent)
        {
            var tileObj = await this.objectPoolManager.Spawn("Tile", parent);
            tileObj.name                                  = $"X:{y}, Y:{x}";
            tileObj.layer                                 = LayerMask.NameToLayer("Tile");
            tileObj.transform.position                    = new Vector3(y, GameStaticValue.TileOffsetY, x);
            tileObj.GetComponent<MeshRenderer>().material = await this.gameAssets.LoadAssetAsync<Material>("TransparentMat");
            return tileObj;
        }
    }
}