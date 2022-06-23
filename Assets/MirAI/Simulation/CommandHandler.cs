using System.Collections.Generic;
using System.Linq;
using Assets.MirAI.Definitions;
using Assets.MirAI.Models;
using UnityEngine;

namespace Assets.MirAI.Simulation {
    public static class CommandHandler {

        private static readonly AiModel _model = AiModel.Instance;
        private static Unit currentUnit;
        private static int currentCommand;

        private static bool ContainCmd(string cmdId) {
            return ActionsRepository.I.Contain(currentCommand, cmdId);
        }

        public static void Handle(Unit unit) {
            currentUnit = unit;
            var node = FindCurrentUnitNode();
            if (node == null)
                return;
            currentCommand = node.Command;
            ExecuteCommand();
        }

        public static bool CheckCondition(int command) {
            currentCommand = command;
            var unitsCount = GetUnitsByCondition().Count;
            if (ContainCmd("Me") && unitsCount == 1) return true;
            var conditionParam = (command & 0xFF0000) >> 16;
            if (ContainCmd("Equal") && (unitsCount == conditionParam)) return true;
            if (ContainCmd("MoreEqual") && (unitsCount >= conditionParam)) return true;
            if (ContainCmd("LessEqual") && (unitsCount <= conditionParam)) return true;
            if (ContainCmd("NotEqual") && (unitsCount != conditionParam)) return true;
            return false;
        }

        private static List<Unit> GetUnitsByCondition() {
            var result = new List<Unit>();
            if (ContainCmd("Me")) {
                if (CheckMeCondition())
                    result.Add(currentUnit);
            }
            else { // if (ContainCmd( "Unit"))
                var team = 0;
                if (ContainCmd("Ally"))
                    team = currentUnit.Team;
                else if (ContainCmd("Enemy"))
                    team = currentUnit.Team == 1 ? 2 : 1;
                if (team == 0)
                    result = _model.Units.Where(x => x.Id != currentUnit.Id).ToList();
                else
                    result = _model.Units.Where(x => (x.Id != currentUnit.Id) && (x.Team == team)).ToList();

                if (result.Count == 0) return result;
                if (ContainCmd("Type")) {
                    if (!ContainCmd("AnyType")) {
                        var type = (currentCommand >> 10) & 0x03;
                        result = result.Where(x => ((int)x.Type == type)).ToList();
                    }
                }
                else { // if (ContainCmd( "HP"))
                    if (ContainCmd("HP100"))
                        result = result.Where(x => (x.Hp == Unit.MaxHp)).ToList();
                    else {
                        float chp = 0;
                        if (ContainCmd("HP75")) chp = 75;
                        else if (ContainCmd("HP50")) chp = 50;
                        else if (ContainCmd("HP25")) chp = 25;
                        chp = Unit.MaxHp / 100 * chp;
                        result = result.Where(x => (x.Hp <= chp)).ToList();
                    }
                }

                if (result.Count == 0) return result;
                if (ContainCmd("InRange"))
                    result = result.Where(x => Distance(currentUnit.X, currentUnit.Y, x.X, x.Y) <= Unit.Range).ToList();
                else if (ContainCmd("OutRange"))
                    result = result.Where(x => Distance(currentUnit.X, currentUnit.Y, x.X, x.Y) > Unit.Range).ToList();
            }
            return result;
        }

        private static bool CheckMeCondition() {
            if (ContainCmd("Type")) {
                var type = (currentCommand >> 10) & 0x03;
                if (type != 0 && type == (int)currentUnit.Type)
                    return true;
            }
            else { // if (ContainCmd( "HP"))
                float perc = currentUnit.Hp / Unit.MaxHp * 100;
                if ((perc == 100 && ContainCmd("HP100"))
                        || (perc <= 25 && ContainCmd("HP25"))
                        || (perc <= 50 && ContainCmd("HP50"))
                        || (perc <= 75 && ContainCmd("HP75")))
                    return true;
            }
            return false;
        }

        private static void ExecuteCommand() {
            var cmd = (currentCommand >> 24) & 0xFF;
            if (cmd == 0) return;                   // "DoNothing"
            if (cmd == 1 || cmd == 2) {             // "GoTo" or "GoFrom"
                var units = GetUnitsByCondition();
                var nearest = FindNearestUnit(units);
                if (nearest != null) {
                    var divider = cmd == 1 ? 1 : -1;
                    var dx = (nearest.X - currentUnit.X) * divider;
                    var dy = (nearest.Y - currentUnit.Y) * divider;
                    var velocity = new Vector2(dx, dy).normalized;
                    currentUnit.Controller.SetUnitVelocity(velocity);
                }
            }
            if (cmd == 3) {                          // "Attack"
                // TODO Attack command
            }
        }

        private static Unit FindNearestUnit(List<Unit> units) {
            Unit result = null;
            var minDistance = 9999999f;
            foreach (var unit in units) {
                var curDistance = Distance(unit.X, unit.Y, currentUnit.X, currentUnit.Y);
                if (curDistance < minDistance) {
                    minDistance = curDistance;
                    result = unit;
                }
            }
            return result;
        }

        private static float Distance(float x1, float y1, float x2, float y2) {
            var dx = x1 - x2;
            var dy = y1 - y2;
            return Mathf.Sqrt(dx * dx + dy * dy);
        }

        private static Node FindCurrentUnitNode() {
            var prog = _model.Programs.Find(x => x.Id == currentUnit.ProgramId);
            return ProgramManager.Run(prog);
        }
    }
}
