using System.Collections.Generic;
using System.Text;
using Assets.MirAI.UI.Widgets;

namespace Assets.MirAI.Models {

    public class Node : IHaveId {

        public int Id { get; set; }
        public int ProgramId { get; set; }
        public NodeType Type { get; set; }
        public int Command { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public List<LinkedChild> Childs { get; set; } = new List<LinkedChild>();
        public NodeWidget Widget { get; set; }
        public bool Viewed { get; set; }

        public void AddChild(Link link, Node node) {
            Childs.Add(new LinkedChild { Link = link, Node = node });
        }

        public void RemoveChild(Node node) {
            var lc = Childs.Find(x => x.Node == node);
            Childs.Remove(lc);
        }

        public Node GetCopy() {
            Node node = new Node();
            node.Id = this.Id;
            node.ProgramId = this.ProgramId;
            node.Type = this.Type;
            node.Command = this.Command;
            node.X = this.X;
            node.Y = this.Y;
            return node;
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