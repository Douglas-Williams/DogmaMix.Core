using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogmaMix.Core.Comparers
{
    /// <summary>
    /// Provides utility methods for combining hash codes from multiple objects.
    /// This functionality builds on the hash codes returned by the
    /// <see cref="object.GetHashCode"/>, <see cref="IEqualityComparer.GetHashCode"/>, 
    /// and <see cref="IEqualityComparer{T}.GetHashCode"/> methods.
    /// </summary>
    public static class HashCodeCombiner
    {
        /// <summary>
        /// Combines the hash codes produced for each item in the specified sequence
        /// by the default comparer for type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items whose hash codes to combine.</typeparam>
        /// <param name="items">The items whose hash codes to combine.</param>
        /// <exception cref="ArgumentNullException"><paramref name="items"/> is <see langword="null"/>.</exception>
        /// <returns>The combined hash code.</returns>
        /// <remarks>
        /// Refer to the remarks on the <see cref="Combine{T}(IEnumerable{T}, IEqualityComparer{T})"/> overload.
        /// </remarks>
        public static int Combine<T>(IEnumerable<T> items)
        {
            ArgumentValidate.NotNull(items, nameof(items));

            return Combine(items, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Combines the hash codes produced for each item in the specified sequence by the specified comparer.
        /// </summary>
        /// <typeparam name="T">The type of the items whose hash codes to combine.</typeparam>
        /// <param name="items">The items whose hash codes to combine.</param>
        /// <param name="comparer">The comparer for computing each item's hash code.</param>
        /// <returns>The combined hash code.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="items"/> or <paramref name="comparer"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// <para>
        /// This method uses the <see href="http://www.isthe.com/chongo/tech/comp/fnv/index.html#FNV-1a">FNV-1a alternate algorithm</see> 
        /// from the <see href="https://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function">Fowler–Noll–Vo hash function</see>
        /// for combining the individual hash codes produced from the sequence items.
        /// </para>
        /// <para>
        /// When computing the combined hash, the entire sequence is scanned. This can have performance implications for large sequences.
        /// One can improve performance by restricting the hash code computation to a subset of the sequence;
        /// however, this would cause the uniqueness (evenness of distribution) to be sacrificed.
        /// For example, one could call <see cref="Enumerable.Take"/> with a small constant on the sequence.
        /// For reference, the internal implementation for the <see cref="IStructuralEquatable.GetHashCode"/> method 
        /// of the <see cref="IStructuralEquatable"/> interface by the <see cref="Array"/> class
        /// in the .NET Framework Class Library only considers the hash codes of the last eight elements
        /// (see <see href="http://referencesource.microsoft.com/#mscorlib/system/array.cs,807">source</see>).
        /// </para>
        /// <list type="bullet">
        /// <listheader>See Also</listheader>
        /// <item><see href="http://eternallyconfuzzled.com/tuts/algorithms/jsw_tut_hashing.aspx#fnv">Hashing</see> by Eternally Confuzzled</item>
        /// <item><see href="http://stackoverflow.com/a/468084/1149773">How do I generate a hashcode from a byte array in C#?</see> (answer), <i>Stack Overflow</i></item>
        /// <item><see href="http://stackoverflow.com/a/7244729/1149773">GetHashCode() on byte[] array</see> (answer) by Jon Skeet, <i>Stack Overflow</i></item>
        /// </list>
        /// </remarks>
        public static int Combine<T>(IEnumerable<T> items, IEqualityComparer<T> comparer)
        {
            ArgumentValidate.NotNull(items, nameof(items));
            ArgumentValidate.NotNull(comparer, nameof(comparer));

            return Combine(items.Select(comparer.GetHashCode));
        }

        private static int Combine(IEnumerable<int> hashCodes)
        {
            unchecked
            {
                const uint fnvPrime = 16777619;
                uint hash = 2166136261;

                foreach (uint item in hashCodes)
                    hash = (hash ^ item) * fnvPrime;

                return (int)hash;
            }
        }
    }
}
