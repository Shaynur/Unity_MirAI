using UnityEngine;
using Assets.MirAI.DB.Tables;

namespace Assets.MirAI.DB {

    public class DBdemo : MonoBehaviour {

        private void Start() {
            DisplayDB();
            //DeleteNode();
            //DisplayDB();
        }

        private void DeleteNode() {
            using var db = new DbContext();
            //var node = new DbNode() { Id = 9, ProgramId = 1, Command = 666, Type = 5, X = 1000, Y = 1000 };
            var node = db.Nodes.GetById(11);
            db.Nodes.Remove(node);
        }

        private void DisplayDB() {
            using var db = new DbContext();

            //var programs = db.Programs.ToList();
            //foreach (var program in programs) {
            //    Debug.Log(program);
            //}

            var nodes = db.Nodes.ToList();//.FindAll(x => x.ProgramId == 2);
            foreach (var node in nodes) {
                Debug.Log(node);
            }
        }
    }
}