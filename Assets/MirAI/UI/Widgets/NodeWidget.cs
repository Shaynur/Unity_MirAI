using Assets.MirAI.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.MirAI.UI.Widgets {

    public class NodeWidget : MonoBehaviour {

        [SerializeField] Text _idText;
        [SerializeField] Text _positionText;

        public EventFromGO OnMove = new EventFromGO();
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
            Node.X = (int)_transform.position.x;
            Node.Y = (int)_transform.position.y;
            UpdateView();
            OnMove?.Invoke(gameObject);
        }

        public void ChangePosition(Vector3 offset) {
            _transform.position += offset;
            OnChangePosition();
        }

        public void SaveToDB() {
            _session.AiModel.UpdateNode(Node);
        }
    }

    public class EventFromGO : UnityEvent<GameObject> { }
}