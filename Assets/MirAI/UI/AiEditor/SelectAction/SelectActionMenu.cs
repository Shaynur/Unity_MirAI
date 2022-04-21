using Assets.MirAI.Utils.Disposables;

namespace Assets.MirAI.UI.AiEditor.SelectAction {

    public class SelectActionMenu : MenuController {

        public readonly CompositeDisposable _trash = new CompositeDisposable();
        //private AiModel _model;

        public override void Start() {
            base.Start();
            //_model = AiModel.Instance;
            //_okButton.interactable = false;
        }

        private void OnDestroy() {
            _trash.Dispose();
        }
    }
}