using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.MirAI.Utils {

    public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {

        [SerializeField] UnityEvent _onDrag;
        [SerializeField] UnityEvent _onEndDrag;
        
        private CanvasGroup _canvasGroup;
        private Vector3 _pressPosition;
        

        private void Awake() {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData) {
            if (_canvasGroup != null)
                _canvasGroup.alpha = .6f;
            _pressPosition = eventData.pointerPressRaycast.worldPosition - transform.position;
        }

        public void OnDrag(PointerEventData eventData) {
            if (eventData.pointerCurrentRaycast.screenPosition == Vector2.zero)
                return;
            var currentPosition = eventData.pointerCurrentRaycast.worldPosition;
            transform.position = currentPosition - _pressPosition;
            _onDrag?.Invoke();
        }

        public void OnEndDrag(PointerEventData eventData) {
            if (_canvasGroup != null)
                _canvasGroup.alpha = 1f;
            _onEndDrag?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData) {
        }
    }
}