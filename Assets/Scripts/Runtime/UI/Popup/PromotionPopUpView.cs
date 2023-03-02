namespace Runtime.UI.Popup
{
    using System;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
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

    [PopupInfo(nameof(PromotionPopUpView), isCloseWhenTapOutside:false)]
    public class PromotionPopUpPresenter : BasePopupPresenter<PromotionPopUpView, PromotionPopUpModel>
    {
        private readonly DiContainer                  diContainer;
        private          PromotionPopUpModel          model;
        public PromotionPopUpPresenter(SignalBus signalBus, ILogService logService, DiContainer diContainer) : base(signalBus, logService) { this.diContainer = diContainer; }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.InitButtonListener();
        }

        private void InitButtonListener()
        {
            this.View.BtnQueenPromotion.onClick.AddListener(this.OnClickQueenPromotion);
            this.View.BtnRookPromotion.onClick.AddListener(this.OnClickRookPromotion);
            this.View.BtnBishopPromotion.onClick.AddListener(this.OnClickBishopPromotion);
            this.View.BtnKnightPromotion.onClick.AddListener(this.OnClickKnightPromotion);
        }

        public override void BindData(PromotionPopUpModel popUpModel) { this.model = popUpModel; }

        private async void OnClickQueenPromotion()
        {
            await this.CloseViewAsync();
            this.model.OnSelectComplete?.Invoke(this.model.PieceTeam, PieceType.Queen);
        }

        private async void OnClickRookPromotion()
        {
            await this.CloseViewAsync();
            this.model.OnSelectComplete?.Invoke(this.model.PieceTeam, PieceType.Castle);
        }

        private async void OnClickBishopPromotion()
        {
            await this.CloseViewAsync();
            this.model.OnSelectComplete?.Invoke(this.model.PieceTeam, PieceType.Bishop);
        }

        private async void OnClickKnightPromotion()
        {
            await this.CloseViewAsync();
            this.model.OnSelectComplete?.Invoke(this.model.PieceTeam, PieceType.Knight);
        }
    }
}