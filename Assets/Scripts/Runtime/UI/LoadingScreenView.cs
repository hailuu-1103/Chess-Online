namespace Runtime.UI
{
    using DG.Tweening;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class LoadingScreenView : BaseView
    {
        [SerializeField] private Slider sliderLoading;

        public Slider SliderLoading => this.sliderLoading;
    }
    [ScreenInfo(nameof(LoadingScreenView))]
    public class LoadingScreenPresenter: BaseScreenPresenter<LoadingScreenView>
    {
        private readonly ChessOnlineSceneDirector sceneDirector;
        private const    float                    FakeLoadingTime = 5;
        public LoadingScreenPresenter(SignalBus signalBus, ChessOnlineSceneDirector sceneDirector) : base(signalBus) { this.sceneDirector = sceneDirector; }

        public override void BindData()
        {
            this.View.SliderLoading.DOValue(0, FakeLoadingTime).SetEase(Ease.Linear).OnComplete(this.LoadMainScene);
        }

        protected override async void OnViewReady()
        {
            base.OnViewReady();
            await this.OpenViewAsync();
        }

        private async void LoadMainScene()
        {
        }
    }
}