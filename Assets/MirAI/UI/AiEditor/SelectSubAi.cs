using System.Linq;
using Assets.MirAI.Models;
using Assets.MirAI.UI.Widgets;
using Assets.MirAI.Utils;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;

namespace Assets.MirAI.UI.AiEditor {

    public class SelectSubAi : MenuController {

        [SerializeField] private EventWithProgram _SelectProgramEvent;
        [SerializeField] private GameObject _itemPrefab;

        public readonly CompositeDisposable _trash = new CompositeDisposable();
        private AiModel _model;
        private ProgramItemWidget _currentItem = null;

        public override void Start() {
            base.Start();
            _model = GameSession.Instance.AiModel;
            _okButton.interactable = false;
            CreateList();
        }

        public void OnItemClick(ProgramItemWidget item) {
            if (_currentItem != null && _currentItem != item) _currentItem.Select(false);
            _currentItem = item;
            item.Select(true);
            _okButton.interactable = true;
            LowerConnectorController.TempConnectorLink.NodeTo.Command = item.Program.Id;
        }

        private void CreateList() {
            var list = _model.Programs.OrderBy(x => x.Name);
            foreach (var program in list) {
                if (program == _model.CurrentProgram) continue;
                var item = GameObjectSpawner.Spawn(_itemPrefab, "SubAiList");
                var widget = item.GetComponent<ProgramItemWidget>();
                widget.Set(program);
                _trash.Retain(widget.ItemClicked.Subscribe(OnItemClick));
            }
        }

        private void OnDestroy() {
            _trash.Dispose();
        }
    }
}