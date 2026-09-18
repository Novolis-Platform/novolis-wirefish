using Frank.WireFish;
using Microsoft.Extensions.DependencyInjection;
using TUnit.Core;

namespace Frank.WireFish.Tests;

public class ServiceCollectionExtensionsTests
{
    [Test]
    public async Task AddWireFish_registers_capture_pipeline()
    {
        var services = new ServiceCollection();
        services.AddWireFish(builder => builder.AddPacketHandler<NoOpPacketHandler>());

        await Assert.That(services.Count(descriptor => descriptor.ServiceType == typeof(IPacketHandler))).IsEqualTo(1);
        await Assert.That(services.Count(descriptor =>
            descriptor.ServiceType == typeof(Microsoft.Extensions.Hosting.IHostedService))).IsGreaterThan(0);
    }

    private sealed class NoOpPacketHandler : IPacketHandler
    {
        public bool CanHandle(DevicePacket packet) => false;

        public Task HandleAsync(DevicePacket packet, CancellationToken cancellationToken) =>
            Task.CompletedTask;
    }
}
