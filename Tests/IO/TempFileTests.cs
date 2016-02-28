using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogmaMix.Core.IO.Tests
{
    [TestClass]
    public class TempFileTests
    {
        [TestMethod]
        public void UsingTempFile_Create()
        {
            string filePath = null;

            using (var tempFile = new TempFile(create: true))
            {
                filePath = tempFile.FilePath;
                VerifyFileIsInTempDirectory(filePath);
                Assert.IsTrue(File.Exists(filePath), "Temporary file was not created.");
                File.WriteAllText(filePath, "Test");
            }
            Assert.IsFalse(File.Exists(filePath), "Temporary file was not deleted.");

            using (var tempFile = new TempFile(create: true, extension: ".txt"))
            {
                filePath = tempFile.FilePath;
                VerifyFileIsInTempDirectory(filePath);
                Assert.AreEqual(".txt", Path.GetExtension(filePath));
                Assert.IsTrue(File.Exists(filePath), "Temporary file was not created.");
                File.WriteAllText(filePath, "Test");
            }
            Assert.IsFalse(File.Exists(filePath), "Temporary file was not deleted.");
        }

        [TestMethod]
        public void UsingTempFile_NoCreate()
        {
            string filePath = null;

            using (var tempFile = new TempFile(create: false))
            {
                filePath = tempFile.FilePath;
                VerifyFileIsInTempDirectory(filePath);
                Assert.IsFalse(File.Exists(filePath), "Temporary file was created.");
                File.WriteAllText(filePath, "Test");
            }
            Assert.IsFalse(File.Exists(filePath), "Temporary file was not deleted.");

            using (var tempFile = new TempFile(create: false, extension: ".txt"))
            {
                filePath = tempFile.FilePath;
                VerifyFileIsInTempDirectory(filePath);
                Assert.AreEqual(".txt", Path.GetExtension(filePath));
                Assert.IsFalse(File.Exists(filePath), "Temporary file was created.");
                File.WriteAllText(filePath, "Test");
            }
            Assert.IsFalse(File.Exists(filePath), "Temporary file was not deleted.");
        }

        [TestMethod]
        public void UsingTempFile_Extensions()
        {
            using (var tempFile = new TempFile(create: false))
                Assert.AreEqual(".tmp", Path.GetExtension(tempFile.FilePath));

            using (var tempFile = new TempFile(create: false, extension: null))
                Assert.AreEqual("", Path.GetExtension(tempFile.FilePath));

            using (var tempFile = new TempFile(create: false, extension: "abc"))
                Assert.AreEqual(".abc", Path.GetExtension(tempFile.FilePath));

            using (var tempFile = new TempFile(create: false, extension: ".abc"))
                Assert.AreEqual(".abc", Path.GetExtension(tempFile.FilePath));

            using (var tempFile = new TempFile(create: false, extension: ".helloworld"))
                Assert.AreEqual(".helloworld", Path.GetExtension(tempFile.FilePath));            
        }

        [TestMethod]
        public void UsingTempFile_Multiple()
        {
            var filePaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            UsingMultipleTemporaryFiles(1024, false, filePaths);

            filePaths.Clear();
            UsingMultipleTemporaryFiles(128, true, filePaths);
            foreach (var filePath in filePaths)
                Assert.IsFalse(File.Exists(filePath), "Temporary file was not deleted.");
        }

        // define recursive function to create new files whilst former ones are still being used
        private static void UsingMultipleTemporaryFiles(int count, bool create, ISet<string> filePaths = null)
        {
            // last recursion step
            if (count == 0)
                return;

            using (var tempFile = new TempFile(create: create))
            {
                if (create)
                    Assert.IsTrue(File.Exists(tempFile.FilePath), "Temporary file was not created.");

                bool added = filePaths.Add(tempFile.FilePath);
                Assert.IsTrue(added, "File path was already assigned to another temporary file.");

                // recursive call
                UsingMultipleTemporaryFiles(count - 1, create, filePaths);
            }
        }

        private static void VerifyFileIsInTempDirectory(string filePath)
        {
            string tempPath = Path.GetTempPath().TrimEnd('\\', '/');
            string parentDirPath = Path.GetDirectoryName(filePath).TrimEnd('\\', '/');
            Assert.AreEqual(tempPath, parentDirPath, "Temporary file is not located in user's temporary folder.");
        }
    }
}