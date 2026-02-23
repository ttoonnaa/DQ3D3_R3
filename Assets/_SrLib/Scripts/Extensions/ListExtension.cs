using System;
using System.Collections.Generic;
using System.Linq;

namespace SrLib
{
    public static class ListExtension
    {
        /// <summary>
        /// List から安全に RemoveAll する
        /// Remove される前に disposeFunc が呼ばれる
        /// </summary>
        public static void SafeRemoveAll<TValue>(this IList<TValue> list, Func<TValue, bool> condition, Action<TValue> disposeFunc)
        {
            // 削除条件に合う要素をまず抽出
            // この ToList は重要、list から独立させることで foreach を回せるようになる
            var valuesToRemove = list.Where(condition).ToList();

            // dispose を呼んでからリストから削除
            foreach (var value in valuesToRemove)
            {
                disposeFunc?.Invoke(value);
                list.Remove(value);
            }
        }
    }
}