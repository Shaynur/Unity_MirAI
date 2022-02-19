using System.Collections.Generic;
using System.Text;

namespace Assets.MirAI.Models {

    public class Program : IHaveId {

        public int Id { get; set; }
        public string Name { get; set; }
        public List<Node> Nodes { get; set; } = new List<Node>();
        public Node RootNode => _rootNode ?? GetRootNode();
        private Node _rootNode = null;

        public void SortNodesByAngle() {
            foreach (var node in Nodes)
                node.Childs.Sort();
        }

        public IEnumerable<Node> DFC() {
            return DFC(RootNode);
        }

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
                    foreach (var node in InternalDFC(child.Node))
                        yield return node;
            }
        }

        private void AllUnViewed() {
            foreach (Node node in Nodes)
                node.Viewed = false;
        }

        private Node GetRootNode() {
            _rootNode = Nodes.Find(x => x.Type == NodeType.Root);
            return _rootNode;
        }

        public override string ToString() {
            StringBuilder ret = new StringBuilder($"Id={Id,-5} Name={Name}");
            return ret.ToString();
        }
    }
}