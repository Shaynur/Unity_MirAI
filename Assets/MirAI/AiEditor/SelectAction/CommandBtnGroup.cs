using Assets.MirAI.Utils.Disposables;
using UnityEngine;

namespace Assets.MirAI.AiEditor.SelectAction {

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
            var action = clickedButton.Action;
            EditNode.Node.Command &= ~action.CommandMask;
            EditNode.Node.Command |= action.Command;
        }


        private void OnDestroy() {
            _trash.Dispose();
        }
    }
}