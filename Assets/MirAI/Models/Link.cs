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

        public override string ToString() {
            StringBuilder ret = new StringBuilder($"FromId={FromId,-5} ToId={ToId,-5}");
            return ret.ToString();
        }

        internal void CalculateGraphdata() {
            var dx = NodeFrom.X - NodeTo.X;
            var dy = NodeFrom.Y - Yoffset - NodeTo.Y;
            Center = new Vector3(NodeTo.X + dx / 2, NodeTo.Y + dy / 2, 0);
            Lenght = Mathf.Sqrt(dx * dx + dy * dy);
            Angle = Mathf.Acos(dy / Lenght) * 180 / Mathf.PI;
            Angle = dx > 0 ? 360 - Angle : Angle;
        }
    }
}