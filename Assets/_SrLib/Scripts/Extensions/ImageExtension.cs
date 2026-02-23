using UnityEngine.UI;

namespace SrLib
{
    public static class ImageExtension
    {
        public static Graphic SetColorR(this Graphic image, float r)
        {
            var color = image.color;
            color.r = r;
            image.color = color;
            return image;
        }
        public static Graphic SetColorG(this Graphic image, float g)
        {
            var color = image.color;
            color.g = g;
            image.color = color;
            return image;
        }
        public static Graphic SetColorB(this Graphic image, float b)
        {
            var color = image.color;
            color.b = b;
            image.color = color;
            return image;
        }
        public static Graphic SetColorA(this Graphic image, float a)
        {
            var color = image.color;
            color.a = a;
            image.color = color;
            return image;
        }
    }
}