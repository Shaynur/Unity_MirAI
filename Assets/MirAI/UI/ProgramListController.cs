using Assets.MirAI.Models;
using Assets.MirAI.UI.Widgets;
using Assets.MirAI.Utils;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MirAI.UI {

    public class ProgramListController : MonoBehaviour {

        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private EditorController _editorController;

        private GameSession _session;
        private ProgramItemWidget _current;
        private readonly CompositeDisposable _trash = new CompositeDisposable();


        private void Start() {
            _session = GameSession.Instance;
            _trash.Retain(_session.AiModel.ProgramsChanged.Subscribe(RedrawList));
            RedrawList();
        }

        public void OnItemClickW(ProgramItemWidget piw) {
            if (_current == piw) return;
            ChangeSelection(piw);
            _session.AiModel.CurrentProgram = piw.Program;
            _editorController.CreateScheme();
        }

        private void ChangeSelection(ProgramItemWidget piw) {
            if (_current != null)
                _current.Select(false);
            piw.Select(true);
            _current = piw;
        }

        public void RedrawList() {
            ClearList();
            CreateList();
        }

        public void CreateList() {
            var list = _session.AiModel.Programs;
            foreach (var program in list) {
                var item = GameObjectSpawner.Spawn(_itemPrefab, "ProgramListContent");
                var widget = item.GetComponent<ProgramItemWidget>();
                widget.Set(program);
                _trash.Retain(widget.ItemClicked.Subscribe(OnItemClickW));
                if(program == _session.AiModel.CurrentProgram) {
                    _current = widget;
                    _current.Select(true);
                }
            }
        }

        public void ClearList() {
            _current = null;
            var items = GetComponentsInChildren<Text>();
            foreach (var item in items) {
                Destroy(item.gameObject);
            }
        }

        private void OnDestroy() {
            _trash.Dispose();
        }
    }
}