using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.MirAI.UI {

    public class EditorCanvasController : MonoBehaviour, IDragHandler {

        [SerializeField] private Canvas _canvas;
        [SerializeField] private Camera _camera;

        public void ChangeCanvasScale(Vector2 scale) {
            if (scale.y > 0)
                CalculateNewScale(0.2f);
            else if (scale.y < 0)
                CalculateNewScale(-0.2f);
        }

        public void OnDrag(PointerEventData eventData) {
            FlyCamera(eventData.delta / _canvas.scaleFactor);
        }

        private void FlyCamera( Vector2 delta ) {
            var cameraNewPosition = _camera.transform.position - new Vector3(delta.x, delta.y, 0);
            _camera.transform.position = cameraNewPosition;
        }

        private void CalculateNewScale(float increment) {
            var localScale = _canvas.transform.localScale;
            _canvas.transform.localScale = new Vector3(localScale.x + increment,
                                                       localScale.y + increment,
                                                       localScale.z + increment);
        }
    }
}