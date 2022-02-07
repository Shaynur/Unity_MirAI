using System.Collections.Generic;
using System.Text;
using Assets.MirAI.DB;

namespace Assets.MirAI.Models {

    public class Program : IAiModelElement {

        public int Id { get; set; }
        public string Name { get; set; }
        public List<Node> Nodes { get; set; } = new List<Node>();
        public IDbRoutines dbRoutines { get; set; }

        public Program() {
            dbRoutines = new DbProgram(this);
        }

        public override string ToString() {
            StringBuilder ret = new StringBuilder($"Id={Id,-5} Name={Name}");
            return ret.ToString();
        }
    }
}