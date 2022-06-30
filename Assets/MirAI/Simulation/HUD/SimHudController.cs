using Assets.MirAI.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.MirAI.Simulation.HUD {
    public class SimHudController : MonoBehaviour {

        [SerializeField] SimManager _simManager;

        public void OnBackButton() {
            SceneManager.LoadScene("MainMenu");
        }

        public void OnNewButton() {
            _simManager.ShowAddUnitMenu();
        }

        public void OnEditButton() {
            var unit = GetSelectedUnit();
            if (unit == null) return;
            EditUnit.Edit(unit);
        }

        public void OnDeleteButton() {
            var unit = GetSelectedUnit();
            if (unit == null) return;
            _simManager.DeleteUnit(unit);
        }

        private Unit GetSelectedUnit() {
            foreach (Unit unit in AiModel.Instance.Units) {
                if (unit.Controller.isSelected())
                    return unit;
            }
            return null;
        }
    }
}