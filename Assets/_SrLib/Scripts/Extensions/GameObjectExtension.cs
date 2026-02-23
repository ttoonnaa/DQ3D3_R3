using System.Collections.Generic;
using UnityEngine;

namespace SrLib
{
    public static class GameObjectExtension
    {
        /// <summary>
        /// GameObject の子供を検索する
        /// </summary>
        public static GameObject FindChild(this GameObject gameObject, string name)
        {
            if (!gameObject)
                return null;

            var transform = gameObject.transform.Find(name);
            if (!transform)
                return null;

            return transform.gameObject;
        }
        
        /// <summary>
        /// GameObject の子供を再帰的に検索する
        /// </summary>
        public static GameObject FindChildRecursively(this GameObject gameObject, string name)
        {
            if (!gameObject)
                return null;
            
            foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.name == name)
                    return child.gameObject;
                
                var result = FindChildRecursively(child.gameObject, name);
                if (result)
                    return result;
            }

            return null;
        }
        
        /// <summary>
        /// GameObject の子供たちを再帰的に検索する
        /// </summary>
        public static List<GameObject> FindChildrenRecursively(this GameObject gameObject, string name)
        {
            if (!gameObject)
                return null;

            var list = new List<GameObject>();
            _FindChildrenRecursively(gameObject, name, list);
            return list;
        }
        
        /// <summary>
        /// GameObject の子供たちを再帰的に検索する
        /// </summary>
        private static void _FindChildrenRecursively(GameObject gameObject, string name, List<GameObject> list)
        {
            foreach (Transform childTransform in gameObject.transform)
            {
                if (childTransform.gameObject.name == name)
                    list.Add(childTransform.gameObject);
                
                _FindChildrenRecursively(childTransform.gameObject, name, list);
            }
        }
        
        /// <summary>
        /// 条件にあう子供を再帰的に検索する
        /// </summary>
        public static GameObject FindChildRecursively(this GameObject parent, System.Predicate<GameObject> match)
        {
            if (!parent)
                return null;
            
            foreach (Transform child in parent.transform)
            {
                if (match(child.gameObject))
                    return child.gameObject;

                var found = _FindChildRecursively(child, match);
                if (found)
                    return found;
            }
            return null;
        }

        /// <summary>
        /// 条件にあう子供を再帰的に検索する
        /// </summary>
        private static GameObject _FindChildRecursively(Transform parent, System.Predicate<GameObject> match)
        {
            foreach (Transform child in parent)
            {
                if (match(child.gameObject))
                    return child.gameObject;

                var found = _FindChildRecursively(child, match);
                if (found)
                    return found;
            }
            
            return null;
        }
        
        /// <summary>
        /// コンポーネントを追加または取得する
        /// </summary>
        public static T SafeAddComponent<T>(this GameObject self) where T : Component
        {
            if (!self)
                return null;
            
            return self.GetComponent<T>() ?? self.AddComponent<T>();
        }
        
#if UNITY_EDITOR
        
        /// <summary>
        /// インスペクターのパスを取得する
        /// </summary>
        public static string GetInspectorPath(this GameObject gameObject, string separator = "/")
        {
            return gameObject.transform.GetInspectorPath(separator);
        }
#endif        
    }
}
