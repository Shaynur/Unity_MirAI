using System.Data;
using System.Text;
using Assets.MirAI.DB.TableDefs;

namespace Assets.MirAI.DB.Tables {
    public class DbLink : Link, IDbEntity {

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
            var count = data.FieldCount;
            if (count != 2) return;
            FromId = data.GetInt32(0);
            ToId = data.GetInt32(1);
        }

        public override string ToString() {
            StringBuilder ret = new StringBuilder($"FromId={FromId,-5} ToId={ToId,-5}");
            return ret.ToString();
        }
    }
}