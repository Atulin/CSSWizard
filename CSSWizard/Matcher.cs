using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSSWizard
{
    static class Matcher
    {
        public static string Swap(this string str)
        {
            string pattern = "\\B#(\\w{3}|\\w{6}|\\w{8})\\b";

            Regex r = new Regex(pattern);
            var res = r.Replace(str, new MatchEvaluator(SwapRgbValue));

            string pattern2 = "rgba\\(([^\\)]+)\\)";
            Regex r2 = new Regex(pattern2);
            var res2 = r2.Replace(res, SwapRgbaValue);

            return res2;
        }

        private static string SwapRgbValue(Match m)
        {
            var hex = m.Value;
            return hex.InvertRgbValueString();
        }

        private static string SwapRgbaValue(Match m)
        {
            var rgba = m.Value;
            return rgba.InvertRgbaValueVerbose();
        }
    }
}
