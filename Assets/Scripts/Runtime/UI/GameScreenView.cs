namespace Runtime.UI
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using Runtime.PlaySceneLogic;
    using TMPro;
    using UniRx;
    using UnityEngine;
    using Utils;
    using Zenject;

    public class GameScreenView : BaseView
    {
        [SerializeField] private TextMeshProUGUI txtTeamTurn;
        [SerializeField] private TextMeshProUGUI txtWhiteTime;
        [SerializeField] private TextMeshProUGUI txtBlackTime;

        public TextMeshProUGUI TxtTeamTurn  => this.txtTeamTurn;
        public TextMeshProUGUI TxtWhiteTime => this.txtWhiteTime;
        public TextMeshProUGUI TxtBlackTime => this.txtBlackTime;
    }

    [ScreenInfo(nameof(GameScreenView))]
    public class GameScreenPresenter : BaseScreenPresenter<GameScreenView>
    {
        private readonly BoardController boardController;

        private CompositeDisposable compositeDisposable;
        public GameScreenPresenter(SignalBus signalBus, BoardController boardController) : base(signalBus) { this.boardController = boardController; }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.OpenViewAsync();
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
    }
}