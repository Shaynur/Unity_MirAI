using System.Linq;
using Assets.MirAI.Models;
using Assets.MirAI.Simulation;
using Assets.MirAI.UI;
using Assets.MirAI.UI.Widgets;
using Assets.MirAI.Utils;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;

namespace Assets.MirAI.AiEditor {

    public class EditUnitMenu : MenuController {

        //[SerializeField] private EventWithProgram _SelectProgramEvent;
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
            EditUnit.Unit.ProgramId = widget.Program.Id;
        }

        private void CreateList() {
            _currentItem = null;
            var selectProg = _model.Programs.Find(x => x.Id == EditUnit.Unit.ProgramId);
            var list = _model.Programs.OrderBy(x => x.Name);
            foreach (var program in list) {
                var item = GameObjectSpawner.Spawn(_itemPrefab, "SubAiList");
                var widget = item.GetComponent<ProgramItemWidget>();
                widget.Set(program);
                if (program == selectProg || _currentItem == null) {
                    OnItemClick(widget);
                }
                _trash.Retain(widget.ItemClicked.Subscribe(OnItemClick));
            }
        }

        private void OnDestroy() {
            _trash.Dispose();
        }
    }
}