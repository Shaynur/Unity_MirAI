using System.Collections.Generic;
using Assets.MirAI.DB.Tables;

namespace Assets.MirAI.DB.TableDefs {

    public class Node {

        public int Id { get; set; }
        public int ProgramId { get; set; }
        public int Type { get; set; }
        public int Command { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public List<Node> Parents { get; set; }
        public List<Node> Childs { get; set; }

        public virtual void AddChild(Node node) {
            Childs.Add(node);
            node.Parents.Add(this);
        }

        public virtual void RemoveChild(Node node) {
            Childs.Remove(node);
            node.Parents.Remove(this);
        }

        public virtual void ClearAllLinks() {
            foreach (Node node in Childs)
                RemoveChild(node);
            foreach (Node node in Parents)
                node.RemoveChild(this);
        }
    }
}