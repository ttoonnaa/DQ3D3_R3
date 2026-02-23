using UnityEngine;

namespace SrLib
{
    public static class TransformExtension
    {
        public static Transform SetPositionX(this Transform transform, float x)
        {
            var position = transform.position;
            position.x = x;
            transform.position = position;
            return transform;
        }
        
        public static Transform SetPositionY(this Transform transform, float y)
        {
            var position = transform.position;
            position.y = y;
            transform.position = position;
            return transform;
        }
        
        public static Transform SetPositionZ(this Transform transform, float z)
        {
            var position = transform.position;
            position.z = z;
            transform.position = position;
            return transform;
        }

        public static Transform SetLocalPositionX(this Transform transform, float x)
        {
            var position = transform.localPosition;
            position.x = x;
            transform.localPosition = position;
            return transform;
        }
        
        public static Transform SetLocalPositionY(this Transform transform, float y)
        {
            var position = transform.localPosition;
            position.y = y;
            transform.localPosition = position;
            return transform;
        }
        
        public static Transform SetLocalPositionZ(this Transform transform, float z)
        {
            var position = transform.localPosition;
            position.z = z;
            transform.localPosition = position;
            return transform;
        }
        
        public static Transform SetLocalScaleX(this Transform transform, float x)
        {
            var scale = transform.localScale;
            scale.x = x;
            transform.localScale = scale;
            return transform;
        }
        
        public static Transform SetLocalScaleY(this Transform transform, float y)
        {
            var scale = transform.localScale;
            scale.y = y;
            transform.localScale = scale;
            return transform;
        }
        
        public static Transform SetLocalScaleZ(this Transform transform, float z)
        {
            var scale = transform.localScale;
            scale.z = z;
            transform.localScale = scale;
            return transform;
        }
        
        public static RectTransform SetAnchoredPositionX(this RectTransform rectTransform, float x)
        {
            var anchoredPosition = rectTransform.anchoredPosition;
            anchoredPosition.x = x;
            rectTransform.anchoredPosition = anchoredPosition;
            return rectTransform;
        }

        public static RectTransform SetAnchoredPositionY(this RectTransform rectTransform, float y)
        {
            var anchoredPosition = rectTransform.anchoredPosition;
            anchoredPosition.y = y;
            rectTransform.anchoredPosition = anchoredPosition;
            return rectTransform;
        }
        
        public static RectTransform SetOffsetMinX(this RectTransform rectTransform, float x)
        {
            var offsetMin = rectTransform.offsetMin;
            offsetMin.x = x;
            rectTransform.offsetMin = offsetMin;
            return rectTransform;
        }

        public static RectTransform SetOffsetMinY(this RectTransform rectTransform, float y)
        {
            var offsetMin = rectTransform.offsetMin;
            offsetMin.y = y;
            rectTransform.offsetMin = offsetMin;
            return rectTransform;
        }

        public static RectTransform SetOffsetMaxX(this RectTransform rectTransform, float x)
        {
            var offsetMax = rectTransform.offsetMax;
            offsetMax.x = x;
            rectTransform.offsetMax = offsetMax;
            return rectTransform;
        }

        public static RectTransform SetOffsetMaxY(this RectTransform rectTransform, float y)
        {
            var offsetMax = rectTransform.offsetMax;
            offsetMax.y = y;
            rectTransform.offsetMax = offsetMax;
            return rectTransform;
        }

        public static RectTransform SetWidth(this RectTransform rectTransform, float width)
        {
            var sizeDelta = rectTransform.sizeDelta;
            sizeDelta.x = width;
            rectTransform.sizeDelta = sizeDelta;
            return rectTransform;
        }

        public static RectTransform SetHeight(this RectTransform rectTransform, float height)
        {
            var sizeDelta = rectTransform.sizeDelta;
            sizeDelta.y = height;
            rectTransform.sizeDelta = sizeDelta;
            return rectTransform;
        }
        
#if UNITY_EDITOR
        
        /// <summary>
        /// インスペクターのパスを取得する
        /// </summary>
        public static string GetInspectorPath(this Transform transform, string separator = "/")
        {
            var path = transform.name;
            var parent = transform.parent;
            while (parent)
            {
                path = $"{parent.name}{separator}{path}";
                parent = parent.parent;
            }

            return path;
        }
#endif
    }
}