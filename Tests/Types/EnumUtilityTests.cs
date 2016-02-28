using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.UnitTesting;

namespace DogmaMix.Core.Types.Tests
{
    [TestClass]
    public class EnumUtilityTests
    {
        [TestMethod]
        public void GetValuesTest()
        {
            EnumerableAssert.AreEqual(Enum.GetValues(typeof(FileOptions)).Cast<FileOptions>(), EnumUtility.GetValues<FileOptions>());
        }
    }
}