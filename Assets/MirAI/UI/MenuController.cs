using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.MirAI.UI {

    public class MenuController : MonoBehaviour {

        [SerializeField] protected Button _okButton;
        [SerializeField] protected Button _cancelButton;

        private Animator _animator;
        private static readonly int Show = Animator.StringToHash("Show");
        private static readonly int Hide = Animator.StringToHash("Hide");

        public UnityEvent OnOk;
        public UnityEvent OnCancel;

        public virtual void Start() {
            _animator = GetComponent<Animator>();
            _animator.SetTrigger(Show);

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
            _animator.SetTrigger(Hide);
        }

        public virtual void OnCloseAnimationComplete() {
            if (_okButton != null)
                _okButton.onClick.RemoveListener(OnOkPressed);
            if (_cancelButton != null)
                _cancelButton.onClick.RemoveListener(OnCancelPressed);
            OnCancel?.RemoveAllListeners();
            OnOk?.RemoveAllListeners();
            Destroy(gameObject);
        }
    }
}