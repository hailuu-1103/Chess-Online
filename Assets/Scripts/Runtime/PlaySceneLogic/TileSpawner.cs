namespace Runtime.PlaySceneLogic
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using UnityEngine;

    public class TileSpawner
    {
        private readonly IGameAssets       gameAssets;
        private readonly ObjectPoolManager objectPoolManager;

        public TileSpawner(IGameAssets gameAssets, ObjectPoolManager objectPoolManager)
        {
            this.gameAssets        = gameAssets;
            this.objectPoolManager = objectPoolManager;
        }

        public async UniTask<GameObject[,]> GenerateAllTiles(int boardRows, int boardColumn, Transform parent)
        {
            var pieces = new GameObject[boardRows, boardColumn];
            for (var i = 0; i < boardRows; i++)
            {
                for (var j = 0; j < boardColumn; j++)
                {
                    pieces[i, j] = await this.GenerateSingleTiles(i, j, parent);
                }
            }

            return pieces;
        }

        private async UniTask<GameObject> GenerateSingleTiles(int x, int y, Transform parent)
        {
            var tileObj = await this.objectPoolManager.Spawn("Tile", parent);
            tileObj.name                                  = $"X:{x}, Y:{y}";
            tileObj.layer                                 = LayerMask.NameToLayer("Tile");
            tileObj.transform.position                    = new Vector3(x, 0.15f, y);
            tileObj.GetComponent<MeshRenderer>().material = await this.gameAssets.LoadAssetAsync<Material>("TransparentMat");
            return tileObj;
        }
    }
}