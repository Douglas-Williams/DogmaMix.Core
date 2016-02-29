using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace DogmaMix.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="OperationContext"/> class.
    /// </summary>
    /// <remarks>
    /// The <see cref="OperationContext"/> class provides access to the execution context of a WCF service method,
    /// and is typically accessed through its <see cref="OperationContext.Current"/> static property.
    /// </remarks>
    public static class OperationContextExtensions
    {
        /// <summary>
        /// Gets the IP address of the client from which the incoming message was sent.
        /// </summary>
        /// <param name="context">The execution context for the current WCF service method.</param>
        /// <returns>The IP address of the client.</returns>
        /// <remarks>
        /// <list type="bullet">
        /// <listheader>References</listheader>
        /// <item><see href="http://stackoverflow.com/a/93437/1149773">Obtaining client IP address in WCF</see>, <i>Stack Overflow</i></item>
        /// </list>
        /// </remarks>
        public static string GetUserHostAddress(this OperationContext context)
        {
            ArgumentValidate.NotNull(context, nameof(context));

            var remoteEndpoint = context.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            return remoteEndpoint.Address;
        }

        /// <summary>
        /// Gets the User-Agent header of the client from which the incoming message was sent.
        /// </summary>
        /// <param name="context">The execution context for the current WCF service method.</param>
        /// <returns>The User-Agent header of the client.</returns>
        public static string GetUserAgent(this OperationContext context)
        {
            ArgumentValidate.NotNull(context, nameof(context));

            var httpRequest = context.IncomingMessageProperties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
            return httpRequest.Headers[HttpRequestHeader.UserAgent];
        }
    }
}
