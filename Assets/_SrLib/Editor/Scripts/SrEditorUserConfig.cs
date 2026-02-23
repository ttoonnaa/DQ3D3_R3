#if UNITY_EDITOR
using UnityEditor;

namespace SrLib.Editor
{
    /// <summary>
    /// ユーザーごとのエディター設定
    /// </summary>
    public static class SrEditorUserConfig
    {
        /// <summary>
        /// string 値を取得
        /// </summary>
        public static string GetString(string key, string defaultValue)
        {
            var strValue = EditorUserSettings.GetConfigValue(key);
            if (string.IsNullOrEmpty(strValue))
                return defaultValue;

            return strValue;
        }

        /// <summary>
        /// string 値を保存
        /// </summary>
        public static void SetString(string key, string value)
        {
            EditorUserSettings.SetConfigValue(key, value);
        }

        /// <summary>
        /// int 値を取得
        /// </summary>
        public static int GetInt(string key, int defaultValue)
        {
            var strValue = EditorUserSettings.GetConfigValue(key);
            if (string.IsNullOrEmpty(strValue))
                return defaultValue;

            if (!int.TryParse(strValue, out var intValue))
                return defaultValue;

            return intValue;
        }

        /// <summary>
        /// int 値を保存
        /// </summary>
        public static void SetInt(string key, int value)
        {
            EditorUserSettings.SetConfigValue(key, value.ToString());
        }

        /// <summary>
        /// bool 値を取得
        /// </summary>
        public static bool GetBool(string key, bool defaultValue)
        {
            var strValue = EditorUserSettings.GetConfigValue(key);
            if (string.IsNullOrEmpty(strValue))
                return defaultValue;

            if (!bool.TryParse(strValue, out var boolValue))
                return defaultValue;

            return boolValue;
        }

        /// <summary>
        /// bool 値を保存
        /// </summary>
        public static void SetBool(string key, bool value)
        {
            EditorUserSettings.SetConfigValue(key, value.ToString());
        }
    }
}
#endif
