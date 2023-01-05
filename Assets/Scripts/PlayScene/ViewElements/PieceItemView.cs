namespace PlayScene.ViewElements
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using PlayScene.Data;
    using UnityEngine;
    using Zenject;

    public class PieceItemModel
    {
        public PieceInfo PieceInfo { get; set; }
        public Transform Parent    { get; set; }
    }

    public class PieceItemView : MonoBehaviour
    {
        [SerializeField] private BoxCollider  boxCollider;
        [SerializeField] private Rigidbody    rigidBody;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private MeshFilter   meshFilter;
        public                   MeshFilter   MeshFilter   => this.meshFilter;
        public                   MeshRenderer MeshRenderer => this.meshRenderer;
        public                   BoxCollider  BoxCollider  => this.boxCollider;
        public                   Rigidbody    RigidBody    => this.rigidBody;

        //DEBUG ONLY
        public int       Row;
        public int       Col;
        public PieceTeam PieceTeam;
        public PieceType PieceType;
    }

    public class PieceItemController : IDisposable
    {
        private readonly IGameAssets gameAssets;

        public PieceItemView  view;
        public PieceItemModel model;

        public PieceItemController(IGameAssets gameAssets) { this.gameAssets = gameAssets; }

        private async void Init(PieceItemView view, PieceItemModel model)
        {
            var pieceDistanceX = GameStaticValue.PieceDistanceX;
            var initialPosX    = GameStaticValue.InitialPiecePosX;
            var initialPosZ    = GameStaticValue.InitialPiecePosZ;
            this.view  = view;
            this.model = model;

            //DEBUG ONLY
            this.view.Col       = model.PieceInfo.Col;
            this.view.Row       = model.PieceInfo.Row;
            this.view.PieceTeam = model.PieceInfo.PieceTeam;
            this.view.PieceType = model.PieceInfo.PieceType;

            this.view.MeshFilter.mesh = model.PieceInfo.PieceType != PieceType.None
                ? await this.gameAssets.LoadAssetAsync<Mesh>(Enum.GetName(typeof(PieceType), model.PieceInfo.PieceType))
                : null;
            this.view.MeshRenderer.material = model.PieceInfo.PieceType != PieceType.None
                ? await this.gameAssets.LoadAssetAsync<Material>(Enum.GetName(typeof(PieceTeam), model.PieceInfo.PieceTeam) + " " + Enum.GetName(typeof(PieceType), model.PieceInfo.PieceType))
                : null;
            GameObject gameObject;
            (gameObject = this.view.gameObject).transform.SetParent(model.Parent);
            var multiplierPosX = pieceDistanceX * model.PieceInfo.Col;
            var multiplierPosZ = pieceDistanceX * model.PieceInfo.Row;
            gameObject.transform.localPosition = new Vector3(initialPosX + multiplierPosX, 0, initialPosZ + multiplierPosZ);
        }

        public void ReplaceData(PieceItemView view, PieceItemModel model)
        {
            this.view  = view;
            this.model = model;
        }

        public void Dispose()
        {
            this.view.Recycle();
            this.view = null;
        }

        public class Factory : PlaceholderFactory<PieceItemModel, UniTask<PieceItemController>>
        {
            private readonly DiContainer diContainer;
            private readonly IGameAssets gameAssets;

            public Factory(DiContainer diContainer, IGameAssets gameAssets)
            {
                this.diContainer = diContainer;
                this.gameAssets  = gameAssets;
            }

            public override async UniTask<PieceItemController> Create(PieceItemModel param)
            {
                var controller = this.diContainer.Instantiate<PieceItemController>();

                var viewPrefab = await this.gameAssets.LoadAssetAsync<GameObject>(nameof(PieceItemView));
                var view       = viewPrefab.Spawn().GetComponent<PieceItemView>();
                controller.Init(view, param);

                return controller;
            }
        }
    }
}