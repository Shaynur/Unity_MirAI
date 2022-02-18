using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.MirAI.Utils;
using Assets.MirAI.Models;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MirAI.UI.AiEditor {

    public class EditorController : MonoBehaviour {

        [SerializeField] private Button _subAiUpButton;

        public readonly CompositeDisposable _trash = new CompositeDisposable();
        private GameSession _session;
        private CanvasCameraController _camController;
        private Rect _viewPort = new Rect();
        private Stack<Program> _subAiStack = new Stack<Program>();

        private void Start() {
            _session = GameSession.Instance;
            _camController = gameObject.GetComponent<CanvasCameraController>();
            _trash.Retain(_session.AiModel.OnLoaded.Subscribe(CreateScheme));
            CreateScheme();
        }

        public void CreateScheme() {
            if (_session.AiModel.CurrentProgram == null) return;
            EditorPartsFactory.I.ClearScheme();
            InitViewport();
            CreateNodes();
            _camController.SetCameraViewport(_viewPort);
        }

        private void InitViewport() {
            var rootNode = _session.AiModel.CurrentProgram.RootNode;
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

        public void GotoSubAi(Node node) {
            var program = _session.AiModel.Programs.Find(x => x.Id == node.Command);
            if (program != null) {
                SubAiDown();
                _session.AiModel.CurrentProgram = program;
            }
        }

        private void SubAiDown() {
            _subAiStack.Push(_session.AiModel.CurrentProgram);
            _subAiUpButton.gameObject.SetActive(true);
        }

        public void SubAiUp() {
            _session.AiModel.CurrentProgram = _subAiStack.Pop();
            if (_subAiStack.Count == 0)
                _subAiUpButton.gameObject.SetActive(false);
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

        public void ToggleSelectionMode() {
            _camController.SelectionMode(true);
        }

        private void OnDestroy() {
            _trash.Dispose();
        }

        //TODO Debug Only
        public void RunProgram() {
            if (_session.AiModel.CurrentProgram == null) return;
            StartCoroutine(LighthNodes(_session.AiModel.CurrentProgram));
        }
        //TODO Debug Only
        private IEnumerator LighthNodes(Program program) {
            program.SortNodesByAngle();
            foreach (var node in program.DFC()) {
                var widget = node.Widget;
                widget.selector.SetState(true);
                yield return new WaitForSeconds(0.5f);
                widget.selector.SetState(false);
            }
        }

        public void OnSelection(Rect rect) {
            UnselectAll();
            var program = _session.AiModel.CurrentProgram;
            foreach (var node in program.Nodes) {
                var curRect = node.Widget.gameObject.GetComponent<RectTransform>().GetWorldRect();
                if (rect.Overlaps(curRect))
                    node.Widget.selector.SetState(true);
            }
        }
    }
}