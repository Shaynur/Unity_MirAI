using System.Data;
using System.Text;
using Assets.MirAI.DB.TableDefs;

namespace Assets.MirAI.DB.Tables {

    public class DbNode : Node, IDbEntity {

        public string GetDeleteCommandSuffix() {
            return " WHERE Id = '" + Id + "';";
        }

        public string GetInsertCommandSuffix() {
            return " (ProgramId, Type, Command, X, Y) VALUES ('"
                + ProgramId + "', '" + Type + "', '" + Command + "', '" + X + "', '" + Y + "');";
        }

        public string GetUpdateCommandSuffix() {
            return " SET ProgramId = '" + ProgramId
                + "', Type = '" + Type
                + "', Command = '" + Command
                + "', X = '" + X
                + "', Y = '" + Y
                + "' WHERE Id = '" + Id + "';";
        }

        public void SetData(IDataRecord data) {
            var count = data.FieldCount;
            if (count != 6) return;
            Id = data.GetInt32(0);
            ProgramId = data.GetInt32(1);
            Type = data.GetInt32(2);
            Command = data.GetInt32(3);
            X = data.GetInt32(4);
            Y = data.GetInt32(5);
        }

        public override string ToString() {
            StringBuilder ret = new StringBuilder($"Id={Id,-5} ProgId={ProgramId,-5} Type={Type,-5} Command={Command,-10} ({X,4},{Y,4})");
            return ret.ToString();
        }
    }
}