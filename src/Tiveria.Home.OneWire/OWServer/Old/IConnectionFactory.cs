using System.Net;
using System.Threading;
using System.Threading.Tasks;


namespace Tiveria.Home.OneWire.OWServer
{
    public interface IConnectionFactory
    {
        /// <summary>
        /// Creates a new connection to an endpoint.
        /// </summary>
        /// <param name="endpoint">The <see cref="EndPoint"/> to connect to.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>
        /// A <see cref="ValueTask{TResult}" /> that represents the asynchronous connect, yielding the <see cref="ConnectionContext" /> for the new connection when completed.
        /// </returns>
        ValueTask<ConnectionContext> ConnectAsync(EndPoint endpoint, CancellationToken cancellationToken = default);
    }
}
