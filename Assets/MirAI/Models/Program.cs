using System.Collections.Generic;

namespace Assets.MirAI.Models {

    public class Program {

        public int Id { get; set; }
        public string Name { get; set; }
        public List<Node> Nodes { get; set; } = new List<Node>();
    }
}