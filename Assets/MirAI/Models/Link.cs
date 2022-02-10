using System.Text;

namespace Assets.MirAI.Models {

    public class Link : IAiModelElement {
        public int Id { get; set; }
        public int FromId { get; set; }
        public int ToId { get; set; }

        public override string ToString() {
            StringBuilder ret = new StringBuilder($"FromId={FromId,-5} ToId={ToId,-5}");
            return ret.ToString();
        }
    }
}