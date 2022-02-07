﻿using System.Collections.Generic;
using System.Text;
using Assets.MirAI.DB;

namespace Assets.MirAI.Models {

    public class Node : IAiModelElement {

        public int Id { get; set; }
        public int ProgramId { get; set; }
        public NodeType Type { get; set; }
        public int Command { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public List<ChildNode> Childs { get; set; } = new List<ChildNode>();
        public IDbRoutines dbRoutines { get; set; }

        public Node() {
            dbRoutines = new DbNode(this);
        }

        public virtual void AddChild(Node node) {
            Childs.Add(new ChildNode() { Node = node });
        }

        public virtual void RemoveChild(Node node) {
            var removeItem = Childs.Find(x => x.Node == node);
            Childs.Remove(removeItem);
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