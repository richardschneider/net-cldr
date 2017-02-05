﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;

namespace Makaretu.Globalization
{
    /// <summary>
    ///   Extensions to make CLDR access easier.
    /// </summary>
    public static class CldrExentsions
    {
        /// <summary>
        ///   Find the first element that matches the XPATH expression
        ///   in the sequence of documents.
        /// </summary>
        /// <param name="docs">
        ///   The documents to seatch.
        /// </param>
        /// <param name="predicate">
        ///   The XPATH expression.
        /// </param>
        /// <returns>
        ///   The matched XElement.
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
        ///   The matched XElement.
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
        ///   The documents to seatch.
        /// </param>
        /// <param name="predicate">
        ///   The XPATH expression.
        /// </param>
        /// <returns>
        ///   The matched XElement or <b>null</b>.
        /// </returns>
        public static XPathNavigator FirstElementOrDefault(
            this IEnumerable<XPathDocument> docs,
            string predicate)
        {
            return docs
                .Select(doc =>
                {
                    var nav = doc.CreateNavigator();
                    var expr = nav.Compile(predicate);
                    expr.SetContext(CldrContext.Default);
                    return nav.SelectSingleNode(expr);
                })
                .FirstOrDefault(e => e != null);
        }
    }
}
