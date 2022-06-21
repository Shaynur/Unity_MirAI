using UnityEngine;
using UnityEngine.UI;

namespace Assets.MirAI.AiEditor.HUD {

    public class TextForSlider : MonoBehaviour {

        [SerializeField] Text _text;

        public void Change(float value) {
            _text.text = value.ToString();
            unchecked {
                EditNode.Node.Command &= (int)0x7F00FFFF;
            }
            EditNode.Node.Command |= (int)value << 16;
        }
    }
}