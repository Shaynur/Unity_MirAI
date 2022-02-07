using System;
using System.Collections.Generic;

namespace Assets.MirAI.Models {

    public class Node {

        public int Id { get; set; }
        public int ProgramId { get; set; }
        public NodeType Type { get; set; }
        public int Command { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public List<ChildNode> Childs { get; set; } = new List<ChildNode>();

        public virtual void AddChild(Node node) {
            Childs.Add(new ChildNode() { Node = node });
        }

        public virtual void RemoveChild(Node node) {
            var removeItem = Childs.Find(x => x.Node == node);
            Childs.Remove(removeItem);
        }

        private void SortingChildsByAngle() {
            throw new NotImplementedException();
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