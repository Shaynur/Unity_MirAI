using UnityEngine;

namespace Assets.MirAI.Simulation {

    public class UnitController : MonoBehaviour {

        private Rigidbody2D _rigidbody;

        private  void Awake() {
            _rigidbody = GetComponent<Rigidbody2D>();
        }


        public void RandomMoveUnit() {
            float dx = Random.Range(0, 40) - 20;
            float dy = Random.Range(0, 40) - 20;
            var newVelocity = new Vector2(dx, dy);
            _rigidbody.velocity = newVelocity;
        }

    }
}