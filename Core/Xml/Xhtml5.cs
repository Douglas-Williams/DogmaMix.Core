using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DogmaMix.Core.Xml
{
    /// <summary>
    /// Provides utility members for XHTML5.
    /// </summary>
    public static class Xhtml5
    {
        private static Lazy<string> EntitiesDtdLazy = new Lazy<string>(() =>
        {
            string dtd;

            using (var stream = ManifestResources.GetXhtml5EntitiesDtd())
            using (var reader = new StreamReader(stream))
                dtd = reader.ReadToEnd();

            dtd = Regex.Replace(dtd, @"\<!--.*?--\>", "", RegexOptions.Singleline);
            return dtd;
        });

        /// <summary>
        /// The <abbr title="document type definition">DTD</abbr> 
        /// for <see href="https://html.spec.whatwg.org/multipage/named-characters.html#named-character-references">named character references</see>
        /// in XHTML5.
        /// </summary>
        public static string EntitiesDtd => EntitiesDtdLazy.Value;
    }
}
