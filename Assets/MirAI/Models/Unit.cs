using Assets.MirAI.Simulation;

namespace Assets.MirAI.Models {
    public class Unit : IHaveId {

        public int Id { get; set; }
        public int ProgramId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public int Team { get; set; }
        public UnitType Type { get; set; }
        public int Hp { get; set; }

        public static float MaxHp = 100;
        public static float Range = 3;
        public UnitController Controller { get; set; }
    }

    public enum UnitType {
        Nope = 0,
        Warrior = 1,
        Healer = 2,
        Wizard = 3
    }
}