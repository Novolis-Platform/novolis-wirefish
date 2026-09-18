using System.Net;
using System.Text;
using PacketDotNet;

namespace Frank.WireFish.Tests;

public sealed class DevicePacketExtensionsBehaviorTests
{
    [Test]
    public async Task GetProtocol_and_payload_helpers()
    {
        var packet = WireFishTestPackets.Tcp(
            IPAddress.Parse("10.0.0.1"),
            IPAddress.Parse("10.0.0.2"),
            4000,
            80,
            tcp => tcp.Acknowledgment = true,
            payload: WireFishTestPackets.AsciiPayload("GET /"));

        await Assert.That(packet.GetProtocol()).IsEqualTo(ProtocolType.Tcp);
        await Assert.That(packet.GetTcpFlags()).IsNotNull();
        await Assert.That(packet.GetMacDestinationAddress()).IsNotNull();
        await Assert.That(packet.GetEthernetType()).IsNotNull();
        await Assert.That(packet.GetIpTotalLength()).IsNotNull();
        await Assert.That(packet.GetPacketSummary()).IsNotNullOrEmpty();
    }

    [Test]
    public async Task Http_on_port_80_detected()
    {
        var packet = WireFishTestPackets.Tcp(
            IPAddress.Parse("192.168.0.1"),
            IPAddress.Parse("192.168.0.2"),
            50000,
            80);
        await Assert.That(packet.IsHttp()).IsTrue();
    }

    [Test]
    public async Task Arp_detection()
    {
        var arp = WireFishTestPackets.Arp();
        await Assert.That(arp.IsArpPacket()).IsTrue();
        await Assert.That(arp.IsIpV4Packet()).IsFalse();
    }

    [Test]
    public async Task Multicast_ethernet_detected()
    {
        var multicast = WireFishTestPackets.MulticastEthernetUdp();
        await Assert.That(multicast.IsMulticast()).IsTrue();
    }

    [Test]
    public async Task Udp_port_helpers()
    {
        var packet = WireFishTestPackets.Udp(
            IPAddress.Parse("192.168.0.1"),
            IPAddress.Parse("192.168.0.2"),
            1234,
            5678);

        await Assert.That(packet.GetSourcePort()).IsEqualTo(1234);
        await Assert.That(packet.GetDestinationPort()).IsEqualTo(5678);
        await Assert.That(packet.IsTcp()).IsFalse();
        await Assert.That(packet.IsDnsPacket()).IsFalse();
    }

    [Test]
    public async Task Tcp_handshake_false_when_ack_set()
    {
        var packet = WireFishTestPackets.Tcp(
            IPAddress.Parse("10.0.0.1"),
            IPAddress.Parse("10.0.0.2"),
            1000,
            2000,
            tcp =>
            {
                tcp.Synchronize = true;
                tcp.Acknowledgment = true;
            });

        await Assert.That(packet.IsTcpHandshake()).IsFalse();
    }

    [Test]
    public async Task Igmp_detection_returns_false_for_tcp()
    {
        var packet = WireFishTestPackets.Tcp(
            IPAddress.Parse("10.0.0.1"),
            IPAddress.Parse("10.0.0.2"),
            1000,
            2000);
        await Assert.That(packet.IsIgmpPacket()).IsFalse();
    }
}
