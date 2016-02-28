using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.Globalization;
using DogmaMix.Core.UnitTesting;

namespace DogmaMix.Core.Comparers.Tests
{
    [TestClass]
    public class KeyEqualityComparerTests
    {
        private class City
        {
            public string Name { get; set; }
            public override string ToString() => Name;
        }

        private static readonly City athens = new City { Name = "Athens" };
        private static readonly City añasco = new City { Name = "Añasco" };
        private static readonly City añascoUppercase = new City { Name = "AÑASCO" };
        private static readonly City añascoCombining = new City { Name = "An\u0303asco" };
        
        [TestMethod]
        public void Equals()
        {
            using (new CultureSwapper(PredefinedCulture.EnglishUnitedStates))
            {
                var comparer = new KeyEqualityComparer<City, string>(city => city.Name);
                EqualityAssert.Equals(añasco, añasco, comparer);
                EqualityAssert.NotEquals(añasco, añascoUppercase, comparer);
                EqualityAssert.NotEquals(añasco, añascoCombining, comparer,
                    "Precomposed character should not equal combining character sequence for default equality comparison (ordinal).");

                comparer = new KeyEqualityComparer<City, string>(city => city.Name, StringComparer.CurrentCulture);
                EqualityAssert.Equals(añasco, añasco, comparer);
                EqualityAssert.NotEquals(añasco, añascoUppercase, comparer);
                EqualityAssert.Equals(añasco, añascoCombining, comparer,
                    "Precomposed character should equal combining character sequence for culture-sensitive comparison (en-US).");

                comparer = new KeyEqualityComparer<City, string>(city => city.Name, StringComparer.CurrentCultureIgnoreCase);
                EqualityAssert.Equals(añasco, añasco, comparer);
                EqualityAssert.Equals(añasco, añascoUppercase, comparer);
                EqualityAssert.Equals(añasco, añascoCombining, comparer);
            }
        }

        [TestMethod]
        public void Equals_Nulls()
        {
            var comparer = new KeyEqualityComparer<City, string>(city => city.Name);
            EqualityAssert.NotEquals(null, athens, comparer);
            EqualityAssert.Equals(null, null, comparer);
            EqualityAssert.NotEquals(athens, null, comparer);
        }
        
        [TestMethod]
        public new void GetHashCode()
        {
            using (new CultureSwapper(PredefinedCulture.EnglishUnitedStates))
            {
                var comparer = new KeyEqualityComparer<City, string>(city => city.Name, StringComparer.CurrentCultureIgnoreCase);
                var añascoHash = comparer.GetHashCode(añasco);
                Assert.AreEqual(añascoHash, comparer.GetHashCode(añascoUppercase));
                Assert.AreEqual(añascoHash, comparer.GetHashCode(añascoCombining));
            }
        }
    }
}
