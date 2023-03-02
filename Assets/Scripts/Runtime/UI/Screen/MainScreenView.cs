namespace Runtime.UI.Screen
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using Runtime.UI.Popup;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class MainScreenView : BaseView
    {
        [SerializeField] private Button btnPlay;
        [SerializeField] private Button btnProfile;
        [SerializeField] private Button btnSetting;

        public Button BtnPlay    => this.btnPlay;
        public Button BtnProfile => this.btnProfile;
        public Button BtnSetting => this.btnSetting;
    }

    [ScreenInfo(nameof(MainScreenView))]
    public class MainScreenPresenter : BaseScreenPresenter<MainScreenView>
    {
        private readonly IScreenManager screenManager;
        public MainScreenPresenter(SignalBus signalBus, IScreenManager screenManager) : base(signalBus) { this.screenManager = screenManager; }

        protected override async void OnViewReady()
        {
            base.OnViewReady();
            await this.OpenViewAsync();
            this.InitButtonListener();
        }

        public override void BindData()
        {
            
        }

        private void OnClickPlayButton()
        {
            this.screenManager.OpenScreen<ModeScreenPresenter>();
        }
        private void OnClickProfileButton()
        {
            // this.screenManager.OpenScreen<ProfileScreenPresenter>();
        }
        private void OnClickSettingButton()
        {
            this.screenManager.OpenScreen<SettingPopupPresenter>();
        }
        private void InitButtonListener()
        {
            this.View.BtnPlay.onClick.AddListener(this.OnClickPlayButton);
            this.View.BtnProfile.onClick.AddListener(this.OnClickProfileButton);
            this.View.BtnSetting.onClick.AddListener(this.OnClickSettingButton);
        }
    }
}