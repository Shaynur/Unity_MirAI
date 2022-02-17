using System.Linq;
using Assets.MirAI.Models;
using Assets.MirAI.UI.Widgets;
using Assets.MirAI.Utils;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;

namespace Assets.MirAI.UI.HUD {

    public class ProgramListController : MonoBehaviour {

        [SerializeField] private GameObject _itemPrefab;

        private GameSession _session;
        private HudController _hudController;
        private ProgramItemWidget _currentItem;
        public readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start() {
            _session = GameSession.Instance;
            _hudController = GetComponentInParent<HudController>();
            _trash.Retain(_session.AiModel.OnLoaded.Subscribe(RedrawList));
            RedrawList();
        }

        public void OnItemClick(ProgramItemWidget item) {
            if (_currentItem == item) {
                _hudController.HideProgramList();
            }
            ChangeSelection(item);
            _session.AiModel.CurrentProgram = item.Program;
            _hudController.OnSelectProgram();
        }

        private void ChangeSelection(ProgramItemWidget item) {
            if (_currentItem != null)
                _currentItem.Select(false);
            item.Select(true);
            _currentItem = item;
        }

        public void RedrawList() {
            ClearList();
            CreateList();
        }

        private void CreateList() {
            var list = _session.AiModel.Programs.OrderBy(x => x.Name);
            foreach (var program in list) {
                var item = GameObjectSpawner.Spawn(_itemPrefab, "ProgramListContent");
                var widget = item.GetComponent<ProgramItemWidget>();
                widget.Set(program);
                _trash.Retain(widget.ItemClicked.Subscribe(OnItemClick));
                if (program == _session.AiModel.CurrentProgram) {
                    _currentItem = widget;
                    _currentItem.Select(true);
                }
            }
        }

        private void ClearList() {
            _currentItem = null;
            var items = GetComponentsInChildren<ProgramItemWidget>();
            foreach (var item in items) {
                Destroy(item.gameObject);
            }
        }

        public void DeleteCurrentProgram() {
            if (_currentItem != null) {
                _session.AiModel.RemoveProgram(_session.AiModel.CurrentProgram.Id);
            }
        }

        private void OnDestroy() {
            _trash.Dispose();
        }
    }
}