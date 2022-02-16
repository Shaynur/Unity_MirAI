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

        public void Toggle() {
            if (_clickBlocker.activeSelf)
                HideProgramList();
            else
                ShowProgramList();
        }

        public void ShowNewProgramMenu() {
            var menu = WindowUtils.CreateWindow("AddProgramMenu", "HUD");
            var controller = menu.GetComponent<AddProgramMenu>();
            controller.EnterNameEvent.Subscribe(GameSession.Instance.AiModel.AddNewProgram);
        }

        public void OnSelectProgram() {
            _editorController.CreateScheme();
        }

        public void OnDeleteButton() {
            if (_progListSH.IsHidden)
                _editorController.DeleteNodes();
            else {
                var menu = WindowUtils.CreateWindow("DeleteProgramMenu", "HUD");
                var deleteController = menu.GetComponent<MenuController>();
                var listController = _programList.GetComponentInChildren<ProgramListController>();
                deleteController.OnOk.Subscribe(listController.DeleteCurrentProgram);
            }
        }
    }
}