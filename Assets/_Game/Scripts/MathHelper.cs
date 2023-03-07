using UnityEngine;

namespace _Game.Scripts
{
    public static class MathHelper
    {
        public static Vector3Int RotateVector3Int(Vector3Int vector, Vector3 euler)
        {
            var rotatedVector = Quaternion.Euler(euler) * vector;
            return Vector3ToVector3Int(rotatedVector);
        }

        public static Vector3Int Vector3ToVector3Int(Vector3 vector)
        {
            return new Vector3Int((int)vector.x, (int)vector.y, (int)vector.z);
        }
    }
}