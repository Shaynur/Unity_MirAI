using Assets.MirAI.UI.Widgets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.MirAI.UI {

    [RequireComponent(typeof(Canvas))]
    public class CanvasCameraController : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler {

        [SerializeField] private Camera _camera;
        [SerializeField] private UnityEvent _onClick;

        private RectTransform _canvasRectTransform;
        private float _newCameraSize;
        private Vector2 _cameraFlyDirection;
        private readonly float _cameraResizeSpeed = 5f;
        private readonly float _cameraFlyInertia = 1.05f;
        private readonly float _deltaMoveCameraDivider = 300f;   // Magic value ??
        private bool _isDragging;


        private void Start() {
            _canvasRectTransform = GetComponent<RectTransform>();
            _newCameraSize = _camera.orthographicSize;
            _isDragging = false;
        }

        private void Update() {
            if (_newCameraSize != _camera.orthographicSize) {
                _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _newCameraSize, Time.deltaTime * _cameraResizeSpeed);
                ConfineCameraInCanvas();
            }
            if (_cameraFlyDirection != Vector2.zero) {
                _camera.transform.position -= (Vector3)_cameraFlyDirection;
                _cameraFlyDirection /= _cameraFlyInertia;
                ConfineCameraInCanvas();
            }
        }

        public void ChangeCameraSize(Vector2 wheelVector) {
            if (wheelVector.y == 0) return;
            var _scaleWhellStep = wheelVector.y > 0 ? 0.5f : 2f;
            var newSize = _camera.orthographicSize * _scaleWhellStep;
            newSize = Mathf.Clamp(newSize, 100, _canvasRectTransform.rect.height / 2);
            _newCameraSize = newSize;
        }

        public void OnDrag(PointerEventData eventData) {
            var delta = eventData.delta * _camera.orthographicSize / _deltaMoveCameraDivider;
            _cameraFlyDirection = new Vector2(delta.x, delta.y);
        }

        private void ConfineCameraInCanvas() {
            bool isChanged = false;
            Rect cameraRect = GetCameraRect();
            Rect canvasRect = GetCanvasRect();
            if (cameraRect.xMin < canvasRect.xMin) {
                cameraRect.xMin = canvasRect.xMin;
                isChanged = true;
            }
            if (cameraRect.yMin < canvasRect.yMin) {
                cameraRect.yMin = canvasRect.yMin;
                isChanged = true;
            }
            if (cameraRect.xMax > canvasRect.xMax) {
                cameraRect.xMax = canvasRect.xMax;
                isChanged = true;
            }
            if (cameraRect.yMax > canvasRect.yMax) {
                cameraRect.yMax = canvasRect.yMax;
                isChanged = true;
            }
            if (isChanged)
                _camera.transform.position = new Vector3(cameraRect.center.x, cameraRect.center.y, _camera.transform.position.z);
        }

        private Rect GetCameraRect() {
            var height = 2 * _camera.orthographicSize;
            var width = height * _camera.aspect;
            var left = _camera.transform.position.x - width / 2;
            var bottom = _camera.transform.position.y - height / 2;
            var cameraRect = new Rect(left, bottom, width, height);
            return cameraRect;
        }

        private Rect GetCanvasRect() {
            var height = _canvasRectTransform.rect.height;
            var width = _canvasRectTransform.rect.width;
            var left = _canvasRectTransform.position.x - width / 2;
            var bottom = _canvasRectTransform.position.y - height / 2;
            var canvasRect = new Rect(left, bottom, width, height);
            return canvasRect;
        }

        public void OnPointerDown(PointerEventData eventData) {
        }

        public void OnPointerUp(PointerEventData eventData) {
            if (!_isDragging)
                _onClick?.Invoke();
            _isDragging = false;
        }

        public void OnBeginDrag(PointerEventData eventData) {
            _isDragging = true;
        }
    }
}