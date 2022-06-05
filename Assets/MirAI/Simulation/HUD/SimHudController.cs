using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.MirAI.Simulation.HUD {
    public class SimHudController : MonoBehaviour {

        public void OnBackButton() {
            SceneManager.LoadScene("MainMenu");
        }
    }
}