using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.MirAI.Utils;
using Assets.MirAI.Models;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Assets.MirAI.UI.AiEditor {

    public class EditorController : MonoBehaviour {

        [SerializeField] private Button _subAiUpButton;

        public readonly CompositeDisposable _trash = new CompositeDisposable();
        private CanvasCameraController _camController;
        private Rect _viewPort = new Rect();
        private readonly Stack<int> _subAiStack = new Stack<int>();
        private AiModel _model;

        public Program CurrentProgram {
            get { return _currentProgram; }
            set {
                _currentProgram = value;
                CreateScheme();
                OnCurrentChanged?.Invoke();
            }
        }
        private Program _currentProgram;
        public UnityEvent OnCurrentChanged = new UnityEvent();

        private void Start() {
            _model = AiModel.Instance;
            _camController = gameObject.GetComponent<CanvasCameraController>();
            _trash.Retain(_model.OnLoaded.Subscribe(AiModelLoaded));
            AiModelLoaded();
        }

        public Vector2 GetScreenCenter() {
            return _camController.GetCameraRect().center;
        }

        public void AiModelLoaded() {
            if (_model.Programs != null && _model.Programs.Count > 0) {
                if (_currentProgram != null)
                    _currentProgram = _model.Programs.Find(x => x.Id == _currentProgram.Id);
                if (_currentProgram == null) {
                    _currentProgram = _model.Programs.OrderBy(x => x.Name).ElementAt(0);
                }
            }
            else _currentProgram = null;
            CreateScheme();
        }

        public void CreateScheme() {
            EditorPartsFactory.I.ClearScheme();
            InitViewport();
            CreateNodes();
            _camController.SetCameraViewport(_viewPort);
        }

        private void InitViewport() {
            var indent = 400f;
            var x = 0f;
            var y = 0f;
            if (_currentProgram != null) {
                x = _currentProgram.RootNode.X;
                y = _currentProgram.RootNode.Y;
            }
            _viewPort = new Rect(x - indent, y - indent, 2 * indent, indent);
        }

        private void ChangeViewportSize(float x, float y) {
            if (_viewPort.xMin > x) _viewPort.xMin = x;
            if (_viewPort.yMin > y) _viewPort.yMin = y;
            if (_viewPort.xMax < x) _viewPort.xMax = x;
            if (_viewPort.yMax < y) _viewPort.yMax = y;
        }

        private void CreateNodes() {
            if (_currentProgram == null) return;
            var program = _currentProgram;
            foreach (var node in program.Nodes) {
                node.Widget = EditorPartsFactory.I.SpawnNode(node);
                float nodeHeight = node.Widget.GetComponent<RectTransform>().rect.height;
                CreateLinks(node, nodeHeight);
                ChangeViewportSize(node.X, node.Y);
                ChangeViewportSize(node.X, node.Y - nodeHeight);
            }
        }

        public void GotoSubAi(Node node) {
            var program = _model.Programs.Find(x => x.Id == node.Command);
            if (program != null) {
                SubAiDown();
                CurrentProgram = program;
            }
        }

        private void SubAiDown() {
            _subAiStack.Push(_currentProgram.Id);
            _subAiUpButton.gameObject.SetActive(true);
        }

        public void SubAiUp() {
            var curProgId = _subAiStack.Pop();
            CurrentProgram = _model.Programs.Find(x => x.Id == curProgId) ?? CurrentProgram;
            if (_subAiStack.Count == 0)
                _subAiUpButton.gameObject.SetActive(false);
        }

        public void ClearSubAiStack() {
            _subAiUpButton.gameObject.SetActive(false);
            _subAiStack.Clear();
        }

        private void CreateLinks(Node node, float nodeHeight) {
            var links = _model.Links.FindAll(x => x.NodeFrom == node);
            foreach (var link in links) {
                link.Yoffset = nodeHeight;
                EditorPartsFactory.I.SpawnLink(link);
            }
        }

        public void MoveNodes(Node fromNode, Vector3 offset) {
            if (_currentProgram == null) return;
            var program = _currentProgram;
            foreach (var n in program.DFC(fromNode)) {
                if (n != fromNode)
                    n.Widget.ChangePosition(offset);
            }
            RedrawAllLinks(program);
        }

        private void RedrawAllLinks(Program program) {
            foreach (var node in program.Nodes)
                foreach (var link in _model.Links.FindAll(x => x.NodeFrom == node))
                    link.Widget.UpdateView();
        }

        public void SaveNodesToDB(Node fromNode) {
            if (_currentProgram == null) return;
            var program = _currentProgram;
            var nodes = program.DFC(fromNode).ToArray<Node>();
            _model.UpdateNodes(nodes);
        }

        public void DeleteNodes() {
            var nodesToDelete = GetSelectedNodes();
            nodesToDelete.RemoveAll(x => x.Type == NodeType.Root);
            if (nodesToDelete.Count > 0)
                _model.RemoveNodes(nodesToDelete.ToArray());
        }

        public List<Node> GetSelectedNodes() {
            List<Node> selectedNodes = new List<Node>();
            if (_currentProgram != null) {
                var program = _currentProgram;
                foreach (var node in program.Nodes)
                    if (node.Widget.selector.IsActiv)
                        selectedNodes.Add(node);
            }
            return selectedNodes;
        }

        public void UnselectAll(Node excludingNode) {
            if (_currentProgram == null) return;
            var program = _currentProgram;
            foreach (var node in program.Nodes)
                if (node != excludingNode)
                    node.Widget.selector.SetState(false);
        }

        public void UnselectAll() {
            UnselectAll(null);
        }

        public void ToggleSelectionMode() {
            _camController.SelectionMode(true);
        }

        public void OnSelection(Rect rect) {
            UnselectAll();
            if (_currentProgram == null) return;
            var program = _currentProgram;
            foreach (var node in program.Nodes) {
                var curRect = node.Widget.gameObject.GetComponent<RectTransform>().GetWorldRect();
                if (rect.Overlaps(curRect))
                    node.Widget.selector.SetState(true);
            }
        }

        private void OnDestroy() {
            _trash.Dispose();
        }

        //TODO Debug Only
        public void RunProgram() {
            if (_currentProgram == null) return;
            StartCoroutine(LighthNodes(_currentProgram));
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
    }
}