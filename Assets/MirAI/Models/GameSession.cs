using UnityEngine;

namespace Assets.MirAI.Models {

    public class GameSession : MonoBehaviour {

        public static GameSession Instance { get; private set; }

        private void Awake() {
            var existSession = GetExistSession();
            if (existSession != null) {
                Destroy(gameObject);
            }
            else {
                Instance = this;
                DontDestroyOnLoad(this);
                InitModels();
            }
        }

        private void InitModels() {
        }

        private object GetExistSession() {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessions) {
                if (gameSession != this) {
                    return gameSession;
                }
            }
            return null;
        }
    }
}