using UnityEngine;
using Assets.MirAI.Models;
using Assets.MirAI.DB.TableDefs;

namespace Assets.MirAI.DB {

    public class DBdemo : MonoBehaviour {

        public AiModel Model { get; private set; }

        private void Start() {
            Model = new AiModel();
            Model.LoadFromDB();

            DisplayDB();

            var prog = Model.AddNewProgram("NewTestProg");
            var root = prog.Nodes[0];
            var action = Model.AddChildNode(root);
            action.ProgramId = prog.Id;
            action.Type = NodeType.Action;
            action.Command = 333;
            action.X = 400;
            action.Y = 400;
            Model.UpdateNode(action);

            DisplayDB();
        }

        private void DisplayDB() {

            foreach (var program in Model.Programs) {
                Debug.Log(program);
            }

            foreach (var node in Model.Nodes) {
                Debug.Log(node);
            }
        }
    }
}