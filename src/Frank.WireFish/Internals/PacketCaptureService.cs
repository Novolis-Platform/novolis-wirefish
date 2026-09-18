using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using PacketDotNet;

using SharpPcap;
using SharpPcap.LibPcap;

namespace Frank.WireFish.Internals;

internal class PacketCaptureService(ILogger<PacketCaptureService> logger, PacketHandler packetHandler) : IHostedService
{
    private readonly CaptureDeviceList _devices = CaptureDeviceList.Instance;

    /// <inheritdoc />
    public Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var device in _devices)
        {
            device.Open(DeviceModes.Promiscuous);
            device.OnPacketArrival += DeviceOnOnPacketArrival;
            device.StartCapture();
        }

        return Task.CompletedTask;
    }

    private void DeviceOnOnPacketArrival(object sender, PacketCapture e)
    {
        var device = (LibPcapLiveDevice)sender;
        var rawPacket = e.GetPacket();
        var packet = Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
        packetHandler.HandlePacket(device, packet).GetAwaiter().GetResult();
    }

    /// <inheritdoc />
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var device in _devices)
        {
            logger.LogDebug("Stopping packet capture service on device {Device}", device.Description);
            device.StopCapture();
            device.Close();
        }
        await Task.CompletedTask;
    }
}