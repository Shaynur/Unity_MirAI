using System.Linq;
using Assets.MirAI.Models;
using Assets.MirAI.UI;
using Assets.MirAI.UI.Widgets;
using Assets.MirAI.Utils;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;

namespace Assets.MirAI.AiEditor {

    public class SelectSubAiMenu : MenuController {

        [SerializeField] private EventWithProgram _SelectProgramEvent;
        [SerializeField] private GameObject _itemPrefab;

        public readonly CompositeDisposable _trash = new CompositeDisposable();
        private AiModel _model;
        private ProgramItemWidget _currentItem = null;

        public override void Start() {
            base.Start();
            _model = AiModel.Instance;
            _okButton.interactable = false;
            CreateList();
        }

        public void OnItemClick(ProgramItemWidget widget) {
            if (_currentItem != null && _currentItem != widget) _currentItem.Select(false);
            _currentItem = widget;
            widget.Select(true);
            _okButton.interactable = true;
            EditNode.Node.Command = widget.Program.Id;
        }

        private void CreateList() {
            var curProg = _model.Programs.Find(x => x.Id == EditNode.Node.ProgramId);
            var selectProg = _model.Programs.Find(x => x.Id == EditNode.Node.Command);
            var list = _model.Programs.OrderBy(x => x.Name);
            foreach (var program in list) {
                if (program == curProg) continue;
                var item = GameObjectSpawner.Spawn(_itemPrefab, "SubAiList");
                var widget = item.GetComponent<ProgramItemWidget>();
                widget.Set(program);
                if (program == selectProg) {
                    _currentItem = widget;
                    widget.Select(true);
                }
                _trash.Retain(widget.ItemClicked.Subscribe(OnItemClick));
            }
        }

        private void OnDestroy() {
            _trash.Dispose();
        }
    }
}