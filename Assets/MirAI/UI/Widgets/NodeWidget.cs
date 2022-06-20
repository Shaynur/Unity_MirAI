using Assets.MirAI.Definitions;
using Assets.MirAI.Models;
using Assets.MirAI.AiEditor;
using Assets.MirAI.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MirAI.UI.Widgets {

    public class NodeWidget : MonoBehaviour {

        [SerializeField] Text _idText;
        [SerializeField] Text _paramText;
        [SerializeField] Image[] _icons = new Image[4];

        public EventNodeMove OnMove;
        public EventWithNode OnEndMove;
        public EventWithNode OnSelect;
        public EventWithNode OnSubAi;
        public Node Node;
        [HideInInspector]
        public SelectorController selector;
        private Transform _transform;

        private void Start() {
            _transform = GetComponent<Transform>();
            selector = GetComponentInChildren<SelectorController>(true);
            UpdateView();
        }

        public void SetData(Node node) {
            Node = node;
        }

        public void UpdateView() {
            switch (Node.Type) {
                case NodeType.Nope:
                    break;
                case NodeType.Root:
                    UpdateRootView();
                    return;
                case NodeType.Action:
                    UpdateActionView();
                    break;
                case NodeType.Condition:
                    UpdateConditionView();
                    break;
                case NodeType.Connector:
                    break;
                case NodeType.SubAI:
                    UpdateSubAiView();
                    return;
            }
            _idText.text = "Id = " + Node.Id;
        }

        private void UpdateActionView() {
            var conditions = ActionsRepository.I.GetConditions2(Node.Command);
            if (conditions.Length > 0) {
                _icons[3].sprite = conditions[0].Icon;
                if (conditions.Length > 2) {
                    for (int i = 1; i < conditions.Length - 1; i++) {
                        _icons[i - 1].sprite = conditions[i].Icon;
                    }
                }
            }
        }

        private void UpdateConditionView() {
            var conditions = ActionsRepository.I.GetConditions2(Node.Command);
            if (conditions.Length > 0) {
                _icons[3].sprite = conditions[conditions.Length - 1].Icon;
                if (conditions.Length > 2) {
                    for (int i = 1; i < conditions.Length - 1; i++) {
                        _icons[i - 1].sprite = conditions[i].Icon;
                    }
                }
            }
        }

        public void UpdateRootView() {
            _idText.text = AiModel.Instance.Programs.Find(x => x.Id == Node.ProgramId).Name;
        }

        public void UpdateSubAiView() {
            var program = AiModel.Instance.Programs.Find(x => x.Id == Node.Command);
            _idText.text = program == null ? "?" : program.Name;
        }

        public void OnChangePosition() {
            var dx = _transform.position.x - Node.X;
            var dy = _transform.position.y - Node.Y;
            var offset = new Vector3(dx, dy, 0);
            WriteNewPosition();
            OnMove?.Invoke(Node, offset);
        }

        private void WriteNewPosition() {
            Node.X = _transform.position.x;
            Node.Y = _transform.position.y;
        }

        public void ChangePosition(Vector3 offset) {
            _transform.position += offset;
            WriteNewPosition();
        }

        public void SaveToDB() {
            OnEndMove?.Invoke(Node);
        }

        public void SwitchSelector() {
            selector.Toggle();
            OnSelect?.Invoke(Node);
        }

        public void OnPressSubAi() {
            OnSubAi?.Invoke(Node);
        }
    }
}