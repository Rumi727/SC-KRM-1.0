using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM
{
    public class RemovableObject
    {
        public bool isRemoved { get; private set; } = false;

        public static bool operator ==(RemovableObject lhs, object rhs) => lhs.Equals(rhs);
        public static bool operator !=(RemovableObject lhs, object rhs) => !lhs.Equals(rhs);

        public override bool Equals(object other)
        {
            if (other == null)
                return isRemoved || base.Equals(other);
            else
                return base.Equals(other);
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
