using Assets.MirAI.Models;
using Assets.MirAI.UI;
using Assets.MirAI.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.MirAI.AiEditor.HUD {

    public class EditorHudController : MonoBehaviour {

        [SerializeField] private GameObject _clickBlocker;
        [SerializeField] private GameObject _buttonsPanel;
        [SerializeField] private GameObject _programList;
        [SerializeField] private EditorController _editorController;

        private ShowHideSideMunu _progListSH;

        private void Start() {
            _progListSH = _programList.GetComponent<ShowHideSideMunu>();
        }

        public void OnBackButton() {
            SceneManager.LoadScene("MainMenu");
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
                var listController = _programList.GetComponentInChildren<ProgramListController>();
                WindowUtils.CreateMenuWindow(
                    "UI/DeleteProgramMenu",
                    "HUD",
                    listController.DeleteCurrentProgram,
                    null);
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
                // TODO on press Copy button when Proglist is open
            }
        }

        public void OnPasteButton() {
            if (_progListSH.IsHidden)
                Clipboard.PasteTo(_editorController);
            else {
                // TODO on press Paste button when Proglist is open
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
            var listController = _programList.GetComponentInChildren<ProgramListController>();
            var controller = (EditProgramNameMenu)WindowUtils.CreateMenuWindow(
                "UI/EditProgramName",
                "HUD",
                listController.RedrawList,
                _editorController.CurrentProgram.RootNode.Widget.UpdateView);
            controller.SetEditProgram(_editorController.CurrentProgram);
        }

        public void ShowNewProgramMenu() {
            WindowUtils.CreateMenuWindow("UI/EditProgramName", "HUD", null, null);
        }

        public void SelectionModeToggle() {
            _editorController.ToggleSelectionMode();
        }
    }
}