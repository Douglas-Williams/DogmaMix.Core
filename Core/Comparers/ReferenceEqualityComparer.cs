using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DogmaMix.Core.Comparers
{
    /// <summary>
    /// Provides an implementation of the <see cref="IEqualityComparer{T}"/> and <see cref="IEqualityComparer"/>
    /// interfaces that determines two objects to be equal if and only if they refer to the same object instance.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    /// <remarks>
    /// <para>
    /// In contrast to the <see cref="EqualityComparer{T}.Default"/> comparer,
    /// this class is designed to ignore any implementation of the <see cref="IEquatable{T}"/>
    /// interface by type <typeparamref name="T"/>, as well as any overrides of the 
    /// <see cref="object.Equals(object)"/> and <see cref="object.GetHashCode()"/> methods.
    /// Rather, it relies on <see cref="object.ReferenceEquals(object, object)"/> to determine equality,
    /// and on <see cref="RuntimeHelpers.GetHashCode(object)"/> to compute hash codes.
    /// </para>
    /// <para>
    /// This class is adapted from the <c>System.Dynamic.Utils.ReferenceEqualityComparer</c> class, 
    /// defined as internal in the .NET Framework Class Library, as well as from various Stack Overflow answers.
    /// </para>
    /// <para>
    /// Type <typeparamref name="T"/> should typically be a reference type.
    /// No generic type constraint has been imposed on this class so as to allow it to be used by other
    /// unconstrained generic types. Reference equality comparisons of value types will always
    /// evaluate to <see langword="false"/>, even if the values are equal.
    /// </para>
    /// <list type="bullet">
    /// <listheader>References</listheader>
    /// <item><see href="http://referencesource.microsoft.com/#System.Core/Microsoft/Scripting/Utils/ReferenceEqualityComparer.cs">ReferenceEqualityComparer</see>, <i>Microsoft Reference Source</i></item>
    /// <item><see href="http://stackoverflow.com/q/1890058/1149773">IEqualityComparer&lt;T&gt; that uses ReferenceEquals</see>, <i>Stack Overflow</i></item>
    /// <item><see href="http://stackoverflow.com/q/11240036/1149773">What does RuntimeHelpers.GetHashCode do</see>, <i>Stack Overflow</i></item>
    /// </list>
    /// </remarks>
    public sealed class ReferenceEqualityComparer<T> : EqualityComparer<T>
    {
        /// <summary>
        /// Gets the default reference equality comparer for the type specified by the generic argument.
        /// </summary>
        public static new ReferenceEqualityComparer<T> Default { get; } = new ReferenceEqualityComparer<T>();

        /// <summary>
        /// Determines whether two objects of type <typeparamref name="T"/> refer to the same object instance.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the specified objects are identical; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(T x, T y)
        {
            return object.ReferenceEquals(x, y);
        }

        /// <summary>
        /// Serves as a hash function for the specified object for hashing algorithms and data structures, 
        /// such as a hash table.
        /// </summary>
        /// <param name="obj">The object for which to get a hash code.</param>
        /// <returns>A hash code for the specified object.</returns>
        /// <remarks>
        /// <see href="http://stackoverflow.com/a/11240130/1149773">Jon Skeet recommends</see> a nullity check:
        /// <blockquote>
        /// The nullity check may be unnecessary due to a similar check in
        /// RuntimeHelpers.GetHashCode, but it's not documented.
        /// </blockquote>
        /// </remarks>
        public override int GetHashCode(T obj)
        {
            if (obj == null)
                return 0;

            return RuntimeHelpers.GetHashCode(obj);
        }
    }
}
