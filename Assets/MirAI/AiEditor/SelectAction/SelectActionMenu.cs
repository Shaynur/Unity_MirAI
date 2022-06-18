using Assets.MirAI.UI;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;

namespace Assets.MirAI.AiEditor.SelectAction {

    public class SelectActionMenu : MenuController {

        [SerializeField] CommandBtnGroup _actionGroup;
        [SerializeField] CommandBtnGroup _objectGroup;
        [SerializeField] CommandBtnGroup _teamGroup;
        [SerializeField] CommandBtnGroup _paramGroup;
        [SerializeField] CommandBtnGroup _typeGroup;
        [SerializeField] CommandBtnGroup _comparsionGroup;
        [SerializeField] CommandBtnGroup _comparsion2Group;
        [SerializeField] CommandBtnGroup _distanceGroup;
        [SerializeField] CommandBtnGroup _comparsionGlobalGroup;
        [SerializeField] CommandBtnGroup _comparsionGlobal2Group;

        public readonly CompositeDisposable _trash = new CompositeDisposable();

        public override void Start() {
            base.Start();
            _actionGroup.enabled = EditNode.Node.Type == Models.NodeType.Action;
        }

        public void OnBtnClick(SelectCommandButton clickedButton) {
            var cmd = clickedButton.Action.Id;
            switch (cmd) {
                case "DoNothing":
                    SwitchPanelsVisible(false, false, false, false, false, false, false);
                    break;
                case "GoTo":
                case "GoFrom":
                    SwitchPanelsVisible(false, true, false, false, false, false, false);
                    break;
                case "Attack":
                    SwitchPanelsVisible(true, false, false, false, false, false, false);
                    break;
                case "Me":
                    SwitchPanelsVisible(true, false, true, true, false, false, false);
                    break;
                case "Unit":
                    SwitchPanelsVisible(true, true, true, true, false, true, true);
                    break;
                case "Type":
                    SwitchPanelsVisible(true, true, true, true, false, true, true);
                    break;
                default:
                    break;
            }
        }

        private void SwitchPanelsVisible(bool obj, bool team, bool param, bool type, bool comp, bool dist, bool gcomp) {
            _objectGroup.enabled = obj;
            _teamGroup.enabled = team;
            _paramGroup.enabled = param;
            _typeGroup.enabled = type;
            _comparsionGroup.enabled = comp;
            _comparsion2Group.enabled = comp;
            _distanceGroup.enabled = dist;
            _comparsionGlobalGroup.enabled = gcomp;
            _comparsionGlobal2Group.enabled = gcomp;
        }

        private void OnDestroy() {
            _trash.Dispose();
        }
    }
}