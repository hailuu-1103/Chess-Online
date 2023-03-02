namespace Runtime.UI.Screen
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using Runtime.PlaySceneLogic;
    using TMPro;
    using UniRx;
    using UnityEngine;
    using Zenject;

    public class GameScreenView : BaseView
    {
        [SerializeField] private TextMeshProUGUI txtTeamTurn;

        public TextMeshProUGUI TxtTeamTurn => this.txtTeamTurn;
    }

    [ScreenInfo(nameof(GameScreenView))]
    public class GameScreenPresenter : BaseScreenPresenter<GameScreenView>
    {
        private readonly BoardController boardController;

        private CompositeDisposable compositeDisposable = new();
        public GameScreenPresenter(SignalBus signalBus, BoardController boardController) : base(signalBus) { this.boardController = boardController; }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.OpenViewAsync();
        }

        public override void BindData()
        {
            this.InitView();
        }

        private void InitView()
        {
            this.compositeDisposable.Add(this.boardController.isWhiteTurn.Subscribe(isWhiteTurn => { this.View.TxtTeamTurn.text = isWhiteTurn ? "White" : "Black"; }));
        }

        public override void Dispose()
        {
            base.Dispose();
            this.compositeDisposable?.Dispose();
        }
    }
}