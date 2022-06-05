using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.MirAI.UI {

    public class PlayerInputReader : MonoBehaviour {

        [SerializeField] private CanvasCameraController _controller;

        public void OnMouseWheel(InputAction.CallbackContext context) {
            if (context.performed) {
                var wheelVector = context.ReadValue<Vector2>();
                _controller.ChangeCameraSize(wheelVector);
            }
        }
    }
}