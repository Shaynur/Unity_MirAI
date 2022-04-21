using Assets.MirAI.Utils.Disposables;
using UnityEngine;

namespace Assets.MirAI.UI.AiEditor.SelectAction {

    public class CommandBtnGroup : MonoBehaviour {

        [SerializeField] private SelectCommandButton[] _actionButtons;

        public readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start() {
            foreach (var button in _actionButtons) {
                _trash.Retain(button.CommandBtnClicked.Subscribe(OnBtnClick));
            }
        }

        public void OnBtnClick(SelectCommandButton clickedButton) {
            foreach (var button in _actionButtons) {
                button.Select(clickedButton == button);
            }
            EditNode.Node.Command &= ~clickedButton.CommandMask;
            EditNode.Node.Command |= clickedButton.Command;
        }


        private void OnDestroy() {
            _trash.Dispose();
        }
    }
}