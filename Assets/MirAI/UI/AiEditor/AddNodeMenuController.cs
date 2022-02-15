using System;
using Assets.MirAI.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.MirAI.UI.AiEditor {

    public class AddNodeMenuController : MonoBehaviour, IPointerDownHandler {

        [HideInInspector]
        public UnityEvent OnCancel;
        public SelectNewNodeEvent OnSelect = new SelectNewNodeEvent();

        public void OnPointerDown(PointerEventData eventData) {
            var go = eventData.pointerCurrentRaycast.gameObject;
            try {
                var type = (NodeType)Enum.Parse(typeof(NodeType), go.name);
                OnSelect?.Invoke(type);
                Close();
            }
            catch {
                return;
            }
        }

        public void OnPressCancel() {
            OnCancel?.Invoke();
            Close();
        }

        private void Close() {
            OnCancel.RemoveAllListeners();
            OnSelect.RemoveAllListeners();
            Destroy(gameObject);
        }
    }

    public class SelectNewNodeEvent : UnityEvent<NodeType> { }
}