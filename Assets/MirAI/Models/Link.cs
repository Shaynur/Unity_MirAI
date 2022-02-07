using System.Text;
using Assets.MirAI.DB;

namespace Assets.MirAI.Models {

    public class Link : IAiModelElement {
        public int Id { get; set; }
        public int FromId { get; set; }
        public int ToId { get; set; }
        public IDbRoutines dbRoutines { get; set; }

        public Link() {
            dbRoutines = new DbLink(this);
        }

        public override string ToString() {
            StringBuilder ret = new StringBuilder($"FromId={FromId,-5} ToId={ToId,-5}");
            return ret.ToString();
        }
    }
}