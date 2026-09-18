namespace Frank.WireFish;

/// <summary>
/// Configures WireFish packet handlers during service registration.
/// </summary>
public interface IWireFishBuilder
{
    /// <summary>
    /// Registers a singleton <see cref="IPacketHandler"/> implementation.
    /// </summary>
    /// <typeparam name="THandler">Handler type resolved from DI.</typeparam>
    /// <returns>The same builder for chaining.</returns>
    IWireFishBuilder AddPacketHandler<THandler>() where THandler : class, IPacketHandler;
}
