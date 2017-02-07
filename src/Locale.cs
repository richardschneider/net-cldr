using System;
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
        public LocaleIdentifier Id { get; private set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Id.ToString();
        }

        /// <summary>
        ///   A sequence of CLDR Documents that contain can a locale resource.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public IEnumerable<XPathDocument> ResourceBundle(string prefix = "common/main/", string suffix = ".xml")
        {
            return Id
                .SearchChain()
                .Select(p => prefix + p + suffix)
                .SelectMany(Cldr.Instance.GetAllDocuments);
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
            return Create(LocaleIdentifier.Parse(id));
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
            // TODO: Caching

            return new Locale(id);
        }

    }
}
