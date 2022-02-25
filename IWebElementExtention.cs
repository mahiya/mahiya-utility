using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System.Collections.Generic;
using System.Linq;

namespace Mahiya.Utility
{
    public static class IWebElementExtention
    {
        public static string EliminateSpaces(this string str)
        {
            return str.Replace("\n", "").Replace(" ", "");
        }

        public static double GetDouble(this string str)
        {
            double d;
            if (!double.TryParse(str, out d)) return 0;
            return d;
        }

        public static string Text(this IHtmlDocument doc, string selectors)
        {
            return doc.QuerySelector(selectors)?.TextContent.EliminateSpaces();
        }

        public static List<string> TextAll(this IElement doc, string selectors)
        {
            return doc.QuerySelectorAll(selectors).Select(d => d?.TextContent.EliminateSpaces()).ToList();
        }

        public static string Text(this IElement doc, string selectors)
        {
            return doc.QuerySelector(selectors)?.TextContent.EliminateSpaces();
        }

        public static List<string> TextAll(this IHtmlDocument doc, string selectors)
        {
            return doc.QuerySelectorAll(selectors).Select(d => d?.TextContent.EliminateSpaces()).ToList();
        }

        public static int? Int(this IHtmlDocument doc, string selectors)
        {
            var t = doc.Text(selectors);
            int r;
            return int.TryParse(t, out r) ? (int?)r : null;
        }

        public static int? Int(this IElement doc, string selectors)
        {
            var t = doc.Text(selectors);
            int r;
            return int.TryParse(t, out r) ? (int?)r : null;
        }

        public static List<int?> IntAll(this IHtmlDocument doc, string selectors)
        {
            return doc.TextAll(selectors).Select(t =>
            {
                int r;
                return int.TryParse(t, out r) ? (int?)r : null;
            }).ToList();
        }

        public static List<int?> IntAll(this IElement doc, string selectors)
        {
            return doc.TextAll(selectors).Select(t =>
            {
                int r;
                return int.TryParse(t, out r) ? (int?)r : null;
            }).ToList();
        }

        public static string Get(this List<string> array, int index)
        {
            return array.Count > index ? array[index] : null;
        }

        public static int? Get(this List<int?> array, int index)
        {
            return array.Count > index ? array[index] : null;
        }
    }
}
