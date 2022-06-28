using Assets.MirAI.AiEditor;
using Assets.MirAI.Models;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MirAI.Simulation {

    [RequireComponent(typeof(Button))]
    public class EditUnitButton : MonoBehaviour {

        [SerializeField] private UnitType _type;
        [SerializeField] private UnitTeam _team;
        [SerializeField] private Image _borderImage;
        private Button _button;
        private EditUnitMenu _menu;

        private void Awake() {
            _button = GetComponent<Button>();
            _button.onClick.Subscribe(OnClick);
            _menu = GetComponentInParent<EditUnitMenu>();
            SelectByUnitData();
        }

        public void OnClick() {
            if (_type != 0)
                EditUnit.Unit.Type = _type;
            if (_team != 0)
                EditUnit.Unit.Team = _team;
            _menu.RedrawButtonsSelectors();
        }

        public void SelectByUnitData() {
            Select(EditUnit.Unit.Type == _type || EditUnit.Unit.Team == _team);
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