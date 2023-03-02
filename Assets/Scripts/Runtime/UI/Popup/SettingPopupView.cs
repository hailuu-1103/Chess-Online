namespace Runtime.UI.Popup
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using Zenject;

    public class SettingPopupView : BaseView
    {
        
    }
    
    [PopupInfo(nameof(SettingPopupView), isEnableBlur:false, isCloseWhenTapOutside:true)]
    public class SettingPopupPresenter : BasePopupPresenter<SettingPopupView>
    {
        public SettingPopupPresenter(SignalBus signalBus) : base(signalBus)
        {
        }

        public override void BindData()
        {
            
        }
    }
}