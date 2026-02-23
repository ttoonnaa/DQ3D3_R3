using UnityEngine;

namespace SrLib
{
    public static class SrLogCore
    {
        public static void Log(string message)
        {
            Debug.Log(message);
        }
        public static void Important(string message)
        {
            Debug.Log($"<color=cyan>{message}</color>");
        }
        public static void Warning(string message)
        {
            Debug.Log($"<color=yellow>{message}</color>");
        }
        public static void Error(string message)
        {
            Debug.LogError($"<color=red>{message}</color>");
        }
    }
}
