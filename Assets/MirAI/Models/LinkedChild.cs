using System;

namespace Assets.MirAI.Models {

    public class LinkedChild : IComparable<LinkedChild> {

        public Link Link { get; set; }
        public Node Node { get; set; }

        public int CompareTo(LinkedChild other) {
            if (other == null) return 1;
            var retval = this.Link.Angle.CompareTo(other.Link.Angle);
            if (retval != 0) return retval;
            return this.Link.Lenght.CompareTo(other.Link.Lenght);
        }
    }
}