using System.Collections.Generic;
using System.Text;

namespace Assets.MirAI.Models {

    public class Program : IAiModelElement {

        public int Id { get; set; }
        public string Name { get; set; }
        public List<Node> Nodes { get; set; } = new List<Node>();

        public IEnumerable<Node> DFC(Node fromNode) {
            AllUnViewed();
            return InternalDFC(fromNode);
        }

        private static IEnumerable<Node> InternalDFC(Node fromNode) {
            if (!fromNode.Viewed)
                yield return fromNode;
            if (!fromNode.Viewed) {
                fromNode.Viewed = true;
                foreach (var child in fromNode.Childs)
                    foreach (var node in InternalDFC(child))
                        yield return node;
            }
        }

        public void AllUnViewed() {
            foreach (Node node in Nodes) {
                node.Viewed = false;
            }
        }

        public override string ToString() {
            StringBuilder ret = new StringBuilder($"Id={Id,-5} Name={Name}");
            return ret.ToString();
        }
    }
}