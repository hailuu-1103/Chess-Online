namespace Runtime.UI
{
    using Assets.Scripts.Runtime.PlaySceneLogic.ChessPiece;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using Runtime.PlaySceneLogic;
    using TMPro;
    using UniRx;
    using UnityEngine;
    using UnityEngine.UI;
    using Utils;
    using Zenject;

    public class GameScreenView : BaseView
    {
        [SerializeField] private TextMeshProUGUI    txtTeamTurn;
        [SerializeField] private TextMeshProUGUI    txtWhiteTime;
        [SerializeField] private TextMeshProUGUI    txtBlackTime;
        [SerializeField] private Button             startButton;
        [SerializeField] private Button             continueButton;
        [SerializeField] private GameObject panelStart;

        public TextMeshProUGUI TxtTeamTurn  => this.txtTeamTurn;
        public TextMeshProUGUI TxtWhiteTime => this.txtWhiteTime;
        public TextMeshProUGUI TxtBlackTime => this.txtBlackTime;
        public Button StartButton => this.startButton;
        public Button ContinueButton => this.continueButton;
        public GameObject PanelStart => this.panelStart;

    }

    [ScreenInfo(nameof(GameScreenView))]
    public class GameScreenPresenter : BaseScreenPresenter<GameScreenView>
    {
        private readonly BoardController boardController;
        private readonly FileManager fileManager;

        private CompositeDisposable compositeDisposable;
        public GameScreenPresenter(SignalBus signalBus, BoardController boardController, FileManager fileManager) : base(signalBus) { 
            this.boardController = boardController; 
            this.fileManager = fileManager;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.OpenViewAsync();
            this.InitButtonListener();
        }

        private void InitButtonListener()
        {
            this.View.StartButton.onClick.AddListener(this.StartNewGame);
            this.View.ContinueButton.onClick.AddListener(this.ContinuePlay);
        }

        public override void BindData()
        {
            this.compositeDisposable = new CompositeDisposable();
            this.InitView();
            this.compositeDisposable.Add(this.boardController.playerBlackTimeRemaining.Subscribe(time => { this.View.TxtBlackTime.SetRemainTime(time); }));
            this.compositeDisposable.Add(this.boardController.playerWhiteTimeRemaining.Subscribe(time => { this.View.TxtWhiteTime.SetRemainTime(time); }));
        }

        private void InitView() { this.compositeDisposable.Add(this.boardController.isWhiteTurn.Subscribe(isWhiteTurn => { this.View.TxtTeamTurn.text = isWhiteTurn ? "White" : "Black"; })); }

        public override void Dispose()
        {
            base.Dispose();
            this.compositeDisposable?.Dispose();
        }

        public void StartNewGame()
        {
            this.boardController.isPlaying = true;
            this.boardController.StartNewGame();
            this.View.PanelStart.SetActive(false);
        }

        public void ContinuePlay()
        {
            GameLog game = this.fileManager.getLastGame();
            if (game != null)
            {
                this.boardController.chessId = this.fileManager.getLastGame().Id;
            }
            else
            {
                this.boardController.chessId = "";
            }
            this.boardController.isPlaying = true;
            this.boardController.StartNewGame();
            this.View.PanelStart.SetActive(false);
        }
    }
}