using Assets.MirAI.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MirAI.UI.AiEditor {

    public class EditProgramName : MenuController {

        [SerializeField] private InputField _field;

        private AiModel _model;
        private Color _normalColor;
        private Color _warningColor = Color.red;
        private Program _program = null;

        private void Awake() {
            _model = AiModel.Instance;
            _normalColor = _field.textComponent.color;
        }

        public override void Start() {
            base.Start();
            if(_program != null)
                _field.text = _program.Name;
            _field.Select();
        }

        public void SetEditProgram(Program program) {
            _program = program;
            _field.text = program.Name;
        }

        public override void OnOkPressed() {
            CheckText(_field.text);
            if (_okButton.interactable == false) return;
            if(_program == null) {
                _model.AddNewProgram(_field.text);
            } else {
                _program.Name = _field.text;
                _model.UpdateProgram(_program);
            }
            base.OnOkPressed();
        }

        public void CheckText(string text) {
            if (IsExist(text)) {
                _okButton.interactable = false;
                _field.textComponent.color = _warningColor;
            }
            else {
                _okButton.interactable = true;
                _field.textComponent.color = _normalColor;
            }
        }

        private bool IsExist(string name) {
            if (string.IsNullOrEmpty(name)) return true;
            var programs = _model.Programs;
            foreach (var program in programs)
                if (program.Name == name)
                    return true;
            return false;
        }
    }
}