using System.Net;
using System.Text;

namespace Frank.WireFish.Tests;

public class DevicePacketSecurityExtensionsTests
{
    [Test]
    public async Task IsTcpNullScan_detects_zero_flag_tcp()
    {
        var packet = WireFishTestPackets.Tcp(
            IPAddress.Parse("10.0.0.1"),
            IPAddress.Parse("10.0.0.2"),
            1000,
            2000);

        await Assert.That(packet.IsTcpNullScan()).IsTrue();
        await Assert.That(packet.IsTcpFinScan()).IsFalse();
        await Assert.That(packet.IsTcpXmasScan()).IsFalse();
    }

    [Test]
    public async Task IsTcpFinScan_detects_fin_only()
    {
        var packet = WireFishTestPackets.Tcp(
            IPAddress.Parse("10.0.0.1"),
            IPAddress.Parse("10.0.0.2"),
            1000,
            2000,
            tcp => tcp.Finished = true);

        await Assert.That(packet.IsTcpFinScan()).IsTrue();
        await Assert.That(packet.IsTcpNullScan()).IsFalse();
    }

    [Test]
    public async Task IsTcpXmasScan_detects_fin_push_urg()
    {
        var packet = WireFishTestPackets.Tcp(
            IPAddress.Parse("10.0.0.1"),
            IPAddress.Parse("10.0.0.2"),
            1000,
            2000,
            tcp =>
            {
                tcp.Finished = true;
                tcp.Push = true;
                tcp.Urgent = true;
            });

        await Assert.That(packet.IsTcpXmasScan()).IsTrue();
    }

    [Test]
    public async Task ContainsSuspiciousPayload_returns_false_for_benign_tcp()
    {
        var packet = WireFishTestPackets.Tcp(
            IPAddress.Parse("192.168.0.1"),
            IPAddress.Parse("192.168.0.2"),
            4000,
            80,
            payload: WireFishTestPackets.AsciiPayload("GET /health HTTP/1.1"));

        await Assert.That(packet.ContainsSuspiciousPayload()).IsFalse();
        await Assert.That(packet.ContainsSqlInjectionAttempt()).IsFalse();
        await Assert.That(packet.IsBufferOverflowAttempt()).IsFalse();
    }

    [Test]
    public async Task IsPingOfDeathAttempt_returns_false_for_normal_tcp()
    {
        var packet = WireFishTestPackets.Tcp(
            IPAddress.Parse("192.168.0.1"),
            IPAddress.Parse("192.168.0.2"),
            443,
            80);

        await Assert.That(packet.IsPingOfDeathAttempt()).IsFalse();
    }

    [Test]
    public async Task IsLandAttack_detects_matching_ip_and_port()
    {
        var address = IPAddress.Parse("203.0.113.10");
        var packet = WireFishTestPackets.Tcp(address, address, 8080, 8080);

        await Assert.That(packet.IsLandAttack()).IsTrue();
    }

    [Test]
    public async Task IsUsingInsecureProtocol_detects_telnet_port()
    {
        var packet = WireFishTestPackets.Tcp(
            IPAddress.Parse("192.168.0.1"),
            IPAddress.Parse("192.168.0.2"),
            50000,
            23);

        await Assert.That(packet.IsUsingInsecureProtocol()).IsTrue();
    }

    [Test]
    public async Task IsIpSpoofed_detects_loopback_source()
    {
        var packet = WireFishTestPackets.Tcp(
            IPAddress.Loopback,
            IPAddress.Parse("192.168.0.2"),
            1000,
            80);

        await Assert.That(packet.IsIpSpoofed()).IsTrue();
    }

}
