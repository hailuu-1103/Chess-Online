namespace Runtime.UI
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.MVP;
    using UnityEngine;
    using UnityEngine.UI;

    public class PromotionItemModel
    {
        public Action<string> OnSelectPromote { get; set; }
        public string         PromotionName   { get; set; }

        public PromotionItemModel(Action<string> onSelectPromote, string promotionName)
        {
            this.OnSelectPromote = onSelectPromote;
            this.PromotionName   = promotionName;
        }
    }

    public class PromotionItemView : TViewMono
    {
        [SerializeField] private Button btnSelectPromote;
        [SerializeField] private Image  imgPromotion;

        public Button BtnSelectPromote => this.btnSelectPromote;

        public void SetView(Sprite promotion) { this.imgPromotion.sprite = promotion; }
    }

    public class PromotionItemPresenter : BaseUIItemPresenter<PromotionItemView, PromotionItemModel>
    {
        private PromotionItemModel model;
        public PromotionItemPresenter(IGameAssets gameAssets) : base(gameAssets) { }

        public override void BindData(PromotionItemModel param)
        {
            this.model = param;
            this.InitButtonListener();
            this.SetUpView();
        }

        private async void SetUpView()
        {
            var promotionSprite = await this.GameAssets.LoadAssetAsync<Sprite>(this.model.PromotionName);
            this.View.SetView(promotionSprite);
        }

        private void InitButtonListener()
        {
            this.View.BtnSelectPromote.onClick.AddListener(this.OnClickPromoteItem);
        }

        private void OnClickPromoteItem()
        {
            Debug.Log($"Click on item! {this.model.PromotionName}");
            this.model.OnSelectPromote?.Invoke(this.model.PromotionName);
        }
    }
}