using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Resolvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.Comparers;
using DogmaMix.Core.UnitTesting;
using DogmaMix.Core.Xml;

namespace DogmaMix.Core.Extensions.Tests
{
    [TestClass]
    public class XmlPreloadedResolverExtensionsTests
    {
        private const string sampleXhtml =
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.1//EN""
    ""http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"">
<html version=""-//W3C//DTD XHTML 1.1//EN""
      xmlns=""http://www.w3.org/1999/xhtml"" xml:lang=""en"">
  <head>
    <title>Test</title>
  </head>
  <body>
    <p>Testing named character references: <span>&nbsp;&reg;&micro;&frac12;&Sigma;</span></p>
  </body>
</html>";

        [TestMethod]
        public void AddXhtml11()
        {
            var xmlResolver = new XmlPreloadedResolver();
            xmlResolver.AddXhtml11();
            EnumerableAssert.Contains(xmlResolver.PreloadedUris, new Uri(Xhtml11.DtdPublicId, UriKind.RelativeOrAbsolute));
            EnumerableAssert.Contains(xmlResolver.PreloadedUris, new Uri(Xhtml11.DtdSystemId, UriKind.RelativeOrAbsolute));
        }

        [TestMethod]
        public void AddXhtml11_XDocument()
        {
            var xmlResolver = new XmlPreloadedResolver();
            xmlResolver.AddXhtml11();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            settings.XmlResolver = xmlResolver;

            XDocument document;
            using (var stringReader = new StringReader(sampleXhtml))
            using (var xmlReader = XmlReader.Create(stringReader, settings))
                document = XDocument.Load(xmlReader);

            XNamespace ns = "http://www.w3.org/1999/xhtml";
            var span = document.Descendants(ns + "span").Single();
            Assert.AreEqual(" ®µ½Σ", span.Value);
        }
        
        [TestMethod]
        public void Add_Override()
        {
            var uri = new Uri("http://test.local");
            byte[] resourceA = new byte[] { 43, 59 };
            byte[] resourceB = new byte[] { 217, 24, 64 };

            var xmlResolver = new XmlPreloadedResolver();
            xmlResolver.Add(uri, resourceA);
            xmlResolver.Add(uri, resourceB, @override: true);

            VerifyResource(resourceB, xmlResolver, uri);
        }

        [TestMethod]
        public void Add_NoOverride()
        {
            var uri = new Uri("http://test.local");
            byte[] resourceA = new byte[] { 43, 59 };
            byte[] resourceB = new byte[] { 217, 24, 64 };

            var xmlResolver = new XmlPreloadedResolver();
            xmlResolver.Add(uri, resourceA);
            xmlResolver.Add(uri, resourceB, @override: false);

            VerifyResource(resourceA, xmlResolver, uri);
        }

        [TestMethod]
        public void Add_CustomComparer()
        {
            var uriA = new Uri("http://test.local/abc");
            var uriB = new Uri("http://test.local/xyz");
            var resourceA = new byte[] { 43, 59 };
            var resourceB = new byte[] { 217, 24, 64 };

            var uriComparer = KeyEqualityComparer.Create((Uri uri) => uri.Host);
            var xmlResolver = new XmlPreloadedResolver(null, XmlKnownDtds.None, uriComparer);

            xmlResolver.Add(uriA, resourceA);
            xmlResolver.Add(uriB, resourceB, @override: false);

            VerifyResource(resourceA, xmlResolver, uriA);
            VerifyResource(resourceA, xmlResolver, uriB);

            xmlResolver.Add(uriB, resourceB, @override: true);

            VerifyResource(resourceB, xmlResolver, uriA);
            VerifyResource(resourceB, xmlResolver, uriB);
        }

        private static void VerifyResource(byte[] expected, XmlPreloadedResolver xmlResolver, Uri uri)
        {
            using (var resource = (Stream)xmlResolver.GetEntity(uri, null, typeof(Stream)))
            {
                EnumerableAssert.AreEqual(expected, resource.ReadToEnd(), "The URI is not mapped to the expected resource in the XML resolver.");
            }
        }
    }
}