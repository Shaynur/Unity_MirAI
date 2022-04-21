using UnityEngine;

namespace Assets.MirAI.Simulation {

    public class UnitController : MonoBehaviour {

        [SerializeField] private Sprite _enemyImg;
        [SerializeField] private Sprite _allyImg;

        private Rigidbody2D _rigidbody;

        private void Awake() {
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        }

        private void Start() {
            SetRandomTeam();
        }

        public void RandomMoveUnit(float stepLenght) {
            float dx = Random.Range(-stepLenght, stepLenght);
            float dy = Random.Range(-stepLenght, stepLenght);
            var newVelocity = new Vector2(dx, dy);
            _rigidbody.velocity = newVelocity;
        }

        private void SetRandomTeam() {
            var renderer = gameObject.GetComponent<SpriteRenderer>();
            var team = Random.Range(0, 100);
            if (team < 50)
                renderer.sprite = _enemyImg;
            else
                renderer.sprite = _allyImg;
        }
    }
}