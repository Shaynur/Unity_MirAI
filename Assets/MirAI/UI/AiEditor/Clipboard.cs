using System.Collections.Generic;
using Assets.MirAI.Models;
using UnityEngine;

namespace Assets.MirAI.UI.AiEditor {

    public static class Clipboard {

        public static List<Node> Nodes;
        private static List<Node> NewNodes;

        public static void CopyFrom(EditorController editor) {
            Nodes = editor.GetSelectedNodes();
        }

        public static void PasteTo(EditorController editor) {
            if (Nodes == null) return;
            PasteNodes(editor);
            PasteLinks();
            AiModel.Instance.LoadFromDB(); // ???
        }

        private static void PasteLinks() {
            var links = new List<Link>();
            for (int i = 0; i < Nodes.Count; i++) {
                var node = Nodes[i];
                foreach (var child in node.Childs) {
                    var indexNodeTo = Nodes.IndexOf(child.Node);
                    if (indexNodeTo != -1) {
                        var link = new Link(NewNodes[i], NewNodes[indexNodeTo]);
                        links.Add(link);
                        EditorPartsFactory.I.SpawnLink(link);
                    }
                }
            }
            AiModel.Instance.AddLinks(links.ToArray());
        }

        private static void PasteNodes(EditorController editor) {
            var curProg = editor.CurrentProgram;
            var offset = editor.GetScreenCenter() - GetHightest();
            NewNodes = new List<Node>();
            foreach (var node in Nodes) {
                var newNode = node.GetCopy();
                newNode.ProgramId = curProg.Id;
                if (newNode.Type == NodeType.Root)
                    newNode.Type = NodeType.Connector;
                if (newNode.Type == NodeType.SubAI && newNode.Command == curProg.Id)
                    newNode.Command = 0;
                newNode.X = node.X + offset.x;
                newNode.Y = node.Y + offset.y;
                NewNodes.Add(newNode);
                EditorPartsFactory.I.SpawnNode(newNode);
            }
            AiModel.Instance.AddNodes(NewNodes.ToArray());
        }

        private static Vector2 GetHightest() {
            if (Nodes.Count == 0) return Vector2.zero;
            Vector2 retval = new Vector2(Nodes[0].X, Nodes[0].Y);
            foreach (var node in Nodes) {
                if (retval.y < node.Y) {
                    retval.x = node.X;
                    retval.y = node.Y;
                }
            }
            return retval;
        }
    }
}