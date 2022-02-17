using Assets.MirAI.Models;
using Assets.MirAI.UI.Widgets;
using Assets.MirAI.Utils;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.MirAI.UI.AiEditor {

    public class LowerConnectorController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

        private Node _parentNode;
        private Node _tempNode;
        private Link _tempLink;
        private GameSession _session;

        private void Start() {
            _parentNode = GetComponentInParent<NodeWidget>().Node;
            _session = GameSession.Instance;
        }

        public void OnBeginDrag(PointerEventData eventData) {
            _parentNode.Widget.gameObject.GetComponent<DragDrop>().IsDragging = true; // ***
            var currentPosition = eventData.pointerCurrentRaycast.worldPosition;
            CreateTemplates(currentPosition);
        }

        public void OnDrag(PointerEventData eventData) {
            if (eventData.pointerCurrentRaycast.screenPosition == Vector2.zero)
                return;
            var currentPosition = eventData.pointerCurrentRaycast.worldPosition;
            UpdateTemplates(currentPosition);
        }

        public void OnEndDrag(PointerEventData eventData) {
            var go = eventData.pointerCurrentRaycast.gameObject;
            if (go.name == "Node") {
                var child = go.GetComponentInParent<NodeWidget>().Node;
                ConnectNodes(child);
            }
            else if (go.name == "Grid")
                CreateSelectNodeWindow();
            else
                ClearTemplates();
        }

        private void CreateTemplates(Vector3 position) {
            _tempNode = new Node { X = position.x, Y = position.y };
            _tempLink = new Link(_parentNode, _tempNode);
            EditorPartsFactory.I.SpawnLink(_tempLink);
        }

        private void UpdateTemplates(Vector3 position) {
            _tempNode.X = position.x;
            _tempNode.Y = position.y;
            _tempLink.Widget.UpdateView();
        }

        private void SaveDbTemplates(NodeType type) {
            _tempNode.Type = type;
            if (_session.AiModel.AddLinkAndChildNode(_tempLink))
                EditorPartsFactory.I.SpawnNode(_tempNode);
            else
                ClearTemplates();
        }

        private void ClearTemplates() {
            Destroy(_tempLink.Widget.gameObject);
            _tempLink = null;
            _tempNode = null;
        }

        private void ConnectNodes(Node child) {
            _tempLink.NodeTo = child;
            _tempLink.ToId = child.Id;
            if (_session.AiModel.AddLink(_tempLink)) {
                _parentNode.AddChild(_tempLink,child);
                _tempLink.Widget.UpdateView();
            }
            else
                ClearTemplates();
        }

        private void CreateSelectNodeWindow() {
            var menu = WindowUtils.CreateWindow("AddNodeMenu", "HUD");
            var addMenuController = menu.GetComponent<AddNodeMenuController>();
            addMenuController.OnCancel.Subscribe(ClearTemplates);
            addMenuController.OnSelect.Subscribe(SaveDbTemplates);
        }
    }
}