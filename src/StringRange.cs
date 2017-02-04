using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Globalization
{
    /// <summary>
    ///   A String Range is a compact format for specifying a list of strings.
    /// </summary>
    /// <see href="http://www.unicode.org/reports/tr35/tr35.html#String_Range">TR #35</see> 
    public static class StringRange
    {
        /// <summary>
        ///   List the strings between X and Y.
        /// </summary>
        /// <param name="x">The starting string, inclusive.</param>
        /// <param name="y">The ending string, inclusive.</param>
        /// <returns>
        ///   An inclusive sequence of strings between <paramref name="x"/>
        ///   and <paramref name="y"/>.
        /// </returns>
        public static IEnumerable<string> Enumerate(string x, string y)
        {
            if (x.Length < 1)
                throw new ArgumentOutOfRangeException(nameof(x), "Must have a character.");
            if (y.Length > x.Length)
                throw new ArgumentOutOfRangeException(nameof(y), "Too many characters.");

            // TODO
            if (y.Length > 1)
                throw new ArgumentOutOfRangeException(nameof(y), "NYI too many characters.");

            var n = y.Length;
            var p = x.Substring(0, x.Length - n);
            var s = x.Substring(x.Length - n, n);

            for (int i = 0; i < n; ++i)
            {
                var from = s[i];
                var to = y[i];
                for (char code = from; code <= to; ++code)
                {
                    yield return p + code;
                }
            }
        }

    }
}
