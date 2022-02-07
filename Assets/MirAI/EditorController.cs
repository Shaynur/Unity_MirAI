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
        private List<LinkUI> _linksUI = new List<LinkUI>();
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private static readonly string schemeContainer = "Scheme";

        private void Start() {
            _session = GameSession.Instance;
            CreateScheme(); // TODO for test
        }

        [ContextMenu("CreateScheme")]
        public void CreateScheme() {
            var currentProgId = 1; // TODO for test
            var program = _session.AiModel.Programs.Find(x => x.Id == currentProgId);
            foreach (var node in program.Nodes) {
                float nodeHeight = SpawnNode(node).GetComponent<RectTransform>().rect.height;
                var links = _session.AiModel.Links.FindAll(x => x.FromId == node.Id);
                foreach (var link in links) {
                    var linkUI = new LinkUI(link, nodeHeight);
                    linkUI.Widget = SpawnLink(linkUI);
                    _linksUI.Add(linkUI);
                }
            }
        }

        public void RedrawLinks() {
            foreach (var link in _linksUI) {
                link.Widget.UpdateView();
            }
        }

        private LinkWidget SpawnLink(LinkUI linkUI) {
            Vector3 position = Vector3.zero;
            var goLinkUI = GameObjectSpawner.Spawn(_linkPrefab, position, schemeContainer);
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