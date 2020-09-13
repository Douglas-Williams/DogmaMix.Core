using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogmaMix.Core.Comparers
{
    /// <summary>
    /// Provides an abstract base class for implementations of the <see cref="IEqualityComparer{T}"/> 
    /// generic interface, implementing the <see cref="IEqualityComparer{T}.Equals(T, T)"/> method
    /// to handle <see langword="null"/> arguments and references to the same object instance,
    /// and call <see cref="EqualsNonNull(T, T)"/> only if neither argument is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    /// <remarks>
    /// <para>
    /// This class is based upon – and intended as an alternative to – 
    /// the <see cref="EqualityComparer{T}"/> class from the .NET Framework Class Library. 
    /// This base class implements the <see cref="IEqualityComparer{T}.Equals(T, T)"/> interface method
    /// through its <see cref="Equals(T, T)"/> method, which handles <see langword="null"/> arguments
    /// and references to the same object instance.
    /// If neither argument is <see langword="null"/>, and the two arguments do not refer to the same object instance,
    /// then the equality comparison result is obtained by calling the <see cref="EqualsNonNull(T, T)"/> abstract
    /// method, which is to be overridden by derived classes.
    /// </para>
    /// <para>
    /// Similarly, the <see cref="IEqualityComparer{T}.GetHashCode(T)"/> interface method is implemented through 
    /// this class's <see cref="GetHashCode(T)"/> method, which returns <c>0</c> for <see langword="null"/> arguments,
    /// and forwards all other arguments to the <see cref="GetHashCodeNonNull(T)"/> abstract method.
    /// This behavior of returning a hash code of <c>0</c> for <see langword="null"/> arguments is consistent 
    /// with the implementation of the <see cref="IEqualityComparer.GetHashCode(object)"/> interface method
    /// in the <see cref="EqualityComparer{T}"/> class of the .NET Framework.
    /// </para>
    /// <para>
    /// Like the <see cref="EqualityComparer{T}"/> class, this base class also provides an explicit interface
    /// implementation of the <see cref="IEqualityComparer.Equals(object, object)"/> and 
    /// <see cref="IEqualityComparer.GetHashCode(object)"/> methods from the 
    /// <see cref="IEqualityComparer"/> non-generic interface.
    /// However, this base class intentionally does not define a <see cref="EqualityComparer{T}.Default"/> property,
    /// since its static inheritance in derived classes may confuse consumers.
    /// </para>
    /// <list type="bullet">
    /// <listheader>References</listheader>
    /// <item><see href="https://stackoverflow.com/q/35940406/1149773">Should derived classes hide the Default and Create static members inherited from Comparer&lt;T&gt;?</see>, <i>Stack Overflow</i></item>
    /// </list>
    /// </remarks>
    [Serializable]
    public abstract class EqualityComparerBase<T> : IEqualityComparer<T>, IEqualityComparer
    {
        private const string InvalidComparerArgumentTypeMessage =
            "Type of argument is not compatible with the generic comparer.";

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualityComparerBase{T}"/> class.
        /// </summary>
        protected EqualityComparerBase()
        { }

        /// <summary>
        /// Determines whether two objects of type <typeparamref name="T"/> are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the specified objects are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(T x, T y)
        {
            if (object.ReferenceEquals(x, y))   // both null or same instance
                return true;
            if (x == null || y == null)
                return false;

            return EqualsNonNull(x, y);
        }

        /// <summary>
        /// When overridden in a derived class, determines whether two objects of type <typeparamref name="T"/>,
        /// neither of which is <see langword="null"/>, are equal.
        /// </summary>
        /// <param name="x">The first non-null object to compare.</param>
        /// <param name="y">The second non-null object to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the specified objects are equal; otherwise, <see langword="false"/>.
        /// </returns>
        protected abstract bool EqualsNonNull(T x, T y);

        /// <summary>
        /// Serves as a hash function for the specified object
        /// for hashing algorithms and data structures, such as a hash table.
        /// </summary>
        /// <param name="obj">The object for which to get a hash code.</param>
        /// <returns>A hash code for the specified object.</returns>
        public int GetHashCode(T obj)
        {
            if (obj == null)
                return 0;

            return GetHashCodeNonNull(obj);
        }

        /// <summary>
        /// When overridden in a derived class, serves as a hash function for the specified object
        /// for hashing algorithms and data structures, such as a hash table.
        /// The specified object will never be <see langword="null"/>.
        /// </summary>
        /// <param name="obj">The non-null object for which to get a hash code.</param>
        /// <returns>A hash code for the specified object.</returns>
        protected abstract int GetHashCodeNonNull(T obj);

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the specified objects are equal; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="x"/> or <paramref name="y"/> is of a type that cannot be cast 
        /// to type <typeparamref name="T" />.
        /// </exception>
        /// <remarks>
        /// This method throws <see cref="ArgumentException"/> if passed an argument of an incompatible type,
        /// even if the other argument is <see langword="null"/>.
        /// This behavior differs from the implementation of the <see cref="EqualityComparer{T}"/> class, 
        /// which does not throw an exception if at least one argument is non-null, or if both
        /// arguments refer to the same object instance (of an incompatible type).
        /// </remarks>
        bool IEqualityComparer.Equals(object x, object y)
        {
            if (x != null && !(x is T))
                throw new ArgumentException(InvalidComparerArgumentTypeMessage, nameof(x));
            if (y != null && !(y is T))
                throw new ArgumentException(InvalidComparerArgumentTypeMessage, nameof(y));

            return Equals((T)x, (T)y);
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
        /// <returns>A hash code for the specified object.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="obj"/> is of a type that cannot be cast to type <typeparamref name="T"/>.
        /// </exception>
        int IEqualityComparer.GetHashCode(object obj)
        {
            if (obj == null)
                return 0;

            if (!(obj is T))
                throw new ArgumentException(InvalidComparerArgumentTypeMessage, nameof(obj));

            return GetHashCode((T)obj);
        }
    }
}
