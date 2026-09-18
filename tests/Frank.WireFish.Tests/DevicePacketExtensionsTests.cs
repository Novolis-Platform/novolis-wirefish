using System.Net;
using System.Net.NetworkInformation;
using PacketDotNet;
using PacketDotNet.Utils;

namespace Frank.WireFish.Tests;

public class DevicePacketExtensionsTests
{
    [Test]
    public async Task GetSourceIPAddress_reads_ipv4_header()
    {
        var bytes = new byte[]
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x00,
            0x45, 0x00, 0x00, 0x28, 0x00, 0x00, 0x00, 0x00, 0x40, 0x06, 0x00, 0x00,
            192, 168, 0, 1,
            192, 168, 0, 2,
            0x00, 0x50, 0x00, 0x51, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x50, 0x02, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        var packet = Packet.ParsePacket(LinkLayers.Ethernet, bytes);
        var devicePacket = new DevicePacket(null!, packet, DateTime.UtcNow);

        await Assert.That(devicePacket.GetSourceIPAddress()).IsEqualTo(IPAddress.Parse("192.168.0.1"));
        await Assert.That(devicePacket.GetDestinationIPAddress()).IsEqualTo(IPAddress.Parse("192.168.0.2"));
        await Assert.That(devicePacket.IsTcp()).IsTrue();
    }

    [Test]
    public async Task Tcp_helpers_read_ports_flags_and_handshake()
    {
        var packet = WireFishTestPackets.Tcp(
            IPAddress.Parse("10.1.0.1"),
            IPAddress.Parse("10.1.0.2"),
            8080,
            443,
            tcp =>
            {
                tcp.Synchronize = true;
                tcp.SequenceNumber = 42;
                tcp.WindowSize = 8192;
            });

        await Assert.That(packet.GetSourcePort()).IsEqualTo(8080);
        await Assert.That(packet.GetDestinationPort()).IsEqualTo(443);
        await Assert.That(packet.IsTcp()).IsTrue();
        await Assert.That(packet.IsUdp()).IsFalse();
        await Assert.That(packet.IsHttp()).IsTrue();
        await Assert.That(packet.IsTcpHandshake()).IsTrue();
        await Assert.That(packet.GetTcpSequenceNumber()).IsEqualTo(42u);
        await Assert.That(packet.GetTcpWindowSize()).IsEqualTo((ushort)8192);
    }

    [Test]
    public async Task Udp_helpers_detect_dns_and_dhcp()
    {
        var dns = WireFishTestPackets.Udp(
            IPAddress.Parse("192.168.1.10"),
            IPAddress.Parse("8.8.8.8"),
            54321,
            53);
        var dhcp = WireFishTestPackets.Udp(
            IPAddress.Parse("0.0.0.0"),
            IPAddress.Parse("255.255.255.255"),
            68,
            67);

        await Assert.That(dns.IsDnsPacket()).IsTrue();
        await Assert.That(dns.IsUdp()).IsTrue();
        await Assert.That(dhcp.IsDhcpPacket()).IsTrue();
    }

    [Test]
    public async Task Layer_helpers_report_ipv4_ethernet_and_multicast()
    {
        var packet = WireFishTestPackets.Tcp(
            IPAddress.Parse("224.0.0.1"),
            IPAddress.Parse("192.168.0.2"),
            5000,
            5001);

        await Assert.That(packet.IsIpV4Packet()).IsTrue();
        await Assert.That(packet.IsIpV6Packet()).IsFalse();
        await Assert.That(packet.IsEthernetPacket()).IsTrue();
        await Assert.That(packet.GetMacSourceAddress()).IsEqualTo(PhysicalAddress.Parse("001122334455"));
        await Assert.That(packet.GetTimeToLive()).IsNotNull();
        await Assert.That(packet.GetPacketLength()).IsGreaterThan(0);
        await Assert.That(packet.ToHexDump()).Contains("45 00");
    }
}
