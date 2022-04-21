using Assets.MirAI.Utils;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MirAI.UI.AiEditor.SelectAction {

    [RequireComponent(typeof(Button))]
    public class SelectCommandButton : MonoBehaviour {

        [HexInt(digits = 8)][SerializeField] private int _commandMask;
        [HexInt(digits = 8)][SerializeField] private int _command;

        public int CommandMask => _commandMask;
        public int Command => _command;
        public CommandBtnClickEvent CommandBtnClicked { get; set; } = new CommandBtnClickEvent();
        private Image _image;
        private Button _button;

        private void Start() {
            _button = GetComponent<Button>();
            _image = GetComponent<Image>();
            _button.onClick.Subscribe(OnClick);
            int c = EditNode.Node.Command & _commandMask;
            Select(c == _command);
        }
        public void OnClick() {
            CommandBtnClicked?.Invoke(this);
        }

        public void Select(bool selected) {
            _image.color = selected ? Color.yellow : Color.green;
        }

        private void OnDestroy() {
            _button.onClick.RemoveListener(OnClick);
        }
    }
}