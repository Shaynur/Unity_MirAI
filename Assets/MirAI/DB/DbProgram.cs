using System;
using System.Data;
using Assets.MirAI.Models;
using Mono.Data.Sqlite;

namespace Assets.MirAI.DB {
    public class DbProgram : DbTable<Program> {

        public DbProgram(string tableName, SqliteConnection connection) : base(tableName, connection) {
        }

        public override SqliteCommand GetCreateTableCommand() {
            var command = _connection.CreateCommand();
            command.CommandText = "CREATE TABLE IF NOT EXISTS " + TableName
                + " ( Id INTEGER NOT NULL CONSTRAINT PK_Programs PRIMARY KEY AUTOINCREMENT, Name VARCHAR(30));";
            return command;
        }

        public override SqliteCommand GetDeleteCommand(Program program) {
            var command = _connection.CreateCommand();
            command.CommandText = "DELETE FROM " + TableName + " WHERE Id = @id;";
            command.Parameters.AddWithValue("@id", program.Id);
            command.Prepare();
            return command;
        }

        public override SqliteCommand GetInsertCommand(Program program) {
            var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO " + TableName + " (Name) VALUES (@name);";
            command.Parameters.AddWithValue("@name", program.Name);
            command.Prepare();
            return command;
        }

        public override SqliteCommand GetUpdateCommand(Program program) {
            var command = _connection.CreateCommand();
            command.CommandText = "UPDATE " + TableName + " SET Name=@name WHERE Id=@id;";
            command.Parameters.AddWithValue("@name", program.Name);
            command.Parameters.AddWithValue("@id", program.Id);
            command.Prepare();
            return command;
        }

        public override Program GetFromReader(IDataRecord data) {
            try {
                Program program = new Program {
                    Id = data.GetInt32(0),
                    Name = data.GetString(1)
                };
                return program;
            }
            catch (Exception ex) {
                throw new DbMirAiException("Convert IDataRecord to Program error.", ex);
            }
        }
    }
}