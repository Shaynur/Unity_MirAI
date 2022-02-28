using Assets.MirAI.Models;
using Assets.MirAI.UI.AiEditor;
using Assets.MirAI.Utils;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;

namespace Assets.MirAI.UI.HUD {

    public class HudController : MonoBehaviour {

        [SerializeField] private GameObject _clickBlocker;
        [SerializeField] private GameObject _buttonsPanel;
        [SerializeField] private GameObject _programList;
        [SerializeField] private EditorController _editorController;

        private ShowHide _progListSH;

        private void Start() {
            _progListSH = _programList.GetComponent<ShowHide>();
        }

        public void ShowProgramList() {
            _editorController.UnselectAll();
            _progListSH.Show();
            _clickBlocker.SetActive(true);
        }

        public void HideProgramList() {
            _progListSH.Hide();
            _clickBlocker.SetActive(false);
        }

        public void ToggleProgramListVisible() {
            if (_clickBlocker.activeSelf)
                HideProgramList();
            else
                ShowProgramList();
        }

        public void OnDeleteButton() {
            if (_progListSH.IsHidden)
                _editorController.DeleteNodes();
            else {
                if (_editorController.CurrentProgram == null) return;
                var menu = WindowUtils.CreateWindow("UI/DeleteProgramMenu", "HUD");
                var deleteController = menu.GetComponent<MenuController>();
                var listController = _programList.GetComponentInChildren<ProgramListController>();
                deleteController.OnOk.Subscribe(listController.DeleteCurrentProgram);
            }
        }

        public void OnEditButton() {
            if (_progListSH.IsHidden)
                ShowEditNodeMenu();
            else
                ShowEditProgramNameMenu();
        }

        public void OnCopyButton() {
            if (_progListSH.IsHidden)
                Clipboard.CopyFrom(_editorController);
            else {
                // on press Copy button when Proglist is open
            }
        }

        public void OnPasteButton() {
            if (_progListSH.IsHidden)
                Clipboard.PasteTo(_editorController);
            else {
                // on press Paste button when Proglist is open
            }
        }

        private void ShowEditNodeMenu() {
            var select = _editorController.GetSelectedNodes();
            if (select.Count == 1) {
                var node = select[0];
                if (node.Type == NodeType.Root) 
                    ShowEditProgramNameMenu();
                else
                    EditNode.Edit(node);
            }
        }

        private void ShowEditProgramNameMenu() {
            if (_editorController.CurrentProgram == null) return;
            var menu = WindowUtils.CreateWindow("UI/EditProgramName", "HUD");
            var controller = menu.GetComponent<EditProgramNameMenu>();
            var listController = _programList.GetComponentInChildren<ProgramListController>();
            controller.SetEditProgram(_editorController.CurrentProgram);
            controller.OnOk.Subscribe(listController.RedrawList);
            controller.OnOk.Subscribe(_editorController.CurrentProgram.RootNode.Widget.UpdateView);
        }

        public void ShowNewProgramMenu() {
            WindowUtils.CreateWindow("UI/EditProgramName", "HUD");
        }

        public void SelectionModeToggle() {
            _editorController.ToggleSelectionMode();
        }
    }
}