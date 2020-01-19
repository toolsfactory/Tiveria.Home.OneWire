using Microsoft.Extensions.DependencyInjection;

namespace Tiveria.Home.OneWire.OWServer
{
    public interface IOWServerConnectionBuilder
    {
        /// <summary>
        /// Gets the builder service collection.
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// Creates a <see cref="OWServerConnection"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="OWServerConnection"/> built using the configured options.
        /// </returns>
        OWServerConnection Build();
    }
}
