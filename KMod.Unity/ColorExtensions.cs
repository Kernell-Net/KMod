using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KMod.Unity
{
    public static class ColorExtensions
    {
        public static string ToHex(this Color color)
        {
            return ColorUtility.ToHtmlStringRGB(color);
        }
        
        public static string ToHexWithAlpha(this Color color)
        {
            return ColorUtility.ToHtmlStringRGBA(color);
        }
        
        public static Color FromHex(string hex)
        {
            Color color;
            if (hex.StartsWith("#"))
                hex = hex.Substring(1);
                
            if (ColorUtility.TryParseHtmlString("#" + hex, out color))
                return color;
                
            return Color.white;
        }
        public static Vector3 ToHSV(this Color color)
        {
            float h, s, v;
            Color.RGBToHSV(color, out h, out s, out v);
            return new Vector3(h, s, v);
        }
        
        public static Color FromHSV(float h, float s, float v, float alpha = 1.0f)
        {
            Color color = Color.HSVToRGB(h, s, v);
            color.a = alpha;
            return color;
        }
        
        public static Vector3 ToHSL(this Color color)
        {
            float r = color.r;
            float g = color.g;
            float b = color.b;
            
            float max = Mathf.Max(r, Mathf.Max(g, b));
            float min = Mathf.Min(r, Mathf.Min(g, b));
            
            float h, s, l;
            l = (max + min) / 2f;
            
            if (max == min)
            {
                h = s = 0f;
            }
            else
            {
                float d = max - min;
                s = l > 0.5f ? d / (2f - max - min) : d / (max + min);
                
                if (max == r)
                    h = (g - b) / d + (g < b ? 6f : 0f);
                else if (max == g)
                    h = (b - r) / d + 2f;
                else
                    h = (r - g) / d + 4f;
                    
                h /= 6f;
            }
            
            return new Vector3(h, s, l);
        }
        
        public static Color FromHSL(float h, float s, float l, float alpha = 1.0f)
        {
            float r, g, b;
            
            if (s == 0f)
            {
                r = g = b = l;
            }
            else
            {
                float q = l < 0.5f ? l * (1f + s) : l + s - l * s;
                float p = 2f * l - q;
                r = HueToRGB(p, q, h + 1f/3f);
                g = HueToRGB(p, q, h);
                b = HueToRGB(p, q, h - 1f/3f);
            }
            
            return new Color(r, g, b, alpha);
        }
        
        private static float HueToRGB(float p, float q, float t)
        {
            if (t < 0f) t += 1f;
            if (t > 1f) t -= 1f;
            if (t < 1f/6f) return p + (q - p) * 6f * t;
            if (t < 1f/2f) return q;
            if (t < 2f/3f) return p + (q - p) * (2f/3f - t) * 6f;
            return p;
        }
        
        // Color manipulation
        public static Color WithAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }
        
        public static Color WithRed(this Color color, float red)
        {
            return new Color(red, color.g, color.b, color.a);
        }
        
        public static Color WithGreen(this Color color, float green)
        {
            return new Color(color.r, green, color.b, color.a);
        }
        
        public static Color WithBlue(this Color color, float blue)
        {
            return new Color(color.r, color.g, blue, color.a);
        }
        
        public static Color Lighten(this Color color, float amount)
        {
            Color.RGBToHSV(color, out float h, out float s, out float v);
            v = Mathf.Clamp01(v + amount);
            return Color.HSVToRGB(h, s, v).WithAlpha(color.a);
        }
        
        public static Color Darken(this Color color, float amount)
        {
            return color.Lighten(-amount);
        }
        
        public static Color Saturate(this Color color, float amount)
        {
            Color.RGBToHSV(color, out float h, out float s, out float v);
            s = Mathf.Clamp01(s + amount);
            return Color.HSVToRGB(h, s, v).WithAlpha(color.a);
        }
        
        public static Color Desaturate(this Color color, float amount)
        {
            return color.Saturate(-amount);
        }
        
        public static Color ShiftHue(this Color color, float amount)
        {
            Color.RGBToHSV(color, out float h, out float s, out float v);
            h = (h + amount) % 1.0f;
            if (h < 0) h += 1.0f;
            return Color.HSVToRGB(h, s, v).WithAlpha(color.a);
        }
        
        public static Color Invert(this Color color)
        {
            return new Color(1f - color.r, 1f - color.g, 1f - color.b, color.a);
        }
        
        public static Color Complement(this Color color)
        {
            return color.ShiftHue(0.5f);
        }
        
        public static Color Blend(this Color color, Color other, float amount)
        {
            return Color.Lerp(color, other, amount);
        }
        
        public static Color Multiply(this Color color, Color other)
        {
            return new Color(
                color.r * other.r,
                color.g * other.g,
                color.b * other.b,
                color.a * other.a
            );
        }
        
        public static Color Screen(this Color color, Color other)
        {
            return new Color(
                1f - (1f - color.r) * (1f - other.r),
                1f - (1f - color.g) * (1f - other.g),
                1f - (1f - color.b) * (1f - other.b),
                color.a
            );
        }
        
        public static Color Overlay(this Color color, Color other)
        {
            return new Color(
                (color.r <= 0.5f) ? (2f * color.r * other.r) : (1f - 2f * (1f - color.r) * (1f - other.r)),
                (color.g <= 0.5f) ? (2f * color.g * other.g) : (1f - 2f * (1f - color.g) * (1f - other.g)),
                (color.b <= 0.5f) ? (2f * color.b * other.b) : (1f - 2f * (1f - color.b) * (1f - other.b)),
                color.a
            );
        }
        
        public static Color[] Monochromatic(this Color color, int count = 5)
        {
            Vector3 hsv = color.ToHSV();
            Color[] result = new Color[count];
            
            for (int i = 0; i < count; i++)
            {
                float v = (float)i / (count - 1);
                float s = hsv.y * (0.5f + 0.5f * v);
                result[i] = FromHSV(hsv.x, s, 0.3f + 0.7f * v, color.a);
            }
            
            return result;
        }
        
        public static Color[] Analogous(this Color color, int count = 3, float angle = 0.1f)
        {
            Vector3 hsv = color.ToHSV();
            Color[] result = new Color[count];
            
            float startHue = hsv.x - angle * (count - 1) / 2f;
            
            for (int i = 0; i < count; i++)
            {
                float h = startHue + angle * i;
                if (h < 0) h += 1f;
                if (h > 1) h -= 1f;
                result[i] = FromHSV(h, hsv.y, hsv.z, color.a);
            }
            
            return result;
        }
        
        public static Color[] Triadic(this Color color)
        {
            Vector3 hsv = color.ToHSV();
            
            return new Color[] {
                color,
                FromHSV((hsv.x + 1f/3f) % 1f, hsv.y, hsv.z, color.a),
                FromHSV((hsv.x + 2f/3f) % 1f, hsv.y, hsv.z, color.a)
            };
        }
        
        public static Color[] Tetradic(this Color color)
        {
            Vector3 hsv = color.ToHSV();
            
            return new Color[] {
                color,
                FromHSV((hsv.x + 0.25f) % 1f, hsv.y, hsv.z, color.a),
                FromHSV((hsv.x + 0.5f) % 1f, hsv.y, hsv.z, color.a),
                FromHSV((hsv.x + 0.75f) % 1f, hsv.y, hsv.z, color.a)
            };
        }
        
        public static float Luminance(this Color color)
        {
            return 0.2126f * color.r + 0.7152f * color.g + 0.0722f * color.b;
        }
        
        public static bool IsDark(this Color color)
        {
            return color.Luminance() < 0.5f;
        }
        
        public static bool IsLight(this Color color)
        {
            return !color.IsDark();
        }
        
        public static Color ContrastColor(this Color color)
        {
            return color.IsDark() ? Color.white : Color.black;
        }
        
        public static float Distance(this Color color, Color other)
        {
            return Mathf.Sqrt(
                Mathf.Pow(color.r - other.r, 2) +
                Mathf.Pow(color.g - other.g, 2) +
                Mathf.Pow(color.b - other.b, 2)
            );
        }
        
        public static int ToRGBInt(this Color color)
        {
            int r = Mathf.RoundToInt(color.r * 255);
            int g = Mathf.RoundToInt(color.g * 255);
            int b = Mathf.RoundToInt(color.b * 255);
            return (r << 16) | (g << 8) | b;
        }
        
        public static Color FromRGBInt(int rgb)
        {
            float r = ((rgb >> 16) & 0xFF) / 255f;
            float g = ((rgb >> 8) & 0xFF) / 255f;
            float b = (rgb & 0xFF) / 255f;
            return new Color(r, g, b, 1f);
        }
        
        public static string ToCssRgb(this Color color)
        {
            return $"rgb({Mathf.RoundToInt(color.r * 255)}, {Mathf.RoundToInt(color.g * 255)}, {Mathf.RoundToInt(color.b * 255)})";
        }
        
        public static string ToCssRgba(this Color color)
        {
            return $"rgba({Mathf.RoundToInt(color.r * 255)}, {Mathf.RoundToInt(color.g * 255)}, {Mathf.RoundToInt(color.b * 255)}, {color.a:F2})";
        }
        
        public static bool HasGoodContrast(this Color foreground, Color background, float minRatio = 4.5f)
        {
            float l1 = foreground.Luminance();
            float l2 = background.Luminance();
            
            float ratio = (Mathf.Max(l1, l2) + 0.05f) / (Mathf.Min(l1, l2) + 0.05f);
            return ratio >= minRatio;
        }
        
        public static Color[] Gradient(Color start, Color end, int steps)
        {
            Color[] colors = new Color[steps];
            for (int i = 0; i < steps; i++)
            {
                float t = (float)i / (steps - 1);
                colors[i] = Color.Lerp(start, end, t);
            }
            return colors;
        }
    }
}