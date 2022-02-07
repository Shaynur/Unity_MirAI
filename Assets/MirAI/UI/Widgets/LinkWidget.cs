using UnityEngine;

namespace Assets.MirAI.UI.Widgets {

    public class LinkWidget : MonoBehaviour {

        public LinkUI LinkUI;

        private void Start() {
        }

        public void SetData(LinkUI linkUI) {
            LinkUI = linkUI;
            UpdateView();
        }

        public void UpdateView() {
            if (LinkUI == null) return;

            var dx = LinkUI.NodeFrom.X - LinkUI.NodeTo.X;
            var dy = LinkUI.NodeFrom.Y - LinkUI.NodeHeight - LinkUI.NodeTo.Y;
            var position = new Vector3(LinkUI.NodeTo.X + dx / 2, LinkUI.NodeTo.Y + dy / 2, 0);
            transform.position = position;

            var lenght = Mathf.Sqrt(dx * dx + dy * dy);
            transform.localScale = new Vector3(transform.localScale.x, lenght, 1f);

            var angle = Mathf.Acos(dy / lenght) * 180 / Mathf.PI;
            angle = dx > 0 ? 360 - angle : angle;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            LinkUI.Angle = angle;
        }
    }
}