using DogmaMix.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.Types;
using DogmaMix.Core.UnitTesting;

namespace DogmaMix.Core.Tests
{
    [TestClass]
    public class ArgumentValidateTests
    {
        [TestMethod]
        public void NotNull()
        {
            string str = "Test";
            ArgumentValidate.NotNull(str, nameof(str));

            str = null;
            var exception = ExceptionAssert.Throws<ArgumentNullException>(() =>
                ArgumentValidate.NotNull(str, nameof(str)));
            Assert.AreEqual(nameof(str), exception.ParamName);

            int? num = 44;
            ArgumentValidate.NotNull(num, nameof(num));

            num = null;
            exception = ExceptionAssert.Throws<ArgumentNullException>(() =>
                ArgumentValidate.NotNull(num, nameof(num)));
            Assert.AreEqual(nameof(num), exception.ParamName);
        }

        [TestMethod]
        public void EnumDefined()
        {
            foreach (var comparison in EnumUtility.GetValues<StringComparison>())
                ArgumentValidate.EnumDefined(comparison, nameof(comparison));

            var invalidComparison = (StringComparison)int.MaxValue;
            var exception = ExceptionAssert.Throws<ArgumentException>(() =>
                ArgumentValidate.EnumDefined(invalidComparison, nameof(invalidComparison)));
            Assert.AreEqual(nameof(invalidComparison), exception.ParamName);
        }

        [TestMethod]
        public void StringIndex()
        {
            string str;
            int idx;

            ArgumentValidate.StringIndex("", nameof(str), 0, nameof(idx));
            ArgumentValidate.StringIndex("abc", nameof(str), 0, nameof(idx));
            ArgumentValidate.StringIndex("abc", nameof(str), 1, nameof(idx));
            ArgumentValidate.StringIndex("abc", nameof(str), 2, nameof(idx));
            ArgumentValidate.StringIndex("abc", nameof(str), 3, nameof(idx));

            StringIndexFail<ArgumentNullException>(null, 0, strFail: true);
            StringIndexFail<ArgumentOutOfRangeException>("", 1, idxFail: true);
            StringIndexFail<ArgumentOutOfRangeException>("abc", 4, idxFail: true);
        }

        [TestMethod]
        public void StringIndexLength()
        {
            string str;
            int idx;
            int len;

            ArgumentValidate.StringIndexLength("", nameof(str), 0, nameof(idx), 0, nameof(len));
            ArgumentValidate.StringIndexLength("abc", nameof(str), 0, nameof(idx), 0, nameof(len));
            ArgumentValidate.StringIndexLength("abc", nameof(str), 0, nameof(idx), 2, nameof(len));
            ArgumentValidate.StringIndexLength("abc", nameof(str), 0, nameof(idx), 3, nameof(len));
            ArgumentValidate.StringIndexLength("abc", nameof(str), 1, nameof(idx), 2, nameof(len));
            ArgumentValidate.StringIndexLength("abc", nameof(str), 2, nameof(idx), 1, nameof(len));
            ArgumentValidate.StringIndexLength("abc", nameof(str), 3, nameof(idx), 0, nameof(len));

            StringIndexLengthFail<ArgumentNullException>(null, 0, 0, strFail: true);
            StringIndexLengthFail<ArgumentOutOfRangeException>("abc", 4, 0, idxFail: true);
            StringIndexLengthFail<ArgumentOutOfRangeException>("abc", 0, 4, lenFail: true);
            StringIndexLengthFail<ArgumentOutOfRangeException>("abc", 2, 2, lenFail: true);
            StringIndexLengthFail<ArgumentOutOfRangeException>("", 1, 0, idxFail: true);
            StringIndexLengthFail<ArgumentOutOfRangeException>("", 0, 1, lenFail: true);
        }

        private static void StringIndexFail<TException>(
            string str, int idx,
            bool strFail = false, bool idxFail = false)
            where TException : ArgumentException
        {
            var exception = ExceptionAssert.Throws<TException>(() =>
                ArgumentValidate.StringIndex(str, nameof(str), idx, nameof(idx)));

            string failParamName =
                strFail ? nameof(str) :
                idxFail ? nameof(idx) : null;

            Assert.AreEqual(failParamName, exception.ParamName);
        }

        private static void StringIndexLengthFail<TException>(
            string str, int idx, int len,
            bool strFail = false, bool idxFail = false, bool lenFail = false)
            where TException : ArgumentException
        {
            var exception = ExceptionAssert.Throws<TException>(() =>
                ArgumentValidate.StringIndexLength(str, nameof(str), idx, nameof(idx), len, nameof(len)));

            string failParamName =
                strFail ? nameof(str) :
                idxFail ? nameof(idx) :
                lenFail ? nameof(len) : null;

            Assert.AreEqual(failParamName, exception.ParamName);
        }
    }
}