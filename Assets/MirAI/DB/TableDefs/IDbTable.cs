using System.Data;

namespace Assets.MirAI.DB.TableDefs {

    public interface IDbTable {

        public void SetData(IDataRecord data);
    }
}