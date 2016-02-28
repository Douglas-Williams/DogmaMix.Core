using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.UnitTesting;

namespace DogmaMix.Core.IO.Tests
{
    [TestClass]
    public class FileOperationTests
    {
        [TestMethod]
        public void CreateEmpty()
        {
            string path = Path.GetTempFileName();
            File.WriteAllText(path, "Hello world!");
            FileOperation.CreateEmpty(path);
            EnumerableAssert.IsEmpty(File.ReadAllBytes(path), "File was not overwritten as empty.");

            File.Delete(path);
            FileOperation.CreateEmpty(path);
            Assert.IsTrue(File.Exists(path), "File was not created as empty.");
            EnumerableAssert.IsEmpty(File.ReadAllBytes(path), "File was not created as empty.");
        }
    }
}