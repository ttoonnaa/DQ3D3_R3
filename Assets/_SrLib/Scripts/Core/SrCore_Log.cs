using UnityEngine;

namespace SrLib
{
    public static class SrCore_Log
    {
        public static void Log(string message)
        {
            Debug.Log(message);
        }
        public static void Important(string message)
        {
            Debug.Log($"<color=yellow>{message}</color>");
        }
        public static void Error(string message)
        {
            Debug.LogError($"<color=red>{message}</color>");
        }
    }
}
