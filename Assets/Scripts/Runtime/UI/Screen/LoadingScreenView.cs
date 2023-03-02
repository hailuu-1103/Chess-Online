namespace Runtime.UI.Screen
{
    using System.Collections;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class LoadingScreenView : BaseView
    {
        [SerializeField] private Slider          sldLoading;
        [SerializeField] private TextMeshProUGUI txtPercent;

        public TextMeshProUGUI TxtPercent => this.txtPercent;
        public Slider          SldLoading => this.sldLoading;
    }

    [ScreenInfo(nameof(LoadingScreenView))]
    public class LoadingScreenPresenter : BaseScreenPresenter<LoadingScreenView>
    {
        private readonly ChessOnlineSceneDirector sceneDirector;
        private const    float                    FakeLoadingTime = 5;
        public LoadingScreenPresenter(SignalBus signalBus, ChessOnlineSceneDirector sceneDirector) : base(signalBus) { this.sceneDirector = sceneDirector; }

        public override async void BindData()
        {
            await UniTask.WhenAll(this.FakeLoadingCoroutine().ToUniTask());
            this.LoadMainScene();
        }

        protected override async void OnViewReady()
        {
            base.OnViewReady();
            await this.OpenViewAsync();
        }

        private IEnumerator FakeLoadingCoroutine()
        {
            var percent = 0;
            while (percent < 100)
            {
                this.ChangeProgressSlideValue(percent);
                yield return new WaitForSeconds(0.02f);
                percent++;
            }
        }

        private void ChangeProgressSlideValue(int percent)
        {
            this.View.SldLoading.value = percent / 100f;
            this.View.TxtPercent.text  = percent + "%";
        }

        private void LoadMainScene()
        {
            this.sceneDirector.LoadMainScene();
        }
    }
}