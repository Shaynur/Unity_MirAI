using UnityEngine;
using UnityEngine.Events;

namespace Assets.MirAI.UI.AiEditor {

    public class DeleteProgramMenu : MonoBehaviour {

        public UnityEvent OnOkButton = new UnityEvent();

        public void OkButtonPressed() {
            OnOkButton?.Invoke();
            Close();
        }

        public void Close() {
            OnOkButton.RemoveAllListeners();
            Destroy(gameObject);
        }
    }
}