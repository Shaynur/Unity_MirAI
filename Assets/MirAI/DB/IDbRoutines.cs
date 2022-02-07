using System.Data;

namespace Assets.MirAI.DB {

    public interface IDbRoutines {

        public void SetData(IDataRecord data);
        public string GetInsertCommandSuffix();
        public string GetUpdateCommandSuffix();
        public string GetDeleteCommandSuffix();
    }
}