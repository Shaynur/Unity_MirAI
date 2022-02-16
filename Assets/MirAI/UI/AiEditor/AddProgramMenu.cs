using Assets.MirAI.Models;
using Assets.MirAI.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MirAI.UI.AiEditor {

    public class AddProgramMenu : MenuController {

        [SerializeField] private UnityEventString _enterNameEvent;
        [SerializeField] private InputField _field;
        [SerializeField] private Text _enteredText;

        public UnityEventString EnterNameEvent => _enterNameEvent;

        private GameSession _session;
        private Color _normalColor;
        private Color _warningColor = Color.red;

        public override void Start() {
            base.Start();
            _session = GameSession.Instance;
            _normalColor = _enteredText.color;
            CheckText(_enteredText.text);
            _field.Select();
        }

        public override void OnOkPressed() {
            CheckText(_enteredText.text);
            if (_okButton.interactable == false) return;
            _enterNameEvent?.Invoke(_enteredText.text);
            base.OnOkPressed();
        }

        public void CheckText(string text) {
            if (IsExist(text)) {
                _okButton.interactable = false;
                _enteredText.color = _warningColor;
            }
            else {
                _okButton.interactable = true;
                _enteredText.color = _normalColor;
            }
        }

        private bool IsExist(string name) {
            if (string.IsNullOrEmpty(name)) return true;
            var programs = _session.AiModel.Programs;
            foreach (var program in programs)
                if (program.Name == name)
                    return true;
            return false;
        }
    }
}