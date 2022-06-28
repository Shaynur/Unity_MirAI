using Assets.MirAI.AiEditor.SelectAction;
using Assets.MirAI.Models;
using Assets.MirAI.Utils;

namespace Assets.MirAI.Simulation {
    public static class EditUnit {

        public static Unit Unit { get; set; }

        public static void Edit(Unit unit) {
            Unit = unit;
            ShowEditWindow();
        }

        private static void ShowEditWindow() {
            WindowUtils.CreateMenuWindow("UI/EditUnit", "HUD", UpdateUnitDb, ClearTemplates);
        }

        private static void UpdateUnitDb() {
            AiModel.Instance.UpdateUnit(Unit);
            Unit.Controller.SetUnitSprites();
        }

        private static void ClearTemplates() {
            Unit = null;
        }
    }
}