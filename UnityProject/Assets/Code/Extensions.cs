using UnityEngine;

namespace Code {
    public static class Extensions {

        public static Vector3 SetY(this Vector3 vector, float y) {
            var copy = vector;
            copy.y = y;
            return copy;
        }
    }
}