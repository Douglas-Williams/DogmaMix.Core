using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogmaMix.Core.Comparers
{
    /// <summary>
    /// Provides an abstract base class for implementations of the <see cref="IComparer{T}"/> generic interface,
    /// implementing the <see cref="IComparer{T}.Compare(T, T)"/> method to handle <see langword="null"/> arguments
    /// and references to the same object instance, and call <see cref="CompareNonNull(T, T)"/> only if 
    /// neither argument is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    /// <remarks>
    /// <para>
    /// This class is based upon – and intended as an alternative to – 
    /// the <see cref="Comparer{T}"/> class from the .NET Framework Class Library. 
    /// This base class implements the <see cref="IComparer{T}.Compare(T, T)"/> interface method through
    /// its <see cref="Compare(T, T)"/> method, which provides the standard expected results for 
    /// <see langword="null"/> arguments, as documented on MSDN:
    /// </para>
    /// <blockquote>
    /// Comparing null with any reference type is allowed and does not generate an exception. 
    /// A null reference is considered to be less than any reference that is not null.
    /// </blockquote>
    /// <para>
    /// If the two arguments refer to the same object instance, <see cref="Compare(T, T)"/> returns zero.
    /// If neither argument is <see langword="null"/>, and the two arguments do not refer to the same object instance,
    /// then the comparison result is obtained by calling the <see cref="CompareNonNull(T, T)"/> abstract method, 
    /// which is to be overridden by derived classes.
    /// </para>
    /// <para>
    /// Like the <see cref="Comparer{T}"/> class, this base class also provides an explicit interface implementation 
    /// of the <see cref="IComparer.Compare"/> method from the <see cref="IComparer"/> non-generic interface.
    /// However, this base class intentionally does not define a <see cref="Comparer{T}.Default"/> property 
    /// or <see cref="Comparer{T}.Create(Comparison{T})"/> method, since their static inheritance
    /// in derived classes may confuse consumers.
    /// </para>
    /// <list type="bullet">
    /// <listheader>References</listheader>
    /// <item><see href="http://stackoverflow.com/q/35940406/1149773">Should derived classes hide the Default and Create static members inherited from Comparer&lt;T&gt;?</see>, <i>Stack Overflow</i></item>
    /// </list>
    /// </remarks>
    [Serializable]
    public abstract class ComparerBase<T> : IComparer<T>, IComparer
    {
        private const string InvalidComparerArgumentTypeMessage =
            "Type of argument is not compatible with the generic comparer.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ComparerBase{T}"/> class.
        /// </summary>
        protected ComparerBase()
        { }

        /// <summary>
        /// Performs a comparison of two objects of the same type and returns a value indicating 
        /// whether one object is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, 
        /// as shown in the following table:
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <term>Meaning</term>
        /// </listheader>
        /// <item>
        /// <term>Less than zero</term>
        /// <term>
        /// <paramref name="x"/> is less than <paramref name="y"/>. -or-
        /// <paramref name="x"/> is <see langword="null"/> and <paramref name="y"/> is not <see langword="null"/>.
        /// </term>
        /// </item>
        /// <item>
        /// <term>Zero</term>
        /// <term>
        /// <paramref name="x"/> equals <paramref name="y"/>. -or-
        /// <paramref name="x"/> and <paramref name="y"/> refer to the same object instance. -or-
        /// <paramref name="x"/> and <paramref name="y"/> are both <see langword="null"/>.
        /// </term>
        /// </item>
        /// <item>
        /// <term>Greater than zero</term>
        /// <term>
        /// <paramref name="x"/> is greater than <paramref name="y"/>. -or-
        /// <paramref name="x"/> is not <see langword="null"/> and <paramref name="y"/> is <see langword="null"/>.
        /// </term>
        /// </item>
        /// </list>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Type <typeparamref name="T"/> implements neither the <see cref="IComparable{T}"/> generic interface 
        /// nor the <see cref="IComparable"/> interface.
        /// </exception>
        public int Compare(T x, T y)
        {
            if (object.ReferenceEquals(x, y))   // both null or same instance
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;

            return CompareNonNull(x, y);
        }

        /// <summary>
        /// When overridden in a derived class, performs a comparison of two objects of the same type,
        /// neither of which is <see langword="null"/>, and returns a value indicating whether 
        /// one object is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first non-null object to compare.</param>
        /// <param name="y">The second non-null object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, 
        /// as shown in the following table:
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <term>Meaning</term>
        /// </listheader>
        /// <item>
        /// <term>Less than zero</term>
        /// <term><paramref name="x"/> is less than <paramref name="y"/>.</term>
        /// </item>
        /// <item>
        /// <term>Zero</term>
        /// <term><paramref name="x"/> equals <paramref name="y"/>.</term>
        /// </item>
        /// <item>
        /// <term>Greater than zero</term>
        /// <term><paramref name="x"/> is greater than <paramref name="y"/>.</term>
        /// </item>
        /// </list>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Type <typeparamref name="T"/> implements neither the <see cref="IComparable{T}"/> generic interface 
        /// nor the <see cref="IComparable"/> interface.
        /// </exception>
        protected abstract int CompareNonNull(T x, T y);

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, 
        /// or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, 
        /// as documented under the <see cref="Compare(T, T)"/> method.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="x"/> or <paramref name="y"/> is of a type that cannot be cast 
        /// to type <typeparamref name="T"/>. -or-
        /// <paramref name="x"/> or <paramref name="y"/> implements neither the <see cref="IComparable{T}"/> 
        /// generic interface nor the <see cref="IComparable"/> interface.
        /// </exception>
        /// <remarks>
        /// This method throws <see cref="ArgumentException"/> if passed an argument of an incompatible type,
        /// even if the other argument is <see langword="null"/>.
        /// This behavior differs from the implementation of the <see cref="Comparer{T}"/> class, 
        /// which does not throw an exception if at least one argument is non-null.
        /// </remarks>
        int IComparer.Compare(object x, object y)
        {
            if (x != null && !(x is T))
                throw new ArgumentException(InvalidComparerArgumentTypeMessage, nameof(x));
            if (y != null && !(y is T))
                throw new ArgumentException(InvalidComparerArgumentTypeMessage, nameof(y));
            
            return Compare((T)x, (T)y);
        }
    }
}
