using System;
using System.Data;
using Assets.MirAI.Models;
using Mono.Data.Sqlite;

namespace Assets.MirAI.DB {

    public class DbNode : DbTable<Node> {

        public DbNode(string tableName, SqliteConnection connection) : base(tableName, connection) {
        }

        public override SqliteCommand GetCreateTableCommand() {
            var command = _connection.CreateCommand();
            command.CommandText = "CREATE TABLE IF NOT EXISTS " + TableName
                + "  ( Id INTEGER NOT NULL CONSTRAINT PK_Nodes PRIMARY KEY AUTOINCREMENT, "
                + "ProgramId INTEGER NOT NULL, "
                + "Type INTEGER NOT NULL, "
                + "Command INTEGER NOT NULL, "
                + "X INTEGER NOT NULL, "
                + "Y INTEGER NOT NULL, "
                + "CONSTRAINT FK_Nodes_Programs_ProgramId FOREIGN KEY(ProgramId) REFERENCES Programs(Id) ON DELETE CASCADE)";
            return command;
        }

        public override SqliteCommand GetDeleteCommand(Node node) {
            var command = _connection.CreateCommand();
            command.CommandText = "DELETE FROM " + TableName + " WHERE Id = @id;";
            command.Parameters.AddWithValue("@id", node.Id);
            command.Prepare();
            return command;
        }

        public override SqliteCommand GetInsertCommand(Node node) {
            var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO " + TableName + " (ProgramId, Type, Command, X, Y) VALUES (@p, @t, @c, @x, @y);";
            command.Parameters.AddWithValue("@p", node.ProgramId);
            command.Parameters.AddWithValue("@t", (int)node.Type);
            command.Parameters.AddWithValue("@c", node.Command);
            command.Parameters.AddWithValue("@x", (int)node.X);
            command.Parameters.AddWithValue("@y", (int)node.Y);
            command.Prepare();
            return command;
        }

        public override SqliteCommand GetUpdateCommand(Node node) {
            var command = _connection.CreateCommand();
            command.CommandText = "UPDATE " + TableName + " SET ProgramId=@p, Type=@t, Command=@c, X=@x, Y=@y WHERE Id=@id;";
            command.Parameters.AddWithValue("@p", node.ProgramId);
            command.Parameters.AddWithValue("@t", (int)node.Type);
            command.Parameters.AddWithValue("@c", node.Command);
            command.Parameters.AddWithValue("@x", (int)node.X);
            command.Parameters.AddWithValue("@y", (int)node.Y);
            command.Parameters.AddWithValue("@id", node.Id);
            command.Prepare();
            return command;
        }

        public override Node GetFromReader(IDataRecord data) {
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