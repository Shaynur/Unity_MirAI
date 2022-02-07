using System;
using System.Data;
using Assets.MirAI.Models;

namespace Assets.MirAI.DB {

    public class DbNode : IDbRoutines {

        private readonly Node _node;

        public DbNode(Node node) {
            _node = node;
        }

        public string GetDeleteCommandSuffix() {
            return " WHERE Id = '" + _node.Id + "';";
        }

        public string GetInsertCommandSuffix() {
            return " (ProgramId, Type, Command, X, Y) VALUES ('"
                + _node.ProgramId + "', '" + (int)_node.Type + "', '" + _node.Command + "', '" + _node.X + "', '" + _node.Y + "');";
        }

        public string GetUpdateCommandSuffix() {
            return " SET ProgramId = '" + _node.ProgramId
                + "', Type = '" + (int)_node.Type
                + "', Command = '" + _node.Command
                + "', X = '" + _node.X
                + "', Y = '" + _node.Y
                + "' WHERE Id = '" + _node.Id + "';";
        }

        public void SetData(IDataRecord data) {
            try {
                _node.Id = data.GetInt32(0);
                _node.ProgramId = data.GetInt32(1);
                _node.Type = (NodeType)data.GetInt32(2);
                _node.Command = data.GetInt32(3);
                _node.X = data.GetInt32(4);
                _node.Y = data.GetInt32(5);
            }
            catch (Exception ex) {
                throw new DbMirAiException("Convert IDataRecord to Node error.", ex);
            }
        }
    }
}