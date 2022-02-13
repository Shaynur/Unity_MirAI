using Assets.MirAI.Models;
using Assets.MirAI.UI.Widgets;
using Assets.MirAI.Utils;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;

namespace Assets.MirAI.UI.AiEditor {

    [CreateAssetMenu(menuName = "MirAi/EditorPartsFactory", fileName = "EditorPartsFactory")]
    public class EditorPartsFactory : ScriptableObject {

        [SerializeField] private GameObject[] _nodePrefabs;
        [SerializeField] private GameObject _linkPrefab;

        public static EditorPartsFactory I => _instance == null ? Load() : _instance;
        private static EditorPartsFactory _instance;
        private static readonly string schemeNodesContainer = "EditorNodes";
        private static readonly string schemeLinksContainer = "EditorLinks";

        private static EditorPartsFactory Load() {
            return _instance = Resources.Load<EditorPartsFactory>("EditorPartsFactory");
        }

        public LinkWidget SpawnLink(Link link) {
            var goLink = GameObjectSpawner.Spawn(_linkPrefab, schemeLinksContainer);
            var linkWidget = goLink.GetComponent<LinkWidget>();
            link.Widget = linkWidget;
            linkWidget.SetData(link);
            return linkWidget;
        }

        public NodeWidget SpawnNode(Node node) {
            Vector3 position = new Vector3(node.X, node.Y, 0);
            var prefab = _nodePrefabs[(int)node.Type];
            var nodeUI = GameObjectSpawner.Spawn(prefab, position, schemeNodesContainer);
            var nodeWidget = nodeUI.GetComponent<NodeWidget>();
            node.Widget = nodeWidget;
            nodeWidget.SetData(node);
            var editorController = nodeWidget.GetComponentInParent<EditorController>();
            editorController._trash.Retain(node.Widget.OnMove.Subscribe(editorController.MoveNodes));
            editorController._trash.Retain(node.Widget.OnEndMove.Subscribe(editorController.SaveNodesToDB));
            editorController._trash.Retain(node.Widget.OnSelect.Subscribe(editorController.UpdateSelectors));
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