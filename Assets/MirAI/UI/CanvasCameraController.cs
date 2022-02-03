using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.MirAI.UI {

    [RequireComponent(typeof(Canvas))]
    public class CanvasCameraController : MonoBehaviour, IDragHandler {

        [SerializeField] private Camera _camera;
        [SerializeField] int _scaleWhellStep = 5;

        private Canvas _canvas;
        private RectTransform _canvasRectTransform;


        private void Start() {
            _canvas = GetComponent<Canvas>();
            _canvasRectTransform = GetComponent<RectTransform>();
        }

        public void ChangeCameraSize(Vector2 wheelVector) {
            if (wheelVector.y > 0)
                CalculateCameraSize(-_scaleWhellStep);
            else if (wheelVector.y < 0)
                CalculateCameraSize(_scaleWhellStep);
        }

        public void OnDrag(PointerEventData eventData) {
            //FlyCamera(eventData.delta / _canvas.scaleFactor);
            FlyCamera(eventData.delta / _canvas.transform.localScale.x);
        }

        private void FlyCamera(Vector2 delta) {
            var cameraNewPosition = _camera.transform.position - new Vector3(delta.x, delta.y, 0);
            _camera.transform.position = cameraNewPosition;
            CheckCameraPosition(true);
        }

        private void CalculateCameraSize(int increment) {
            var newSize = _camera.orthographicSize + increment;
            if (newSize < 100 || newSize > _canvasRectTransform.rect.height / 2) return;
            _camera.orthographicSize = newSize;
            CheckCameraPosition(true);
        }

        private bool CheckCameraPosition(bool correct) {
            bool noChanges = true;
            Rect cameraRect = GetCameraRect();
            Rect canvasRect = GetCanvasRect();
            if (cameraRect.xMin < canvasRect.xMin) {
                if (correct) {
                    cameraRect.xMin = canvasRect.xMin;
                }
                noChanges = false;
            }
            if (cameraRect.yMin < canvasRect.yMin) {
                if (correct) {
                    cameraRect.yMin = canvasRect.yMin;
                }
                noChanges = false;
            }
            if (cameraRect.xMax > canvasRect.xMax) {
                if (correct) {
                    cameraRect.xMax = canvasRect.xMax;
                }
                noChanges = false;
            }
            if (cameraRect.yMax > canvasRect.yMax) {
                if (correct) {
                    cameraRect.yMax = canvasRect.yMax;
                }
                noChanges = false;
            }
            if (!noChanges) {
                _camera.transform.position = new Vector3(cameraRect.center.x, cameraRect.center.y, _camera.transform.position.z);
            }
            return noChanges;
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
            var scale = _canvas.transform.localScale.x;
            var height = _canvasRectTransform.rect.height * scale;
            var width = _canvasRectTransform.rect.width * scale;
            var left = _canvas.transform.position.x - width / 2;
            var bottom = _canvas.transform.position.y - height / 2;
            var canvasRect = new Rect(left, bottom, width, height);
            return canvasRect;
        }
    }
}