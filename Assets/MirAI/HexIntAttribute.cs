using System;
using UnityEngine;

namespace Assets.MirAI {
    // Create the "HexInt" Attribute that can be applied to a property.
    // Optionally, you can specify the number of digits to "zero pad" it.
    public sealed class HexIntAttribute : PropertyAttribute {

        public int digits;
        public string FormatString {
            get {
                if (digits == 0) {
                    return "X";
                }
                else {
                    return string.Format("X{0}", digits);
                }
            }
        }

        public HexIntAttribute(int digits) {
            if (digits < 0)
                throw new ArgumentOutOfRangeException("Digits cannot be negative");
            this.digits = digits;
        }

        public HexIntAttribute() : this(0) {
        }
    }
}