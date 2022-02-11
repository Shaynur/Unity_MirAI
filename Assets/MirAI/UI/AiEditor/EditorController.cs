using System.Linq;
using Assets.MirAI.Models;
using Assets.MirAI.UI.Widgets;
using Assets.MirAI.Utils;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;

namespace Assets.MirAI.UI.AiEditor {

    public class EditorController : MonoBehaviour {

        [SerializeField] private GameObject _nodePrefab;
        [SerializeField] private GameObject _linkPrefab;

        private GameSession _session;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private static readonly string schemeContainer = "Scheme";

        private void Start() {
            _session = GameSession.Instance;
            _trash.Retain(_session.AiModel.ProgramsChanged.Subscribe(CreateScheme));
            CreateScheme();
        }

        [ContextMenu("CreateScheme")]
        public void CreateScheme() {
            if (_session.AiModel.CurrentProgram == null) return;
            ClearScheme();
            CreateNodes();
        }

        private void CreateNodes() {
            var program = _session.AiModel.CurrentProgram;
            foreach (var node in program.Nodes) {
                node.Widget = SpawnNode(node);
                float nodeHeight = node.Widget.GetComponent<RectTransform>().rect.height; // ?????? gameObject ?
                CreateLinks(node, nodeHeight);
            }
        }

        private void CreateLinks(Node node, float nodeHeight) {
            var links = _session.AiModel.Links.FindAll(x => x.NodeFrom == node);
            foreach (var link in links) {
                link.Yoffset = nodeHeight;
                SpawnLink(link);
            }
        }

        private void ClearScheme() {
            var container = GameObjectSpawner.GetContainer(schemeContainer);
            Component[] items = container.GetComponentsInChildren<LinkWidget>();
            foreach (var item in items)
                Destroy(item.gameObject);
            items = container.GetComponentsInChildren<NodeWidget>();
            foreach (var item in items)
                Destroy(item.gameObject);
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
                foreach (var link in _session.AiModel.Links.FindAll(x => x.FromId == node.Id))
                    link.Widget.UpdateView();
        }

        public void SaveNodesToDB(Node fromNode) {
            var program = _session.AiModel.CurrentProgram;
            var nodes = program.DFC(fromNode).ToArray<Node>();
            _session.AiModel.UpdateNodes(nodes);
        }

        private void SpawnLink(Link link) {
            var goLink = GameObjectSpawner.Spawn(_linkPrefab, schemeContainer);
            var linkWidget = goLink.GetComponent<LinkWidget>();
            link.Widget = linkWidget;
            linkWidget.SetData(link);
        }

        public NodeWidget SpawnNode(Node node) {
            Vector3 position = new Vector3(node.X, node.Y, 0);
            var nodeUI = GameObjectSpawner.Spawn(_nodePrefab, position, schemeContainer);
            var nodeWidget = nodeUI.GetComponent<NodeWidget>();
            nodeWidget.SetData(node);
            _trash.Retain(nodeWidget.OnMove.Subscribe(MoveNodes));
            _trash.Retain(nodeWidget.OnEndMove.Subscribe(SaveNodesToDB));
            return nodeWidget;
        }

        private void OnDestroy() {
            _trash.Dispose();
        }
    }
}