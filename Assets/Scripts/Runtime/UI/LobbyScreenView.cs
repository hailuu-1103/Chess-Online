namespace Runtime.UI
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class LobbyScreenView : BaseView
    {
        [SerializeField] private TMP_InputField inputCreate;
        [SerializeField] private TMP_InputField inputRoom;
        [SerializeField] private Button         btnCreate;
        [SerializeField] private Button         btnRoom;

        public TMP_InputField InputCreate => this.inputCreate;
        public TMP_InputField InputRoom   => this.inputRoom;
        public Button         BtnCreate   => this.btnCreate;
        public Button         BtnRoom     => this.btnRoom;
    }

    [ScreenInfo(nameof(LobbyScreenView))]
    public class LobbyScreenPresenter : BaseScreenPresenter<LobbyScreenView>
    {
        public LobbyScreenPresenter(SignalBus signalBus) : base(signalBus) { }

        public override    void BindData()    { }

        protected override async void OnViewReady()
        {
            base.OnViewReady();
            await this.OpenViewAsync();
        }
    }
}