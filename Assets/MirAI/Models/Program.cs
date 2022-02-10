using System.Collections.Generic;
using System.Text;

namespace Assets.MirAI.Models {

    public class Program : IAiModelElement {

        public int Id { get; set; }
        public string Name { get; set; }
        public List<Node> Nodes { get; set; } = new List<Node>();

        public override string ToString() {
            StringBuilder ret = new StringBuilder($"Id={Id,-5} Name={Name}");
            return ret.ToString();
        }
    }
}