using UnityEngine;

namespace Assets.MirAI.AiEditor {

    public class SelectorController : MonoBehaviour {

        public bool IsActiv => gameObject.activeSelf;

        public void Toggle() {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        public void SetState(bool state) {
            gameObject.SetActive(state);
        }
    }
}