using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogmaMix.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogmaMix.Core.UnitTesting
{
    /// <summary>
    /// Verifies true/false propositions associated with exceptions in unit tests.
    /// </summary>
    public static class ExceptionAssert
    {
        /// <summary>
        /// Verifies that an exception (of any type) is thrown during the execution of <paramref name="action"/>.
        /// The assertion fails if no uncaught exception is thrown.
        /// </summary>
        /// <param name="action">The action delegate that should throw the exception.</param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        /// <returns>The exception that was thrown by <paramref name="action"/>.</returns>
        /// <remarks>
        /// Refer to the remarks on the <see cref="Throws{TException}(Action, string)"/> overload.
        /// </remarks>
        public static Exception Throws(Action action, string message = null)
        {
            ArgumentValidate.NotNull(action, nameof(action));

            var asyncFunc = action.WrapAsync();
            var task = ThrowsAsync(asyncFunc, message);
            return task.GetResult();   // task is always completed; returns immediately
        }

        /// <summary>
        /// Verifies that an exception (of any type) is thrown during the asynchronous execution of <paramref name="asyncFunc"/>.
        /// The assertion fails if no uncaught exception is thrown.
        /// </summary>
        /// <param name="asyncFunc">The asynchronous delegate that should throw the exception.</param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains the exception that was thrown by <paramref name="asyncFunc"/>.
        /// </returns>
        /// <remarks>
        /// Refer to the remarks on the <see cref="ThrowsAsync{TException}(Func{Task}, string)"/> overload.
        /// </remarks>
        public static Task<Exception> ThrowsAsync(Func<Task> asyncFunc, string message = null)
        {
            ArgumentValidate.NotNull(asyncFunc, nameof(asyncFunc));

            return ThrowsAsyncInner<Exception>(asyncFunc,
                $"Expected exception, but none was thrown. {message}",
                t => $"Expected exception, but \"{t.Name}\" was thrown instead. {message}");
                // Message for wrong type should never actually be needed.
        }

        /// <summary>
        /// Verifies that an exception of type <typeparamref name="TException"/> (or derived therefrom)
        /// is thrown during the execution of <paramref name="action"/>.
        /// The assertion fails if no uncaught exception is thrown,
        /// of if another type of exception is thrown.
        /// </summary>
        /// <typeparam name="TException">The type of the exception expected to be thrown.</typeparam>
        /// <param name="action">The action delegate that should throw the exception.</param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        /// <returns>The exception that was thrown by <paramref name="action"/>.</returns>
        /// <remarks>
        /// <para>
        /// This method is similar to <see cref="ExpectedExceptionAttribute"/>, but allows multiple 
        /// exception-related verifications to be performed within a single test method.
        /// This may be desirable for trivial tests, such as <see cref="ArgumentNullException"/> being thrown across all parameters.
        /// </para>
        /// <para>
        /// In the spirit of the <see href="https://en.wikipedia.org/wiki/Liskov_substitution_principle">Liskov substitution principle</see>,
        /// this method also allows exceptions of types <i>derived</i> from <typeparamref name="TException"/> by default,
        /// whereas <see cref="ExpectedExceptionAttribute"/> requires its <see cref="ExpectedExceptionAttribute.AllowDerivedTypes"/>
        /// property to be set to <see langword="true"/>.
        /// </para>
        /// </remarks>
        /// <example>
        /// For example, if one were implementing the
        /// <see cref="Enumerable.Join{TOuter, TInner, TKey, TResult}(IEnumerable{TOuter}, IEnumerable{TInner}, Func{TOuter, TKey}, Func{TInner, TKey}, Func{TOuter, TInner, TResult})"/>
        /// method, one could the following test (using the definitions for <c>people</c> and <c>pets</c> given under the MSDN example):
        /// <code>
        /// [TestMethod]
        /// public void Expect_Join()
        /// {
        ///     ExceptionAssert.Throws&lt;ArgumentNullException&gt;(() =&gt; Enumerable.Join((Person[])null, pets, person =&gt; person, pet =&gt; pet.Owner, (person, pet) =&gt; new { OwnerName = person.Name, Pet = pet.Name }));
        ///     ExceptionAssert.Throws&lt;ArgumentNullException&gt;(() =&gt; people.Join((Pet[])null, person =&gt; person, pet =&gt; pet.Owner, (person, pet) =&gt; new { OwnerName = person.Name, Pet = pet.Name }));
        ///     ExceptionAssert.Throws&lt;ArgumentNullException&gt;(() =&gt; people.Join(pets, null, pet =&gt; pet.Owner, (person, pet) =&gt; new { OwnerName = person.Name, Pet = pet.Name }));
        ///     ExceptionAssert.Throws&lt;ArgumentNullException&gt;(() =&gt; people.Join(pets, person =&gt; person, null, (person, pet) =&gt; new { OwnerName = person.Name, Pet = pet.Name }));
        ///     ExceptionAssert.Throws&lt;ArgumentNullException&gt;(() =&gt; people.Join(pets, person =&gt; person, pet =&gt; pet.Owner, (Func&lt;Person, Pet, string&gt;)null));
        /// }
        /// </code>
        /// </example>
        public static TException Throws<TException>(Action action, string message = null)
            where TException : Exception
        {
            ArgumentValidate.NotNull(action, nameof(action));

            var asyncFunc = action.WrapAsync();
            var completedTask = ThrowsAsync<TException>(asyncFunc, message);
            return completedTask.GetResult();   // task is always completed; returns immediately
        }

        /// <summary>
        /// Verifies that an exception of type <typeparamref name="TException"/> (or derived therefrom)
        /// is thrown during the asynchronous execution of <paramref name="asyncAction"/>.
        /// The assertion fails if no uncaught exception is thrown,
        /// of if another type of exception is thrown.
        /// </summary>
        /// <typeparam name="TException">The type of the exception expected to be thrown.</typeparam>
        /// <param name="asyncAction">The asynchronous delegate that should throw the exception.</param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains the exception that was thrown by <paramref name="asyncAction"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method extends the <see cref="Throws{TException}(Action, string)"/> overload to support asynchronous delegates.
        /// Refer to the remarks on the said overload for the general functionality of these methods.
        /// </para>
        /// <list type="bullet">
        /// <listheader>See Also</listheader>
        /// <item><see href="https://msdn.microsoft.com/en-us/magazine/dn818493.aspx#code-snippet-8">Async Programming : Unit Testing Asynchronous Code</see> by Stephen Cleary</item>
        /// </list>
        /// </remarks>
        /// <example>
        /// <code>
        /// var exception = await ExceptionAssert.ThrowsAsync&lt;ArgumentNullException&gt;(async () => 
        /// {
        ///     using (var stream = new MemoryStream())
        ///     {
        ///         var buffer = new byte[128];
        ///         await stream.ReadAsync(buffer, 0, 256).ConfigureAwait(false);
        ///     }
        /// });
        /// </code>
        /// </example>
        public static Task<TException> ThrowsAsync<TException>(Func<Task> asyncAction, string message = null)
            where TException : Exception
        {
            ArgumentValidate.NotNull(asyncAction, nameof(asyncAction));

            return ThrowsAsyncInner<TException>(asyncAction,
                $"Expected exception of type \"{typeof(TException).Name}\", but no expection was thrown. {message}",
                t => $"Expected exception of type \"{typeof(TException).Name}\", but \"{t.Name}\" was thrown instead. {message}");
        }

        /// <summary>
        /// Verifies that an exception of type <see cref="AssertFailedException"/>
        /// is thrown during the execution of <paramref name="action"/>.
        /// The assertion fails if no uncaught exception is thrown,
        /// of if another type of exception is thrown.
        /// </summary>
        /// <param name="action">The action delegate that should throw the exception.</param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        /// <returns>The exception that was thrown by <paramref name="action"/>.</returns>
        /// <example>
        /// This method is useful for writing unit tests to verify the implementation of methods in custom <c>…Assert</c> classes.
        /// For example, it is used for testing the <see cref="EnumerableAssert"/> class implemented in this library.
        /// <code>
        /// ExceptionAssert.ThrowsAssertFailed(() =&gt; Assert.Fail());
        /// ExceptionAssert.ThrowsAssertFailed(() =&gt; Assert.AreEqual(16, 32));
        /// </code>
        /// </example>
        public static AssertFailedException ThrowsAssertFailed(Action action, string message = null)
        {
            ArgumentValidate.NotNull(action, nameof(action));

            // Could call ThrowsInner instead to customize messages.
            return Throws<AssertFailedException>(action, message);
        }

        private static async Task<TException> ThrowsAsyncInner<TException>(Func<Task> action, string messageNoException, Func<Type, string> messageWrongType)
            where TException : Exception
        {
            try
            {
                await action().ConfigureAwait(false);
            }
            catch (TException e)
            {
                return e;
            }
            catch (Exception e)
            {
                var message = messageWrongType(e.GetType());
                throw new AssertFailedException(message, e);
            }

            throw new AssertFailedException(messageNoException);
        }
    }
}
