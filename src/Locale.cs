using Common.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Makaretu.Globalization
{
    /// <summary>
    ///   A set of preferences that tend to be shared across significant swaths of the world.
    /// </summary>
    public class Locale
    {
        class Alias
        {
            public string From { get; set; }
            public string To { get; set; }
        }

        static ILog log = LogManager.GetLogger(typeof(Locale));
        static Dictionary<string, string> ParentLocales;
        // TODO: build aliases from root.xml
        static List<Alias> Aliases = new List<Alias>
        {
            new Alias
            {
                From = "calendar[@type='buddhist']/months",
                To = "calendar[@type='gregorian']/months"
            },
            new Alias
            {
                From = "calendar[@type='buddhist']/dateTimeFormats",
                To = "calendar[@type='generic']/dateTimeFormats"
            }
        };

        static ConcurrentDictionary<string, Locale> LocaleCache = new ConcurrentDictionary<string, Locale>();

        static Locale()
        {
            ParentLocales = Cldr.Instance
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
        /// <returns></returns>
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
        ///   A sequence of "languages" that should be searched.
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
                if (ParentLocales.ContainsKey(locale))
                {
                    var parent = LocaleIdentifier.Parse(ParentLocales[locale]);
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
        public XPathNavigator Find(string predicate)
        {
            var nav = ResourceBundle().FirstElementOrDefault(predicate);
            if (nav != null)
                return nav;

            var alias = Aliases.FirstOrDefault(a => predicate.Contains(a.From));
            if (alias != null)
            {
                return Find(predicate.Replace(alias.From, alias.To));
            }

            throw new KeyNotFoundException($"Cannot find CLDR '{predicate}'.");
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
        public static Locale Create(LocaleIdentifier id)
        {
            var cid = id.CanonicalForm();
            if (log.IsDebugEnabled)
                log.DebugFormat("Resolved locale '{0}' to '{1}'", id, cid);
            return LocaleCache.GetOrAdd(cid.ToString(), name => new Locale(cid));
        }

    }
}
