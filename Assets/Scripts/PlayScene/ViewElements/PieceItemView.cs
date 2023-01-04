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

        public MeshRenderer MeshRenderer => this.meshRenderer;
        public BoxCollider  BoxCollider  => this.boxCollider;
        public Rigidbody    RigidBody    => this.rigidBody;

        public int Row;
        public int Col;
    }

    public class PieceItemController : IDisposable
    {
        public PieceItemView  view;
        public PieceItemModel model;

        internal void Init(PieceItemView view, PieceItemModel model)
        {
            this.view  = view;
            this.model = model;
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