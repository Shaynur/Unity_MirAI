using Assets.MirAI.UI.AiEditor;
using UnityEngine;

namespace Assets.MirAI.UI.HUD {

    public class HudController : MonoBehaviour {

        [SerializeField] private GameObject _clickBlocker;
        [SerializeField] private GameObject _buttonsPanel;
        [SerializeField] private GameObject _programList;
        [SerializeField] private EditorController _editorController;

        private void Start() {
        }

        public void ShowProgramList() {
            _programList.GetComponent<ShowHide>().Show();
            _clickBlocker.SetActive(true);
        }

        public void HideProgramList() {
            _programList.GetComponent<ShowHide>().Hide();
            _clickBlocker.SetActive(false);
        }

        public void Toggle() {
            if (_clickBlocker.activeSelf)
                HideProgramList();
            else
                ShowProgramList();
        }

        public void OnSelectProgram() {
            _editorController.CreateScheme();
        }

        public void OnDeleteButton() {
            _editorController.DeleteNodes();
        }
    }
}