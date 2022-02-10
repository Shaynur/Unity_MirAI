using System.Collections.Generic;
using Assets.MirAI.Models;
using Assets.MirAI.UI;
using Assets.MirAI.UI.Widgets;
using Assets.MirAI.Utils;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;

namespace Assets.MirAI {

    public class EditorController : MonoBehaviour {

        [SerializeField] private GameObject _nodePrefab;
        [SerializeField] private GameObject _linkPrefab;

        private GameSession _session;
        private List<LinkUI> _linksUI;
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
            _linksUI = new List<LinkUI>();
            var program = _session.AiModel.CurrentProgram;
            foreach (var node in program.Nodes) {
                float nodeHeight = SpawnNode(node).GetComponent<RectTransform>().rect.height;
                CreateLinks(node, nodeHeight);
            }
        }

        private void CreateLinks(Node node, float nodeHeight) {
            var links = _session.AiModel.Links.FindAll(x => x.FromId == node.Id);
            foreach (var link in links) {
                var linkUI = new LinkUI(link, nodeHeight);
                linkUI.Widget = SpawnLink(linkUI);
                _linksUI.Add(linkUI);
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

        public void RedrawLinks() {
            foreach (var link in _linksUI) {
                link.Widget.UpdateView();
            }
        }

        private LinkWidget SpawnLink(LinkUI linkUI) {
            var goLinkUI = GameObjectSpawner.Spawn(_linkPrefab, schemeContainer);
            var linkWidget = goLinkUI.GetComponent<LinkWidget>();
            linkWidget.SetData(linkUI);
            return linkWidget;
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