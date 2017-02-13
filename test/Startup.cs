using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sepia.Globalization
{
    [TestClass]
    public class Startup
    {
        [AssemblyInitialize]
        public static void InstallCldr(TestContext context)
        {
            var version = Cldr.Instance.DownloadLatestAsync().Result;
        }
    }
}