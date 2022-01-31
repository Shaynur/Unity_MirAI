using UnityEngine;
using Assets.MirAI.DB.TableDefs;
using Assets.MirAI.DB.Tables;
using System;

namespace Assets.MirAI.DB {

    public class DBdemo : MonoBehaviour {

        private void Start() {
            DisplayNodes();
            DisplayPrograms();
        }

        private void DisplayPrograms() {
            var programsTable = new DbTableDef<DbProgram>("Programs");
            var nodes = programsTable.GetAllRecords();
            foreach (var node in nodes) {
                Debug.Log(node);
            }
        }

        private void DisplayNodes() {
            var nodesTable = new DbTableDef<DbNode>("Nodes");
            var nodes = nodesTable.GetAllRecords();
            foreach (var node in nodes) {
                Debug.Log(node);
            }
        }
    }
}