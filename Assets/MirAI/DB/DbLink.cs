using System;
using System.Data;
using Assets.MirAI.Models;
using Mono.Data.Sqlite;

namespace Assets.MirAI.DB {
    public class DbLink : DbTable<Link> {

        public DbLink(string tableName, SqliteConnection connection) : base(tableName, connection) {
        }

        public override SqliteCommand GetCreateTableCommand() {
            var command = _connection.CreateCommand();
            command.CommandText = "CREATE TABLE IF NOT EXISTS " + TableName
                + "  ( FromId INTEGER NOT NULL, "
                + "ToId INTEGER NOT NULL, "
                + "CONSTRAINT PK_Links PRIMARY KEY (FromId, ToId), "
                + "CONSTRAINT FK_Links_Nodes_FromId FOREIGN KEY(FromId) REFERENCES Nodes(Id) ON DELETE CASCADE, "
                + "CONSTRAINT FK_Links_Nodes_ToId FOREIGN KEY(ToId) REFERENCES Nodes(Id) ON DELETE CASCADE);";
            return command;
        }

        public override SqliteCommand GetDeleteCommand(Link link) {
            var command = _connection.CreateCommand();
            command.CommandText = "DELETE FROM " + TableName + " WHERE FromId = @from AND ToId = @to;";
            command.Parameters.AddWithValue("@from", (int)link.FromId);
            command.Parameters.AddWithValue("@to", (int)link.ToId);
            command.Prepare();
            return command;
        }

        public override SqliteCommand GetInsertCommand(Link link) {
            var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO " + TableName + " (FromId, ToId) VALUES (@from, @to);";
            command.Parameters.AddWithValue("@from", (int)link.FromId);
            command.Parameters.AddWithValue("@to", (int)link.ToId);
            command.Prepare();
            return command;
        }

        public override SqliteCommand GetUpdateCommand(Link link) {
            var command = _connection.CreateCommand();
            command.CommandText = "UPDATE " + TableName + ";";
            return command;
        }

        public override Link GetFromReader(IDataRecord data) {
            try {
                Link link = new Link(data.GetInt32(0), data.GetInt32(1));
                return link;
            }
            catch (Exception ex) {
                throw new DbMirAiException("Convert IDataRecord to Program error.", ex);
            }
        }
    }
}