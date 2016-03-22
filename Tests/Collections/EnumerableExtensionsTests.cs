using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.Collections;
using DogmaMix.Core.Randomization;
using DogmaMix.Core.UnitTesting;

namespace DogmaMix.Core.Extensions.Tests
{
    [TestClass]
    public class EnumerableExtensionsTests
    {
        private static readonly char[] sampleChars = new[] { 'h', '…', '5', 'G', '8', 'E', 'w', 'ü', 'h', 'A' };
        private static readonly string[] sampleStrings = new[] { "foo", "BAR", "Hello", "World" };

        [TestMethod]
        public void IndexOf_Found()
        {
            Assert.AreEqual(0, sampleChars.IndexOf('h'));
            Assert.AreEqual(2, sampleChars.IndexOf('5'));
            Assert.AreEqual(9, sampleChars.IndexOf('A'));

            Assert.AreEqual(0, sampleChars.IndexOf(char.IsLower));
            Assert.AreEqual(2, sampleChars.IndexOf(c => c >= '0' && c <= '9'));
            Assert.AreEqual(9, sampleChars.IndexOf(c => char.ToLower(c) == 'a'));

            Assert.AreEqual(1, sampleStrings.IndexOf("BAR"));
            Assert.AreEqual(1, sampleStrings.IndexOf("bar", StringComparer.OrdinalIgnoreCase));
            Assert.AreEqual(3, sampleStrings.IndexOf("WORLD", StringComparer.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void IndexOf_NotFound()
        {
            Assert.AreEqual(-1, sampleChars.IndexOf('B'));
            Assert.AreEqual(-1, sampleChars.IndexOf(char.IsSurrogate));

            Assert.AreEqual(-1, sampleStrings.IndexOf("bar"));
            Assert.AreEqual(-1, sampleStrings.IndexOf("world", StringComparer.InvariantCulture));
        }

        [TestMethod]
        public void IndexOf_Empty()
        {
            var empty = Enumerable.Empty<char>();
            Assert.AreEqual(-1, empty.IndexOf('B'));
            Assert.AreEqual(-1, empty.IndexOf(char.IsLetter));
        }

        [TestMethod]
        public void IndexesOf_Found()
        {
            EnumerableAssert.AreEqual(new[] { 0, 8 }, sampleChars.IndexesOf('h'));
            EnumerableAssert.AreEqual(new[] { 0, 6, 7, 8 }, sampleChars.IndexesOf(char.IsLower));

            EnumerableAssert.AreEqual(new[] { 1 }, sampleStrings.IndexesOf("bar", StringComparer.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void IndexesOf_NotFound()
        {
            EnumerableAssert.AreEqual(new int[0], sampleChars.IndexesOf('B'));
            EnumerableAssert.AreEqual(new int[0], sampleChars.IndexesOf(char.IsSurrogate));

            EnumerableAssert.AreEqual(new int[0], sampleStrings.IndexesOf("bar"));
            EnumerableAssert.AreEqual(new int[0], sampleStrings.IndexesOf("world", StringComparer.InvariantCulture));
        }

        [TestMethod]
        public void Append()
        {
            EnumerableAssert.AreEqual(new[] { 'Z' }, Enumerable.Empty<char>().Append('Z'));
            EnumerableAssert.AreEqual(sampleChars.Concat(new[] { 'Z' }), sampleChars.Append('Z'));
        }

        [TestMethod]
        public void Prepend()
        {
            EnumerableAssert.AreEqual(new[] { 'A' }, Enumerable.Empty<char>().Prepend('A'));
            EnumerableAssert.AreEqual((new[] { 'A' }).Concat(sampleChars), sampleChars.Prepend('A'));
        }

        [TestMethod]
        public void AsArray()
        {
            var bytes = CachedRandom.Current.NextBytes(1024);
            Assert.AreSame(bytes, bytes.AsArray());

            var sequence = bytes.Where(b => b > 127);
            EnumerableAssert.AreEqual(sequence, sequence.AsArray());
        }
        
        [TestMethod]
        public void TryFastCountTest()
        {
            int count;

            Assert.IsTrue(new[] { 'a', 'b', 'c' }.TryFastCount(out count));
            Assert.AreEqual(3, count);

            Assert.IsTrue("qwerty".TryFastCount(out count));
            Assert.AreEqual(6, count);

            Assert.IsTrue(new List<Version>().TryFastCount(out count));
            Assert.AreEqual(0, count);

            Assert.IsFalse(EnumerableUtility.Yield(3, 5, 9).TryFastCount(out count));
            Assert.AreEqual(-1, count);

            Assert.IsFalse(EnumerateFail<int>().TryFastCount(out count));
            Assert.AreEqual(-1, count);
        }

        [TestMethod]
        public void FastCountTest()
        {
            Assert.AreEqual(3, new[] { 'a', 'b', 'c' }.FastCount());
            Assert.AreEqual(6, "qwerty".FastCount());
            Assert.AreEqual(0, new List<Version>().FastCount());

            Assert.AreEqual(null, EnumerableUtility.Yield(3, 5, 9).FastCount());
            Assert.AreEqual(null, EnumerateFail<int>().FastCount());
        }

        private IEnumerable<T> EnumerateFail<T>()
        {
            Assert.Fail("The sequence was enumerated.");
            yield break;   // never reached
        }
    }
}