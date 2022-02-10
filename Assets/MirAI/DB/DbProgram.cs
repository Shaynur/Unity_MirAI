using System;
using System.Data;
using Assets.MirAI.Models;
using Mono.Data.Sqlite;

namespace Assets.MirAI.DB {
    public class DbProgram : DbTable<Program> {

        public DbProgram(string tableName, SqliteConnection connection) : base(tableName, connection) {
        }

        public override string GetDeleteCommandSuffix(Program program) {
            return " WHERE Id = '" + program.Id + "';";
        }

        public override string GetInsertCommandSuffix(Program program) {
            return " (Name) VALUES ('" + program.Name + "');";
        }

        public override string GetUpdateCommandSuffix(Program program) {
            return " SET Name = '" + program.Name + "' WHERE Id = '" + program.Id + "';";
        }

        public override Program CreateByData(IDataRecord data) {
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