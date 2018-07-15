using System;
using System.Collections.Generic;
using System.Globalization;

namespace CSSWizard
{
    static class Color
    {
        // Convert string to RGB color
        public static RgbColor StringToRgbColor(this string str)
        {
            int chunkLength = 2;
            List<string> result = new List<string>();

            if (String.IsNullOrEmpty(str)) throw new ArgumentException();
            if (chunkLength < 1) throw new ArgumentException();

            for (int i = 0; i < str.Length; i += chunkLength)
            {
                if (chunkLength + i > str.Length)
                    chunkLength = str.Length - i;

                result.Add(str.Substring(i, chunkLength));
            }

            return new RgbColor(
                int.Parse(result[0], NumberStyles.HexNumber), 
                int.Parse(result[1], NumberStyles.HexNumber),
                int.Parse(result[2], NumberStyles.HexNumber)
            );
        }

        // Convert string to RGBA color
        public static RgbaColor StringToRgbaColor(this string str)
        {
            int chunkLength = 2;
            List<string> result = new List<string>();

            if (String.IsNullOrEmpty(str)) throw new ArgumentException();
            if (chunkLength < 1) throw new ArgumentException();

            for (int i = 0; i < str.Length; i += chunkLength)
            {
                if (chunkLength + i > str.Length)
                    chunkLength = str.Length - i;

                result.Add(str.Substring(i, chunkLength));
            }

            return new RgbaColor(
                int.Parse(result[0], NumberStyles.HexNumber), 
                int.Parse(result[1], NumberStyles.HexNumber),
                int.Parse(result[2], NumberStyles.HexNumber),
                int.Parse(result[3], NumberStyles.HexNumber) / 255.0
            );
        }

    }

    /***********************************************************\
    | Class representing the RGB colour
    \***********************************************************/
    class RgbColor
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        public RgbColor(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        // RGB to RGBA conversion
        public RgbaColor RgbToRgba()
        {
            return new RgbaColor(Red, Green, Blue, 0.0);
        }

        // RGB to HSV conversion
        public HsvColor RgbToHsv()
        {
            double red = Red / 255.0;
            double green = Green / 255.0;
            double blue = Blue / 255.0;

            double hue = 0, saturation, value;

            double cMax = Math.Max(Math.Max(red, green), blue);
            double cMin = Math.Min(Math.Min(red, green), blue);
            double delta = cMax - cMin;

            // Calculate Hue
            if (Math.Abs(delta) < 0.0001)
                hue = 0;
            else if (Math.Abs(cMax - red) < 0.0001)
                hue = (60 * ((green - blue) / delta));
            else if (Math.Abs(cMax - green) < 0.0001)
                hue = (60 * (2.0 + (blue - red) / delta));
            else if (Math.Abs(cMax - blue) < 0.0001)
                hue = (60 * (4.0 + (red - green) / delta));

            // Check if Hue isn't negative
            if (Math.Sign(hue) == -1) hue += 360;

            // Calculate saturation
            if (Math.Abs(cMax) < 0.0001)
                saturation = 0;
            else
                saturation = (delta / cMax);

            // Calculate value
            value = (int) cMax;

            // Return
            return new HsvColor((int) hue, (int) (saturation*100), (int) (value/100));
        }

        // ToString override
        public override string ToString()
        {
            return "#" + $"{Red:X2}" + $"{Green:X2}" + $"{Blue:X2}";
        }
    }

    /***********************************************************\
    | Class representing the RGBA colour
    \***********************************************************/
    class RgbaColor
    {
        public double Red { get; set; }
        public double Green { get; set; }
        public double Blue { get; set; }
        public double Alpha { get; set; }

        public RgbaColor(double red, double green, double blue, double alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        // RGBA to RGB conversion
        public RgbColor RgbaToRgb()
        {
            return new RgbColor((int) Red, (int) Green, (int) Blue);
        }

        // RGBA to HSV conversion
        public HsvColor RgbaToHsv()
        {
            double red = Red / 255.0;
            double green = Green / 255.0;
            double blue = Blue / 255.0;

            double hue = 0, saturation, value;

            double cMax = Math.Max(Math.Max(red, green), blue);
            double cMin = Math.Min(Math.Min(red, green), blue);
            double delta = cMax - cMin;

            // Calculate Hue
            if (Math.Abs(delta) < 0.0001)
                hue = 0;
            else if (Math.Abs(cMax - red) < 0.0001)
                hue = (60 * ((green - blue) / delta));
            else if (Math.Abs(cMax - green) < 0.0001)
                hue = (60 * (2.0 + (blue - red) / delta));
            else if (Math.Abs(cMax - blue) < 0.0001)
                hue = (60 * (4.0 + (red - green) / delta));

            // Check if Hue isn't negative
            if (Math.Sign(hue) == -1) hue += 360;

            // Calculate saturation
            if (Math.Abs(cMax) < 0.0001)
                saturation = 0;
            else
                saturation = (delta / cMax);

            // Calculate value
            value = (int) cMax;

            // Return
            return new HsvColor((int) hue, (int) saturation, (int) (value/100));
        }

        // ToString override
        public string ToString(bool asHashCode = true)
        {
            if (asHashCode)
                return "#" + $"{(int) Red:X2}" + $"{(int) Green:X2}" + $"{(int) Blue:X2}" + $"{(int) Blue:X2}";
            return "rgba(" + Red + ", " + Green + ", " + Blue + ", " + Alpha.ToString("0.00", CultureInfo.InvariantCulture) + ")";
        }
    }

    /***********************************************************\
    | Class representing the HSV colour
    \***********************************************************/
    class HsvColor
    {
        public int Hue { get; set; }
        public int Saturation { get; set; }
        public int Value { get; set; }

        public HsvColor(int hue, int saturation, int value)
        {
            Hue = hue;
            Saturation = saturation;
            Value = value;
        }

        // HSV to RGB conversion
        public RgbColor HsvToRgb()
        {
            double red, green, blue;

            if (Saturation == 0)
            {
                red = Value;
                green = Value;
                blue = Value;
            }
            else
            {
                if (Hue == 360)
                    Hue = 0;
                else
                    Hue = Hue / 60;

                double i = Hue;
                double f = Hue - i;
                
                double p = Value * (1.0 - Saturation);
                double q = Value * (1.0 - (Saturation * f));
                double t = Value * (1.0 - (Saturation * (1.0 - f)));

                switch (i)
                {
                    case 0:
                        red = Value;
                        green = t;
                        blue = p;
                        break;

                    case 1:
                        red = q;
                        green = Value;
                        blue = p;
                        break;

                    case 2:
                        red = p;
                        green = Value;
                        blue = t;
                        break;

                    case 3:
                        red = p;
                        green = q;
                        blue = Value;
                        break;

                    case 4:
                        red = t;
                        green = p;
                        blue = Value;
                        break;

                    default:
                        red = Value;
                        green = p;
                        blue = q;
                        break;
                }

            }

            return new RgbColor((int) (red * 25500), (int) (green * 25500), (int) (blue * 25500));
        }

        // HSV to RGBA conversion
        public RgbaColor HsvToRgba()
        {
            return HsvToRgb().RgbToRgba();
        }

        // HSV to string
        public override string ToString()
        {
            return "hsv(" + Hue + ", " + Saturation + ", " + Value + ")";
        }
    }

}
