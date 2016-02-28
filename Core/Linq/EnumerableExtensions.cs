using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogmaMix.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="IEnumerable{T}"/> interface.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Returns the zero-based index of the first occurrence of the specified value in the sequence,
        /// using the default equality comparer to compare it with the sequence's elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence that contains the elements to search through.</param>
        /// <param name="value">The value to search for.</param>
        /// <returns>
        /// The zero-based index position of the first occurrence of an element in <paramref name="source"/>
        /// that is equal to <paramref name="value"/>, if found; 
        /// or -1 if <paramref name="source"/> is empty or no match is found.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// This method is modelled after the <see cref="Array.IndexOf{T}(T[],T)"/> method of the .NET Framework Class Library,
        /// but generalized to apply to all sequences (not just arrays).
        /// </remarks>
        public static int IndexOf<TSource>(this IEnumerable<TSource> source, TSource value)
        {
            ArgumentValidate.NotNull(source, nameof(source));

            return source.IndexOf(value, EqualityComparer<TSource>.Default);
        }

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified value in the sequence,
        /// using the specified equality comparer to compare it with the sequence's elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence that contains the elements to search through.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to compare values.</param>
        /// <returns>
        /// The zero-based index position of the first occurrence of an element in <paramref name="source"/>
        /// that is equal to <paramref name="value"/>, if found; 
        /// or -1 if <paramref name="source"/> is empty or no match is found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="comparer"/> is <see langword="null"/>.
        /// </exception>
        public static int IndexOf<TSource>(this IEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
        {
            ArgumentValidate.NotNull(source, nameof(source));
            ArgumentValidate.NotNull(comparer, nameof(comparer));

            return source.IndexOf(element => comparer.Equals(element, value));
        }

        /// <summary>
        /// Returns the zero-based index of the first occurrence of an element in the sequence 
        /// that matches the condition defined by the specified predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence that contains the elements to search through.</param>
        /// <param name="predicate">The function that defines the condition to search for.</param>
        /// <returns>
        /// The zero-based index position of the first occurrence of an element in <paramref name="source"/>
        /// for which <paramref name="predicate"/> returns <see langword="true"/>, if found; 
        /// or -1 if <paramref name="source"/> is empty or no match is found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        /// <para>
        /// This method is modelled after the <see cref="Array.FindIndex{T}(T[],Predicate{T})"/> method 
        /// of the .NET Framework Class Library, but generalized to apply to all sequences (not just arrays).
        /// </para>
        /// <list type="bullet">
        /// <listheader>See Also</listheader>
        /// <item><see href="http://stackoverflow.com/a/12695631/1149773">Get Index of First non-Whitespace Character in C# String</see> (answer), <i>Stack Overflow</i></item>
        /// </list>
        /// </remarks>
        public static int IndexOf<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            ArgumentValidate.NotNull(source, nameof(source));
            ArgumentValidate.NotNull(source, nameof(predicate));

            // Since IndexesOf is implemented as an iterator, the FirstOrDefault() call 
            // will only cause it to iterate up to the first match.
            var indexes = source.IndexesOf(predicate);
            return indexes.Cast<int?>().FirstOrDefault() ?? -1;
        }

        /// <summary>
        /// Returns the sequence of zero-based indexes of occurrences of the specified value in the sequence,
        /// using the default equality comparer to compare it with the sequence's elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence that contains the elements to search through.</param>
        /// <param name="value">The value to search for.</param>
        /// <returns>
        /// The sequence of zero-based index positions of elements in <paramref name="source"/>
        /// that are equal to <paramref name="value"/>.
        /// If <paramref name="source"/> is empty or no match is found, an empty sequence is returned.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// This method is implemented by using deferred execution. For more details, 
        /// refer to the remarks on the <see cref=" IndexesOf{TSource}(IEnumerable{TSource},Func{TSource,bool})"/> overload.
        /// </remarks>
        public static IEnumerable<int> IndexesOf<TSource>(this IEnumerable<TSource> source, TSource value)
        {
            ArgumentValidate.NotNull(source, nameof(source));

            return source.IndexesOf(value, EqualityComparer<TSource>.Default);
        }

        /// <summary>
        /// Returns the sequence of zero-based indexes of occurrences of the specified value in the sequence,
        /// using the specified equality comparer to compare it with the sequence's elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence that contains the elements to search through.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to compare values.</param>
        /// <returns>
        /// The sequence of zero-based index positions of elements in <paramref name="source"/>
        /// that are equal to <paramref name="value"/>.
        /// If <paramref name="source"/> is empty or no match is found, an empty sequence is returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="comparer"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        /// This method is implemented by using deferred execution. For more details, 
        /// refer to the remarks on the <see cref=" IndexesOf{TSource}(IEnumerable{TSource},Func{TSource,bool})"/> overload.
        /// </remarks>
        public static IEnumerable<int> IndexesOf<TSource>(this IEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
        {
            ArgumentValidate.NotNull(source, nameof(source));
            ArgumentValidate.NotNull(comparer, nameof(comparer));

            return source.IndexesOf(element => comparer.Equals(element, value));
        }

        /// <summary>
        /// Returns the zero-based indexes of the occurrences of elements in the sequence 
        /// that match the condition defined by the specified predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence that contains the elements to search through.</param>
        /// <param name="predicate">The function that defines the condition to search for.</param>
        /// <returns>
        /// The sequence of zero-based index positions of elements in <paramref name="source"/>
        /// for which <paramref name="predicate"/> returns <see langword="true"/>.
        /// If <paramref name="source"/> is empty or no match is found, an empty sequence is returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        /// <para>
        /// This method is implemented by using deferred execution. 
        /// The immediate return value is an object that stores all the information that is required to perform the action.
        /// The query represented by this method is not executed until the object is enumerated either by calling
        /// its <see cref="IEnumerable{T}.GetEnumerator"/> method directly or by using <see langword="foreach"/>.
        /// </para>
        /// <para>
        /// Furthermore, this method is implemented as an <see href="https://msdn.microsoft.com/en-us/library/9k7k7cf0.aspx">iterator</see>
        /// that uses <see href="https://msdn.microsoft.com/en-us/library/bb943859.aspx#Anchor_1">lazy evaluation</see>.
        /// Each call to the iterator would only process elements up to next match.
        /// </para>
        /// </remarks>
        public static IEnumerable<int> IndexesOf<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            ArgumentValidate.NotNull(source, nameof(source));
            ArgumentValidate.NotNull(predicate, nameof(predicate));

            int i = 0;

            foreach (TSource element in source)
            {
                if (predicate(element))
                    yield return i;

                i++;
            }
        }

        /// <summary>
        /// Appends the specified element to the end of the sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence after which to append the element.</param>
        /// <param name="suffix">The element to append to the sequence.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that consists of the elements of <paramref name="source"/>, 
        /// followed by <paramref name="suffix"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// <para>
        /// This method is implemented by using deferred execution. 
        /// The immediate return value is an object that stores all the information that is required to perform the action.
        /// The query represented by this method is not executed until the object is enumerated either by calling
        /// its <see cref="IEnumerable{T}.GetEnumerator"/> method directly or by using <see langword="foreach"/>.
        /// </para>
        /// <para>
        /// This method does not modify the <paramref name="source"/> sequence instance.
        /// Instead, it returns a new sequence that wraps <paramref name="source"/>.
        /// </para>
        /// <para>
        /// This method is equivalent to initializing a single-element sequence from <paramref name="suffix"/>, and passing it 
        /// as the second argument to <see cref="Enumerable.Concat{TSource}(IEnumerable{TSource},IEnumerable{TSource})"/>.
        /// For example: <c>source.Concat(new[] { suffix })</c>.
        /// </para>
        /// </remarks>
        public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> source, TSource suffix)
        {
            ArgumentValidate.NotNull(source, nameof(source));

            foreach (TSource item in source)
                yield return item;

            yield return suffix;
        }

        /// <summary>
        /// Prepends the specified element to the beginning of the sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence before which to prepend the element.</param>
        /// <param name="prefix">The element to prepend to the sequence.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that consists of <paramref name="prefix"/>, 
        /// followed by the elements of <paramref name="source"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// <para>
        /// This method is implemented by using deferred execution. 
        /// The immediate return value is an object that stores all the information that is required to perform the action.
        /// The query represented by this method is not executed until the object is enumerated either by calling
        /// its <see cref="IEnumerable{T}.GetEnumerator"/> method directly or by using <see langword="foreach"/>.
        /// </para>
        /// <para>
        /// This method does not modify the <paramref name="source"/> sequence instance.
        /// Instead, it returns a new sequence that wraps <paramref name="source"/>.
        /// </para>
        /// <para>
        /// This method is equivalent to initializing a single-element sequence from <paramref name="prefix"/>, and passing it 
        /// as the first argument to <see cref="Enumerable.Concat{TSource}(IEnumerable{TSource},IEnumerable{TSource})"/>.
        /// For example: <c>(new[] { prefix }).Concat(source)</c>.
        /// </para>
        /// </remarks>
        public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource prefix)
        {
            ArgumentValidate.NotNull(source, nameof(source));

            yield return prefix;

            foreach (TSource item in source)
                yield return item;
        }
        
        /// <summary>
        /// Returns the specified sequence as an array.
        /// If <paramref name="source"/> can be cast as an array, it is returned unchanged;
        /// otherwise, an array is created from it through the <see cref="Enumerable.ToArray{TSource}"/> extension method.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to cast as an array or create an array from.</param>
        /// <returns>An array that contains the elements from <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// This extension method is similar to the <see cref="Enumerable.ToArray{TSource}"/> extension method,
        /// but avoids the overhead of allocating and populating a fresh array when <paramref name="source"/> 
        /// is already an array and could be used directly.
        /// However, this extension method should not be used in scenarios where one needs to ensure that the original array
        /// cannot be inadvertently modified through the returned reference. In such cases, a fresh copy should always
        /// be created through <see cref="Enumerable.ToArray{TSource}"/>.
        /// </remarks>
        public static TSource[] AsArray<TSource>(this IEnumerable<TSource> source)
        {
            ArgumentValidate.NotNull(source, nameof(source));

            return source as TSource[] ?? source.ToArray();
        }
    }
}
