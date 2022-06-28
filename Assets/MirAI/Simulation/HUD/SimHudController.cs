using Assets.MirAI.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.MirAI.Simulation.HUD {
    public class SimHudController : MonoBehaviour {

        public void OnBackButton() {
            SceneManager.LoadScene("MainMenu");
        }

        public void OnNewButton() {
        }

        public void OnEditButton() {
            var unit = FindSelectedUnit();
            if (unit == null) return;
            EditUnit.Edit(unit);
        }

        private Unit FindSelectedUnit() {
            foreach (Unit unit in AiModel.Instance.Units) {
                if (unit.Controller.isSelected())
                    return unit;
            }
            return null;
        }
    }
}