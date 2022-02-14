using System.Collections;
using UnityEngine;

namespace Assets.MirAI.UI {

    public class ShowHide : MonoBehaviour {

        [SerializeField] private bool _scaleByX;
        [SerializeField] private bool _scaleByY;

        private RectTransform _rt;
        private Rect _defRect;
        private Vector3 _defScale;
        private readonly bool _hiding = true;
        private readonly bool _opening = false;
        private float _slider;
        private bool _isHidden;

        private void Start() {
            _rt = GetComponent<RectTransform>();
            _defRect = _rt.rect;
            _defScale = _rt.localScale;
            _isHidden = false;
        }

        [ContextMenu("Hide")]
        public void Hide() {
            if (_isHidden) return;
            StartCoroutine(SetState(_hiding));
            _isHidden = true;
        }

        [ContextMenu("Show")]
        public void Show() {
            if(!_isHidden) return;
            StartCoroutine(SetState(_opening));
            _isHidden = false;
        }

        [ContextMenu("Toggle")]
        public void Toggle() {
            StartCoroutine(SetState(!_isHidden));
            _isHidden = !_isHidden;
        }

        private IEnumerator SetState(bool state) {
            _slider = state ? 1f : 0f;
            while (MoveSlider(state)) {
                var scaleX = _rt.localScale.x;
                if (_scaleByX) {
                    scaleX = Mathf.Lerp(0, _defScale.x, _slider);
                    var w = Mathf.Lerp(0, _defRect.width, _slider);
                    _rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
                }
                var scaleY = _rt.localScale.y;
                if (_scaleByY) {
                    scaleY = Mathf.Lerp(0, _defScale.y, _slider);
                    var h = Mathf.Lerp(0, _defRect.height, _slider);
                    _rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
                }
                _rt.localScale = new Vector3(scaleX, scaleY, 1);
                yield return new WaitForSeconds(0.02f);
            }
        }

        private bool MoveSlider(bool state) {
            _slider += state ? -0.1f : 0.1f;
            if (_slider < -0.1f || _slider > 1.1f)
                return false;
            return true;
        }
    }
}