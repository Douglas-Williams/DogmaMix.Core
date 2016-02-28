using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.Globalization;
using DogmaMix.Core.UnitTesting;

namespace DogmaMix.Core.Comparers.Tests
{
    [TestClass]
    public class KeyComparerTests
    {
        private class City
        {
            public string Name { get; set; }
        }

        private static readonly City athens = new City { Name = "Athens" };
        private static readonly City añasco = new City { Name = "Añasco" };
        private static readonly City paris = new City { Name = "Paris" };
        private static readonly City madrid = new City { Name = "Madrid" };
        private static readonly City newYork = new City { Name = "New York" };

        [TestMethod]
        public void Compare()
        {
            using (new CultureSwapper(PredefinedCulture.EnglishUnitedStates))
            {
                var message = "'ñ' should be less than 't' for default comparison (current culture, en-US).";
                var comparer = new KeyComparer<City, string>(city => city.Name);
                CompareAssert.IsLessThan(añasco, athens, comparer, message);
                CompareAssert.IsEqualTo(añasco, añasco, comparer);
                CompareAssert.IsGreaterThan(athens, añasco, comparer, message);

                message = "'ñ' should greater less than 't' for ordinal comparison.";
                comparer = new KeyComparer<City, string>(city => city.Name, StringComparer.Ordinal);
                CompareAssert.IsGreaterThan(añasco, athens, comparer, message);
                CompareAssert.IsEqualTo(añasco, añasco, comparer);
                CompareAssert.IsLessThan(athens, añasco, comparer, message);
            }
        }

        [TestMethod]
        public void Compare_Nulls()
        {
            var comparer = new KeyComparer<City, string>(city => city.Name);
            CompareAssert.IsLessThan(null, athens, comparer);
            CompareAssert.IsEqualTo(null, null, comparer);
            CompareAssert.IsGreaterThan(athens, null, comparer);
        }

        [TestMethod]
        public void Sorting()
        {
            using (new CultureSwapper(PredefinedCulture.EnglishUnitedStates))
            {
                var cities = new[] { paris, newYork, athens, madrid, añasco };

                var sortedSet = new SortedSet<City>(cities, new KeyComparer<City, string>(city => city.Name));
                EnumerableAssert.AreEqual(new[] { añasco, athens, madrid, newYork, paris }, sortedSet);

                sortedSet = new SortedSet<City>(cities, new KeyComparer<City, string>(city => city.Name, StringComparer.Ordinal));
                EnumerableAssert.AreEqual(new[] { athens, añasco, madrid, newYork, paris }, sortedSet);
            }
        }
    }
}
