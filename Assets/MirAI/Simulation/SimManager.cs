using System.Collections;
using System.Linq;
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
            _trash.Retain(_model.OnLoaded.Subscribe(CreateUnitsGOs));
            DestroyUnitsGOs();
            CreateUnitsGOs();
        }

        private void DestroyUnitsGOs() {
            foreach (var unit in _model.Units)
                DestroyUnitGO(unit);
        }

        private void CreateUnitsGOs() {
            foreach (var unit in _model.Units)
                CreateUnitGO(unit);
        }

        public void DestroyUnitGO(Unit unit) {
            var controller = unit.Controller;
            if (controller != null) {
                var go = controller.gameObject;
                if (go != null) {
                    Destroy(go);
                }
            }
        }

        private void CreateUnitGO(Unit unit) {
            var position = new Vector3(unit.X, unit.Y, 0);
            var item = GameObjectSpawner.Spawn(_unitPrefab, position, "Units_Container");
            item.transform.position = new Vector3(item.transform.position.x,item.transform.position.y,0);
            var unitController = item.GetComponent<UnitController>();
            unit.Controller = unitController;
            unitController.Unit = unit;
        }

        public void ShowAddUnitMenu() {
            EditUnit.Unit = new Unit {
                Team = UnitTeam.Team_1,
                Type = UnitType.Warrior
            };
            WindowUtils.CreateMenuWindow("UI/EditUnit", "HUD", AddUnitOk, AddUnitCancel);
        }

        private void AddUnitOk() {
            StopTimer();
            AiModel.Instance.AddUnit(EditUnit.Unit);
            CreateUnitGO(EditUnit.Unit);
        }

        private void AddUnitCancel() {
            EditUnit.Unit = null;
        }

        public void DeleteUnit(Unit unit) {
            StopTimer();
            DestroyUnitGO(unit);
            AiModel.Instance.RemoveUnit(unit);
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
                foreach (var unit in _model.Units.ToList()) {
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