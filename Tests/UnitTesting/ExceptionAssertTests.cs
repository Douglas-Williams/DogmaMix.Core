using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogmaMix.Core.UnitTesting.Tests
{
    [TestClass]
    public class ExceptionAssertTests
    {
        [TestMethod]
        public void Throws_Join()
        {
            var people = new[] 
            {
                new Person { Name = "Ben", City = "London" },
                new Person { Name = "Erik", City = "New York" },
                new Person { Name = "Gary", City = "Paris" },
            };

            var pets = new[]
            {
                new Pet { Name = "Sophie", Owner = people.Single(p => p.Name == "Ben") },
                new Pet { Name = "Max", Owner = people.Single(p => p.Name == "Gary") },
                new Pet { Name = "Fido", Owner = people.Single(p => p.Name == "Ben") },
            };

            var e1 = ExceptionAssert.Throws<ArgumentNullException>(() => Enumerable.Join((Person[])null, pets, person => person, pet => pet.Owner, (person, pet) => new { OwnerName = person.Name, Pet = pet.Name }));
            var e2 = ExceptionAssert.Throws<ArgumentNullException>(() => people.Join((Pet[])null, person => person, pet => pet.Owner, (person, pet) => new { OwnerName = person.Name, Pet = pet.Name }));
            var e3 = ExceptionAssert.Throws<ArgumentNullException>(() => people.Join(pets, null, pet => pet.Owner, (person, pet) => new { OwnerName = person.Name, Pet = pet.Name }));
            var e4 = ExceptionAssert.Throws<ArgumentNullException>(() => people.Join(pets, person => person, null, (person, pet) => new { OwnerName = person.Name, Pet = pet.Name }));
            var e5 = ExceptionAssert.Throws<ArgumentNullException>(() => people.Join(pets, person => person, pet => pet.Owner, (Func<Person,Pet,string>)null));

            Assert.IsInstanceOfType(e1, typeof(ArgumentNullException));
            Assert.IsInstanceOfType(e2, typeof(ArgumentNullException));
            Assert.IsInstanceOfType(e3, typeof(ArgumentNullException));
            Assert.IsInstanceOfType(e4, typeof(ArgumentNullException));
            Assert.IsInstanceOfType(e5, typeof(ArgumentNullException));
        }

        [TestMethod]
        public void Throws_Types()
        {
            var e1 = ExceptionAssert.Throws(() => { throw new ArgumentOutOfRangeException(); });
            var e2 = ExceptionAssert.Throws<ArgumentException>(() => { throw new ArgumentOutOfRangeException(); });
            var e3 = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() => { throw new ArgumentOutOfRangeException(); });

            Assert.IsInstanceOfType(e1, typeof(ArgumentOutOfRangeException));
            Assert.IsInstanceOfType(e2, typeof(ArgumentOutOfRangeException));
            Assert.IsInstanceOfType(e3, typeof(ArgumentOutOfRangeException));
        }

        [TestMethod]
        public void Throws_NoException()
        {
            try
            {
                ExceptionAssert.Throws<ArgumentNullException>(() => { });
            }
            catch (AssertFailedException)
            {
                return;
            }

            Assert.Fail("Expected assertion failure.");
        }

        [TestMethod]
        public void Throws_WrongType()
        {
            try
            {
                ExceptionAssert.Throws<ArgumentNullException>(() => { throw new ArgumentOutOfRangeException(); });
            }
            catch (AssertFailedException)
            {
                return;
            }

            Assert.Fail("Expected assertion failure.");
        }

        [TestMethod]
        public async Task ThrowsAsync_Types()
        {
#pragma warning disable 1998
            var e1 = await ExceptionAssert.ThrowsAsync(async () => { throw new ArgumentOutOfRangeException(); });
            var e2 = await ExceptionAssert.ThrowsAsync<ArgumentException>(async () => { throw new ArgumentOutOfRangeException(); });
            var e3 = await ExceptionAssert.ThrowsAsync<ArgumentOutOfRangeException>(async () => { throw new ArgumentOutOfRangeException(); });
#pragma warning restore 1998

            Assert.IsInstanceOfType(e1, typeof(ArgumentOutOfRangeException));
            Assert.IsInstanceOfType(e2, typeof(ArgumentOutOfRangeException));
            Assert.IsInstanceOfType(e3, typeof(ArgumentOutOfRangeException));
        }

        [TestMethod]
        public async Task ThrowsAsync_NoException()
        {
            try
            {
                await ExceptionAssert.ThrowsAsync<ArgumentNullException>(() => Task.CompletedTask);
            }
            catch (AssertFailedException)
            {
                return;
            }

            Assert.Fail("Expected assertion failure.");
        }

        [TestMethod]
        public async Task ThrowsAsync_WrongType()
        {
            try
            {
#pragma warning disable 1998
                await ExceptionAssert.ThrowsAsync<ArgumentNullException>(async () => { throw new ArgumentOutOfRangeException(); });
#pragma warning restore 1998
            }
            catch (AssertFailedException)
            {
                return;
            }

            Assert.Fail("Expected assertion failure.");
        }

        [TestMethod]
        public async Task ThrowsAsync_MemoryStream()
        {
            var e = await ExceptionAssert.ThrowsAsync<ArgumentException>(async () => 
            {
                using (var stream = new MemoryStream())
                {
                    var buffer = new byte[128];
                    await stream.ReadAsync(buffer, 0, 256);
                }
            });

            Assert.IsInstanceOfType(e, typeof(ArgumentException));
        }

        [TestMethod]
        public void ThrowsAssertFailed()
        {
            var e1 = ExceptionAssert.ThrowsAssertFailed(() => Assert.Fail());
            var e2 = ExceptionAssert.ThrowsAssertFailed(() => Assert.AreEqual(16, 32));

            Assert.IsInstanceOfType(e1, typeof(AssertFailedException));
            Assert.IsInstanceOfType(e2, typeof(AssertFailedException));
        }        

        private class Person
        {
            public string Name { get; set; }
            public string City { get; set; }
        }

        private class Pet
        {
            public string Name { get; set; }
            public Person Owner { get; set; }
        }
    }
}
