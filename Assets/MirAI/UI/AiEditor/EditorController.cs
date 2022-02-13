using System;
using System.Linq;
using Assets.MirAI.Models;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.MirAI.UI.AiEditor {

    public class EditorController : MonoBehaviour {

        private GameSession _session;
        public readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start() {
            _session = GameSession.Instance;
            CreateScheme();
        }

        [ContextMenu("CreateScheme")]
        public void CreateScheme() {
            if (_session.AiModel.CurrentProgram == null) return;
            EditorPartsFactory.I.ClearScheme();
            CreateNodes();
        }

        private void CreateNodes() {
            var program = _session.AiModel.CurrentProgram;
            foreach (var node in program.Nodes) {
                node.Widget = EditorPartsFactory.I.SpawnNode(node);
                float nodeHeight = node.Widget.GetComponent<RectTransform>().rect.height;
                CreateLinks(node, nodeHeight);
            }
        }

        private void CreateLinks(Node node, float nodeHeight) {
            var links = _session.AiModel.Links.FindAll(x => x.NodeFrom == node);
            foreach (var link in links) {
                link.Yoffset = nodeHeight;
                EditorPartsFactory.I.SpawnLink(link);
            }
        }

        public void MoveNodes(Node fromNode, Vector3 offset) {
            var program = _session.AiModel.CurrentProgram;
            foreach (var n in program.DFC(fromNode)) {
                if (n != fromNode)
                    n.Widget.ChangePosition(offset);
            }
            RedrawAllLinks(program);
        }

        private void RedrawAllLinks(Program program) {
            foreach (var node in program.Nodes)
                foreach (var link in _session.AiModel.Links.FindAll(x => x.NodeFrom == node))
                    link.Widget.UpdateView();
        }

        public void SaveNodesToDB(Node fromNode) {
            var program = _session.AiModel.CurrentProgram;
            var nodes = program.DFC(fromNode).ToArray<Node>();
            _session.AiModel.UpdateNodes(nodes);
        }

        public void UpdateSelectors(Node excludingNode) {
            var program = _session.AiModel.CurrentProgram;
            foreach (var node in program.Nodes)
                if (node != excludingNode)
                    node.Widget.selector.SetState(false);
        }

        public void UnselectAll() {
            UpdateSelectors(null);
        }

        private void OnDestroy() {
            _trash.Dispose();
        }
    }
}