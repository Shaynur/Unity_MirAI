using Assets.MirAI.Definitions;
using Assets.MirAI.Utils;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MirAI.AiEditor.SelectAction {

    [RequireComponent(typeof(Button))]
    public class SelectCommandButton : MonoBehaviour {

        [Actions][SerializeField] private string _actionString;
        [SerializeField] private Image _borderImage;

        public ActionsDef Action => ActionsRepository.I.Get(_actionString);

        public CommandBtnClickEvent CommandBtnClicked { get; set; } = new CommandBtnClickEvent();
        private Button _button;

        private void Awake() {
            _button = GetComponent<Button>();
            _button.onClick.Subscribe(OnClick);

            var action = Action;
            int c = EditNode.Node.Command & action.CommandMask;
            Select(c == action.Command);

            var actionIcon = GetComponent<Image>();
            if (actionIcon != null)
                actionIcon.sprite = action.Icon;
        }

        public void OnClick() {
            CommandBtnClicked?.Invoke(this);
        }

        public void Select(bool selected) {
            if (_borderImage != null)
                _borderImage.color = selected ? Color.yellow : Color.green;
        }

        private void OnDestroy() {
            _button.onClick.RemoveListener(OnClick);
        }
    }
}