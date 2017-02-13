using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;

namespace Sepia.Globalization
{
    /// <summary>
    ///   Extensions to make CLDR access easier.
    /// </summary>
    /// <remarks>
    ///    These are simple XPath lookups for multiple documents. CLDR inheritance is not performed.
    ///    <para>
    ///    The <see cref="CldrContext"/> is used to provide extra XPath varaibles and functions.
    ///    </para>
    /// </remarks>
    public static class CldrExentsions
    {
        static ILog log = LogManager.GetLogger(typeof(CldrExentsions));

        /// <summary>
        ///   Find the first element that matches the XPATH expression
        ///   in the sequence of documents.
        /// </summary>
        /// <param name="docs">
        ///   The documents to search.
        /// </param>
        /// <param name="predicate">
        ///   The XPATH expression.
        /// </param>
        /// <returns>
        ///   The matched element.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        ///   No elements match the <paramref name="predicate"/>.
        /// </exception>
        public static XPathNavigator FirstElement(
            this IEnumerable<XPathDocument> docs,
            string predicate)
        {
            var element = FirstElementOrDefault(docs, predicate);
            if (element == null)
                throw new KeyNotFoundException($"Cannot find CLDR '{predicate}'.");

            return element;
        }

        /// <summary>
        ///   Find the first element that matches the XPATH expression
        ///   in the sequence of documents or return a default value.
        /// </summary>
        /// <param name="docs">
        ///   The documents to seatch.
        /// </param>
        /// <param name="predicate">
        ///   The XPATH expression.
        /// </param>
        /// <param name="defaultAction">
        ///   The action to perform when no elements match
        ///   the <paramref name="predicate"/>.
        /// </param>
        /// <returns>
        ///   The matched element.
        /// </returns>
        /// <exception cref="KeyNotFoundException ">
        ///   No elements match the <paramref name="predicate"/>.
        /// </exception>
        public static XPathNavigator FirstElementOrDefault(
            this IEnumerable<XPathDocument> docs,
            string predicate,
            Func<IEnumerable<XPathDocument>, XPathNavigator> defaultAction)
        {
            var element = FirstElementOrDefault(docs, predicate);
            if (element != null)
                return element;
            return defaultAction(docs);
        }

        /// <summary>
        ///   Find the first element that matches the XPATH expression
        ///   in the sequence of documents or return null.
        /// </summary>
        /// <param name="docs">
        ///   The documents to search.
        /// </param>
        /// <param name="predicate">
        ///   The XPATH expression.
        /// </param>
        /// <returns>
        ///   The matched element or <b>null</b>.
        /// </returns>
        public static XPathNavigator FirstElementOrDefault(
            this IEnumerable<XPathDocument> docs,
            string predicate)
        {
            if (log.IsTraceEnabled)
                log.Trace("Finding first " + predicate);

            return docs
                .Select(doc =>
                {
                    var nav = doc.CreateNavigator();
                    var expr = nav.Compile(predicate);
                    expr.SetContext(CldrContext.Default);
                    var node = nav.SelectSingleNode(expr);
                    if (node != null && log.IsTraceEnabled)
                    {
                        log.Trace("Found in " + node.BaseURI);
                    }
                    return node;
                })
                .FirstOrDefault(e => e != null);
        }

        /// <summary>
        ///   Find all the elements that matche the XPATH expression
        ///   in the sequence of documents.
        /// </summary>
        /// <param name="docs">
        ///   The documents to search.
        /// </param>
        /// <param name="predicate">
        ///   The XPATH expression.
        /// </param>
        /// <returns>
        ///   Sequence of matched elements.
        /// </returns>
        public static IEnumerable<XPathNavigator> Elements(
            this IEnumerable<XPathDocument> docs,
            string predicate)
        {
            if (log.IsTraceEnabled)
                log.Trace("Finding all " + predicate);

            return docs
                .SelectMany(doc =>
                {
                    var nav = doc.CreateNavigator();
                    var expr = nav.Compile(predicate);
                    expr.SetContext(CldrContext.Default);
                    return nav.Select(expr).OfType<XPathNavigator>();
                });
        }

    }
}
