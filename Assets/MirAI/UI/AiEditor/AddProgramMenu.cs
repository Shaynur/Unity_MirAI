using Assets.MirAI.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MirAI.UI.AiEditor {

    public class AddProgramMenu : MonoBehaviour {

        [SerializeField] private Text _enteredText;
        [SerializeField] private Button _okButton;
        [SerializeField] private Text _hintText;

        private readonly string _normalHint = " *The program name must be unique and no longer than 40 characters.";
        private readonly string _warningHint = " *The program name is not valid! Try entering a different name.";
        private Color _normalColor;
        private Color _warningColor = Color.red;

        private GameSession _session;

        private void Start() {
            _session = GameSession.Instance;
            _normalColor = _hintText.color;
            CheckText(_enteredText.text);
        }

        public void CheckText(string text) {
            if (IsExist(text)) {
                _hintText.text = _warningHint;
                _hintText.color = _warningColor;
                _okButton.interactable = false;
            }
            else {
                _hintText.text = _normalHint;
                _hintText.color = _normalColor;
                _okButton.interactable = true;
            }
        }

        public void OkPressed() {
            CheckText(_enteredText.text);
            if (_okButton.interactable == false) return;
            _session.AiModel.AddNewProgram(_enteredText.text);
            Close();
        }

        private bool IsExist(string name) {
            if (string.IsNullOrEmpty(name)) return true;

            var programs = _session.AiModel.Programs;
            foreach (var program in programs)
                if (program.Name == name)
                    return true;
            return false;
        }

        public void Close() {
            Destroy(gameObject);
        }
    }
}