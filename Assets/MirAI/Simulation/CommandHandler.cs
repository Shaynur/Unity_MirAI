using Assets.MirAI.Models;

namespace Assets.MirAI.Simulation {
    public static class CommandHandler {

        private static AiModel _model = AiModel.Instance;

        public static void Handle(Unit unit) {
            var node = FindCurrentUnitNode(unit);
            if (node == null)
                return;
            ExecuteCommand(node.Command);
        }

        private static void ExecuteCommand(int command) {
            var p1 = command & 0xFF;
            var p2 = (command >> 8) & 0xFF;
            //TODO do what the command want
        }

        public static bool CheckCondition(int command) {
            //TODO check condition-node for valid
            return true;
        }

        private static Node FindCurrentUnitNode(Unit unit) {
            var prog = _model.Programs.Find(x => x.Id == unit.ProgramId);
            return ProgramManager.Run(prog);
        }
    }
}

// Action Command list:
//
//  byte0:
//      0: None
//      1: Go to...
//      2: Go from...
//
//  byte1:
//      0: None
//      1: Enemy
//      2: Ally
//