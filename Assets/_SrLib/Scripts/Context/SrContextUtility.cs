using UnityEngine;

namespace SrLib
{
    public static class SrContextUtility
    {
        /*
        /// <summary>
        /// プレハブからインスタンスを生成する
        /// </summary>
        public static GameObject Instantiate(SrContext context, GameObject prefab, GameObject parent)
        {
            if (!prefab)
                return null;

            var gameObject = Object.Instantiate(prefab, parent?.transform, false);

            Setup(context, gameObject);

            return gameObject;
        }
        
        /// <summary>
        /// 作成されたインスタンスをセットアップする
        /// </summary>
        public static void Setup(SrContext context, GameObject gameObject)
        {
            foreach (var component in gameObject.GetComponentsInChildren<SrComponentBase>(true))
            {
                component.Setup(context);
            }
            foreach (var component in gameObject.GetComponentsInChildren<ISrComponent>(true))
            {
                component.Setup(context);
            }
        }
        */
    }
}
