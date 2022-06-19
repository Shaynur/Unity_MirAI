using System;
using Assets.MirAI.Models;
using Assets.MirAI.Utils;
using UnityEngine.EventSystems;
using Assets.MirAI.UI;

namespace Assets.MirAI.AiEditor {

    public class AddNodeMenuController : MenuController, IPointerDownHandler {

        public void OnPointerDown(PointerEventData eventData) {
            var go = eventData.pointerCurrentRaycast.gameObject;
            try {
                var type = (NodeType)Enum.Parse(typeof(NodeType), go.name);
                EditNode.Node.Type = type;
                switch (type) {
                    case NodeType.SubAI: {
                            WindowUtils.CreateMenuWindow(
                                "UI/SelectSubAi",
                                "HUD",
                                OnOkPressed,
                                OnCancelPressed);
                            return;
                        }
                    case NodeType.Action: {
                            WindowUtils.CreateMenuWindow(
                                "UI/SelectAction2",
                                "HUD",
                                OnOkPressed,
                                OnCancelPressed);
                            return;
                        }
                    case NodeType.Condition: {
                            WindowUtils.CreateMenuWindow(
                                "UI/SelectAction2",
                                "HUD",
                                OnOkPressed,
                                OnCancelPressed);
                            return;
                        }
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