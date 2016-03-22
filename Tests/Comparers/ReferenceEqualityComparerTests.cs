using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.UnitTesting;

namespace DogmaMix.Core.Comparers.Tests
{
    [TestClass]
    public class ReferenceEqualityComparerTests
    {
        [TestMethod]
        public void Equals()
        {
            var a = new IntWrapper(5);
            var b = new IntWrapper(5);

            EqualityAssert.Equals(a, a, EqualityComparer<IntWrapper>.Default);
            EqualityAssert.Equals(a, b, EqualityComparer<IntWrapper>.Default);
            EqualityAssert.Equals(a, a, ReferenceEqualityComparer<IntWrapper>.Default);
            EqualityAssert.NotEquals(a, b, ReferenceEqualityComparer<IntWrapper>.Default);

            int x = 5;
            int y = 5;

            EqualityAssert.Equals(x, x, EqualityComparer<int>.Default);
            EqualityAssert.Equals(x, y, EqualityComparer<int>.Default);
            EqualityAssert.NotEquals(x, x, ReferenceEqualityComparer<int>.Default);
            EqualityAssert.NotEquals(x, y, ReferenceEqualityComparer<int>.Default);
        }

        private class IntWrapper : IEquatable<IntWrapper>
        {
            public IntWrapper(int value)
            {
                Value = value;
            }

            public int Value { get; }

            public bool Equals(IntWrapper other)
            {
                return other != null && Value == other.Value;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as IntWrapper);
            }

            public override int GetHashCode()
            {
                return Value.GetHashCode();
            }

            public override string ToString()
            {
                return Value.ToString();
            }
        }
    }
}