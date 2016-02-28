using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DogmaMix.Core.Threading
{
    /// <summary>
    /// Provides utility methods for creating and running <see cref="Thread"/> objects.
    /// </summary>
    public static class ThreadFactory
    {
        /// <summary>
        /// Creates and starts a new thread to execute the specified action delegate.
        /// </summary>
        /// <param name="action">The action delegate to execute in the thread.</param>
        /// <returns>The created thread.</returns>
        /// <remarks>
        /// <para>
        /// This method initializes a new thread through the <see cref="Thread(ThreadStart)"/> 
        /// constructor and calls its <see cref="Thread.Start()"/> method.
        /// Its purpose is conceptually similar 
        /// to the <see cref="TaskFactory.StartNew(Action)"/> method of the <see cref="TaskFactory"/> class, 
        /// or the <see cref="Task.Run(Action)"/> method of the <see cref="Task"/> class,
        /// but operates at the level of threads rather than tasks.
        /// </para>
        /// <para>
        /// Be careful with captured variables in the closure of the <paramref name="action"/> delegate.
        /// In the example below, the loop counter <c>i</c> is captured by the lambda expression;
        /// its value is likely to have been incremented <i>before</i> being read by the new thread.
        /// Consequently, the same value might be output by multiple threads; in the worst case,
        /// all threads would output the final value "8".
        /// <code>
        /// var threads = new Thread[8];
        /// for (int i = 0; i &lt; threads.Length; i++)
        ///     threads[i] = ThreadUtility.StartNew(() =&gt; Console.WriteLine(i));
        /// for (int i = 0; i &lt; threads.Length; i++)
        ///     threads[i].Join();
        /// </code>
        /// This issue can be fixed by using the <see cref="StartNew{TParam}(TParam, Action{TParam})"/>
        /// parameterized overload instead, passing the loop counter as the parameter:
        /// <code>
        /// var threads = new Thread[8];
        /// for (int i = 0; i &lt; threads.Length; i++)
        ///     threads[i] = ThreadUtility.StartNew(i, x =&gt; Console.WriteLine(x));
        /// for (int i = 0; i &lt; threads.Length; i++)
        ///     threads[i].Join();
        /// </code>
        /// Alternatively, one could use LINQ and avoid the need of a loop counter altogether:
        /// <code>
        /// Enumerable.Range(0, 8)
        ///           .Select(i =&gt; ThreadUtility.StartNew(() =&gt; Console.WriteLine(i)))
        ///           .ToList()
        ///           .ForEach(thread =&gt; thread.Join());
        /// </code>
        /// </para>
        /// </remarks>
        public static Thread StartNew(Action action)
        {
            ArgumentValidate.NotNull(action, nameof(action));

            var thread = new Thread(new ThreadStart(action));
            thread.Start();
            return thread;
        }

        /// <summary>
        /// Creates and starts a new thread to execute the specified action delegate,
        /// supplying the specified parameter value.
        /// </summary>
        /// <typeparam name="TParam">The type of the parameter to be supplied to the action delegate.</typeparam>
        /// <param name="parameter">The parameter to be supplied to the action delegate.</param>
        /// <param name="action">
        /// The action delegate to execute in the thread.
        /// When run, the delegate will be passed <paramref name="parameter"/> as an argument.
        /// </param>
        /// <returns>The created thread.</returns>
        /// <remarks>
        /// <para>
        /// This method initializes a new thread through the <see cref="Thread(ParameterizedThreadStart)"/> 
        /// constructor and calls its <see cref="Thread.Start(object)"/> method,
        /// supplying <paramref name="parameter"/> as the argument.
        /// Its purpose is conceptually similar to the <see cref="TaskFactory.StartNew(Action{object}, object)"/> method
        /// of the <see cref="TaskFactory"/> class, but operates at the level of threads rather than tasks.
        /// Unlike the .NET Framework classes, this method uses generics to ensure compile-time type safety.
        /// </para>
        /// <para>
        /// Refer to the remarks on the <see cref="StartNew(Action)"/> overload regarding captured variables.
        /// </para>
        /// </remarks>
        public static Thread StartNew<TParam>(TParam parameter, Action<TParam> action)
        {
            ArgumentValidate.NotNull(action, nameof(action));

            Action<object> actionObj = (object obj) => action((TParam)obj);
            var thread = new Thread(new ParameterizedThreadStart(actionObj));
            thread.Start(parameter);
            return thread;
        }
    }
}
