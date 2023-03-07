namespace Runtime.UI
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using Runtime.Input;
    using Runtime.PlaySceneLogic.ChessPiece;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class PromotionPopUpModel
    {
        public Action<PieceTeam, PieceType> OnSelectComplete { get; set; }
        public PieceTeam      PieceTeam        { get; set; }

        public PromotionPopUpModel(Action<PieceTeam, PieceType> onSelectComplete, PieceTeam pieceTeam)
        {
            this.OnSelectComplete = onSelectComplete;
            this.PieceTeam        = pieceTeam;
        }
    }

    public class PromotionPopUpView : BaseView
    {
        [SerializeField] private Button btnQueenPromotion;
        [SerializeField] private Button btnRookPromotion;
        [SerializeField] private Button btnBishopPromotion;
        [SerializeField] private Button btnKnightPromotion;
        [SerializeField] private Button btnClose;
        public                   Button BtnQueenPromotion  => this.btnQueenPromotion;
        public                   Button BtnRookPromotion   => this.btnRookPromotion;
        public                   Button BtnBishopPromotion => this.btnBishopPromotion;
        public                   Button BtnKnightPromotion => this.btnKnightPromotion;
        public                   Button BtnClose           => this.btnClose;
    }

    [PopupInfo(nameof(PromotionPopUpView), isEnableBlur: false, isCloseWhenTapOutside:false)]
    public class PromotionPopUpPresenter : BasePopupPresenter<PromotionPopUpView, PromotionPopUpModel>
    {
        #region inject

        private readonly DiContainer diContainer;
        private readonly GameInput gameInput;
        
        #endregion

        private readonly IGameAssets gameAssets;
        private          List<PromotionItemPresenter> promotionItemPresenters = new();
        private          PromotionPopUpModel          model;
        public PromotionPopUpPresenter(SignalBus signalBus, ILogService logService, IGameAssets gameAssets, DiContainer diContainer, GameInput input) : base(signalBus, logService) {
            this.gameAssets = gameAssets;
            this.diContainer = diContainer;
            this.gameInput = input;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.InitButtonListener();
            this.gameInput.IsBlockInput = true;
        }

        private void InitButtonListener()
        {
            this.View.BtnQueenPromotion.onClick.AddListener(this.OnClickQueenPromotion);
            this.View.BtnRookPromotion.onClick.AddListener(this.OnClickRookPromotion);
            this.View.BtnBishopPromotion.onClick.AddListener(this.OnClickBishopPromotion);
            this.View.BtnKnightPromotion.onClick.AddListener(this.OnClickKnightPromotion);
        }

        public override void BindData(PromotionPopUpModel popUpModel) 
        { 
            this.model = popUpModel;
            setView();
        }
        public async void setView()
        {
            this.View.BtnQueenPromotion.image.sprite = await this.gameAssets.LoadAssetAsync<Sprite>($"queen_{this.model.PieceTeam}");
            this.View.BtnBishopPromotion.image.sprite = await this.gameAssets.LoadAssetAsync<Sprite>($"bishop_{this.model.PieceTeam}");
            this.View.BtnKnightPromotion.image.sprite = await this.gameAssets.LoadAssetAsync<Sprite>($"knight_{this.model.PieceTeam}");
            this.View.BtnRookPromotion.image.sprite = await this.gameAssets.LoadAssetAsync<Sprite>($"rook_{this.model.PieceTeam}");
        }
        private async void OnClickQueenPromotion()
        {
            await this.CloseViewAsync();
            this.model.OnSelectComplete?.Invoke(this.model.PieceTeam, PieceType.Queen);
            this.gameInput.IsBlockInput = false;
        }

        private async void OnClickRookPromotion()
        {
            await this.CloseViewAsync();
            this.model.OnSelectComplete?.Invoke(this.model.PieceTeam, PieceType.Castle);
            this.gameInput.IsBlockInput = false;
        }

        private async void OnClickBishopPromotion()
        {
            await this.CloseViewAsync();
            this.model.OnSelectComplete?.Invoke(this.model.PieceTeam, PieceType.Bishop);
            this.gameInput.IsBlockInput = false;
        }

        private async void OnClickKnightPromotion()
        {
            await this.CloseViewAsync();
            this.model.OnSelectComplete?.Invoke(this.model.PieceTeam, PieceType.Knight);
            this.gameInput.IsBlockInput = false;
        }
    }
}