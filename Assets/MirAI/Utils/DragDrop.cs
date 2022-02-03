using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.MirAI.Utils {

    public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {

        [SerializeField] private Canvas _canvas;

        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;

        private void Awake() {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData) {
            if (_canvasGroup != null)
                _canvasGroup.alpha = .6f;
        }

        public void OnDrag(PointerEventData eventData) {
            //_rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
            _rectTransform.anchoredPosition += eventData.delta / _canvas.transform.localScale.x;
        }

        public void OnEndDrag(PointerEventData eventData) {
            if (_canvasGroup != null)
                _canvasGroup.alpha = 1f;
        }

        public void OnPointerDown(PointerEventData eventData) {
            //Debug.Log("OnPointerDown");
        }
    }
}