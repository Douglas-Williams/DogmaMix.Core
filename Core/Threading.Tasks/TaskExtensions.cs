using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogmaMix.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="Task"/> and <see cref="Task{TResult}"/> classes.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Waits for the <see cref="Task"/> to complete execution,
        /// with exceptions being rethrown rather than wrapped into an <see cref="AggregateException"/>.
        /// </summary>
        /// <param name="task">The task for which to wait.</param>
        /// <remarks>
        /// <para>
        /// This extension method is similar to the <see cref="Task.Wait()"/> method on the <see cref="Task"/> class.
        /// However, this method reproduces the behavior of the <see langword="await"/> keyword by rethrowing any unhandled exceptions,
        /// rather than wrapping them into an <see cref="AggregateException"/> like <see cref="Task.Wait()"/> does.
        /// If the task contains multiple exceptions (such as from a <see cref="Task.WhenAll(Task[])"/> call), only one is rethrown.
        /// </para>
        /// <para>
        /// Refer to the remarks on the <see cref="GetResult{TResult}(Task{TResult})"/> overload for more details.
        /// </para>
        /// </remarks>
        public static void GetResult(this Task task)
        {
            ArgumentValidate.NotNull(task, nameof(task));

            task.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Gets the result value of the <see cref="Task{TResult}"/>, 
        /// with exceptions being rethrown rather than wrapped into an <see cref="AggregateException"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by <paramref name="task"/>.</typeparam>
        /// <param name="task">The task for which to retrieve the result.</param>
        /// <returns>The result value of <paramref name="task"/>.</returns>
        /// <remarks>
        /// <para>
        /// This extension method is similar to the <see cref="Task{TResult}.Result"/> property on the <see cref="Task{TResult}"/> class.
        /// However, this method reproduces the behavior of the <see langword="await"/> keyword by rethrowing any unhandled exceptions,
        /// rather than wrapping them into an <see cref="AggregateException"/> like <see cref="Task{TResult}.Result"/> does.
        /// If the task contains multiple exceptions (such as from a <see cref="Task.WhenAll(Task[])"/> call), only one is rethrown.
        /// </para>
        /// <para>
        /// Calls to this method block the calling thread until the asynchronous operation represented by <paramref name="task"/> is complete.
        /// One is strongly encouraged to consider using the <see langword="await"/> keyword instead to maintain asynchrony.
        /// </para>
        /// <blockquote>
        /// One last remark: you should avoid using <see cref="Task{TResult}.Result"/> and <see cref="Task.Wait()"/> 
        /// as much as possible as they always encapsulate the inner exception in an <see cref="AggregateException"/> 
        /// and replace the message by a generic one (<c>"One or more errors occurred"</c>), which makes debugging harder. 
        /// Even if the synchronous version shouldn't be used that often, you should strongly consider using 
        /// <c>Task.GetAwaiter().GetResult()</c> instead.
        /// </blockquote>
        /// <blockquote>
        /// [The code] uses <c>GetAwaiter().GetResult()</c> instead of <see cref="Task.Wait()"/> because <see cref="Task.Wait()"/>
        /// will wrap any exceptions inside an <see cref="AggregateException"/>.
        /// </blockquote>
        /// <list type="bullet">
        /// <listheader>References</listheader>
        /// <item><see href="https://stackoverflow.com/q/17284517/1149773">Is Task.Result the same as .GetAwaiter.GetResult()?</see>, <i>Stack Overflow</i></item>
        /// <item><see href="https://blog.stephencleary.com/2014/12/a-tour-of-task-part-6-results.html">A Tour of Task, Part 6: Results</see> by Stephen Cleary</item>
        /// <item><see href="https://github.com/aspnet/Security/issues/59">Replace any Task.Result calls with Task.GetAwaiter().GetResult()</see></item>
        /// <item><see href="https://msdn.microsoft.com/en-us/magazine/dn818493.aspx#code-snippet-5">Async Programming : Unit Testing Asynchronous Code</see> by Stephen Cleary</item>
        /// </list>
        /// </remarks>
        public static TResult GetResult<TResult>(this Task<TResult> task)
        {
            ArgumentValidate.NotNull(task, nameof(task));

            return task.GetAwaiter().GetResult();
        }
    }
}
