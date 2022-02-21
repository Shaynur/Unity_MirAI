using System;
using Assets.MirAI.Models;
using Assets.MirAI.Utils;
using Assets.MirAI.Utils.Disposables;
using UnityEngine.EventSystems;

namespace Assets.MirAI.UI.AiEditor {

    public class AddNodeMenuController : MenuController, IPointerDownHandler {

        public void OnPointerDown(PointerEventData eventData) {
            var go = eventData.pointerCurrentRaycast.gameObject;
            try {
                var type = (NodeType)Enum.Parse(typeof(NodeType), go.name);
                EditNode.Node.Type = type;
                switch (type) {
                    case NodeType.SubAI:
                        var menu = WindowUtils.CreateWindow("SelectSubAi", "HUD");
                        var controller = menu.GetComponent<SelectSubAiMenu>();
                        controller.OnCancel.Subscribe(OnCancelPressed);
                        controller.OnOk.Subscribe(OnOkPressed);
                        return;
                    default:
                        break;
                }
                OnOk?.Invoke();
                Close();
            }
            catch {
                return;
            }
        }
    }
}