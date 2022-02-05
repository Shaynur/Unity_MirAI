using UnityEngine;
using Assets.MirAI.Models;
using Assets.MirAI.DB;
using System;

namespace Assets.MirAI {

    public class DBdemo : MonoBehaviour {

        //public AiModel Model { get; private set; }
        private GameSession _session;

        private void Start() {
            _session = GameSession.Instance;
            try {
                //_session.AiModel.LoadFromDB();

                DisplayDB();
                //SomeTestWork();
                //DisplayDB();
            }
            catch (DbMirAiException ex) {
                Debug.LogException(ex);
            }
            catch (Exception ex) {
                Debug.LogException(ex);
            }
        }

        private void SomeTestWork() {
            var prog = _session.AiModel.AddNewProgram("NewTestProg");
            var root = prog.Nodes[0];
            var action = _session.AiModel.AddChildNode(root);
            action.ProgramId = prog.Id;
            action.Type = NodeType.Action;
            action.Command = 333;
            action.X = 400;
            action.Y = 400;
            _session.AiModel.UpdateNode(action);
        }

        private void DisplayDB() {
            foreach (var program in _session.AiModel.Programs) {
                Debug.Log(program);
            }
            foreach (var node in _session.AiModel.Nodes) {
                Debug.Log(node);
            }
        }
    }
}