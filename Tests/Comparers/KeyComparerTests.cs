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
                var comparer = KeyComparer.Create((City city) => city.Name);
                CompareAssert.IsLessThan(añasco, athens, comparer, message);
                CompareAssert.IsEqualTo(añasco, añasco, comparer);
                CompareAssert.IsGreaterThan(athens, añasco, comparer, message);

                message = "'ñ' should greater less than 't' for ordinal comparison.";
                comparer = KeyComparer.Create((City city) => city.Name, StringComparer.Ordinal);
                CompareAssert.IsGreaterThan(añasco, athens, comparer, message);
                CompareAssert.IsEqualTo(añasco, añasco, comparer);
                CompareAssert.IsLessThan(athens, añasco, comparer, message);
            }
        }

        [TestMethod]
        public void Compare_Nulls()
        {
            var comparer = KeyComparer.Create((City city) => city.Name);
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

                var sortedSet = new SortedSet<City>(cities, KeyComparer.Create((City city) => city.Name));
                EnumerableAssert.AreEqual(new[] { añasco, athens, madrid, newYork, paris }, sortedSet);

                sortedSet = new SortedSet<City>(cities, KeyComparer.Create((City city) => city.Name, StringComparer.Ordinal));
                EnumerableAssert.AreEqual(new[] { athens, añasco, madrid, newYork, paris }, sortedSet);
            }

            int[] nums = new[] { 47, -32, -54, 18, 62, -71, 58 };
            var byAbs = new SortedSet<int>(nums, KeyComparer.Create((int n) => Math.Abs(n)));
            EnumerableAssert.AreEqual(new [] { 18, -32, 47, -54, 58, 62, -71 }, byAbs);
        }

        [TestMethod]
        public void Person_SortedSet()
        {
            var persons = new[]
            {
                new Person { Ssn = "324-00-3015", DateOfBirth = new DateTime(1970, 12, 17), FirstName = "Tod",     LastName = "Temme" },
                new Person { Ssn = "548-00-1592", DateOfBirth = new DateTime(1968, 03, 13), FirstName = "Lucia",   LastName = "Armstrong" },
                new Person { Ssn = "129-00-7416", DateOfBirth = new DateTime(1982, 09, 02), FirstName = "Spencer", LastName = "Weaver" },
                new Person { Ssn = "831-00-6391", DateOfBirth = new DateTime(1974, 04, 30), FirstName = "Celia",   LastName = "Potter" },
                new Person { Ssn = "714-00-6502", DateOfBirth = new DateTime(1966, 11, 19), FirstName = "Powell",  LastName = "Beck" },
            };

            var bySsn = new SortedSet<Person>(persons, KeyComparer.Create(
                (Person p) => p.Ssn));

            // Not suitable for real-world scenarios, since SortedSet<T> does not allow duplicates.
            var byDateOfBirth = new SortedSet<Person>(persons, KeyComparer.Create(
                (Person p) => p.DateOfBirth));
            var byFullName = new SortedSet<Person>(persons, KeyComparer.Create(
                (Person p) => $"{p.FirstName} {p.LastName}", StringComparer.Ordinal));

            EnumerableAssert.AreEqual(new[] { "Spencer", "Tod", "Lucia", "Powell", "Celia" }, bySsn.Select(p => p.FirstName));
            EnumerableAssert.AreEqual(new[] { "Powell", "Lucia", "Tod", "Celia", "Spencer" }, byDateOfBirth.Select(p => p.FirstName));
            EnumerableAssert.AreEqual(new[] { "Celia", "Lucia", "Powell", "Spencer", "Tod" }, byFullName.Select(p => p.FirstName));
            
            var person = new Person { Ssn = "301-00-1582", DateOfBirth = new DateTime(1984, 11, 01), FirstName = "Teddy", LastName = "Wake" };
            bySsn.Add(person);
            EnumerableAssert.AreEqual(new[] { "Spencer", "Teddy", "Tod", "Lucia", "Powell", "Celia" }, bySsn.Select(p => p.FirstName));
        }

        [TestMethod]
        public void Nums_Abs()
        {
            var nums = new[] { 47, -32, -54, 18, 62, -71, 58 };
            var byAbs = new SortedSet<int>(nums, KeyComparer.Create((int n) => Math.Abs(n)));
            EnumerableAssert.AreEqual(new[] { 18, -32, 47, -54, 58, 62, -71 }, byAbs);
        }
        
        private partial class Person
        {
            public string Ssn { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}
