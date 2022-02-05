using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.MirAI {

    public class EditorController : MonoBehaviour {

        private void Start() {
            SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
        }
    }
}