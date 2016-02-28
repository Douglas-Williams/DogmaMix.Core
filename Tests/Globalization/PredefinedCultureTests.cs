using System;
using System.Globalization;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogmaMix.Core.Globalization.Tests
{
    [TestClass]
    public class PredefinedCultureTests
    {
        [TestMethod]
        public void Cultures()
        {
            Assert.AreEqual("en-US", PredefinedCulture.EnglishUnitedStates.Name);
            Assert.AreEqual("en-GB", PredefinedCulture.EnglishUnitedKingdom.Name);
            Assert.AreEqual("en", PredefinedCulture.English.Name);
            Assert.AreEqual("de-AT", PredefinedCulture.GermanAustria.Name);
            Assert.AreEqual("fr-CA", PredefinedCulture.FrenchCanada.Name);
            Assert.AreEqual("es", PredefinedCulture.Spanish.Name);
        }
    }
}
