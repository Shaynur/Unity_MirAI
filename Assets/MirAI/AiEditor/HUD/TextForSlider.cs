using UnityEngine;
using UnityEngine.UI;

namespace Assets.MirAI.AiEditor.HUD {

    public class TextForSlider : MonoBehaviour {

        [SerializeField] Text _text;

        public void Change(float value) {
            _text.text = value.ToString();
        }
    }
}