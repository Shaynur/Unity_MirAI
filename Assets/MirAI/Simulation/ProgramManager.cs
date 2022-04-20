using Assets.MirAI.Models;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.MirAI.Simulation {
    public static class ProgramManager {

        private static readonly int MaxLenght = 500;

        public static bool CheckAllProgramsLenght() {
            bool result = true;
            var sb = new StringBuilder();
            foreach (var program in AiModel.Instance.Programs) {
                int lenght = GetProgramLenght(program);
                if (lenght > MaxLenght) {
                    result = false;
                }
                sb.Append("Program \"" + program.Name + "\" lenght= " + lenght + "\n");
            }
            Debug.Log(sb.ToString());
            return result;
        }

        public static Node Run(Program program) {
            foreach (var node in GlobalDFC(program)) {
                if (node.Type == NodeType.Action) {
                    return node;
                }
                else if (node.Type == NodeType.Condition) {
                    node.Viewed = !CommandHandler.CheckCondition(node.Command);
                }
            }
            return null;
        }

        private static int GetProgramLenght(Program program) {
            int lenght = 0;
            foreach (var node in GlobalDFC(program)) {
                switch (node.Type) {
                    case NodeType.Action:
                    case NodeType.Condition:
                        lenght++;
                        break;
                }
                if (lenght > MaxLenght)
                    break;
            }
            return lenght;
        }

        private static IEnumerable<Node> GlobalDFC(Program program) {
            foreach (var node in program.DFC()) {
                yield return node;
                if (node.Type == NodeType.SubAI) {
                    node.Viewed = true;
                    var nextProgram = AiModel.Instance.Programs.Find(x => x.Id == node.Command);
                    if (nextProgram != null)
                        foreach (var subAInode in GlobalDFC(nextProgram))
                            yield return subAInode;
                }
            }
        }
    }
}