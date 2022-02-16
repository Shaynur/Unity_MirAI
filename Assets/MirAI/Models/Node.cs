using System.Collections.Generic;
using System.Text;
using Assets.MirAI.UI.Widgets;

namespace Assets.MirAI.Models {

    public class Node : IAiModelElement {

        public int Id { get; set; }
        public int ProgramId { get; set; }
        public NodeType Type { get; set; }
        public int Command { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        //public List<Node> Childs { get; set; } = new List<Node>();
        public List<LinkedChild> LinkedChilds { get; set; } = new List<LinkedChild>();

        public NodeWidget Widget { get; set; }

        public bool Viewed;

        public virtual void AddChild(Link link, Node node) {
            LinkedChilds.Add(new LinkedChild { Link = link, Node = node });
        }

        public virtual void RemoveChild(Node node) {
            var lc = LinkedChilds.Find(x => x.Node == node);
            LinkedChilds.Remove(lc);
        }

        public override string ToString() {
            StringBuilder ret = new StringBuilder($"Id={Id,-5} ProgId={ProgramId,-5} Type={Type,-5} Command={Command,-10} ({X,4},{Y,4})");
            return ret.ToString();
        }
    }

    public enum NodeType {
        Nope = 0,
        Root = 1,
        Action = 2,
        Condition = 3,
        Connector = 4,
        SubAI = 5
    }
}