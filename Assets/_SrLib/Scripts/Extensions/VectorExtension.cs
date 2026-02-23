using UnityEngine;

namespace SrLib
{
    public static class VectorExtension
    {
        public static Vector3 ToVector3(this Vector2 vector2, float z)
        {
            return new Vector3(vector2.x, vector2.y, z);
        }    
    }
}