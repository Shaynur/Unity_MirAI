using System.Data;

namespace Assets.MirAI.DB.TableDefs {

    public interface IDbEntity {

        public void SetData(IDataRecord data);
        public string GetInsertCommandSuffix();
        public string GetUpdateCommandSuffix();
        public string GetDeleteCommandSuffix();
    }
}