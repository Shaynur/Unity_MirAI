using Assets.MirAI.Models;
using Assets.MirAI.UI.Widgets;

namespace Assets.MirAI.UI {

    public class LinkUI {

        public Link Link { get; set; }
        public Node NodeFrom { get; set; }
        public Node NodeTo { get; set; }
        public LinkWidget Widget { get; set; }
        public float Angle { get; set; }
        public float NodeHeight { get; set; }

        private GameSession _session;

        public LinkUI(Link link, float nodeHeight = 0) {
            _session = GameSession.Instance;
            Link = link;
            NodeHeight = nodeHeight;
            NodeFrom = _session.AiModel.Nodes.Find(x => x.Id == Link.FromId);
            NodeTo = _session.AiModel.Nodes.Find(x => x.Id == Link.ToId);
        }
    }
}