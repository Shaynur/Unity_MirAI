using System;
using System.Collections.Generic;
using System.Data;
using Assets.MirAI.Models;
using Mono.Data.Sqlite;
using static UnityEditorInternal.ReorderableList;
using static UnityEngine.UI.GridLayoutGroup;
using UnityEngine.InputSystem;

namespace Assets.MirAI.DB {

    public class DbUnit : DbTable<Unit> {

        public DbUnit(string tableName, SqliteConnection connection) : base(tableName, connection) {
        }

        public override SqliteCommand GetCreateTableCommand() {
            var command = _connection.CreateCommand();
            command.CommandText = "CREATE TABLE IF NOT EXISTS " + TableName
                + "  ( Id INTEGER NOT NULL CONSTRAINT PK_Units PRIMARY KEY AUTOINCREMENT, "
                + "ProgramId INTEGER DEFAULT(0) NOT NULL, "
                + "X INTEGER NOT NULL, "
                + "Y INTEGER NOT NULL, "
                + "CONSTRAINT FK_Units_Programs_ProgramId FOREIGN KEY(ProgramId) REFERENCES Programs(Id) ON DELETE SET DEFAULT)";
            return command;
        }

        public override SqliteCommand GetDeleteCommand(Unit unit) {
            var command = _connection.CreateCommand();
            command.CommandText = "DELETE FROM " + TableName + " WHERE Id = @id;";
            command.Parameters.AddWithValue("@id", unit.Id);
            command.Prepare();
            return command;
        }

        public override SqliteCommand GetInsertCommand(Unit unit) {
            var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO " + TableName + " (ProgramId, X, Y) VALUES (@p, @x, @y);";
            command.Parameters.AddWithValue("@p", unit.ProgramId);
            command.Parameters.AddWithValue("@x", (int)unit.X);
            command.Parameters.AddWithValue("@y", (int)unit.Y);
            command.Prepare();
            return command;
        }

        public override SqliteCommand GetUpdateCommand(Unit unit) {
            var command = _connection.CreateCommand();
            command.CommandText = "UPDATE " + TableName + " SET ProgramId=@p, X=@x, Y=@y WHERE Id=@id;";
            command.Parameters.AddWithValue("@p", unit.ProgramId);
            command.Parameters.AddWithValue("@x", (int)unit.X);
            command.Parameters.AddWithValue("@y", (int)unit.Y);
            command.Parameters.AddWithValue("@id", unit.Id);
            command.Prepare();
            return command;
        }

        public override Unit GetFromReader(IDataRecord data) {
            try {
                Unit unit = new Unit {
                    Id = data.GetInt32(0),
                    ProgramId = data.GetInt32(1),
                    X = data.GetInt32(2),
                    Y = data.GetInt32(3)
                };
                return unit;
            }
            catch (Exception ex) {
                throw new DbMirAiException("Convert IDataRecord to Unit error.", ex);
            }
        }
    }
}