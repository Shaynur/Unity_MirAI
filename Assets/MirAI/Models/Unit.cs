using Assets.MirAI.Simulation;

namespace Assets.MirAI.Models {
    public class Unit : IHaveId {

        public int Id { get; set; }
        public int ProgramId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public UnitController Controller { get; set; }
    }
}