using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.Globalization;

namespace DogmaMix.Core.Comparers.Tests
{
    [TestClass]
    public class HashCodeCombinerTests
    {
        [TestMethod]
        public void Combine()
        {
            var a = new[] { 55, 7, 34, 87, 24, 23, 76 };

            Assert.AreEqual(
                HashCodeCombiner.Combine(a),
                HashCodeCombiner.Combine(a.ToList()));

            using (new CultureSwapper(PredefinedCulture.EnglishUnitedStates))
            {
                var x = new[] { "Madrid", "Athens", "Paris" };
                var y = new[] { "Madrid", "ATHENS", "Paris" };

                Assert.AreEqual(
                    HashCodeCombiner.Combine(x, StringComparer.CurrentCultureIgnoreCase),
                    HashCodeCombiner.Combine(y, StringComparer.CurrentCultureIgnoreCase));
            }
        }
    }
}
