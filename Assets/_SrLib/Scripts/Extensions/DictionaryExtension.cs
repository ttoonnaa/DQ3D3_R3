using System;
using System.Collections.Generic;
using System.Linq;

namespace SrLib
{
    public static class DictionaryExtension
    {
        /// <summary>
        /// Dictionary から安全に Get する
        /// 要素が無い場合は defalut が生成される
        /// </summary>
        public static TValue SafeGetDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary.TryGetValue(key, out var value))
                return value;
            
            value = default;
            dictionary[key] = value;

            return value;
        }

        /// <summary>
        /// Dictionary から安全に Get する
        /// 要素が無い場合は new で生成して Add される
        /// </summary>
        public static TValue SafeGet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
        {
            if (dictionary.TryGetValue(key, out var value))
                return value;
            
            value = new TValue();
            dictionary[key] = value;

            return value;
        }

        /// <summary>
        /// Dictionary から安全に Get する
        /// 要素が無い場合は valueFactory で生成して Add される
        /// </summary>
        public static TValue SafeGet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueFactory)
        {
            if (dictionary.TryGetValue(key, out var value))
                return value;

            value = valueFactory();
            dictionary[key] = value;

            return value;
        }
        
        /// <summary>
        /// Dictionary から安全に Remove する
        /// Remove される前に disposeFunc が呼ばれる
        /// </summary>
        public static void SafeRemove<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Action<TValue> disposeFunc)
        {
            if (!dictionary.TryGetValue(key, out var value))
                return;

            disposeFunc?.Invoke(value);
            dictionary.Remove(key);
        }
        
        /// <summary>
        /// Dictionary から安全に RemoveAll する
        /// Remove される前に disposeFunc が呼ばれる
        /// </summary>
        public static void SafeRemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Func<TValue, bool> condition, Action<TValue> disposeFunc)
        {
            var keysToRemove = dictionary
                .Where(pair => condition(pair.Value))
                .Select(pair => pair.Key)
                .ToList();      // この ToList は重要、dictionary から独立させることで foreach を回せるようになる

            // 抽出したキーをすべて削除
            foreach (var key in keysToRemove)
            {
                disposeFunc?.Invoke(dictionary[key]);
                dictionary.Remove(key);
            }
        }
    }
}

