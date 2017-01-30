using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace Makaretu.Globalization
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
        static string repositoryFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "UnicodeCLDR");

        /// <summary>
        ///   The instance of the CLDR.
        /// </summary>
        /// <remarks>
        ///   CLDR implements the singleton pattern.  All access to CLDR
        ///   is via this property.
        /// </remarks>
        public static Cldr Instance = new Cldr();

        /// <summary>
        ///   Sources for CLDR data.
        /// </summary>
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
                                return new Version(version);
                            }
                        }
                    }
            }
            catch (FileNotFoundException)
            {
                // Eat it.  Returns "0.0.0" for unknown.
            }

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
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false
            };
            var url = "http://www.unicode.org/Public/cldr/latest";
            using (var unicode = new HttpClient(handler))
            using (var response = await unicode.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                var parts = response.Headers.Location.Segments;
                var version = parts[parts.Length - 1];
                if (!version.Contains('.'))
                    version += ".0";
                return new Version(version);
            }
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
        public Task<string[]> DownloadAsync(Version version)
        {
            if (Directory.Exists(repositoryFolder))
            {
                // TODO: delete all files
            }
            else
            {
                Directory.CreateDirectory(repositoryFolder);
            }

            var files = new[] { "core.zip", "keyboards.zip" };
            var tasks= files.Select(name => DownloadAsync(name, version));
            return Task.WhenAll(tasks);
        }

        async Task<string> DownloadAsync(string filename, Version version)
        {
            var v = version.ToString();
            while (v.EndsWith(".0"))
            {
                v = v.Substring(0, v.Length - 2);
            }

            // Sorry, unicode.org doesn't support secure download
            var url = $"http://unicode.org/Public/cldr/{v}/{filename}";
            var path = Path.Combine(repositoryFolder, filename);
            using (var local = File.Create(path))
            using (var unicode = new HttpClient())
            using (var response = await unicode.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            using (var content = await response.Content.ReadAsStreamAsync())
            {
                await content.CopyToAsync(local);
            }

            if (filename.ToLowerInvariant().EndsWith(".zip"))
            {
                var zipFolder = Path.Combine(repositoryFolder, Path.GetFileNameWithoutExtension(filename));
                ZipFile.ExtractToDirectory(path, zipFolder);
            }
            return path;
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
        ///   A sequence of <b>XDocuments</b> that match the <paramref name="name"/>.
        /// </returns>
        /// <remarks>
        ///   Each <see cref="Repositories">Reposistory</see> is searched for
        ///   for the document <paramref name="name"/>.
        /// </remarks>
        public IEnumerable<XDocument> GetDocuments(string name)
        {
            var found = false;
            foreach (var repo in Repositories)
            {
                var path = Path.Combine(repo, name);
                if (File.Exists(path))
                {
                    found = true;
                    yield return XDocument.Load(path);
                }
            }

            if (!found)
                throw new FileNotFoundException($"CLDR data '{name}' does not exist.");
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
                    yield return File.OpenText(path);
                }
            }

            if (!found)
                throw new FileNotFoundException($"CLDR data '{name}' does not exist.");
        }
    }
}
