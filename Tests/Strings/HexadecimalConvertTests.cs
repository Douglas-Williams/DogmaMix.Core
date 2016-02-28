using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogmaMix.Core.Strings.Tests
{
    [TestClass]
    public class HexadecimalConvertTests
    {
        [TestMethod]
        public void ToHexadecimal_Empty()
        {
            ToHexadecimalInner("", new byte[0]);
        }

        [TestMethod]
        public void ToHexadecimal_SingleByte()
        {
            ToHexadecimalInner("00", 0x00);
            ToHexadecimalInner("0F", 0x0F);
            ToHexadecimalInner("FF", 0xFF);
        }

        [TestMethod]
        public void ToHexadecimal_MultipleBytes()
        {
            ToHexadecimalInner("29779B1A4D91BB2FD084A4", 0x29, 0x77, 0x9B, 0x1A, 0x4D, 0x91, 0xBB, 0x2F, 0xD0, 0x84, 0xA4);
        }

        [TestMethod]
        public void FromHexadecimal_Empty()
        {
            FromHexadecimalInner("", new byte[0]);
        }

        [TestMethod]
        public void FromHexadecimal_SingleByte()
        {
            FromHexadecimalInner("00", 0x00);
            FromHexadecimalInner("0F", 0x0F);
            FromHexadecimalInner("FF", 0xFF);
        }

        [TestMethod]
        public void FromHexadecimal_MultipleBytes()
        {
            FromHexadecimalInner("29779B1A4D91BB2FD084A4", 0x29, 0x77, 0x9B, 0x1A, 0x4D, 0x91, 0xBB, 0x2F, 0xD0, 0x84, 0xA4);
        }

        [TestMethod]
        public void FromHexadecimal_Lowercase()
        {
            FromHexadecimalInner("29779b1a4d91bb2fd084a4", 0x29, 0x77, 0x9B, 0x1A, 0x4D, 0x91, 0xBB, 0x2F, 0xD0, 0x84, 0xA4);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void FromHexadecimal_OddCharacterCount()
        {
            HexadecimalConvert.FromHexadecimal("29779B1A4D91BB2FD084A");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void FromHexadecimal_NonHexCharacters()
        {
            HexadecimalConvert.FromHexadecimal("29,77,9B");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void FromHexadecimal_NonHexLetterCharacters()
        {
            HexadecimalConvert.FromHexadecimal("29779B4G");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void FromHexadecimal_NonHexUnicodeCharacters()
        {
            HexadecimalConvert.FromHexadecimal("29—77—9B");
        }

        private void ToHexadecimalInner(string expected, params byte[] input)
        {
            string actual = HexadecimalConvert.ToHexadecimal(input);
            Assert.AreEqual(expected, actual);
        }

        private void FromHexadecimalInner(string input, params byte[] expected)
        {
            byte[] actual = HexadecimalConvert.FromHexadecimal(input);
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}