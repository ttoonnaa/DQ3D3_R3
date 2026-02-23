using UnityEditor;

namespace SrLib.Editor
{
    public static class SrAssetReImporter
    {
        [MenuItem("SrLib/Reimport All Assets (Update Meta File)")]
        public static void ReimportAll()
        {
            AssetDatabase.ForceReserializeAssets();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}