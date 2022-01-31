using System.Collections.Generic;

namespace Assets.MirAI.DB.TableDefs {

    public class DbProgramDef {

        public int Id { get; set; }
        public string Name { get; set; }
        public List<DbNodeDef> Nodes { get; set; } = new List<DbNodeDef>();
    }
}