using System.Collections.Generic;
using System.Linq;
using Assets.MirAI.Models;
using Assets.MirAI.UI.AiEditor;
using Assets.MirAI.UI.Widgets;
using Assets.MirAI.Utils;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;

namespace Assets.MirAI.UI.HUD {

    public class ProgramListController : MonoBehaviour {

        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private GameObject _content;
        [SerializeField] private EditorController _editorController;

        public readonly CompositeDisposable _trash = new CompositeDisposable();
        private AiModel _model;
        private HudController _hudController;
        private ProgramItemWidget _currentItem;
        private readonly List<ProgramItemWidget> _itemList = new List<ProgramItemWidget>();

        private void Start() {
            _model = AiModel.Instance;
            _hudController = GetComponentInParent<HudController>();
            _trash.Retain(_model.OnLoaded.Subscribe(RedrawList));
            _trash.Retain(_editorController.OnCurrentChanged.Subscribe(ChangeCurrentProgram));
            RedrawList();
        }

        public void OnItemClick(ProgramItemWidget item) {
            if (_currentItem == item)
                _hudController.HideProgramList();
            else
                _editorController.ClearSubAiStack();
            _editorController.CurrentProgram = item.Program;
        }

        public void ChangeCurrentProgram() {
            RedrawList();
            var curProg = _editorController.CurrentProgram;
            var newCurrent = _itemList.Find(x => x.Program == curProg);
            ChangeSelection(newCurrent);
        }

        private void ChangeSelection(ProgramItemWidget item) {
            if (item == null) return;
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
            var list = _model.Programs.OrderBy(x => x.Name);
            foreach (var program in list) {
                var item = GameObjectSpawner.Spawn(_itemPrefab, _content.name);
                var widget = item.GetComponent<ProgramItemWidget>();
                widget.Set(program);
                _trash.Retain(widget.ItemClicked.Subscribe(OnItemClick));
                _itemList.Add(widget);
                if (program == _editorController.CurrentProgram) {
                    _currentItem = widget;
                    _currentItem.Select(true);
                }
            }
        }

        private void ClearList() {
            _currentItem = null;
            foreach (var item in _itemList)
                Destroy(item.gameObject);
            _itemList.Clear();
        }

        public void DeleteCurrentProgram() {
            if (_currentItem != null) {
                _model.RemoveProgram(_editorController.CurrentProgram);
            }
        }

        private void OnDestroy() {
            _trash.Dispose();
        }
    }
}