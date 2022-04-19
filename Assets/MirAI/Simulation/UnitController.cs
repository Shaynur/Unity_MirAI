using UnityEngine;

namespace Assets.MirAI.Simulation {

    public class UnitController : MonoBehaviour {

        private Rigidbody2D _rigidbody;

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody2D>();
        }


        public void RandomMoveUnit(float stepLenght) {
            float dx = Random.Range(-stepLenght, stepLenght);
            float dy = Random.Range(-stepLenght, stepLenght);
            var newVelocity = new Vector2(dx, dy);
            _rigidbody.velocity = newVelocity;
        }

    }
}