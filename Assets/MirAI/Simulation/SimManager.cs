using System.Collections;
using Assets.MirAI.Models;
using Assets.MirAI.Utils;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;

namespace Assets.MirAI.Simulation {

    public class SimManager : MonoBehaviour {

        [SerializeField][Range(0.1f, 1f)] private float _delay = 1f;
        [SerializeField] private GameObject _unitPrefab;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private AiModel _model;
        private bool _isActive = false;

        private void Start() {
            _model = AiModel.Instance;
            _trash.Retain(_model.OnLoaded.Subscribe(CreateUnits));
            CreateUnits();
        }

        private void CreateUnits() {
            foreach (var unit in _model.Units) {
                var position = new Vector3(unit.X, unit.Y, 0);
                var item = GameObjectSpawner.Spawn(_unitPrefab, position, "Units_Container");
                var unitController = item.GetComponent<UnitController>();
                unit.Controller = unitController;
                unitController.Unit = unit;
            }
        }

        public void Switch() {
            if (_isActive == false) {
                if (ProgramManager.CheckAllProgramsLenght())
                    StartTimer();
                else
                    //TODO если не пройдена проверка на длинну програм
                    Debug.Log("Program Lenght is to much");
            }
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
                foreach (var unit in _model.Units) {
                    yield return new WaitForSeconds(_delay);
                    Tick(unit);
                }
            }
        }

        private void Tick(Unit unit) {
            CommandHandler.Handle(unit);
        }

        private void OnDestroy() {
            _trash.Dispose();
        }
    }
}