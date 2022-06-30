using Assets.MirAI.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.MirAI.Simulation {

    public class UnitController : MonoBehaviour, IPointerDownHandler {

        [SerializeField] private Image _unitBodyImage;
        [SerializeField] private Image _unitTypeImage;
        [SerializeField] private GameObject _selector;
        [SerializeField] private Sprite[] _teamImgs = new Sprite[2];
        [SerializeField] private Sprite[] _typeImgs = new Sprite[3];

        public Unit Unit { get; set; }
        private Rigidbody2D _rigidbody;
        private LineRenderer _circleRenderer;

        private void Awake() {
            _rigidbody = gameObject.GetComponentInChildren<Rigidbody2D>();
        }

        private void Start() {
            _circleRenderer = GetComponent<LineRenderer>();
            _circleRenderer.widthMultiplier = 0.1f;
            DrawCircle(100, Unit.Range);
            if (Unit != null) {
                SetUnitSprites();
                Unit.Hp = (int)Unit.MaxHp;
            }
        }

        private void LateUpdate() {
            if (Unit != null) {
                Unit.X = _rigidbody.transform.position.x;
                Unit.Y = _rigidbody.transform.position.y;
            }
        }

        private void DrawCircle(int steps, float radius) {
            _circleRenderer.positionCount = steps;
            for (int currentStep = 0; currentStep < steps; currentStep++) {
                float circumferenceProgress = (float)currentStep / steps;
                float currentRadian = circumferenceProgress * 2 * Mathf.PI;
                float xScaled = Mathf.Cos(currentRadian);
                float yScaled = Mathf.Sin(currentRadian);
                float x = xScaled * radius;
                float y = yScaled * radius;
                Vector3 currentPosition = new Vector3(x, y, 0);
                _circleRenderer.SetPosition(currentStep, currentPosition);
            }
        }

        public void SetUnitVelocity(Vector2 velocity) {
            _rigidbody.velocity = velocity;
        }

        public void SetUnitSprites() {
            _unitBodyImage.sprite = _teamImgs[(int)Unit.Team - 1];
            _unitTypeImage.sprite = _typeImgs[(int)Unit.Type - 1];
        }

        public void OnPointerDown(PointerEventData eventData) {
            Select(!_selector.activeSelf);
            UnselectOtherUnits();
        }

        private void UnselectOtherUnits() {
            foreach (var unit in AiModel.Instance.Units) {
                if (unit != Unit)
                    unit.Controller.Select(false);
            }
        }


        public void Select(bool value) {
            _selector.SetActive(value);
        }

        public bool isSelected() {
            return _selector.activeSelf;
        }
    }
}