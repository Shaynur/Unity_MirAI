using Assets.MirAI.Models;
using UnityEngine;

namespace Assets.MirAI.Simulation {

    public class UnitController : MonoBehaviour {

        [SerializeField] private SpriteRenderer _typeSpriteRenderer;
        [SerializeField] private Sprite[] _teamImgs = new Sprite[2];
        [SerializeField] private Sprite[] _typeImgs = new Sprite[3];

        public Unit Unit { get; set; }
        private Rigidbody2D _rigidbody;
        private LineRenderer _circleRenderer;

        private void Awake() {
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        }

        private void Start() {
            _circleRenderer = GetComponent<LineRenderer>();
            _circleRenderer.widthMultiplier = 0.1f;
            SetUnitSprites();
            Unit.Hp = (int)Unit.MaxHp;
            DrawCircle(100, Unit.Range);
        }

        private void LateUpdate() {
            Unit.X = _rigidbody.transform.position.x;
            Unit.Y = _rigidbody.transform.position.y;
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

        private void SetUnitSprites() {
            var renderer = gameObject.GetComponent<SpriteRenderer>();
            renderer.sprite = _teamImgs[(int)Unit.Team - 1];
            _typeSpriteRenderer.sprite = _typeImgs[(int)Unit.Type - 1];
        }
    }
}