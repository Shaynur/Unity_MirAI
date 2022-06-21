using Assets.MirAI.Definitions;
using Assets.MirAI.UI;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MirAI.AiEditor.SelectAction {

    public class SelectActionMenu : MenuController {

        [SerializeField] CommandBtnGroup _actionGroup;
        [SerializeField] CommandBtnGroup _objectGroup;
        [SerializeField] CommandBtnGroup _teamGroup;
        [SerializeField] CommandBtnGroup _paramGroup;
        [SerializeField] CommandBtnGroup _typeGroup;
        [SerializeField] CommandBtnGroup _comparsionGroup;
        [SerializeField] CommandBtnGroup _distanceGroup;
        [SerializeField] CommandBtnGroup _countGroup;
        [SerializeField] CommandBtnGroup _count2Group;
        [SerializeField] Slider _countSlider;

        public readonly CompositeDisposable _trash = new CompositeDisposable();
        Selector ON = Selector.On;
        Selector OFF = Selector.Off;
        Selector SKIP = Selector.NoChange;

        public override void Start() {
            base.Start();
            if (EditNode.Node.Type == Models.NodeType.Action) {
                SetActionMode(true);
                if (ActionContain("DoNothing"))
                    SwitchPanelsVisible("DoNothing");
                else if (ActionContain("Attack"))
                    SwitchPanelsVisible("Attack");
                else SwitchPanelsVisible("GoTo");
            }
            else {
                SetActionMode(false);
                if (ActionContain("Me"))
                    SwitchPanelsVisible("Me");
                else
                    SwitchPanelsVisible("AnyTeam");
                _countSlider.value = (EditNode.Node.Command >> 16) & 0xFF;
            }
        }

        void SetActionMode(bool isActionNode) {
            _actionGroup.gameObject.SetActive(isActionNode);
            _countGroup.gameObject.SetActive(!isActionNode);
            _count2Group.gameObject.SetActive(!isActionNode);
        }

        public void OnBtnClick(SelectCommandButton clickedButton) {
            var action = clickedButton.Action;
            var actionId = action.Id;
            SwitchPanelsVisible(actionId);
        }

        void SwitchPanelsVisible(string actionId) {
            switch (actionId) {
                case "DoNothing":
                    EditNode.Node.Command = 0;
                    SetPanelsVisible(OFF, OFF, OFF, OFF, OFF, OFF, SKIP);
                    break;
                case "GoTo":
                case "GoFrom":
                    SetPanelsVisible(OFF, ON, OFF, OFF, OFF, OFF, SKIP);
                    EditNode.Node.Command = ActionsRepository.I.SetAction(EditNode.Node.Command, "Unit");
                    _objectGroup.Redraw();
                    SwitchPanelsVisible("AnyTeam");
                    break;
                case "Attack": {
                        SetPanelsVisible(ON, SKIP, ON, SKIP, SKIP, SKIP, SKIP);
                        if (ActionContain("Me"))
                            SwitchPanelsVisible("Me");
                        else
                            SwitchPanelsVisible("AnyTeam");
                        break;
                    }
                case "Me": {
                        if (ActionContain("Type"))
                            SetPanelsVisible(ON, OFF, ON, ON, OFF, OFF, OFF);
                        else
                            SetPanelsVisible(ON, OFF, ON, OFF, ON, OFF, OFF);
                        break;
                    }
                case "Unit":
                    if (EditNode.Node.Type == Models.NodeType.Action)
                        SetPanelsVisible(SKIP, ON, SKIP, SKIP, SKIP, ON, OFF);
                    else
                        SetPanelsVisible(SKIP, ON, SKIP, SKIP, SKIP, ON, ON);
                    break;
                case "Enemy":
                case "Ally":
                case "AnyTeam": {
                        if (ActionContain("Type"))
                            SetPanelsVisible(SKIP, ON, ON, ON, OFF, ON, SKIP);
                        else
                            SetPanelsVisible(SKIP, ON, ON, OFF, ON, ON, SKIP);
                        break;
                    }
                case "Type":
                    SetPanelsVisible(SKIP, SKIP, ON, ON, OFF, SKIP, SKIP);
                    break;
                case "HP":
                    SetPanelsVisible(SKIP, SKIP, ON, OFF, ON, SKIP, SKIP);
                    break;
                default:
                    break;
            }
        }

        private bool ActionContain(string id) {
            return ActionsRepository.I.Contain(EditNode.Node.Command, id);
        }

        private void SetPanelsVisible(Selector obj, Selector team, Selector param, Selector type, Selector comp, Selector dist, Selector count) {
            SetGoStatus(_objectGroup.gameObject, obj);
            SetGoStatus(_teamGroup.gameObject, team);
            SetGoStatus(_paramGroup.gameObject, param);
            SetGoStatus(_typeGroup.gameObject, type);
            SetGoStatus(_comparsionGroup.gameObject, comp);
            SetGoStatus(_distanceGroup.gameObject, dist);
            SetGoStatus(_countGroup.gameObject, count);
            SetGoStatus(_count2Group.gameObject, count);
        }

        void SetGoStatus(GameObject go, Selector selector) {
            if (selector == Selector.NoChange)
                return;
            go.SetActive(selector == Selector.On);
        }

        private void OnDestroy() {
            _trash.Dispose();
        }
    }

    enum Selector : int {
        Off = 0,
        On = 1,
        NoChange = 2
    }
}