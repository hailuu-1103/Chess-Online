namespace Runtime.UI
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using Runtime.PlaySceneLogic.ChessPiece;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Serialization;
    using UnityEngine.UI;
    using Zenject;

    public enum GameResultStatus
    {
        Win,
        Lose,
        Draw
    }

    public class GameResultPopupModel
    {
        public PieceTeam        WinTeam      { get; set; }
        public GameResultStatus ResultStatus { get; set; }
        public string           ResultCause  { get; set; }

        public GameResultPopupModel(PieceTeam winTeam, GameResultStatus resultStatus, string resultCause)
        {
            this.WinTeam      = winTeam;
            this.ResultStatus = resultStatus;
            this.ResultCause  = resultCause;
        }
    }

    public class GameResultPopupView : BaseView
    {
        [SerializeField]                                    private TextMeshProUGUI txtTeam;
        [SerializeField]                                    private TextMeshProUGUI txtGameResult;
        [SerializeField]                                    private TextMeshProUGUI txtResultCause;
        [SerializeField] private Button          btnRematch;

        public Button BtnRematch                                                    => this.btnRematch;

        public void SetView(string team, string gameResult, string resultCause)
        {
            this.txtTeam.text        = team;
            this.txtGameResult.text  = gameResult;
            this.txtResultCause.text = resultCause;
        }
    }

    [PopupInfo(nameof(GameResultPopupView))]
    public class GameResultPopupPresenter : BasePopupPresenter<GameResultPopupView, GameResultPopupModel>
    {
        private GameResultPopupModel model;
        public GameResultPopupPresenter(SignalBus signalBus, ILogService logService) : base(signalBus, logService) { }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.InitButtonListener();
        }

        public override void BindData(GameResultPopupModel popupModel)
        {
            this.model = popupModel;
            this.InitView();
        }

        private void InitView() { this.View.SetView(this.model.WinTeam.ToString(), this.model.ResultStatus.ToString(), this.model.ResultCause); }

        private void InitButtonListener() { this.View.BtnRematch.onClick.AddListener(this.OnClickRematchButton); }

        private void OnClickRematchButton() { this.logService.LogWithColor("Implement rematch game here!", Color.green); }
    }
}