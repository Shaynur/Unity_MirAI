using Assets.MirAI.Models;
using UnityEngine;

namespace Assets.MirAI.UI {

    public class HudController : MonoBehaviour {

        private GameSession _session;

        private void Start() {
            _session = GameSession.Instance;
        }

        public void LoadAi() {
            _session.AiModel.LoadFromDB();
        }
    }
}