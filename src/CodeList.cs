using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Globalization
{
    /// <summary>
    ///   A list of defined codes.
    /// </summary>
    /// <remarks>
    ///   Each code can be a token or a <see cref="StringRange"/>.
    /// </remarks>
    public class CodeList
    {
        IEnumerable<string> codes;

        /// <summary>
        ///   Creates a new instance of the <see cref="CodeList"/> class with
        ///   the text.
        /// </summary>
        /// <param name="codes">
        ///   White space separated list of codes.
        /// </param>
        public CodeList(string codes)
        {
            this.codes = codes.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        ///   The separator for a <see cref="StringRange"/>.
        /// </summary>
        /// <value>
        ///   Defaults to '~'.
        /// </value>
        public char Separator { get; set; } = '~';

        /// <summary>
        ///   Determines if the code is defined.
        /// </summary>
        /// <param name="code">
        ///   The code to check.
        /// </param>
        /// <returns>
        ///   <b>true</b> if the <paramref name="code"/> is defined; otherwise, <b>false</b>.
        /// </returns>
        public bool Contains(string code)
        {
            foreach (var defined in codes)
            {
                if (code == defined)
                    return true;
                var i = defined.IndexOf(Separator);
                if (i > 0)
                {
                    var found = StringRange
                        .Enumerate(defined.Substring(0, i), defined.Substring(i + 1))
                        .Any(c => c == code);
                    if (found)
                        return true;
                }
            }
            return false;
        }

    }
}
