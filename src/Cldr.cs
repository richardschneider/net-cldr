using Common.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Sepia.Globalization
{
    /// <summary>
    ///   Provides access to the Unicode Common Local Data Repository
    ///   (<see href="http://cldr.unicode.org/index">CLDR</see>).
    /// </summary>
    /// <remarks>
    ///   CLDR implements the singleton pattern.  All access to the CLDR
    ///   is via the <see cref="Instance"/> property.
    /// </remarks>
    public class Cldr
    {
        static ILog log = LogManager.GetLogger(typeof(Cldr));

        static string repositoryFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "UnicodeCLDR");

        /// <summary>
        ///   The official source for CLDR data.
        /// </summary>
        /// <value>
        ///   http://unicode.org/Public/cldr/
        /// </value>
        public static string OriginUrl = "http://unicode.org/Public/cldr/";

        /// <summary>
        ///   The instance of the CLDR.
        /// </summary>
        /// <value>
        ///   Provides access to the Unicode CLDR.
        /// </value>
        /// <remarks>
        ///   CLDR implements the singleton pattern.  All access to CLDR
        ///   is via this property.
        /// </remarks>
        public static Cldr Instance = new Cldr();

        ConcurrentDictionary<string, XPathDocument> DocumentCache = new ConcurrentDictionary<string, XPathDocument>();

        /// <summary>
        ///   Local sources for CLDR data.
        /// </summary>
        /// <value>
        ///   The folder(s) that may contain CLDR files.
        /// </value>
        /// <remarks>
        ///   Sometimes the user or an app developer may want to add/change 
        ///   CLDR data supplied by Unicode.  This is a list of directories
        ///   to search for the CLDR data.
        ///   <para>
        ///   It is initially set to the repo provided by Unicode.  To
        ///   add your overrides insert the path at the head of list.
        ///   </para>
        /// </remarks>
        public List<string> Repositories = new List<string>
        {
            Path.Combine(repositoryFolder, "core")
        };

        /// <summary>
        ///   Create a new instance of CLDR.
        /// </summary>
        /// <remarks>
        ///   <b>Clrd</b> implements the Singleton Pattern.  Use the
        ///   <see cref="Cldr.Instance"/> property.
        /// </remarks>
        protected Cldr()
        {
        }

        #region Synchronisation
        /// <summary>
        ///   Gets the current, locally cached, version of the CLDR.
        /// </summary>
        /// <returns>
        ///   If the CLDR is not locally cached, then "0.0.0" is returned.
        /// </returns>
        /// <remarks>
        ///   The version number is defined in common/dtd/ldml.dtd
        /// </remarks>
        public Version CurrentVersion()
        {
            const string vdef = "<!ATTLIST version cldrVersion CDATA #FIXED \"";

            try
            {
                foreach (var dtd in GetTextDocuments("common/dtd/ldml.dtd"))
                    using (dtd)
                    {
                        while (true)
                        {
                            var s = dtd.ReadLine();
                            if (s.StartsWith(vdef))
                            {
                                var version = "";
                                for (int i = vdef.Length; s[i] != '"'; ++i)
                                {
                                    version += s[i];
                                }
                                if (!version.Contains('.'))
                                    version += ".0";
                                var v = new Version(version);
                                if (log.IsDebugEnabled)
                                    log.Debug($"Current version {v}");
                                return v;
                            }
                        }
                    }
            }
            catch (FileNotFoundException)
            {
                // Eat it.  Returns "0.0.0" for unknown.
            }

            log.Warn("CLDR data is missing");
            return new Version("0.0.0");
        }

        /// <summary>
        ///   Gets the latest published version number from unicode.org.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        ///   The latest version number is obtained by doing a "GET http://www.unicode.org/Public/cldr/latest"
        ///   and retrieving the Location header for the re-direct.
        /// </remarks>
        public static async Task<Version> LatestVersionAsync()
        {
            // Hack for https://github.com/richardschneider/net-cldr/issues/1
            if (Environment.GetEnvironmentVariable("CI")?.ToLowerInvariant() == "true")
            {
                OriginUrl = "http://ftp.lanet.lv/ftp/mirror/unicode/cldr/";
                return new Version("30.0.2");
            }

            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
                UseProxy = false
            };
            var url = OriginUrl + "latest";
            if (log.IsDebugEnabled)
                log.Debug($"GET {url}");
            using (var unicode = new HttpClient(handler))
            using (var response = await unicode.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                var parts = response.Headers.Location.Segments;
                var version = parts[parts.Length - 1];
                if (!version.Contains('.'))
                    version += ".0";

                var v = new Version(version);
                if (log.IsDebugEnabled)
                    log.Debug($"Latest version {v}");
                return v;
            }
        }

        /// <summary>
        ///   Downloads the latest version of CLDR.
        /// </summary>
        /// <returns>
        ///   The latest published version number.
        /// </returns>
        /// <remarks>
        ///   Only performs a download when the <see cref="CurrentVersion"/> is less than the <see cref="LatestVersionAsync"/>.
        /// </remarks>
        public async Task<Version> DownloadLatestAsync()
        {
            var latestVersion = await LatestVersionAsync();
            if (CurrentVersion() < latestVersion)
            {
                await DownloadAsync(latestVersion);
            }

            return latestVersion;
        }

        /// <summary>
        ///   Downloads the specified CLDR <see cref="Version"/> to the current, locally cached, copy.
        /// </summary>
        /// <param name="version">
        ///   The CLDR version to use locally.
        /// </param>
        /// <returns>
        ///   The local paths to the downloaded files.
        /// </returns>
        public async Task<string[]> DownloadAsync(Version version)
        {
            if (Directory.Exists(repositoryFolder))
            {
                Directory.Delete(repositoryFolder, true);
            }
            Directory.CreateDirectory(repositoryFolder);

            var files = new[] { "core.zip", "keyboards.zip" };
            var tasks = files.Select(name => DownloadAsync(name, version));
            var result = await Task.WhenAll(tasks);

            ClearCaches();
            return result;
        }

        async Task<string> DownloadAsync(string filename, Version version)
        {
            var v = version.ToString();
            while (v.EndsWith(".0"))
            {
                v = v.Substring(0, v.Length - 2);
            }

            var url = $"{OriginUrl}{v}/{filename}";
            var path = Path.Combine(repositoryFolder, filename);
            if (log.IsDebugEnabled)
                log.Debug($"GET {url}");
            using (var local = File.Create(path))
            using (var unicode = new HttpClient())
            using (var response = await unicode.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                using (var content = await response.Content.ReadAsStreamAsync())
                {
                    await content.CopyToAsync(local);
                }
            }

            if (filename.ToLowerInvariant().EndsWith(".zip"))
            {
                var zipFolder = Path.Combine(repositoryFolder, Path.GetFileNameWithoutExtension(filename));
                if (log.IsDebugEnabled)
                    log.Debug($"Unzipping {filename}");
                ZipFile.ExtractToDirectory(path, zipFolder);
                if (log.IsDebugEnabled)
                    log.Debug($"Deleting {filename}");
                File.Delete(path);
            }
            return path;
        }

        void ClearCaches()
        {
            DocumentCache.Clear();
        }

        #endregion

        /// <summary>
        ///   Gets the CLDR XML document(s) with the specified name.
        /// </summary>
        /// <param name="name">
        ///   The SVN trunk relative name of the document, such
        ///   as "common/supplemental/supplementalData.xml".
        /// </param>
        /// <returns>
        ///   A sequence of <b>XPathDocuments</b> that match the <paramref name="name"/>.
        /// </returns>
        /// <exception cref="FileNotFoundException">
        ///   When no document with the <paramref name="name"/> exists.
        /// </exception>
        /// <remarks>
        ///   Each <see cref="Repositories">Reposistory</see> is searched for
        ///   for the document <paramref name="name"/>.
        /// </remarks>
        public IEnumerable<XPathDocument> GetDocuments(string name)
        {
            var found = false;
            foreach (var repo in Repositories)
            {
                var path = Path.Combine(repo, name);
                var doc = DocumentCache.GetOrAdd(path, fqn =>
                {
                    if (File.Exists(fqn))
                    {
                        if (log.IsDebugEnabled)
                            log.Debug($"Loading document {fqn}");
                        return new XPathDocument(fqn);
                    }
                    return null;
                });
                if (doc != null)
                {
                    found = true;
                    yield return doc;
                }
            }

            if (!found)
                throw new FileNotFoundException($"CLDR data '{name}' does not exist.");
        }

        /// <summary>
        ///   Gets all the CLDR XML document(s) with the specified name, non-existent documents
        ///   are ignored.
        /// </summary>
        /// <param name="name">
        ///   The SVN trunk relative name of the document, such
        ///   as "common/main/EN_US.xml".
        /// </param>
        /// <returns>
        ///   A sequence of <b>XPathDocuments</b> that match the <paramref name="name"/>.
        /// </returns>
        /// <remarks>
        ///   Each <see cref="Repositories">Reposistory</see> is searched for
        ///   for the document <paramref name="name"/>.
        ///   <para>
        ///   If no document is found, an empty sequence is returned.
        ///   </para>
        /// </remarks>
        public IEnumerable<XPathDocument> GetAllDocuments(string name)
        {
            foreach (var repo in Repositories)
            {
                var path = Path.Combine(repo, name);
                var doc = DocumentCache.GetOrAdd(path, fqn =>
                {
                    if (File.Exists(fqn))
                    {
                        if (log.IsDebugEnabled)
                            log.Debug($"Loading document {fqn}");
                        return new XPathDocument(fqn);
                    }
                    return null;
                });
                if (doc != null)
                {
                    yield return doc;
                }
            }
        }

        /// <summary>
        ///   Gets the CLDR text document(s) with the specified name.
        /// </summary>
        /// <param name="name">
        ///   The SVN trunk relative name of the document, such
        ///   as "common/dtd/ldml.dtd".
        /// </param>
        /// <returns>
        ///   A sequence of <b>StreamReaders</b> that match the <paramref name="name"/>.
        /// </returns>
        /// <remarks>
        ///   Each <see cref="Repositories">Reposistory</see> is searched for
        ///   for the document <paramref name="name"/>.
        /// </remarks>
        public IEnumerable<StreamReader> GetTextDocuments(string name)
        {
            var found = false;
            foreach (var repo in Repositories)
            {
                var path = Path.Combine(repo, name);
                if (File.Exists(path))
                {
                    found = true;
                    if (log.IsDebugEnabled)
                        log.Debug($"Loading text document {path}");
                    yield return File.OpenText(path);
                }
            }

            if (!found)
                throw new FileNotFoundException($"CLDR data '{name}' does not exist.");
        }

        #region Code validation

        /// <summary>
        ///   Determines if the language code is defined.
        /// </summary>
        /// <param name="code">
        ///   The language code to check.
        /// </param>
        /// <returns>
        ///   <b>true</b> if the <paramref name="code"/> is defined; otherwise, <b>false</b>.
        /// </returns>
        public bool IsLanguageDefined(string code)
        {
            if (code == "root")
                return true;

            return null != this
                .GetDocuments("common/validity/language.xml")
                .FirstElementOrDefault($"supplementalData/idValidity/id[@type='language'][cldr:contains-code(.,'{code}')]");
        }

        /// <summary>
        ///   Determines if the script code is defined.
        /// </summary>
        /// <param name="code">
        ///   The script (writting system) code to check.
        /// </param>
        /// <returns>
        ///   <b>true</b> if the <paramref name="code"/> is defined; otherwise, <b>false</b>.
        /// </returns>
        public bool IsScriptDefined(string code)
        {
            return null != this
                .GetDocuments("common/validity/script.xml")
                .FirstElementOrDefault($"supplementalData/idValidity/id[@type='script'][cldr:contains-code(.,'{code}')]");
        }

        /// <summary>
        ///   Determines if the region code is defined.
        /// </summary>
        /// <param name="code">
        ///   The region code to check.
        /// </param>
        /// <returns>
        ///   <b>true</b> if the <paramref name="code"/> is defined; otherwise, <b>false</b>.
        /// </returns>
        public bool IsRegionDefined(string code)
        {
            return null != this
                .GetDocuments("common/validity/region.xml")
                .FirstElementOrDefault($"supplementalData/idValidity/id[@type='region'][cldr:contains-code(.,'{code}')]");
        }

        /// <summary>
        ///   Determines if the currency code is defined.
        /// </summary>
        /// <param name="code">
        ///   The currency code to check.
        /// </param>
        /// <returns>
        ///   <b>true</b> if the <paramref name="code"/> is defined; otherwise, <b>false</b>.
        /// </returns>
        public bool IsCurrencyDefined(string code)
        {
            return null != this
                .GetDocuments("common/validity/currency.xml")
                .FirstElementOrDefault($"supplementalData/idValidity/id[@type='currency'][cldr:contains-code(.,'{code}')]");
        }

        /// <summary>
        ///   Determines if the language variant code is defined.
        /// </summary>
        /// <param name="code">
        ///   The language variant code to check.
        /// </param>
        /// <returns>
        ///   <b>true</b> if the <paramref name="code"/> is defined; otherwise, <b>false</b>.
        /// </returns>
        public bool IsVariantDefined(string code)
        {
            return null != this
                .GetDocuments("common/validity/variant.xml")
                .FirstElementOrDefault($"supplementalData/idValidity/id[@type='variant'][cldr:contains-code(.,'{code}')]");
        }
        #endregion
    }
}
