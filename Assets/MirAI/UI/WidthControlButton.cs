using Assets.MirAI.Utils.Disposables;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.MirAI.UI {

    [RequireComponent(typeof(Button))]
    public class WidthControlButton : MonoBehaviour, IDragHandler {

        [SerializeField] GameObject _controlledObject;
        [SerializeField] Image _arrow;

        private RectTransform _controlledRect;
        private Button _button;
        private float _width;

        private void Start() {
            _controlledRect = _controlledObject.GetComponent<RectTransform>();
            _button = GetComponent<Button>();
            _button.onClick.Subscribe(OnClick);
        }

        public void OnClick() {
            var width = _controlledRect.rect.width;
            if (width != 0) {
                _width = width;
                SetNewWidth(0);
                _arrow.transform.localScale = new Vector3(-1, 1, 1);
            }
            else {
                SetNewWidth(_width);
                _arrow.transform.localScale = Vector3.one;
            }
        }

        public void OnDrag(PointerEventData eventData) {
            SetNewWidth(_controlledRect.rect.width + eventData.delta.x);
        }

        private void SetNewWidth(float width) {
            if (width < 0) return;
            _controlledRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }

        private void OnDestroy() {
            _button.onClick.RemoveListener(OnClick);
        }
    }
}