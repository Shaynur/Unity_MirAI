using System;
using Assets.MirAI.Models;
using Assets.MirAI.Utils;
using UnityEngine.EventSystems;

namespace Assets.MirAI.UI.AiEditor {

    public class AddNodeMenuController : MenuController, IPointerDownHandler {

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
    }
}