using System;
using System.Globalization;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogmaMix.Core.Globalization.Tests
{
    [TestClass]
    public class CultureSwapperTests
    {
        [TestMethod]
        public void CultureSwapper()
        {
            var formerCulture = CultureInfo.CurrentCulture;

            using (new CultureSwapper(PredefinedCulture.FrenchFrance))
            {
                Assert.AreEqual(PredefinedCulture.FrenchFrance.Name, CultureInfo.CurrentCulture.Name);

                using (new CultureSwapper(PredefinedCulture.GermanAustria))
                {
                    Assert.AreEqual(PredefinedCulture.GermanAustria.Name, CultureInfo.CurrentCulture.Name);

                    using (new CultureSwapper(PredefinedCulture.JapaneseJapan))
                    {
                        Assert.AreEqual(PredefinedCulture.JapaneseJapan.Name, CultureInfo.CurrentCulture.Name);
                    }

                    Assert.AreEqual(PredefinedCulture.GermanAustria.Name, CultureInfo.CurrentCulture.Name);
                }

                Assert.AreEqual(PredefinedCulture.FrenchFrance.Name, CultureInfo.CurrentCulture.Name);
            }
            
            Assert.AreEqual(formerCulture.Name, CultureInfo.CurrentCulture.Name);
        }
    }
}
