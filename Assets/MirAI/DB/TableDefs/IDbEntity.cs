using System.Data;
using Mono.Data.Sqlite;

namespace Assets.MirAI.DB.TableDefs {

    public interface IDbEntity {

        public SqliteConnection Connection { get; set; }
        public void SetData(IDataRecord data);
        public string GetInsertCommandSuffix();
        public string GetUpdateCommandSuffix();
        public string GetDeleteCommandSuffix();
    }
}