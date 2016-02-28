using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogmaMix.Core.UnitTesting.Tests
{
    [TestClass]
    public class CompareAssertTests
    {
        [TestMethod]
        public void IsLessThan()
        {
            CompareAssert.IsLessThan(4, 7);

            CompareAssert.IsLessThan("B", "a", StringComparer.Ordinal,
                "\"B\" should be less than \"a\" for case-sensitive ordinal comparison.");

            ExceptionAssert.ThrowsAssertFailed(
                () => CompareAssert.IsLessThan("B", "a", StringComparer.OrdinalIgnoreCase),
                "\"B\" should not be less than \"a\" for case-insensitive ordinal comparison.");

            ExceptionAssert.ThrowsAssertFailed(
                () => CompareAssert.IsLessThan("B", "b", StringComparer.OrdinalIgnoreCase),
                "\"B\" should not be less than \"b\" for case-insensitive ordinal comparison.");

            CompareAssert.IsLessThan(null, "x");

            ExceptionAssert.ThrowsAssertFailed(
                () => CompareAssert.IsLessThan("x", null),
                "\"x\" should not be less than (null).");

            ExceptionAssert.ThrowsAssertFailed(
                () => CompareAssert.IsLessThan<string>(null, null),
                "(null) should not be less than (null).");
        }

        [TestMethod]
        public void IsLessThanOrEqualTo()
        {
            CompareAssert.IsLessThanOrEqualTo(4, 7);
            CompareAssert.IsLessThanOrEqualTo(4, 4);

            CompareAssert.IsLessThanOrEqualTo("B", "a", StringComparer.Ordinal,
                "\"B\" should be less than or equal to \"a\" for case-sensitive ordinal comparison.");

            CompareAssert.IsLessThanOrEqualTo("B", "b", StringComparer.OrdinalIgnoreCase,
                "\"B\" should be less than or equal to \"b\" for case-insensitive ordinal comparison.");

            ExceptionAssert.ThrowsAssertFailed(
                () => CompareAssert.IsLessThanOrEqualTo("B", "a", StringComparer.OrdinalIgnoreCase),
                "\"B\" should not be less than or equal to \"a\" for case-insensitive ordinal comparison.");

            CompareAssert.IsLessThanOrEqualTo<string>(null, null);
            CompareAssert.IsLessThanOrEqualTo(null, "x");

            ExceptionAssert.ThrowsAssertFailed(
                () => CompareAssert.IsLessThanOrEqualTo("x", null),
                "\"x\" should not be less than or equal to (null).");
        }

        [TestMethod]
        public void IsGreaterThan()
        {
            CompareAssert.IsGreaterThan(7, 4);

            CompareAssert.IsGreaterThan("a", "B", StringComparer.Ordinal,
                "\"a\" should be greater than \"B\" for case-sensitive ordinal comparison.");

            ExceptionAssert.ThrowsAssertFailed(
                () => CompareAssert.IsGreaterThan("a", "B", StringComparer.OrdinalIgnoreCase),
                "\"a\" should not be greater than \"B\" for case-insensitive ordinal comparison.");

            ExceptionAssert.ThrowsAssertFailed(
                () => CompareAssert.IsGreaterThan("b", "B", StringComparer.OrdinalIgnoreCase),
                "\"b\" should not be greater than \"B\" for case-insensitive ordinal comparison.");

            CompareAssert.IsGreaterThan("x", null);

            ExceptionAssert.ThrowsAssertFailed(
                () => CompareAssert.IsGreaterThan(null, "x"),
                "(null) should not be greater than \"x\".");

            ExceptionAssert.ThrowsAssertFailed(
                () => CompareAssert.IsGreaterThan<string>(null, null),
                "(null) should not be greater than (null).");
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo()
        {
            CompareAssert.IsGreaterThanOrEqualTo(7, 4);
            CompareAssert.IsGreaterThanOrEqualTo(4, 4);

            CompareAssert.IsGreaterThanOrEqualTo("a", "B", StringComparer.Ordinal,
                "\"a\" should be greater than or equal to \"B\" for case-sensitive ordinal comparison.");

            CompareAssert.IsGreaterThanOrEqualTo("b", "B", StringComparer.OrdinalIgnoreCase,
                "\"b\" should be greater than or equal to \"B\" for case-insensitive ordinal comparison.");

            ExceptionAssert.ThrowsAssertFailed(
                () => CompareAssert.IsGreaterThanOrEqualTo("a", "B", StringComparer.OrdinalIgnoreCase),
                "\"a\" should not be greater than or equal to \"B\" for case-insensitive ordinal comparison.");

            CompareAssert.IsGreaterThanOrEqualTo<string>(null, null);
            CompareAssert.IsGreaterThanOrEqualTo("x", null);

            ExceptionAssert.ThrowsAssertFailed(
                () => CompareAssert.IsGreaterThanOrEqualTo(null, "x"),
                "(null) should not be greater than or equal to \"x\".");
        }

        [TestMethod]
        public void IsEqualTo()
        {
            CompareAssert.IsEqualTo(4, 4);
            CompareAssert.IsEqualTo("B", "B");

            CompareAssert.IsEqualTo("B", "b", StringComparer.OrdinalIgnoreCase,
                "\"B\" should be equal to \"b\" for case-insensitive ordinal comparison.");

            ExceptionAssert.ThrowsAssertFailed(
                () => CompareAssert.IsEqualTo("B", "A"),
                "\"B\" should not be equal to \"A\".");

            ExceptionAssert.ThrowsAssertFailed(
                () => CompareAssert.IsEqualTo("B", "b", StringComparer.Ordinal),
                "\"B\" should not be equal to \"b\" for case-sensitive ordinal comparison.");

            CompareAssert.IsEqualTo<string>(null, null);

            ExceptionAssert.ThrowsAssertFailed(
                () => CompareAssert.IsEqualTo(null, "x"),
                "(null) should not be equal to \"x\".");

            ExceptionAssert.ThrowsAssertFailed(
                () => CompareAssert.IsEqualTo("x", null),
                "\"x\" should not be equal to (null).");
        }

        [TestMethod]
        public void IsNotEqualTo()
        {
            CompareAssert.IsNotEqualTo(4, 5);
            CompareAssert.IsNotEqualTo("B", "A");

            CompareAssert.IsNotEqualTo("B", "b", StringComparer.Ordinal,
                "\"B\" should not be equal to \"b\" for case-sensitive ordinal comparison.");

            ExceptionAssert.ThrowsAssertFailed(
                () => CompareAssert.IsNotEqualTo("B", "B"),
                "\"B\" should not be not equal to \"B\".");

            ExceptionAssert.ThrowsAssertFailed(
                () => CompareAssert.IsNotEqualTo("B", "b", StringComparer.OrdinalIgnoreCase),
                "\"B\" should not be not equal to \"b\" for case-insensitive ordinal comparison.");

            CompareAssert.IsNotEqualTo(null, "x");
            CompareAssert.IsNotEqualTo("x", null);

            ExceptionAssert.ThrowsAssertFailed(
                () => CompareAssert.IsNotEqualTo<string>(null, null),
                "(null) should not be not equal to (null).");            
        }
    }
}
