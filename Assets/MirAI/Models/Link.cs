using System.Text;
using Assets.MirAI.UI.Widgets;
using UnityEngine;

namespace Assets.MirAI.Models {

    public class Link : IAiModelElement {
        public int Id { get; set; }
        public int FromId { get; set; }
        public int ToId { get; set; }

        public Node NodeFrom { get; set; }
        public Node NodeTo { get; set; }
        public LinkWidget Widget { get; set; }
        public Vector3 Center { get; set; }
        public float Angle { get; set; }
        public float Lenght { get; set; }
        public float Yoffset { get; set; }

        public Link(int fromId, int toId) {
            FromId = fromId;
            ToId = toId;
        }

        public Link(Node nodeFrom, Node nodeTo) {
            NodeFrom = nodeFrom;
            NodeTo = nodeTo;
            FromId = nodeFrom.Id;
            ToId = nodeTo.Id;
            if (NodeFrom.Widget != null) {
                Yoffset = NodeFrom.Widget.GetComponent<RectTransform>().rect.height;
                CalculateGraphdata();
            }
        }

        public void CalculateGraphdata() {
            var dx = NodeFrom.X - NodeTo.X;
            var dy = NodeFrom.Y - Yoffset - NodeTo.Y;
            Center = new Vector3(NodeTo.X + dx / 2, NodeTo.Y + dy / 2, 0);
            Lenght = Mathf.Sqrt(dx * dx + dy * dy);
            if (Lenght != 0) {
                Angle = Mathf.Acos(dy / Lenght) * 180 / Mathf.PI;
                Angle = dx > 0 ? 180 - Angle : 180 + Angle;
                Lenght = Mathf.Max(Lenght - 16, 0);
            }
        }

        public override string ToString() {
            StringBuilder ret = new StringBuilder($"FromId={FromId,-5} ToId={ToId,-5}");
            return ret.ToString();
        }
    }
}