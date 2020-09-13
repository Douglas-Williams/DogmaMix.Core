using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.Globalization;

namespace DogmaMix.Core.Strings.Tests
{
    [TestClass]
    public class SubstringTests
    {
        [TestMethod]
        public void Compare()
        {
            using (new CultureSwapper(PredefinedCulture.EnglishUnitedStates))
            {
                Assert.AreEqual(0, Math.Sign(Substring.Compare("abcdef", 2, 2, "CD", 0, 2, StringComparison.CurrentCultureIgnoreCase)));
                Assert.AreEqual(-1, Math.Sign(Substring.Compare("abcdef", 1, 3, "CD", 0, 2, StringComparison.OrdinalIgnoreCase)),
                    "'b' should be less than 'C' for case-insensitive comparison");
                Assert.AreEqual(1, Math.Sign(Substring.Compare("abcdef", 1, 3, "CD", 0, 2, StringComparison.Ordinal)),
                    "'b' should be greater than 'C' for case-sensitive comparison");
                Assert.AreEqual(0, Math.Sign(Substring.Compare("Encyclopædia", 8, 2, "aedia", 0, 3, StringComparison.CurrentCulture)));

                Assert.AreEqual(0, Math.Sign(Substring.Compare("café", 0, 4, "cafe\u0301", 0, 5, StringComparison.CurrentCulture)));
                Assert.AreEqual(0, Math.Sign(Substring.Compare("café", 0, 4, "cafe\u0301", 0, 5, StringComparison.CurrentCultureIgnoreCase)));
                Assert.AreEqual(0, Math.Sign(Substring.Compare("café", 0, 4, "cafe\u0301", 0, 5, StringComparison.InvariantCulture)));
                Assert.AreEqual(0, Math.Sign(Substring.Compare("café", 0, 4, "cafe\u0301", 0, 5, StringComparison.InvariantCultureIgnoreCase)));
                Assert.AreEqual(1, Math.Sign(Substring.Compare("café", 0, 4, "cafe\u0301", 0, 5, StringComparison.Ordinal)));
                Assert.AreEqual(1, Math.Sign(Substring.Compare("café", 0, 4, "cafe\u0301", 0, 5, StringComparison.OrdinalIgnoreCase)));

                Assert.AreEqual(-1, Math.Sign(Substring.Compare("café", 0, 4, "CAFE\u0301", 0, 5, StringComparison.CurrentCulture)));
                Assert.AreEqual(0, Math.Sign(Substring.Compare("café", 0, 4, "CAFE\u0301", 0, 5, StringComparison.CurrentCultureIgnoreCase)));
                Assert.AreEqual(-1, Math.Sign(Substring.Compare("café", 0, 4, "CAFE\u0301", 0, 5, StringComparison.InvariantCulture)));
                Assert.AreEqual(0, Math.Sign(Substring.Compare("café", 0, 4, "CAFE\u0301", 0, 5, StringComparison.InvariantCultureIgnoreCase)));
                Assert.AreEqual(1, Math.Sign(Substring.Compare("café", 0, 4, "CAFE\u0301", 0, 5, StringComparison.Ordinal)));
                Assert.AreEqual(1, Math.Sign(Substring.Compare("café", 0, 4, "CAFE\u0301", 0, 5, StringComparison.OrdinalIgnoreCase)));

                Assert.AreEqual(0, Math.Sign(Substring.Compare("café", 2, 2, "cafe\u0301", 2, 3, StringComparison.CurrentCulture)));
                Assert.AreEqual(1, Math.Sign(Substring.Compare("café", 0, 4, "cafe\u0301", 0, 4, StringComparison.CurrentCulture)));

                Assert.AreEqual(1, Math.Sign(Substring.Compare("abc", 1, 2, "", 0, 0, StringComparison.Ordinal)));
                Assert.AreEqual(-1, Math.Sign(Substring.Compare("", 0, 0, "abc", 0, 2, StringComparison.Ordinal)));
                Assert.AreEqual(0, Math.Sign(Substring.Compare("abc", 1, 0, "", 0, 0, StringComparison.Ordinal)));
                Assert.AreEqual(0, Math.Sign(Substring.Compare("abc123", 2, 3, "ABC123", 2, 3, StringComparison.OrdinalIgnoreCase)));
            }
        }

        [TestMethod]
        public void Compare_ÆbleApple()
        {
            // Sorting and String Comparison in .NET Framework
            // https://msdn.microsoft.com/en-us/goglobal/bb688122.aspx#euf

            using (new CultureSwapper(PredefinedCulture.EnglishUnitedStates))
            {
                string message = "\"Æble\" is less than \"Apple\" for the English culture.";
                Assert.AreEqual(-1, Math.Sign(Substring.Compare("Æble", 0, 4, "Apple", 0, 5, StringComparison.CurrentCulture)), message);
                Assert.AreEqual(-1, Math.Sign(Substring.Compare("Æble", 0, 4, "Apple", 0, 5, StringComparison.CurrentCultureIgnoreCase)), message);
            }

            using (new CultureSwapper(PredefinedCulture.DanishDenmark))
            {
                string message = "\"Æble\" is less than \"Apple\" for the invariant culture.";
                Assert.AreEqual(-1, Math.Sign(Substring.Compare("Æble", 0, 4, "Apple", 0, 5, StringComparison.InvariantCulture)), message);
                Assert.AreEqual(-1, Math.Sign(Substring.Compare("Æble", 0, 4, "Apple", 0, 5, StringComparison.InvariantCulture)), message);

                message = "\"Æble\" is greater than \"Apple\" for the Danish culture.";
                Assert.AreEqual(1, Math.Sign(Substring.Compare("Æble", 0, 4, "Apple", 0, 5, StringComparison.CurrentCulture)), message);
                Assert.AreEqual(1, Math.Sign(Substring.Compare("Æble", 0, 4, "Apple", 0, 5, StringComparison.CurrentCultureIgnoreCase)), message);
            };
        }
    }
}