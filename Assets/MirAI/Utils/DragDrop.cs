using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.MirAI.Utils {

    public class DragDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {

        [SerializeField] UnityEvent _onClick;
        [SerializeField] UnityEvent _onDrag;
        [SerializeField] UnityEvent _onEndDrag;

        private CanvasGroup _canvasGroup;
        private Vector3 _lokalPressPosition;
        public bool IsDragging;


        private void Awake() {
            _canvasGroup = GetComponent<CanvasGroup>();
            IsDragging = false;
        }

        public void OnBeginDrag(PointerEventData eventData) {
            IsDragging = true;
            if (_canvasGroup != null)
                _canvasGroup.alpha = .6f;
            _lokalPressPosition = eventData.pointerPressRaycast.worldPosition - transform.position;
        }

        public void OnDrag(PointerEventData eventData) {
            if (eventData.pointerCurrentRaycast.screenPosition == Vector2.zero)
                return;
            transform.position = eventData.pointerCurrentRaycast.worldPosition - _lokalPressPosition;
            _onDrag?.Invoke();
        }

        public void OnEndDrag(PointerEventData eventData) {
            if (_canvasGroup != null)
                _canvasGroup.alpha = 1f;
            _onEndDrag?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData) {
            if (!IsDragging)
                _onClick?.Invoke();
            IsDragging = false;
        }

        public void OnPointerDown(PointerEventData eventData) {
        }
    }
}