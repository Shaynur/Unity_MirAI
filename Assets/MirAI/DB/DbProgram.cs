using System;
using System.Data;
using Assets.MirAI.Models;

namespace Assets.MirAI.DB {
    public class DbProgram : IDbRoutines {

        private readonly Program _program;

        public DbProgram(Program program) {
            _program = program;
        }

        public string GetDeleteCommandSuffix() {
            return " WHERE Id = '" + _program.Id + "';";
        }

        public string GetInsertCommandSuffix() {
            return " (Name) VALUES ('" + _program.Name + "');";
        }

        public string GetUpdateCommandSuffix() {
            return " SET Name = '" + _program.Name + "' WHERE Id = '" + _program.Id + "';";
        }

        public void SetData(IDataRecord data) {
            try {
                _program.Id = data.GetInt32(0);
                _program.Name = data.GetString(1);
            }
            catch (Exception ex) {
                throw new DbMirAiException("Convert IDataRecord to Program error.", ex);
            }
        }
    }
}