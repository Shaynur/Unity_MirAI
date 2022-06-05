using Assets.MirAI.Models;
using Assets.MirAI.UI.Widgets;
using Assets.MirAI.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.MirAI.AiEditor {

    public class LowerConnector : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

        private Node _parentNode;

        private void Start() {
            _parentNode = GetComponentInParent<NodeWidget>().Node;
        }

        public void OnBeginDrag(PointerEventData eventData) {
            _parentNode.Widget.gameObject.GetComponent<DragDrop>().IsDragging = true;
            var currentPosition = eventData.pointerCurrentRaycast.worldPosition;
            EditNode.CreateTemplates(currentPosition, _parentNode);
        }

        public void OnDrag(PointerEventData eventData) {
            if (eventData.pointerCurrentRaycast.screenPosition == Vector2.zero)
                return;
            var currentPosition = eventData.pointerCurrentRaycast.worldPosition;
            EditNode.UpdateTemplates(currentPosition);
        }

        public void OnEndDrag(PointerEventData eventData) {
            var go = eventData.pointerCurrentRaycast.gameObject;
            if (go.name == "Node") {
                var child = go.GetComponentInParent<NodeWidget>().Node;
                EditNode.ConnectNodes(_parentNode, child);
            }
            else if (go.name == "Grid")
                EditNode.CreateSelectNodeWindow();
            else
                EditNode.ClearTemplates();
        }
    }
}