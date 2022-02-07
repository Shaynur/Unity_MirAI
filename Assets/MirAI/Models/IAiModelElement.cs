using Assets.MirAI.DB;

namespace Assets.MirAI.Models {
    public interface IAiModelElement {
        public int Id { get; set; }
        public IDbRoutines dbRoutines { get; set; }
    }
}