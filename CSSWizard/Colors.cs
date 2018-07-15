using System;
using System.Globalization;
using System.Text;
using System.Windows.Media;

namespace CSSWizard
{
    internal static class Colors
    {
        public struct HsbColor
        {
            // The Hue of the color [0,360]
            public double H;

            // The Saturation of the color [0,1]
            public double S;

            // The Brightness of the color [0,1]
            public double B;

            // The Alpha (opaqueness) of the color [0,1]
            public double A;
        }

        public static Color HexToRgb(this string hexRgbColor)
        {
            var rgbColor = new Color();

            if (hexRgbColor.Length == 4)
            {
                string newStr = "#";
                foreach (var ch in hexRgbColor.Substring(1,3))
                {
                    newStr += ch + ch;
                }

                hexRgbColor = newStr;
            }

            var s = hexRgbColor.Substring(1, 2);
            var i = int.Parse(s, NumberStyles.HexNumber);
            rgbColor.R = Convert.ToByte(i);

            s = hexRgbColor.Substring(3, 2);
            i = int.Parse(s, NumberStyles.HexNumber);
            rgbColor.G = Convert.ToByte(i);

            s = hexRgbColor.Substring(5, 2);
            i = int.Parse(s, NumberStyles.HexNumber);
            rgbColor.B = Convert.ToByte(i);

            if (hexRgbColor.Length > 7)
            {
                s = hexRgbColor.Substring(7, 2);
                i = int.Parse(s, NumberStyles.HexNumber);
                rgbColor.A = Convert.ToByte(i);
            }
            else
            {
                rgbColor.A = Convert.ToByte(255);
            }

            return rgbColor;
        }

        public static Color VerboseRgbaToRgba(this string verboseColor)
        {
            string colStr = verboseColor.Replace("rgba(", "").Replace(")", "").Replace(", ", ",");
            var colArr = colStr.Split(',');
            var colIntArr = new int[4];

            for (int i = 0; i < 3; i++)
            {
                colIntArr[i] = int.Parse(colArr[i]);
            }

            colIntArr[3] = (int) Math.Round(double.Parse(colArr[3], CultureInfo.InvariantCulture) * 255);

            var newcol = new Color
            {
                R = (byte) colIntArr[0],
                G = (byte) colIntArr[1],
                B = (byte) colIntArr[2],
                A = (byte) colIntArr[3]
            };
            return newcol;
        }

        public static string RgbToHex(this Color rgbColor, bool withAlpha = false)
        {
            var sb = new StringBuilder("#");
            sb.Append($"{rgbColor.R:X2}");
            sb.Append($"{rgbColor.G:X2}");
            sb.Append($"{rgbColor.B:X2}");
            if (withAlpha) sb.Append($"{rgbColor.A:X2}");
            return sb.ToString();
        }

        public static string RgbaToString(this Color rgbaColor)
        {
            var sb = new StringBuilder("rgba(");
            sb.Append(rgbaColor.R.ToString(CultureInfo.InvariantCulture) + ", ");
            sb.Append(rgbaColor.G.ToString(CultureInfo.InvariantCulture) + ", ");
            sb.Append(rgbaColor.B.ToString(CultureInfo.InvariantCulture) + ", ");
            sb.Append(((double)rgbaColor.A/255).ToString(CultureInfo.InvariantCulture) + ")");
            return sb.ToString();
        }

        public static string HsbToString(this HsbColor hsbColor)
        {
            var sb = new StringBuilder("hsva(");
            sb.Append(hsbColor.H.ToString(CultureInfo.InvariantCulture) + ", ");
            sb.Append(hsbColor.S.ToString(CultureInfo.InvariantCulture) + ", ");
            sb.Append(hsbColor.B.ToString(CultureInfo.InvariantCulture) + ", ");
            sb.Append(hsbColor.A.ToString(CultureInfo.InvariantCulture) + ")");
            return sb.ToString();
        }

        public static HsbColor RgbToHsb(this Color rgbColor)
        {
            /* Hue values range between 0 and 360. All 
             * other values range between 0 and 1. */

            // Create HSB color object
            var hsbColor = new HsbColor();

            // Get RGB color component values
            var r = Convert.ToInt32(rgbColor.R);
            var g = Convert.ToInt32(rgbColor.G);
            var b = Convert.ToInt32(rgbColor.B);
            var a = Convert.ToInt32(rgbColor.A);

            // Get min, max, and delta values
            double min = Math.Min(Math.Min(r, g), b);
            double max = Math.Max(Math.Max(r, g), b);
            var delta = max - min;

            /* Black (max = 0) is a special case. We 
             * simply set HSB values to zero and exit. */

            // Black: Set HSB and return
            if (max == 0.0)
            {
                hsbColor.H = 0.0;
                hsbColor.S = 0.0;
                hsbColor.B = 0.0;
                hsbColor.A = a;
                return hsbColor;
            }

            /* Now we process the normal case. */

            // Set HSB Alpha value
            var alpha = Convert.ToDouble(a);
            hsbColor.A = alpha / 255;

            // Set HSB Hue value
            if (r == max) hsbColor.H = (g - b) / delta;
            else if (g == max) hsbColor.H = 2 + (b - r) / delta;
            else if (b == max) hsbColor.H = 4 + (r - g) / delta;
            hsbColor.H *= 60;
            if (hsbColor.H < 0.0) hsbColor.H += 360;

            // Set other HSB values
            hsbColor.S = delta / max;
            hsbColor.B = max / 255;

            // Set return value
            return hsbColor;
        }

        public static Color HsbToRgb(this HsbColor hsbColor)
        {
            // Initialize
            var rgbColor = new Color();

            /* Gray (zero saturation) is a special case.We simply
             * set RGB values to HSB Brightness value and exit. */

            // Gray: Set RGB and return
            if (hsbColor.S == 0.0)
            {
                rgbColor.A = Convert.ToByte(hsbColor.A);
                rgbColor.R = Convert.ToByte(hsbColor.B * 255);
                rgbColor.G = Convert.ToByte(hsbColor.B * 255);
                rgbColor.B = Convert.ToByte(hsbColor.B * 255);
                return rgbColor;
            }

            /* Now we process the normal case. */

            var h = hsbColor.H == 360 ? 0 : hsbColor.H / 60;
            var i = Convert.ToInt32(Math.Truncate(h));
            var f = h - i;

            var p = hsbColor.B * (1.0 - hsbColor.S);
            var q = hsbColor.B * (1.0 - hsbColor.S * f);
            var t = hsbColor.B * (1.0 - hsbColor.S * (1.0 - f));

            double r, g, b;
            switch (i)
            {
                case 0:
                    r = hsbColor.B;
                    g = t;
                    b = p;
                    break;

                case 1:
                    r = q;
                    g = hsbColor.B;
                    b = p;
                    break;

                case 2:
                    r = p;
                    g = hsbColor.B;
                    b = t;
                    break;

                case 3:
                    r = p;
                    g = q;
                    b = hsbColor.B;
                    break;

                case 4:
                    r = t;
                    g = p;
                    b = hsbColor.B;
                    break;

                default:
                    r = hsbColor.B;
                    g = p;
                    b = q;
                    break;
            }

            // Set WPF Color object
            rgbColor.A = Convert.ToByte(hsbColor.A * 255);
            rgbColor.R = Convert.ToByte(r * 255);
            rgbColor.G = Convert.ToByte(g * 255);
            rgbColor.B = Convert.ToByte(b * 255);

            // Set return value
            return rgbColor;
        }

        public static HsbColor InvertValue(this HsbColor col)
        {
            return new HsbColor
            {
                H = col.H,
                S = col.S,
                B = 1 - col.B,
                A = col.A
            };
        }

        public static string InvertRgbValueString(this string str)
        {
            var rgbIn = str.HexToRgb();
            var hsbIn = rgbIn.RgbToHsb();
            var hsbOut = hsbIn.InvertValue();
            var rgbOut = hsbOut.HsbToRgb();
            var strOut = rgbOut.RgbToHex();

            return strOut;
        }

        public static string InvertRgbaValueVerbose(this string str)
        {
            var rgbaIn = str.VerboseRgbaToRgba();
            var hsbIn = rgbaIn.RgbToHsb();
            var hsbOut = hsbIn.InvertValue();
            var rgbOut = hsbOut.HsbToRgb();
            var strOut = rgbOut.RgbaToString();

            return strOut;
        }
    }
}