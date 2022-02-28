using System;
using Assets.MirAI.Models;
using Assets.MirAI.Utils;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;

namespace Assets.MirAI.UI.AiEditor {
    public static class EditNode {

        public static Link Link { get; set; }
        public static Node Node { get; set; }
        private static Node _originalNode;

        public static void CreateTemplates(Vector3 position, Node parentNode) {
            Node = new Node {
                X = position.x,
                Y = position.y,
                ProgramId = parentNode.ProgramId
            };
            Link = new Link(parentNode, Node);
            EditorPartsFactory.I.SpawnLink(Link);
        }

        public static void UpdateTemplates(Vector3 position) {
            Node.X = position.x;
            Node.Y = position.y;
            Link.Widget.UpdateView();
        }

        public static void SaveNewLinkAndNodeToDb() {
            if (AiModel.Instance.AddLinkAndChildNode(Link))
                EditorPartsFactory.I.SpawnNode(Node);
            else
                ClearTemplates();
            Node = null;
            Link = null;
        }

        public static void UpdateNodeDb() {
            _originalNode = Node.GetCopy();
            AiModel.Instance.UpdateNode(_originalNode);
            _originalNode.Widget.UpdateView();
        }

        public static void ConnectNodes(Node parent, Node child) {
            Link.NodeTo = child;
            Link.ToId = child.Id;
            if (AiModel.Instance.AddLink(Link)) {
                parent.AddChild(Link, child);
                Link.Widget.UpdateView();
            }
            else
                ClearTemplates();
            Node = null;
            Link = null;
        }

        public static void ClearTemplates() {
            if (Link != null)
                GameObject.Destroy(Link.Widget.gameObject);
            Node = null;
            Link = null;
        }

        public static void Edit(Node node) {
            _originalNode = node;
            Node = _originalNode.GetCopy();
            switch (node.Type) {
                case NodeType.Action:
                    EditAction();
                    break;
                case NodeType.Condition:
                    EditCondition();
                    break;
                case NodeType.SubAI:
                    EditSubAi();
                    break;
                default:
                    break;
            }
        }

        private static void EditAction() {
            throw new NotImplementedException(); //TODO EditAction()
        }

        private static void EditCondition() {
            throw new NotImplementedException(); //TODO EditCondition()
        }

        private static void EditSubAi() {
            var menu = WindowUtils.CreateWindow("UI/SelectSubAi", "HUD");
            var controller = menu.GetComponent<SelectSubAiMenu>();
            controller.OnCancel.Subscribe(ClearTemplates);
            controller.OnOk.Subscribe(UpdateNodeDb);
        }

        public static void CreateSelectNodeWindow() {
            var menu = WindowUtils.CreateWindow("UI/AddNodeMenu", "HUD");
            var addMenuController = menu.GetComponent<AddNodeMenuController>();
            addMenuController.OnCancel.Subscribe(ClearTemplates);
            addMenuController.OnOk.Subscribe(SaveNewLinkAndNodeToDb);
        }
    }
}