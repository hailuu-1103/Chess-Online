namespace Runtime.UI
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using Zenject;

    public class PromotionPopUpView : BaseView
    {
        
    }
    [PopupInfo(nameof(PromotionPopUpView))]
    public class PromotionPopupPresenter : BasePopupPresenter<PromotionPopUpView>
    {
        public PromotionPopupPresenter(SignalBus signalBus) : base(signalBus)
        {
        }

        public override void BindData() { throw new System.NotImplementedException(); }
    }
}