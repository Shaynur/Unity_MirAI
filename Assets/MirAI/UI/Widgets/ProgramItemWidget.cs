using Assets.MirAI.Models;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.MirAI.UI.Widgets {

    [RequireComponent(typeof(Text))]
    public class ProgramItemWidget : MonoBehaviour {

        [SerializeField] Text _text;
        [SerializeField] private Color _defaultColor = Color.gray;
        [SerializeField] private Color _selectColor = Color.yellow;

        public Program Program;
        public ItemClick ItemClicked = new ItemClick();
        private Button _button;

        private void Start() {
            if (_text.color != _selectColor)
                Select(false);
            _button = GetComponent<Button>();
            _button.onClick.Subscribe(OnClick);
        }

        public void Select(bool selected) {
            _text.color = selected ? _selectColor : _defaultColor;
        }

        public void Set(Program program) {
            Program = program;
            _text.text = Program.Name;
        }

        public void OnClick() {
            ItemClicked?.Invoke(this);
        }

        private void OnDestroy() {
            _button.onClick.RemoveListener(OnClick);
        }
    }

    public class ItemClick : UnityEvent<ProgramItemWidget> { }
}