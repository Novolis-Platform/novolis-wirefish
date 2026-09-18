using Novolis.Messaging.Channels;
using Frank.WireFish.Internals;

using Microsoft.Extensions.DependencyInjection;

namespace Frank.WireFish;

/// <summary>
/// DI registration for WireFish packet capture and handlers.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds WireFish capture services and configures packet handlers.
    /// </summary>
    /// <param name="services">Service collection to extend.</param>
    /// <param name="wireFishBuilder">Callback that registers handlers on the builder.</param>
    /// <returns>The same <paramref name="services"/> instance.</returns>
    public static IServiceCollection AddWireFish(this IServiceCollection services, Action<IWireFishBuilder> wireFishBuilder)
    {
        services.AddPacketCaptureService();
        var builder = new WireFishBuilder(services);
        wireFishBuilder(builder);
        return services;
    }

    internal static IServiceCollection AddPacketCaptureService(this IServiceCollection services)
    {
        services.AddChannel<DevicePacket>();
        services.AddHostedService<PacketCaptureService>();
        services.AddSingleton<PacketHandler>();
        services.AddSingleton<InterfaceProvider>();
        services.AddHostedService<DevicePacketHandler>();
        return services;
    }
}
