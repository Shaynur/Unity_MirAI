using Assets.MirAI.Models;
using Assets.MirAI.UI.Widgets;
using Assets.MirAI.Utils;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;

namespace Assets.MirAI.AiEditor {

    [CreateAssetMenu(menuName = "MirAi/EditorPartsFactory", fileName = "EditorPartsFactory")]
    public class EditorPartsFactory : ScriptableObject {

        [SerializeField] private GameObject[] _nodePrefabs;
        [SerializeField] private GameObject _linkPrefab;

        public static EditorPartsFactory I => _instance == null ? Load() : _instance;
        private static EditorPartsFactory _instance;
        private const string schemeNodesContainer = "Nodes_Container";
        private const string schemeLinksContainer = "Links_Container";

        private static EditorPartsFactory Load() {
            return _instance = Resources.Load<EditorPartsFactory>("EditorPartsFactory");
        }

        public LinkWidget SpawnLink(Link link, string containerName = schemeLinksContainer) {
            var goLink = GameObjectSpawner.Spawn(_linkPrefab, containerName);
            var linkWidget = goLink.GetComponent<LinkWidget>();
            link.Widget = linkWidget;
            linkWidget.SetData(link);
            return linkWidget;
        }

        public NodeWidget SpawnNode(Node node, string containerName = schemeNodesContainer) {
            Vector3 position = new Vector3(node.X, node.Y, 0);
            var prefab = _nodePrefabs[(int)node.Type];
            var nodeUI = GameObjectSpawner.Spawn(prefab, position, containerName);
            var nodeWidget = nodeUI.GetComponent<NodeWidget>();
            node.Widget = nodeWidget;
            nodeWidget.SetData(node);
            var editorController = nodeWidget.GetComponentInParent<EditorController>();
            editorController._trash.Retain(node.Widget.OnMove.Subscribe(editorController.MoveNodes));
            editorController._trash.Retain(node.Widget.OnEndMove.Subscribe(editorController.SaveNodesToDB));
            editorController._trash.Retain(node.Widget.OnSelect.Subscribe(editorController.UnselectAll));
            editorController._trash.Retain(node.Widget.OnSubAi.Subscribe(editorController.GotoSubAi));
            return nodeWidget;
        }

        public void ClearScheme() {
            Component[] items = GameObjectSpawner.GetContainer(schemeLinksContainer).GetComponentsInChildren<LinkWidget>();
            foreach (var item in items)
                Destroy(item.gameObject);

            items = GameObjectSpawner.GetContainer(schemeNodesContainer).GetComponentsInChildren<NodeWidget>();
            foreach (var item in items)
                Destroy(item.gameObject);
        }
    }
}