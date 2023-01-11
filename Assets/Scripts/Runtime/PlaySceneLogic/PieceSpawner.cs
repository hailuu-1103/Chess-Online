namespace Runtime.PlaySceneLogic
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using UnityEngine;

    public class PieceSpawner
    {
        private readonly IGameAssets       gameAssets;
        private readonly ObjectPoolManager objectPoolManager;

        public PieceSpawner(IGameAssets gameAssets, ObjectPoolManager objectPoolManager)
        {
            this.gameAssets        = gameAssets;
            this.objectPoolManager = objectPoolManager;
        }

        public async UniTask<GameObject[,]> GenerateAllPiece(int boardRows, int boardColumn, Transform parent)
        {
            var pieces = new GameObject[boardRows, boardColumn];
            for (var i = 0; i < boardRows; i++)
            {
                for (var j = 0; j < boardColumn; j++)
                {
                    pieces[i, j] = await this.GenerateSinglePiece(i, j, parent);
                }
            }

            return pieces;
        }

        private async UniTask<GameObject> GenerateSinglePiece(int x, int y, Transform parent)
        {
            var pieceObj = await this.objectPoolManager.Spawn("Piece", parent);
            pieceObj.name                                  = $"X:{x}, Y:{y}";
            pieceObj.layer                                 = LayerMask.NameToLayer("Piece");
            pieceObj.transform.position                    = new Vector3(x, 0.15f, y);
            pieceObj.GetComponent<MeshRenderer>().material = await this.gameAssets.LoadAssetAsync<Material>("TransparentMat");
            return pieceObj;
        }
    }
}