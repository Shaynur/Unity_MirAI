using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.MirAI.UI {

    public class ContentMenu : MonoBehaviour, IPointerDownHandler {

        public void OnPointerDown(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Right) {
                Debug.Log("Right mb pressed");
            }
        }
    }
}