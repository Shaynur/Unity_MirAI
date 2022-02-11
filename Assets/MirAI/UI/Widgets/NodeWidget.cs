using Assets.MirAI.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.MirAI.UI.Widgets {

    public class NodeWidget : MonoBehaviour {

        [SerializeField] Text _idText;
        [SerializeField] Text _positionText;

        public EventNodeMove OnMove = new EventNodeMove();
        public EventFromNode OnEndMove = new EventFromNode();
        public Node Node;
        private GameSession _session;
        private Transform _transform;

        private void Start() {
            _session = GameSession.Instance;
            _transform = GetComponent<Transform>();
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

        public void WriteNewPosition() {
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
    }

    public class EventNodeMove : UnityEvent<Node, Vector3> { }
    public class EventFromNode : UnityEvent<Node> { }
}