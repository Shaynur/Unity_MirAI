using Assets.MirAI.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.MirAI.UI.Widgets {

    public class NodeWidget : MonoBehaviour {

        [SerializeField] Text _idText;
        [SerializeField] Text _positionText;

        public UnityEvent OnMove;
        private GameSession _session;
        private Node _node;
        private Transform _transform;

        private void Start() {
            _session = GameSession.Instance;
            _transform = GetComponent<Transform>();
            UpdateView();
        }

        public void SetData(Node node) {
            _node = node;
            UpdateView();
        }

        public void UpdateView() {
            _idText.text = "Id = " + _node.Id;
            _positionText.text = "(x,y) = " + _node.X + ", " + _node.Y;
        }

        public void OnChangePosition() {
            _node.X = (int)_transform.position.x;
            _node.Y = (int)_transform.position.y;
            UpdateView();
            OnMove?.Invoke();
        }

        public void ChangePosition(Vector3 offset) {
            _transform.position += offset;
            OnChangePosition();
        }

        public void SaveToDB() {
            _session.AiModel.UpdateNode(_node);
        }
    }
}