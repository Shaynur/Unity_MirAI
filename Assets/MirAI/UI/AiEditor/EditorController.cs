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
                float nodeHeight = SpawnNode(node).GetComponent<RectTransform>().rect.height;
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

        public void RedrawLinks(GameObject go) {
            var node = go.GetComponent<NodeWidget>().Node;

            RedrawAllLinksIn(l => l.NodeTo == node);

            foreach (var n in _session.AiModel.CurrentProgram.DFC(node))
                RedrawAllLinksIn(l => l.NodeFrom == n);
        }

        private void RedrawAllLinksIn(System.Predicate<Link> match) {
            foreach (var link in _session.AiModel.Links.FindAll(match))
                link.Widget.UpdateView();
        }

        private void SpawnLink(Link link) {
            var goLink = GameObjectSpawner.Spawn(_linkPrefab, schemeContainer);
            var linkWidget = goLink.GetComponent<LinkWidget>();
            link.Widget = linkWidget;
            linkWidget.SetData(link);
        }

        public GameObject SpawnNode(Node node) {
            Vector3 position = new Vector3(node.X, node.Y, 0);
            var nodeUI = GameObjectSpawner.Spawn(_nodePrefab, position, schemeContainer);
            var nodeWidget = nodeUI.GetComponent<NodeWidget>();
            nodeWidget.SetData(node);
            _trash.Retain(nodeWidget.OnMove.Subscribe(RedrawLinks));
            return nodeUI;
        }

        private void OnDestroy() {
            _trash.Dispose();
        }
    }
}