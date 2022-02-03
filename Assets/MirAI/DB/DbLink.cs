using System;
using System.Data;
using System.Text;
using Assets.MirAI.Models;

namespace Assets.MirAI.DB {
    public class DbLink : Link, IDbEntity {

        public int Id { get; set; } // only for IDbEntity. Not used.

        public string GetDeleteCommandSuffix() {
            return " WHERE FromId = '" + FromId + "' AND ToId = '" + ToId + "';";
        }

        public string GetInsertCommandSuffix() {
            return " (FromId, ToId) VALUES ('" + FromId + "', '" + ToId + "');";
        }

        public string GetUpdateCommandSuffix() {
            return ";";
        }

        public void SetData(IDataRecord data) {
            try {
                FromId = data.GetInt32(0);
                ToId = data.GetInt32(1);
            }
            catch (Exception ex) {
                throw new DbMirAiException("Convert IDataRecord to Program error.", ex);
            }
        }

        public override string ToString() {
            StringBuilder ret = new StringBuilder($"FromId={FromId,-5} ToId={ToId,-5}");
            return ret.ToString();
        }
    }
}