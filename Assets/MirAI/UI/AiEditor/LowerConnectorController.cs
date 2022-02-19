using Assets.MirAI.Models;
using Assets.MirAI.UI.Widgets;
using Assets.MirAI.Utils;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.MirAI.UI.AiEditor {

    public class LowerConnectorController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

        public static Link TempConnectorLink { get; private set; }
        private Node _parentNode;
        private AiModel _model;

        private void Start() {
            _model= AiModel.Instance;
            _parentNode = GetComponentInParent<NodeWidget>().Node;
        }

        public void OnBeginDrag(PointerEventData eventData) {
            _parentNode.Widget.gameObject.GetComponent<DragDrop>().IsDragging = true;
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
            TempConnectorLink = new Link(_parentNode, new Node { X = position.x, Y = position.y });
            EditorPartsFactory.I.SpawnLink(TempConnectorLink);
        }

        private void UpdateTemplates(Vector3 position) {
            TempConnectorLink.NodeTo.X = position.x;
            TempConnectorLink.NodeTo.Y = position.y;
            TempConnectorLink.Widget.UpdateView();
        }

        private void SaveDbTemplates() {
            if (_model.AddLinkAndChildNode(TempConnectorLink))
                EditorPartsFactory.I.SpawnNode(TempConnectorLink.NodeTo);
            else
                ClearTemplates();
        }

        private void ClearTemplates() {
            Destroy(TempConnectorLink.Widget.gameObject);
            TempConnectorLink.NodeTo = null;
            TempConnectorLink = null;
        }

        private void ConnectNodes(Node child) {
            TempConnectorLink.NodeTo = child;
            TempConnectorLink.ToId = child.Id;
            if (_model.AddLink(TempConnectorLink)) {
                _parentNode.AddChild(TempConnectorLink, child);
                TempConnectorLink.Widget.UpdateView();
            }
            else
                ClearTemplates();
        }

        private void CreateSelectNodeWindow() {
            var menu = WindowUtils.CreateWindow("AddNodeMenu", "HUD");
            var addMenuController = menu.GetComponent<AddNodeMenuController>();
            addMenuController.OnCancel.Subscribe(ClearTemplates);
            addMenuController.OnOk.Subscribe(SaveDbTemplates);
        }
    }
}