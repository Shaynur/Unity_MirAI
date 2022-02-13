using System;
using Assets.MirAI.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.MirAI.UI.AiEditor {

    public class AddNodeMenuController : MonoBehaviour, IPointerDownHandler {

        public UnityEvent OnCancel;
        public SelectNewNodeEvent OnSelect = new SelectNewNodeEvent();

        public void OnPointerDown(PointerEventData eventData) {
            var go = eventData.pointerCurrentRaycast.gameObject;
            if (go.name == "Cancel") {
                OnCancel?.Invoke();
                Close();
                return;
            }

            try {
                var type = (NodeType)Enum.Parse(typeof(NodeType), go.name);
                OnSelect?.Invoke(type);
                Close();
            }
            catch {
                return;
            }
        }

        private void Close() {
            OnCancel.RemoveAllListeners();
            OnSelect.RemoveAllListeners();
            Destroy(gameObject);
        }
    }

    public class SelectNewNodeEvent : UnityEvent<NodeType> { }
}