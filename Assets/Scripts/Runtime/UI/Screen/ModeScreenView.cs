namespace Runtime.UI.Screen
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class ModeScreenView : BaseView
    {
        [SerializeField]             private Button btnOfflineMode;
        [SerializeField] private Button      btnOnlineMode;

        public Button BtnOfflineMode => this.btnOfflineMode;
        public Button BtnOnlineMode  => this.btnOnlineMode;
    }
    
    [ScreenInfo(nameof(ModeScreenView))]
    public class ModeScreenPresenter : BaseScreenPresenter<ModeScreenView>
    {
        private readonly ChessOnlineSceneDirector sceneDirector;

        public ModeScreenPresenter(SignalBus signalBus, ChessOnlineSceneDirector sceneDirector) : base(signalBus) { this.sceneDirector = sceneDirector; }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.InitButtonListener();
        }

        public override void BindData()
        {
            
        }

        private void InitButtonListener()
        {
            this.View.BtnOfflineMode.onClick.AddListener(this.OnClickOfflineModeButton);
            this.View.BtnOnlineMode.onClick.AddListener(this.OnClickOnlineModeButton);
        }

        private void OnClickOnlineModeButton()
        {
            
        }

        private async void OnClickOfflineModeButton()
        {
            await this.sceneDirector.LoadPlayScene();
            this.CloseView();
        }
    }
}