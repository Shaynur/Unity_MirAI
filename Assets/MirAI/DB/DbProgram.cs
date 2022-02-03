using System;
using System.Data;
using System.Text;
using Assets.MirAI.Models;

namespace Assets.MirAI.DB {
    public class DbProgram : Program, IDbEntity {

        public string GetDeleteCommandSuffix() {
            return " WHERE Id = '" + Id + "';";
        }

        public string GetInsertCommandSuffix() {
            return " (Name) VALUES ('" + Name + "');";
        }

        public string GetUpdateCommandSuffix() {
            return " SET Name = '" + Name + "' WHERE Id = '" + Id + "';";
        }

        public void SetData(IDataRecord data) {
            try {
                Id = data.GetInt32(0);
                Name = data.GetString(1);
            }
            catch (Exception ex) {
                throw new DbMirAiException("Convert IDataRecord to Program error.", ex);
            }
        }

        public override string ToString() {
            StringBuilder ret = new StringBuilder($"Id={Id,-5} Name={Name}");
            return ret.ToString();
        }
    }
}