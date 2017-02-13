using Common.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Sepia.Globalization
{
    /// <summary>
    ///   A set of preferences that tend to be shared across significant swaths of the world.
    /// </summary>
    public class Locale
    {
        static ILog log = LogManager.GetLogger(typeof(Locale));
        static Lazy<Dictionary<string, string>> ParentLocales = new Lazy<Dictionary<string, string>>(LoadParentLocales);
        static Lazy<Alias[]> Aliases = new Lazy<Alias[]>(LoadAliases);
        static ConcurrentDictionary<string, Locale> LocaleCache = new ConcurrentDictionary<string, Locale>();
        static Regex countFallback = new Regex(
            @"\[@count='(?:(?!other).)*'\]",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        ConcurrentDictionary<string, XPathNavigator> QueryCache = new ConcurrentDictionary<string, XPathNavigator>();
        string currencyCode;

        /// <summary>
        ///   Creates a new instance of the <see cref="Locale"/> class with the
        ///   specified <see cref="LocaleIdentifier"/>.
        /// </summary>
        /// <param name="id">
        ///   Identifies a set of preferences.
        /// </param>
        /// <remarks>
        ///   The Create method should be used, so that locale caching can be used.
        /// </remarks>
        public Locale(LocaleIdentifier id)
        {
            Id = id;
        }

        /// <summary>
        ///   Identifies a set of preferences for the locale.
        /// </summary>
        /// <value>
        ///   The <see cref="LocaleIdentifier">canonical form</see> of the locale
        ///   identifier.
        /// </value>
        public LocaleIdentifier Id { get; private set; }

        /// <summary>
        ///   The primary currency of the locale.
        /// </summary>
        /// <value>
        ///   An ISO 4217 currency code.
        /// </value>
        /// <remarks>
        ///   The primary currency is based on the locale's <see cref="LocaleIdentifier.Region"/>.
        /// </remarks>
        public string CurrencyCode {
            get
            {
                if (currencyCode == null)
                {
                    currencyCode = Cldr.Instance
                        .GetDocuments("common/supplemental/supplementalData.xml")
                        .FirstElement($"supplementalData/currencyData/region[@iso3166='{Id.Region}']/currency/@iso4217")
                        .Value;
                }
                return currencyCode;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Id.ToString();
        }

        /// <summary>
        ///   A sequence of CLDR documents that can contain a locale resource.
        /// </summary>
        /// <param name="prefix">
        ///   The SVN trunk relative name of the document.  Defaults to "common/main/".
        /// </param>
        /// <param name="suffix">
        ///   The file extension.  Defaults to ".xml".
        /// </param>
        /// <returns>
        ///   All the documents that are named by the <see cref="SearchChain"/>.
        /// </returns>
        public IEnumerable<XPathDocument> ResourceBundle(string prefix = "common/main/", string suffix = ".xml")
        {
            return SearchChain()
                .Select(p => prefix + p + suffix)
                .SelectMany(Cldr.Instance.GetAllDocuments);
        }

        /// <summary>
        ///   The search chain for a locale resource.
        /// </summary>
        /// <returns>
        ///   A sequence of "files" that should be searched.
        /// </returns>
        /// <remarks>
        ///   Same as <see cref="LocaleIdentifier.SearchChain"/> but applies
        ///   any "parent locales" overrides.
        /// </remarks>
        public IEnumerable<string> SearchChain()
        {
            return SearchChainRecursive(Id);
        }

        static IEnumerable<string> SearchChainRecursive(LocaleIdentifier id)
        {
            foreach (var locale in id.SearchChain())
            {
                yield return locale;
                if (ParentLocales.Value.ContainsKey(locale))
                {
                    var parent = LocaleIdentifier.Parse(ParentLocales.Value[locale]);
                    foreach (var p in SearchChainRecursive(parent))
                    {
                        yield return p;
                    }
                    yield break;
                }
            }
        }

        /// <summary>
        ///   Find the first XML element that matches the XPath expression
        ///   in the <see cref="ResourceBundle"/>.
        /// </summary>
        /// <param name="predicate">
        ///   The XPath expression to match.
        /// </param>
        /// <returns>
        ///   The matched element.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        ///   No elements match the <paramref name="predicate"/>.
        /// </exception>
        /// <remarks>
        ///   If the <paramref name="predicate"/> cannot be matched, then it is recursively
        ///   modified with the "root aliases" and retried.
        ///   Lateral inheritance is also implemented. <c>[@count='x']</c> becomes
        ///   <c>[@count='x' or @count='other']</c>.
        ///   <para>
        ///   The <paramref name="predicate"/> is cached to improve performance.
        ///   </para>
        /// </remarks>
        public XPathNavigator Find(string predicate)
        {
            var nav = FindOrDefault(predicate);
            if (nav == null)
                throw new KeyNotFoundException($"Cannot find CLDR '{predicate}'.");
            return nav;
        }

        /// <summary>
        ///   Find the first (or none) XML element that matches the XPath expression
        ///   in the <see cref="ResourceBundle"/>.
        /// </summary>
        /// <param name="predicate">
        ///   The XPath expression to match.
        /// </param>
        /// <returns>
        ///   The matched element or <b>null</b>.
        /// </returns>
        /// <remarks>
        ///   If the <paramref name="predicate"/> cannot be matched, then it is recursively
        ///   modified with the "root aliases" and retried.
        ///   Lateral inheritance is also implemented. <c>[@count='x']</c> becomes
        ///   <c>[@count='x' or @count='other']</c>.
        ///   <para>
        ///   The <paramref name="predicate"/> is cached to improve performance.
        ///   </para>
        /// </remarks>
        public XPathNavigator FindOrDefault(string predicate)
        {
            // Lateral inheritance.
            predicate = countFallback.Replace(predicate, (match) =>
            {
                var s = match.ToString().TrimEnd(']');
                return s + " or @count='other']";
            });

            return QueryCache.GetOrAdd(predicate, (key) =>
            {
                // Find it.
                var nav = ResourceBundle().FirstElementOrDefault(key);
                if (nav != null)
                    return nav;

                // Try root aliases.
                var alias = Aliases.Value
                    .Where(a => key.Contains(a.From) && !key.Contains(a.To))
                    .OrderByDescending(a => a.From.Length)
                    .FirstOrDefault();
                if (alias != null)
                {
                    if (log.IsTraceEnabled)
                        log.TraceFormat("Trying alias '{0}' => '{1}'", alias.From, alias.To);

                    return FindOrDefault(key.Replace(alias.From, alias.To));
                }

                if (log.IsTraceEnabled)
                    log.TraceFormat("Not found '{0}'", key);

                return null;
            });
        }

        /// <summary>
        ///   Creates or reuses a locale with the specified string locale identifier.
        /// </summary>
        /// <param name="id">
        ///   A case insensitive string containing a locale identifier, based on BCP47.
        /// </param>
        /// <exception cref="FormatException">
        ///   <paramref name="id"/> is not in the correct format.
        /// </exception>
        /// <returns>
        ///   A locale for the specified <paramref name="id"/>.
        /// </returns>
        /// <seealso cref="LocaleIdentifier.Parse"/>
        public static Locale Create(string id)
        {
            return LocaleCache.GetOrAdd(id, name => Create(LocaleIdentifier.Parse(name)));
        }

        /// <summary>
        ///   Creates or reuses a locale with the specified <see cref="LocaleIdentifier"/>.
        /// </summary>
        /// <param name="id">
        ///   A locale identifier.
        /// </param>
        /// <returns>
        ///   A locale for the specified <paramref name="id"/>.
        /// </returns>
        /// <remarks>
        ///   Uses the <see cref="LocaleIdentifier.CanonicalForm"/> of the
        ///   <paramref name="id"/>.
        /// </remarks>
        public static Locale Create(LocaleIdentifier id)
        {
            var cid = id.CanonicalForm();
            if (log.IsDebugEnabled)
                log.DebugFormat("Resolved locale '{0}' to '{1}'", id, cid);
            return LocaleCache.GetOrAdd(cid.ToString(), name => new Locale(cid));
        }

        static Dictionary<string,string> LoadParentLocales()
        {
            if (log.IsDebugEnabled)
                log.Debug("Loading parent locales");

            return Cldr.Instance
                .GetDocuments("common/supplemental/supplementalData.xml")
                .Elements("supplementalData/parentLocales/parentLocale")
                .SelectMany(x =>
                {
                    var parent = x.GetAttribute("parent", "");
                    return x
                        .GetAttribute("locales", "").Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(child => new { Parent = parent, Child = child });
                })
                .ToDictionary(x => x.Child, x => x.Parent);
        }

        static Alias[] LoadAliases()
        {
            if (log.IsDebugEnabled)
                log.Debug("Loading root aliases");

            return Cldr.Instance
                .GetDocuments("common/main/root.xml")
                .Elements("//alias[@source='locale']")
                .Select(e => new Alias(e))
                .ToArray();
        }

        class Alias
        {
            public Alias(XPathNavigator nav)
            {
                var from = new StringBuilder();
                To = nav.GetAttribute("path", "");
                var target = nav.Clone();
                while (To.StartsWith("../"))
                {
                    target.MoveToParent();
                    from.Append(target.LocalName);
                    if (target.HasAttributes)
                    {
                        target.MoveToFirstAttribute();
                        do
                        {
                            from.AppendFormat("[@{0}='{1}']", target.LocalName, target.Value);
                        } while (target.MoveToNextAttribute());
                        target.MoveToParent(); // go back to the element
                    }
                    from.Append('/');
                    To = To.Substring(3);
                }
                From = String.Join("/", from.ToString().TrimEnd('/').Split('/').Reverse());
            }

            public string From { get; set; }
            public string To { get; set; }
        }

    }
}
