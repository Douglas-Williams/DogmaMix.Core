using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogmaMix.Core.UnitTesting.Tests
{
    [TestClass]
    public class EqualityAssertTests
    {
        [TestMethod]
        public void Equals()
        {
            EqualityAssert.Equals(4, 4);
            EqualityAssert.Equals("B", "B");

            EqualityAssert.Equals("B", "b", StringComparer.OrdinalIgnoreCase,
                "\"B\" should be equal to \"b\" for case-insensitive ordinal comparison.");

            ExceptionAssert.ThrowsAssertFailed(
                () => EqualityAssert.Equals("B", "A"),
                "\"B\" should not be equal to \"A\".");

            ExceptionAssert.ThrowsAssertFailed(
                () => EqualityAssert.Equals("B", "b", StringComparer.Ordinal),
                "\"B\" should not be equal to \"b\" for case-sensitive ordinal comparison.");

            EqualityAssert.Equals<string>(null, null);

            ExceptionAssert.ThrowsAssertFailed(
                () => EqualityAssert.Equals(null, "x"),
                "(null) should not be equal to \"x\".");

            ExceptionAssert.ThrowsAssertFailed(
                () => EqualityAssert.Equals("x", null),
                "\"x\" should not be equal to (null).");
        }

        [TestMethod]
        public void IsNotEqualTo()
        {
            EqualityAssert.NotEquals(4, 5);
            EqualityAssert.NotEquals("B", "A");

            EqualityAssert.NotEquals("B", "b", StringComparer.Ordinal,
                "\"B\" should not be equal to \"b\" for case-sensitive ordinal comparison.");

            ExceptionAssert.ThrowsAssertFailed(
                () => EqualityAssert.NotEquals("B", "B"),
                "\"B\" should not be not equal to \"B\".");

            ExceptionAssert.ThrowsAssertFailed(
                () => EqualityAssert.NotEquals("B", "b", StringComparer.OrdinalIgnoreCase),
                "\"B\" should not be not equal to \"b\" for case-insensitive ordinal comparison.");

            EqualityAssert.NotEquals(null, "x");
            EqualityAssert.NotEquals("x", null);

            ExceptionAssert.ThrowsAssertFailed(
                () => EqualityAssert.NotEquals<string>(null, null),
                "(null) should not be not equal to (null).");            
        }

        [TestMethod]
        public void Equals_BuggyComparer()
        {
            ExceptionAssert.ThrowsAssertFailed(
                () => EqualityAssert.Equals("A", "a", new BuggyComparer()),
                "Hash codes of equal objects must match.");
        }

        private class BuggyComparer : EqualityComparer<string>
        {
            private int counter = 1;

            public override bool Equals(string x, string y)
            {
                return true;
            }

            public override int GetHashCode(string obj)
            {
                return Interlocked.Increment(ref counter);
            }
        }
    }
}
