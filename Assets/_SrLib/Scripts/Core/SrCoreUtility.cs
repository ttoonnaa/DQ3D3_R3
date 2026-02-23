using System;
using System.Collections.Generic;
using UnityEngine;

namespace SrLib
{
    public static class SrCoreUtility
    {
        /// <summary>
        /// 文字列から Enum に変換する（デフォルト値を指定できる）
        /// </summary>
        public static T StringToEnum<T>(string value, T defaultValue)
        {
            if (Enum.TryParse(typeof(T), value, out var result))
                return (T)result;

            return defaultValue;
        }

        /// <summary>
        /// 整数から Enum に変換する（デフォルト値を指定できる）
        /// </summary>
        public static T IntegerToEnum<T>(int value, T defaultValue) where T : Enum
        {
            if (!Enum.IsDefined(typeof(T), value))
                return defaultValue;

            return (T)Enum.ToObject(typeof(T), value);
        }

        /// <summary>
        /// 文字列として保存されている整数（"1" など）から Enum に変換する（デフォルト値を指定できる）
        /// 文字列しか保存できない EditorUserSettings などで使う
        /// </summary>
        public static T IntegerToEnum<T>(string value, T defaultValue) where T : Enum
        {
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            if (!int.TryParse(value, out var intValue))
                return defaultValue;
            if (!Enum.IsDefined(typeof(T), intValue))
                return defaultValue;

            return (T)Enum.ToObject(typeof(T), intValue);
        }
        
        /// <summary>
        /// 要素を作成済みのリストを作成する
        /// </summary>
        public static List<T> CreateObjectList<T>(int size) where T : new()
        {
            // コンストラクタの引数は capacity なので領域のみ確保される
            var list = new List<T>(size);
            for (var i = 0; i < size; i++)
            {
                list.Add(new T());
            }

            return list;
        }
        
        /// <summary>
        /// int から Color を作成
        /// </summary>
        public static Color ToColor(int r, int g, int b, int a = 255)
        {
            return new Color32((byte)r, (byte)g, (byte)b, (byte)a);
        }
        
        /// <summary>
        /// 白のスプライトを作成
        /// </summary>
        public static Sprite CreateWhiteSprite(Vector2? pivot = null)
        {
            pivot ??= new Vector2(0, 1);
            var texture = Texture2D.whiteTexture;
            var rect = new Rect(0, 0, texture.width, texture.height);
            var border = Vector4.zero;
            return Sprite.Create(texture, rect, pivot.Value, 100f, 1, SpriteMeshType.FullRect, border);
        }        

        /// <summary>
        /// List を複製する（浅いコピー）
        /// </summary>
        public static List<T> CloneList<T>(List<T> src)
        {
            if (src == null)
                return null;

            var dst = new List<T>(src.Count);
            foreach (var srcValue in src)
            {
                dst.Add(srcValue);
            }
            
            return dst;
        }
        
        /// <summary>
        /// Dictionary を複製する（浅いコピー）
        /// </summary>
        public static Dictionary<T1, T2> CloneDictionary<T1, T2>(Dictionary<T1, T2> src)
        {
            if (src == null)
                return null;
            
            var dst = new Dictionary<T1, T2>();
            foreach (var (key, srcValue) in src)
            {
                dst.Add(key, srcValue);
            }

            return dst;
        }

        /// <summary>
        /// Unix 時刻を求める（ミリ秒）
        /// </summary>
        public static long GetUnixTimeMills()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}



