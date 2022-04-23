using Assets.MirAI.Models;
using UnityEngine;

namespace Assets.MirAI.Simulation {

    public class UnitController : MonoBehaviour {

        [SerializeField] private Sprite _enemyImg;
        [SerializeField] private Sprite _allyImg;

        public Unit Unit { get; set; }
        private Rigidbody2D _rigidbody;

        private void Awake() {
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        }

        private void Start() {
            SetTeamSprite();
        }

        private void LateUpdate() {
            Unit.X = _rigidbody.transform.position.x;
            Unit.Y = _rigidbody.transform.position.y;
        }

        public void SetUnitVelocity(Vector2 velocity) {
            _rigidbody.velocity = velocity;
        }

        private void SetTeamSprite() {
            var renderer = gameObject.GetComponent<SpriteRenderer>();
            if (Unit.Team == 1)
                renderer.sprite = _enemyImg;
            else if (Unit.Team == 2)
                renderer.sprite = _allyImg;
        }
    }
}