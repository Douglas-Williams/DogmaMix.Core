using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogmaMix.Core.UnitTesting
{
    /// <summary>
    /// Verifies true/false propositions associated with sequences in unit tests.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Several methods in this class are auto-generated as wrappers over the <see cref="CollectionAssert"/> class.
    /// This class is designed to support <see cref="IEnumerable{T}"/> sequences in general
    /// (insulating consumers from the necessity of <see cref="Enumerable.ToList"/> calls),
    /// and uses generic types to enforce type safety at compile-time.
    /// </para>
    /// <para>
    /// For each public static method in <see cref="CollectionAssert"/>, 
    /// this class provides an equivalent method with the same name, 
    /// but whose signature replaces parameter types as follows:
    /// <list type="bullet">
    /// <item><see cref="ICollection"/> parameters become <see cref="IEnumerable{T}"/></item>
    /// <item><see cref="IComparer"/> parameters become <see cref="IComparer{T}"/></item>
    /// <item><see cref="Object"/> parameters for elements become the generic type</item>
    /// </list>
    /// Each wrapper method is implemented by converting the arguments and passing them to
    /// the corresponding <see cref="CollectionAssert"/> method.
    /// </para>
    /// </remarks>
    public static partial class EnumerableAssert
    {
        /// <summary>
        /// Verifies that the specified sequence is empty. 
        /// The assertion fails if the sequence contains any elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">The sequence that should be empty.</param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public static void IsEmpty<T>(IEnumerable<T> sequence, string message = "")
        {
            ArgumentValidate.NotNull(sequence, nameof(sequence));

            if (sequence.Any())
                throw new AssertFailedException($"The sequence was expected to be empty, but actually contains {sequence.Count()} element(s). {message}");
        }

        /// <summary>
        /// Verifies that the specified sequence contains the specified number of elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="sequence"/>.</typeparam>
        /// <param name="count">The expected number of elements that the sequence should contain.</param>
        /// <param name="sequence">The sequence that should <paramref name="count"/> elements.</param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public static void HasCount<T>(int count, IEnumerable<T> sequence, string message = "")
        {
            ArgumentValidate.NotNull(sequence, nameof(sequence));
            
            if (sequence.Count() != count)
                throw new AssertFailedException($"The sequence was expected to contain {count} element(s), but actually contains {sequence.Count()}. {message}");
        }
    }
}
