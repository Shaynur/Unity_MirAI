using Assets.MirAI.Models;
using Assets.MirAI.UI.AiEditor;
using Assets.MirAI.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MirAI.UI.Widgets {

    public class NodeWidget : MonoBehaviour {

        [SerializeField] Text _idText;

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
            UpdateView();
        }

        public void UpdateView() {
            switch (Node.Type) {
                case NodeType.Nope:
                    break;
                case NodeType.Root:
                    UpdateRootView();
                    return;
                case NodeType.Action:
                    break;
                case NodeType.Condition:
                    break;
                case NodeType.Connector:
                    break;
                case NodeType.SubAI:
                    UpdateSubAiView();
                    return;
            }
            _idText.text = "Id = " + Node.Id;
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
            UpdateView();
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
            //if (selector.IsActiv)
            OnSelect?.Invoke(Node);
        }

        public void OnPressSubAi() {
            OnSubAi?.Invoke(Node);
        }
    }
}