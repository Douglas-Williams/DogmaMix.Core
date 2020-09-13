using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogmaMix.Core.Disposables;
using DogmaMix.Core.Extensions;

namespace DogmaMix.Core.IO
{
    /// <summary>
    /// Represents a temporary file that is automatically deleted when the class instance is disposed.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When an instance of this class is created, its <see cref="FilePath"/> property would be populated
    /// with the full path for a new uniquely-named temporary file.
    /// The file path is located within the user's temporary folder (as identified through <see cref="Path.GetTempPath()"/>).
    /// The temporary file would be deleted, if it exists, when the <see cref="IDisposable.Dispose()"/> method is called
    /// on the class instance.
    /// If the temporary file does not exist at that time, the call would have no effect; no exception is thrown.
    /// </para>
    /// <para>
    /// This class provides a similar function to the <see cref="Path.GetTempFileName()"/> method of the <see cref="Path"/> class,
    /// but offers the following advantages:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// The temporary file is deleted when the instance is disposed, relieving consumers of the responsibility 
    /// of deleting it manually through a <see langword="finally"/> block.
    /// </item>
    /// <item>
    /// Consumers can optionally specify that the temporary file should not be created as empty in advance,
    /// avoiding the extra filesystem call that this involves, and thereby improving performance.
    /// </item>
    /// <item>
    /// The filename is constructed from a concatenation of calls to <see cref="Path.GetRandomFileName()"/>, 
    /// each of which returns a cryptographically strong, random string.
    /// Unlike <see cref="Path.GetTempFileName()"/> and <see cref="Path.GetRandomFileName()"/>,
    /// this class does not follow the <see href="https://en.wikipedia.org/wiki/8.3_filename">8.3 filename</see> restriction,
    /// generating filenames that are at least 32 characters long.
    /// Consequently, the risk of collision with other temporary files becomes negligible.
    /// </item>
    /// <item>
    /// The filename extension can be customized, avoiding an extra call to <see cref="Path.ChangeExtension(string, string)"/>
    /// by consumer.
    /// </item>
    /// </list>
    /// <para>
    /// This class would fail to delete the temporary file if the current thread or process is terminated prematurely,
    /// or the consumer neglects to call the <see cref="IDisposable.Dispose"/> method when done.
    /// To ensure that the file is deleted in such cases, callers should specify pass the <c>create</c> parameter 
    /// as <see langword="false"/> when calling the class constructor, then create and open the file through 
    /// a <see cref="FileStream"/> using the <see cref="FileOptions.DeleteOnClose"/> option,
    /// which instructs the operating system to automatically delete the file when it is no longer in use.
    /// </para>
    /// <para>
    /// This class may throw exceptions from its <see cref="IDisposable.Dispose()"/> method.
    /// When <see cref="IDisposable.Dispose()"/> is called implicitly at the end of a <see langword="using"/> block,
    /// or explicitly within a <see langword="finally"/> block, such exceptions may cause other exceptions 
    /// thrown from the main block of the statement to be hidden and lost.
    /// If this behavior is not desirable, consider using the 
    /// <see cref="DisposableExtensions.Using{TDisposable}(TDisposable, DisposeExceptionStrategy, Action{TDisposable})"/>
    /// extension method, or one its overloads, with the appropriate <see cref="DisposeExceptionStrategy"/> option.
    /// </para>
    /// <list type="bullet">
    /// <listheader>References</listheader>
    /// <item><see href="https://stackoverflow.com/q/400140/1149773">How do I automatically delete tempfiles in c#?</see>, <i>Stack Overflow</i></item>
    /// <item><see href="https://stackoverflow.com/q/1519429/1149773">Handling with temporary file stream</see>, <i>Stack Overflow</i></item>
    /// <item><see href="https://stackoverflow.com/q/35602760/1149773">Delegate-parameterized method vs IDisposable implementation for temporary file operation</see>, <i>Stack Overflow</i></item>
    /// </list>
    /// </remarks>
    public class TempFile : Disposable
    {
        private const int MinFileNameLength = 32;

        /// <summary>
        /// The full path of the temporary file.
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TempFile"/> class,
        /// populating the <see cref="FilePath"/> property with the full path for a new uniquely-named temporary file.
        /// The temporary file will be deleted, if it exists, when the <see cref="IDisposable.Dispose"/> method is called.
        /// </summary>
        /// <param name="extension">
        /// The extension for the temporary file, specified with or without the leading period, ".".
        /// If specified as <see langword="null"/>, the temporary file will have no extension.
        /// If the argument is omitted, the default extension of ".tmp" is used.
        /// </param>
        /// <param name="create">
        /// <see langword="true"/> to create the temporary file as empty within the constructor;
        /// <see langword="false"/> to refrain from creating the file.
        /// If the argument is omitted, the temporary file is created.
        /// </param>
        public TempFile(string extension = ".tmp", bool create = true)
        {
            FilePath = GetRandomTempFilePath(extension);
            
            if (create)
                FileOperation.CreateEmpty(FilePath);
        }

        /// <summary>
        /// Deletes the temporary file, it if exists.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/>, since the method call always comes from 
        /// the <see cref="Disposable.Dispose()"/> method.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                File.Delete(FilePath);
        }

        private static string GetRandomTempFilePath(string extension)
        {
            var fileNameBuilder = new StringBuilder(MinFileNameLength);
            do
            {
                var randomPart = Path.GetRandomFileName().Replace(".", "");
                fileNameBuilder.Append(randomPart);
            }
            while (fileNameBuilder.Length < MinFileNameLength);

            string fileName = fileNameBuilder.ToString();
            fileName = Path.ChangeExtension(fileName, extension);

            string filePath = Path.Combine(Path.GetTempPath(), fileName);
            return filePath;
       }
    }
}
