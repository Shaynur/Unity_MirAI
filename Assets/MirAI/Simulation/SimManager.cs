using System.Collections;
using UnityEngine;

namespace Assets.MirAI.Simulation {

    public class SimManager : MonoBehaviour {

        [SerializeField][Range(0.1f, 1f)] private float _delay = 1f;
        [SerializeField] private UnitController _controller;

        private bool _isActive = false;

        public void Switch() {
            if (_isActive == false)
                StartTimer();
            else
                StopTimer();
        }

        public void StartTimer() {
            _isActive = true;
            StartCoroutine(SimTimer());
        }

        public void StopTimer() {
            _isActive = false;
            StopCoroutine(SimTimer());
        }

        private IEnumerator SimTimer() {
            while (_isActive) {
                yield return new WaitForSeconds(_delay);
                // do some work here
                DoSomeWork();
            }
        }

        private void DoSomeWork() {
            _controller.RandomMoveUnit();
        }
    }
}