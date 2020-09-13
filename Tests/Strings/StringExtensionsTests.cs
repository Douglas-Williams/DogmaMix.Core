using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.Globalization;
using DogmaMix.Core.UnitTesting;

namespace DogmaMix.Core.Extensions.Tests
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void Contains()
        {
            using (new CultureSwapper(PredefinedCulture.EnglishUnitedStates))
            {
                Assert.IsFalse("HomeController".Contains("controller", StringComparison.CurrentCulture));
                Assert.IsTrue("HomeController".Contains("controller", StringComparison.CurrentCultureIgnoreCase));

                Assert.IsFalse("Encyclopædia".Contains("aed", StringComparison.Ordinal));
                Assert.IsTrue("Encyclopædia".Contains("aed", StringComparison.CurrentCulture));
            }
        }
        
        [TestMethod]
        public void Find_Exceptions()
        {
            int matchIndex, matchLength;

            ExceptionAssert.Throws<ArgumentNullException>(() => StringExtensions.Find(null, "abc", StringComparison.CurrentCulture, out matchIndex, out matchLength));
            ExceptionAssert.Throws<ArgumentNullException>(() => "Hello".Find(null, StringComparison.CurrentCulture, out matchIndex, out matchLength));
            ExceptionAssert.Throws<ArgumentOutOfRangeException>(() => "Hello".Find("abc", 6, StringComparison.CurrentCulture, out matchIndex, out matchLength));
            ExceptionAssert.Throws<ArgumentOutOfRangeException>(() => "Hello".Find("abc", 0, 6, StringComparison.CurrentCulture, out matchIndex, out matchLength));
            ExceptionAssert.Throws<ArgumentOutOfRangeException>(() => "Hello".Find("abc", 3, 3, StringComparison.CurrentCulture, out matchIndex, out matchLength));
            ExceptionAssert.Throws<ArgumentException>(() => "Hello".Find("abc", (StringComparison)int.MaxValue, out matchIndex, out matchLength));
        }

        [TestMethod]
        public void Find_IndexLength()
        {
            using (new CultureSwapper(PredefinedCulture.EnglishUnitedStates))
            {
                FindInnerTest("ABCDEF", "CDE", StringComparison.Ordinal, 2, 3);
                FindInnerTest("ABCDEF", "A", StringComparison.Ordinal, 0, 1);
                FindInnerTest("ABCDEF", "EF", StringComparison.Ordinal, 4, 2);
                FindInnerTest("ABCDEF", "ABCDEF", StringComparison.Ordinal, 0, 6);
                FindInnerTest("ABCDEF", "ABCDEFG", StringComparison.Ordinal, -1, -1);
                FindInnerTest("ABCDEF", "abc", StringComparison.Ordinal, -1, -1);
                FindInnerTest("ABCDEF", "abc", StringComparison.OrdinalIgnoreCase, 0, 3);
                FindInnerTest("Encyclopædia", "aedia", StringComparison.CurrentCulture, 8, 4);

                FindInnerTest("Encyclopædia", "aedi", 8, 3, StringComparison.CurrentCulture, 8, 3);
                FindInnerTest("Encyclopædia", "aedi", 7, 4, StringComparison.CurrentCulture, 8, 3);
                FindInnerTest("Encyclopædia", "aedi", 8, 2, StringComparison.CurrentCulture, -1, -1);
                FindInnerTest("Encyclopædia", "aedi", 7, 3, StringComparison.CurrentCulture, -1, -1);
                FindInnerTest("Encyclopædia", "aedi", 9, 3, StringComparison.CurrentCulture, -1, -1);

                FindInnerTest("Encyclopædia", "AEDI", 8, 3, StringComparison.CurrentCultureIgnoreCase, 8, 3);
                FindInnerTest("Encyclopædia", "aedi", 8, 3, StringComparison.InvariantCulture, 8, 3);
                FindInnerTest("Encyclopædia", "AEDI", 8, 3, StringComparison.InvariantCultureIgnoreCase, 8, 3);
                FindInnerTest("Encyclopædia", "aedi", 8, 3, StringComparison.Ordinal, -1, -1);
                FindInnerTest("Encyclopædia", "AEDI", 8, 3, StringComparison.OrdinalIgnoreCase, -1, -1);

                FindInnerTest("Encyclopædia", "", StringComparison.CurrentCulture, 0, 0);
                FindInnerTest("Encyclopædia", "", 2, StringComparison.CurrentCulture, 2, 0);
                FindInnerTest("Encyclopædia", "", 5, 3, StringComparison.CurrentCulture, 5, 0);
                FindInnerTest("Encyclopædia", "", "Encyclopædia".Length, StringComparison.CurrentCulture, "Encyclopædia".Length, 0);
                
                using (new CultureSwapper(PredefinedCulture.GermanGermany))
                {
                    FindInnerTest("• Yesss!", "ß", StringComparison.CurrentCulture, 4, 2);
                }

                FindInnerTest("file", "İL", StringComparison.CurrentCulture, -1, -1);
                FindInnerTest("file", "İL", StringComparison.CurrentCultureIgnoreCase, -1, -1);
                using (new CultureSwapper(PredefinedCulture.TurkishTurkey))
                {
                    FindInnerTest("file", "İL", StringComparison.CurrentCulture, -1, -1);
                    FindInnerTest("file", "İL", StringComparison.CurrentCultureIgnoreCase, 1, 2);
                }

                FindInnerTest("ABCDEF", "", StringComparison.CurrentCulture, 0, 0);
                FindInnerTest("ABCDEF", "", 2, 3, StringComparison.CurrentCulture, 2, 0);
                FindInnerTest("ABCDEF", "", 6, 0, StringComparison.CurrentCulture, 6, 0);
                FindInnerTest("", "", StringComparison.CurrentCulture, 0, 0);
                FindInnerTest("", "ABC", StringComparison.CurrentCulture, -1, -1);

                // Jon Skeet: https://stackoverflow.com/q/15980310/1149773
                // U+00E9 is a single code point for e-acute
                // e followed by U+0301 still means e-acute, but from two code points
                FindInnerTest("25 b\u00e9d 2013", "be\u0301d", StringComparison.CurrentCulture, 3, 3);

                // Esailija: https://stackoverflow.com/questions/15980310/how-can-i-perform-a-culture-sensitive-starts-with-operation-from-the-middle-of#comment23190863_16062528
                using (new CultureSwapper(PredefinedCulture.German))
                {
                    FindInnerTest("x ßheßieß y", "sshessiess", StringComparison.CurrentCulture, 2, 7);
                }

                // Mårten Wikström: https://stackoverflow.com/a/22513015/1149773
                FindInnerTest("x b\u00e9d y", "be\u0301d", 2, StringComparison.CurrentCulture, 2, 3);
                FindInnerTest("x be\u0301d y", "b\u00e9d", 2, StringComparison.CurrentCulture, 2, 4);
                FindInnerTest("x b\u00e9d", "be\u0301d", 2, StringComparison.CurrentCulture, 2, 3);
                FindInnerTest("x be\u0301d", "b\u00e9d", 2, StringComparison.CurrentCulture, 2, 4);
                FindInnerTest("b\u00e9d y", "be\u0301d", 0, StringComparison.CurrentCulture, 0, 3);
                FindInnerTest("be\u0301d y", "b\u00e9d", 0, StringComparison.CurrentCulture, 0, 4);
                FindInnerTest("b\u00e9d", "be\u0301d", 0, StringComparison.CurrentCulture, 0, 3);
                FindInnerTest("be\u0301d", "b\u00e9d", 0, StringComparison.CurrentCulture, 0, 4);
                FindInnerTest("b\u00e9", "be\u0301d", 0, StringComparison.CurrentCulture, -1, -1);
                FindInnerTest("be\u0301", "b\u00e9d", 0, StringComparison.CurrentCulture, -1, -1);

                // Michael Liu: https://stackoverflow.com/q/20480016/1149773
                FindInnerTest("déf", "é", StringComparison.CurrentCulture, 1, 1);
                FindInnerTest("déf", "e\u0301", StringComparison.CurrentCulture, 1, 1);
                FindInnerTest("de\u0301f", "é", StringComparison.CurrentCulture, 1, 2);
                FindInnerTest("œf", "œ", StringComparison.CurrentCulture, 0, 1);
                FindInnerTest("œf", "oe", StringComparison.CurrentCulture, 0, 1);
                FindInnerTest("oef", "œ", StringComparison.CurrentCulture, 0, 2);

                // Surrogate pairs:
                FindInnerTest("𥀀𥀁𥀂𥀃𥀄", "𥀀𥀁𥀂", StringComparison.CurrentCulture, 0, 6);
                FindInnerTest("𥀀𥀁𥀂𥀃𥀄", "𥀃𥀄", StringComparison.CurrentCulture, 6, 4);
                FindInnerTest("𥀀𥀁𥀂𥀃𥀄", "\uDC03", StringComparison.CurrentCulture, 7, 1);
                FindInnerTest("𥀀𥀁𥀂𥀃𥀄", "\uDC03𥀄", StringComparison.CurrentCulture, 7, 3);
                FindInnerTest("𥀀𥀁𥀂𥀃𥀄", "\uDC00\uD854\uDC01\uD854", StringComparison.CurrentCulture, 1, 4);
                FindInnerTest("𥀀𥀁𥀂𥀃ææ𥀄", "aeae𥀄", StringComparison.CurrentCulture, 8, 4);
                FindInnerTest("𥀀𥀁𥀂𥀃ææ𥀄x", "aeae𥀄", StringComparison.CurrentCulture, 8, 4);
            }
        }

        private static void FindInnerTest(string source, string searchValue, StringComparison comparisonType, int expectedIndex, int expectedLength)
        {
            int actualIndex, actualLength;
            bool actualResult = source.Find(searchValue, comparisonType, out actualIndex, out actualLength);

            Assert.AreEqual(expectedIndex >= 0, actualResult);
            Assert.AreEqual(expectedIndex, actualIndex);
            Assert.AreEqual(expectedLength, actualLength);
        }

        private static void FindInnerTest(string source, string searchValue, int searchIndex, StringComparison comparisonType, int expectedIndex, int expectedLength)
        {
            int actualIndex, actualLength;
            bool actualResult = source.Find(searchValue, searchIndex, comparisonType, out actualIndex, out actualLength);

            Assert.AreEqual(expectedIndex >= 0, actualResult);
            Assert.AreEqual(expectedIndex, actualIndex);
            Assert.AreEqual(expectedLength, actualLength);
        }

        private static void FindInnerTest(string source, string searchValue, int searchIndex, int searchLength, StringComparison comparisonType, int expectedIndex, int expectedLength)
        {
            int actualIndex, actualLength;
            bool actualResult = source.Find(searchValue, searchIndex, searchLength, comparisonType, out actualIndex, out actualLength);

            Assert.AreEqual(expectedIndex >= 0, actualResult);
            Assert.AreEqual(expectedIndex, actualIndex);
            Assert.AreEqual(expectedLength, actualLength);
        }

        [TestMethod]
        public void Find_RemoveFirst_Inline()
        {
            using (new CultureSwapper(PredefinedCulture.EnglishUnitedStates))
            {
                // LukeH: https://stackoverflow.com/a/2201648/1149773
                var sourceString = "Encyclopædia";
                var removeString = "aedia";
                int index, length;
                sourceString.Find(removeString, StringComparison.CurrentCulture, out index, out length);
                string cleanPath = index < 0 ? sourceString : sourceString.Remove(index, length);
                Assert.AreEqual("Encyclop", cleanPath);
            }
        }

        [TestMethod]
        public void Find_RemoveFirst_Method()
        {
            using (new CultureSwapper(PredefinedCulture.EnglishUnitedStates))
            {
                Assert.AreEqual("abe", RemoveFirst("abcde", "cd"));
                Assert.AreEqual("", RemoveFirst("ani­mal", "animal"));
                Assert.AreEqual("ct", RemoveFirst("cåt", "å"));
                Assert.AreEqual("caf", RemoveFirst("café", "é"));
                Assert.AreEqual("encyclop", RemoveFirst("encyclopædia", "aedia"));
                
                Assert.AreEqual("abe", RemoveFirst("abcde", "cd"));
                Assert.AreEqual("", RemoveFirst("ani\u00ADmal", "animal"));
                Assert.AreEqual("ct", RemoveFirst("c\u0061\u030At", "\u00E5"));
                Assert.AreEqual("caf", RemoveFirst("caf\u00E9", "\u0065\u0301"));
                Assert.AreEqual("encyclop", RemoveFirst("encyclop\u00E6dia", "\u0061\u0065dia"));
            }
        }

        private static string RemoveFirst(string source, string value)
        {
            int index, length;
            if (source.Find(value, StringComparison.CurrentCulture, out index, out length))
                return source.Remove(index, length);

            return source;
        }

        [TestMethod]
        public void RemovePrefix()
        {
            using (new CultureSwapper(PredefinedCulture.EnglishUnitedStates))
            {
                Assert.AreEqual("Controller", "HomeController".RemovePrefix("Home"));
                Assert.AreEqual("Controller", "HomeController".RemovePrefix("home", StringComparison.OrdinalIgnoreCase));

                Assert.AreEqual("HomeController", "HomeController".RemovePrefix("abc"));
                Assert.AreEqual("HomeController", "HomeController".RemovePrefix("home"));

                Assert.AreEqual("Café Bräunerhof", "Café Bräunerhof".RemovePrefix("Cafe\u0301 ", StringComparison.Ordinal));
                Assert.AreEqual("Bräunerhof", "Café Bräunerhof".RemovePrefix("Cafe\u0301 ", StringComparison.CurrentCulture));

                Assert.AreEqual("Æble", "Æble".RemovePrefix("AE", StringComparison.OrdinalIgnoreCase));
                Assert.AreEqual("le", "Æble".RemovePrefix("AEb", StringComparison.CurrentCulture));
                Assert.AreEqual("le", "Æble".RemovePrefix("aeb", StringComparison.CurrentCultureIgnoreCase));

                Assert.AreEqual("ABC", "ABC".RemovePrefix("", StringComparison.CurrentCulture));
            }
        }

        [TestMethod]
        public void RemoveSuffix()
        {
            using (new CultureSwapper(PredefinedCulture.EnglishUnitedStates))
            {
                Assert.AreEqual("Home", "HomeController".RemoveSuffix("Controller"));
                Assert.AreEqual("Home", "HomeController".RemoveSuffix("controller", StringComparison.OrdinalIgnoreCase));

                Assert.AreEqual("HomeController", "HomeController".RemoveSuffix("abc"));
                Assert.AreEqual("HomeController", "HomeController".RemoveSuffix("controller"));

                Assert.AreEqual("Café", "Café".RemoveSuffix("e\u0301", StringComparison.Ordinal));
                Assert.AreEqual("Caf", "Café".RemoveSuffix("e\u0301", StringComparison.CurrentCulture));

                Assert.AreEqual("ABC", "ABC".RemoveSuffix("", StringComparison.CurrentCulture));
            }
        }

        [TestMethod]
        public void AsString()
        {
            Assert.AreEqual("", Enumerable.Empty<char>().AsString());
            Assert.AreEqual("çä𥀀", new char[] { 'ç', 'a', '\u0308', "𥀀"[0], "𥀀"[1] }.AsString());

            var str = "Hello world! 3 + 4 = 7. End.";
            Assert.AreEqual(str, str.AsEnumerable().AsString());
            Assert.AreEqual("HelloworldEnd", str.Where(char.IsLetter).AsString());
            Assert.AreEqual("347", str.Where(char.IsDigit).AsString());
        }
    }
}