using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Resolvers;
using DogmaMix.Core.Xml;

namespace DogmaMix.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="XmlPreloadedResolver"/> class.
    /// </summary>
    public static class XmlPreloadedResolverExtensions
    {
        /// <summary>
        /// Adds the well-known <abbr title="document type definition">DTD</abbr> that is defined in XHTML 1.1
        /// to the <see cref="XmlPreloadedResolver"/> store.
        /// This DTD is already cached and embedded in the current assembly, so no network connections are required.
        /// </summary>
        /// <param name="resolver">The resolver to which to add the DTD for XHTML 1.1.</param>
        /// <param name="override">
        /// Whether to add the DTD and override the mappings for the URIs associated with XHTML 1.1 if they are already defined in the store.
        /// This can happen if the DTD for XHTML 1.1 is included by default in the <see cref="XmlPreloadedResolver"/> class
        /// in future or alternative implementations of the .NET Framework, 
        /// or if the said DTD has already been added manually to the <paramref name="resolver"/> instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the DTD was added to the store and mapped to at least one of its URIs;
        /// <see langword="false"/> if both URIs were already defined and not overridden.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <see cref="XmlPreloadedResolver"/> class is preloaded by default with DTDs defined in XHTML 1.0 and RSS 0.91, 
        /// but has not been updated to include XHTML 1.1 as of .NET Framework 4.6.1.
        /// Per the reference source for the <see href="http://referencesource.microsoft.com/#System.Xml/System/Xml/Resolvers/XmlPreloadedResolver.cs,33dd036cf1a84ad9">AddKnownDtd</see>
        /// private method of the <see cref="XmlPreloadedResolver"/> class, the default DTDs are mapped to both their public and their system identifiers.
        /// In the same vein, this method maps the DTD for XHTML 1.1 to its public identifier, <see cref="Xhtml11.DtdPublicId"/>,
        /// as well as to its system identifier, <see cref="Xhtml11.DtdSystemId"/>.
        /// </para>
        /// <para>
        /// The flattened version of the DTD for XHTML 1.1, which is embedded in the current assembly,
        /// is available at <see href="https://www.w3.org/TR/xhtml11/DTD/xhtml11-flat.dtd">xhtml11-flat.dtd</see> 
        /// on the W3C website.
        /// </para>
        /// <list type="bullet">
        /// <listheader>See Also</listheader>
        /// <item><see href="https://en.wikipedia.org/wiki/XML_Catalog">XML Catalog</see>, <i>Wikipedia</i></item>
        /// <item><see href="http://stackoverflow.com/q/1645767/1149773">How do I resolve entities when loading into an XDocument?"</see>, <i>Stack Overflow</i></item>
        /// <item><see href="http://stackoverflow.com/q/3733255/1149773">How to speed up loading DTD through DOCTYPE</see>, <i>Stack Overflow</i></item>
        /// </list>
        /// </remarks>
        /// <example>
        /// <code>
        /// var xmlResolver = new XmlPreloadedResolver();
        /// xmlResolver.AddXhtml11();
        /// 
        /// XmlReaderSettings settings = new XmlReaderSettings();
        /// settings.DtdProcessing = DtdProcessing.Parse;
        /// settings.XmlResolver = xmlResolver;
        /// 
        /// XDocument document;
        /// using (var xmlReader = XmlReader.Create(input, settings))
        ///     document = XDocument.Load(xmlReader);
        /// </code>
        /// </example>
        public static void AddXhtml11(this XmlPreloadedResolver resolver, bool @override = false)
        {
            ArgumentValidate.NotNull(resolver, nameof(resolver));

            Add(resolver, new Uri(Xhtml11.DtdPublicId, UriKind.RelativeOrAbsolute), ManifestResources.xhtml11_flat_dtd, @override);
            Add(resolver, new Uri(Xhtml11.DtdSystemId, UriKind.RelativeOrAbsolute), ManifestResources.xhtml11_flat_dtd, @override);
        }
        
        /// <summary>
        /// Adds a <see cref="Stream"/> to the <see cref="XmlPreloadedResolver"/> store and maps it to a URI. 
        /// If the store already contains a mapping for the same URI, 
        /// the existing mapping is overridden if <paramref name="override"/> is specified as <see langword="true"/>.
        /// </summary>
        /// <param name="resolver">The resolver to which to add the data mapped to the URI.</param>
        /// <param name="uri">The URI of the data that is being added to the <see cref="XmlPreloadedResolver"/> store.</param>
        /// <param name="value">A <see cref="Stream"/> with the data that corresponds to the provided URI.</param>
        /// <param name="override">
        /// Whether to add <paramref name="value"/> and override the mapping for <paramref name="uri"/> 
        /// even if it is already defined in the store.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> was added to the store and mapped to <paramref name="uri"/>;
        /// <see langword="false"/> if <paramref name="uri"/> was already defined and not overridden.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <see cref="XmlPreloadedResolver.Add(Uri, byte[])"/> method of the <see cref="XmlPreloadedResolver"/> class
        /// always overrides the existing mapping for <paramref name="uri"/> if it is already defined in the store. 
        /// This behaviour is not always desirable, as the caller might consider preloaded mappings in the store –
        /// if present – to be more authoritative than their supplied ones.
        /// This extension method allows callers to customize this behaviour through the <paramref name="override"/> parameter.
        /// </para>
        /// <para>
        /// If the <paramref name="override"/> parameter is specified as <see langword="false"/>,
        /// this method would call the <see cref="Enumerable.Contains{TSource}(IEnumerable{TSource}, TSource)"/> extension method
        /// on the <see cref="XmlPreloadedResolver.PreloadedUris"/> sequence of the <paramref name="resolver"/> to determine
        /// whether the store already contains <paramref name="uri"/>.
        /// The said extension method interally checks whether the type of the sequence implements <see cref="ICollection{T}"/>;
        /// if so, the <see cref="ICollection{T}.Contains"/> method in that implementation is invoked to obtain the result.
        /// Otherwise, the elements are compared to the specified value using the default equality comparer,
        /// <see cref="EqualityComparer{T}.Default"/>.
        /// </para>
        /// <para>
        /// Under the current implementation of the <see cref="XmlPreloadedResolver"/> class in the .NET Framework,
        /// the sequence returned by the <see cref="XmlPreloadedResolver.PreloadedUris"/> property is of type 
        /// <see cref="Dictionary{TKey, TValue}.KeyCollection"/>, whose <see cref="ICollection{T}.Contains(T)"/> implementation
        /// abides by the <see cref="IEqualityComparer{T}"/> instance that is associated with its <see cref="Dictionary{TKey, TValue}"/>.
        /// If the <paramref name="resolver"/> was initialized with a custom <see cref="IEqualityComparer{Uri}"/> comparer specified through 
        /// the third parameter to the <see cref="XmlPreloadedResolver(XmlResolver, XmlKnownDtds, IEqualityComparer{Uri})"/> constructor overload,
        /// then this comparer would have been used to initialize the internal dictionary, and will be consulted when the
        /// <see cref="ICollection{T}.Contains(T)"/> method is called to check for the presence of <paramref name="uri"/>.
        /// </para>
        /// <para>
        /// However, if the internal implementation of the <see cref="XmlPreloadedResolver"/> class is changed,
        /// in future or alternative versions of the .NET Framework, such that the sequence returned by 
        /// <see cref="XmlPreloadedResolver.PreloadedUris"/> no longer implements <see cref="ICollection{T}"/> this way,
        /// then URI comparisons would no longer honour custom <see cref="IEqualityComparer{Uri}"/> instances.
        /// In such cases, one should no longer rely on the <paramref name="override"/> parameter, but perform the check externally,
        /// using the same <see cref="IEqualityComparer{Uri}"/> instance that was used to initialize <paramref name="resolver"/>
        /// to compare <paramref name="uri"/> against the <see cref="XmlPreloadedResolver.PreloadedUris"/> sequence.
        /// </para>
        /// </remarks>
        public static bool Add(this XmlPreloadedResolver resolver, Uri uri, Stream value, bool @override)
        {
            ArgumentValidate.NotNull(resolver, nameof(resolver));
            ArgumentValidate.NotNull(uri, nameof(uri));
            ArgumentValidate.NotNull(value, nameof(value));

            if (@override || !resolver.PreloadedUris.Contains(uri))
            {
                resolver.Add(uri, value);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a byte array to the <see cref="XmlPreloadedResolver"/> store and maps it to a URI. 
        /// If the store already contains a mapping for the same URI, 
        /// the existing mapping is overridden if <paramref name="override"/> is specified as <see langword="true"/>.
        /// </summary>
        /// <param name="resolver">The resolver to which to add the data mapped to the URI.</param>
        /// <param name="uri">The URI of the data that is being added to the <see cref="XmlPreloadedResolver"/> store.</param>
        /// <param name="value">A byte array with the data that corresponds to the provided URI.</param>
        /// <param name="override">
        /// Whether to add <paramref name="value"/> and override the mapping for <paramref name="uri"/> 
        /// even if it is already defined in the store.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> was added to the store and mapped to <paramref name="uri"/>;
        /// <see langword="false"/> if <paramref name="uri"/> was already defined and not overridden.
        /// </returns>
        /// <remarks>
        /// Refer to the remarks on the <see cref="Add(XmlPreloadedResolver, Uri, Stream, bool)"/> overload.
        /// </remarks>
        public static bool Add(this XmlPreloadedResolver resolver, Uri uri, byte[] value, bool @override)
        {
            ArgumentValidate.NotNull(resolver, nameof(resolver));
            ArgumentValidate.NotNull(uri, nameof(uri));
            ArgumentValidate.NotNull(value, nameof(value));

            if (@override || !resolver.PreloadedUris.Contains(uri))
            {
                resolver.Add(uri, value);
                return true;
            }

            return false;
        }
    }
}
