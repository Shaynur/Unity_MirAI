using Assets.MirAI.Models;
using UnityEngine;

namespace Assets.MirAI.UI.Widgets {

    public class LinkWidget : MonoBehaviour {

        public Link Link;

        public void SetData(Link link) {
            Link = link;
            UpdateView();
        }

        public void UpdateView() {
            if (Link == null) return;
            Link.CalculateGraphdata();
            transform.position = Link.Center;
            transform.localScale = new Vector3(transform.localScale.x, Link.Lenght, 1f);
            transform.rotation = Quaternion.AngleAxis(Link.Angle, Vector3.forward);
        }

        public void DeleteLink() {
            AiModel.Instance.RemoveLink(Link);
            Link.NodeFrom.RemoveChild(Link.NodeTo);
            Destroy(gameObject);
        }
    }
}