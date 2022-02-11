using System;
using System.Data;
using Assets.MirAI.Models;
using Mono.Data.Sqlite;

namespace Assets.MirAI.DB {

    public class DbNode : DbTable<Node> {

        public DbNode(string tableName, SqliteConnection connection) : base(tableName, connection) {
        }

        public override string GetDeleteCommandSuffix(Node node) {
            return " WHERE Id = '" + node.Id + "';";
        }

        public override string GetInsertCommandSuffix(Node node) {
            return " (ProgramId, Type, Command, X, Y) VALUES ('"
                + node.ProgramId + "', '" + (int)node.Type + "', '" + node.Command + "', '" + (int)node.X + "', '" + (int)node.Y + "');";
        }

        public override string GetUpdateCommandSuffix(Node node) {
            return " SET ProgramId = '" + node.ProgramId
                + "', Type = '" + (int)node.Type
                + "', Command = '" + node.Command
                + "', X = '" + (int)node.X
                + "', Y = '" + (int)node.Y
                + "' WHERE Id = '" + node.Id + "';";
        }

        public override Node CreateByData(IDataRecord data) {
            try {
                Node node = new Node {
                    Id = data.GetInt32(0),
                    ProgramId = data.GetInt32(1),
                    Type = (NodeType)data.GetInt32(2),
                    Command = data.GetInt32(3),
                    X = data.GetInt32(4),
                    Y = data.GetInt32(5)
                };
                return node;
            }
            catch (Exception ex) {
                throw new DbMirAiException("Convert IDataRecord to Node error.", ex);
            }
        }
    }
}