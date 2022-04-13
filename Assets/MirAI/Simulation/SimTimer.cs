using System.Collections;
using UnityEngine;

namespace Assets.MirAI.Simulation {

    public class SimTimer : MonoBehaviour {

        [SerializeField] private float _delay = 1f;

        public void StartSimulation() {
            StartCoroutine(SimulationTimer());
        }

        private IEnumerator SimulationTimer() {
            yield return new WaitForSeconds(_delay);
            // some work every delay
        }
    }
}