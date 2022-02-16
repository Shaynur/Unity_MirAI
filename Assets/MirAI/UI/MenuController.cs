using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.MirAI.UI {

    public class MenuController : MonoBehaviour {

        [SerializeField] protected Button _okButton;
        [SerializeField] protected Button _cancelButton;

        public UnityEvent OnOk;
        public UnityEvent OnCancel;

        public virtual void Start() {
            if (_okButton != null)
                _okButton.onClick.AddListener(OnOkPressed);
            if(_cancelButton != null)
                _cancelButton.onClick.AddListener(OnCancelPressed);
        }

        public virtual void OnOkPressed() {
            OnOk?.Invoke();
            Close();
        }

        public virtual void OnCancelPressed() {
            OnCancel?.Invoke();
            Close();
        }

        public virtual void Close() {
            _okButton?.onClick.RemoveListener(OnOkPressed);
            _cancelButton?.onClick.RemoveListener(OnCancelPressed);
            OnCancel?.RemoveAllListeners();
            OnOk?.RemoveAllListeners();
            Destroy(gameObject);
        }
    }
}