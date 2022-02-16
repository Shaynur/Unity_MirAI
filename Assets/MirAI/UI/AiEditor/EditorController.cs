using System;
using System.Collections.Generic;
using System.Linq;
using Assets.MirAI.Models;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;

namespace Assets.MirAI.UI.AiEditor {

    public class EditorController : MonoBehaviour {

        private CanvasCameraController _camController;

        private GameSession _session;
        public readonly CompositeDisposable _trash = new CompositeDisposable();
        private Rect _viewPort = new Rect();

        private void Start() {
            _session = GameSession.Instance;
            _camController = gameObject.GetComponent<CanvasCameraController>();
            _trash.Retain(_session.AiModel.OnLoaded.Subscribe(CreateScheme));
            CreateScheme();
        }

        [ContextMenu("CreateScheme")]
        public void CreateScheme() {
            if (_session.AiModel.CurrentProgram == null) return;
            EditorPartsFactory.I.ClearScheme();
            InitViewport();
            CreateNodes();
            _camController.SetCameraViewport(_viewPort);
        }

        private void InitViewport() {
            var rootNode = _session.AiModel.CurrentProgram?.Nodes.Find(x=>x.Type ==NodeType.Root);
            var indent = 400f;
            _viewPort = new Rect(rootNode.X - indent, rootNode.Y - indent, 2 * indent, indent);
        }

        private void ChangeViewportSize(float x, float y) {
            if (_viewPort.xMin > x) _viewPort.xMin = x;
            if (_viewPort.yMin > y) _viewPort.yMin = y;
            if (_viewPort.xMax < x) _viewPort.xMax = x;
            if (_viewPort.yMax < y) _viewPort.yMax = y;
        }

        private void CreateNodes() {
            var program = _session.AiModel.CurrentProgram;
            foreach (var node in program.Nodes) {
                node.Widget = EditorPartsFactory.I.SpawnNode(node);
                float nodeHeight = node.Widget.GetComponent<RectTransform>().rect.height;
                CreateLinks(node, nodeHeight);
                ChangeViewportSize(node.X, node.Y);
                ChangeViewportSize(node.X, node.Y - nodeHeight);
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

        public void DeleteNodes() {
            var program = _session.AiModel.CurrentProgram;
            List<Node> nodesToDelete = new List<Node>();
            foreach (var node in program.Nodes)
                if (node.Widget.selector.IsActiv && node.Type != NodeType.Root)
                    nodesToDelete.Add(node);
            if (nodesToDelete.Count > 0) {
                _session.AiModel.RemoveNodes(nodesToDelete.ToArray());
            }
        }

        private void OnDestroy() {
            _trash.Dispose();
        }
    }
}