using System.Data;

namespace Assets.MirAI.DB {

    public interface IDbEntity {

        public int Id { get; set; }
        public void SetData(IDataRecord data);
        public string GetInsertCommandSuffix();
        public string GetUpdateCommandSuffix();
        public string GetDeleteCommandSuffix();
    }
}