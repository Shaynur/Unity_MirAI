using Assets.MirAI.Models;
using Assets.MirAI.UI.AiEditor;
using Assets.MirAI.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MirAI.UI.Widgets {

    public class NodeWidget : MonoBehaviour {

        [SerializeField] Text _idText;
        [SerializeField] Text _positionText;

        public EventNodeMove OnMove = new EventNodeMove();
        public EventFromNode OnEndMove = new EventFromNode();
        public EventFromNode OnSelect = new EventFromNode();
        public Node Node;
        private Transform _transform;
        public SelectorController selector;

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
            _idText.text = "Id = " + Node.Id;
            _positionText.text = "(x,y) = " + Node.X + ", " + Node.Y;
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
            if (selector.IsActiv)
                OnSelect?.Invoke(Node);
        }
    }
}