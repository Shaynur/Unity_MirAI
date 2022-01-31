using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.MirAI.UI {

    public class MainMenuWindow : MonoBehaviour {

        public void OnAiEditor() {
            SceneManager.LoadScene("AiEditor");
        }

        public void OnExit() {

            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}