using System.Net;
using System.Text;

namespace Frank.WireFish.Tests;

public sealed class DevicePacketSecurityBehaviorTests
{
    [Test]
    public async Task ContainsSuspiciousPayload_detects_known_patterns()
    {
        var packet = WireFishTestPackets.WithRootPayload(WireFishTestPackets.AsciiPayload("contains malicious payload"));
        await Assert.That(packet.ContainsSuspiciousPayload()).IsTrue();
    }

    [Test]
    public async Task ContainsSuspiciousPayload_detects_nop_sled()
    {
        var payload = new byte[10];
        Array.Fill(payload, (byte)0x90);
        var packet = WireFishTestPackets.WithRootPayload(payload);
        await Assert.That(packet.ContainsSuspiciousPayload()).IsTrue();
    }

    [Test]
    public async Task ContainsSqlInjectionAttempt_detects_pattern()
    {
        var packet = WireFishTestPackets.WithRootPayload(Encoding.UTF8.GetBytes("username' OR '1'='1"));
        await Assert.That(packet.ContainsSqlInjectionAttempt()).IsTrue();
    }

    [Test]
    public async Task IsBufferOverflowAttempt_detects_repetitive_payload()
    {
        var payload = new byte[1001];
        Array.Fill(payload, (byte)'A');
        var packet = WireFishTestPackets.WithRootPayload(payload);
        await Assert.That(packet.IsBufferOverflowAttempt()).IsTrue();
    }

    [Test]
    public async Task IsIpSpoofed_detects_broadcast_source()
    {
        var packet = WireFishTestPackets.Tcp(
            IPAddress.Broadcast,
            IPAddress.Parse("192.168.0.2"),
            1000,
            80);
        await Assert.That(packet.IsIpSpoofed()).IsTrue();
    }

    [Test]
    public async Task IsIpSpoofed_detects_private_source()
    {
        var packet = WireFishTestPackets.Tcp(
            IPAddress.Parse("10.0.0.5"),
            IPAddress.Parse("8.8.8.8"),
            1000,
            80);
        await Assert.That(packet.IsIpSpoofed()).IsTrue();
    }

    [Test]
    public async Task IsUsingInsecureProtocol_detects_ftp()
    {
        var packet = WireFishTestPackets.Tcp(
            IPAddress.Parse("192.168.0.1"),
            IPAddress.Parse("192.168.0.2"),
            50000,
            21);
        await Assert.That(packet.IsUsingInsecureProtocol()).IsTrue();
    }

    [Test]
    public async Task IsLandAttack_false_for_different_ports()
    {
        var address = IPAddress.Parse("203.0.113.10");
        var packet = WireFishTestPackets.Tcp(address, address, 8080, 8081);
        await Assert.That(packet.IsLandAttack()).IsFalse();
    }

    [Test]
    public async Task HasSuspiciousTcpOptions_returns_false_for_default_tcp()
    {
        var packet = WireFishTestPackets.Tcp(
            IPAddress.Parse("192.168.0.1"),
            IPAddress.Parse("192.168.0.2"),
            1000,
            80);
        await Assert.That(packet.HasSuspiciousTcpOptions()).IsFalse();
    }

    [Test]
    public async Task IsPingOfDeathAttempt_and_oversized_icmp_return_false_for_tcp()
    {
        var packet = WireFishTestPackets.Tcp(
            IPAddress.Parse("192.168.0.1"),
            IPAddress.Parse("192.168.0.2"),
            443,
            80);
        await Assert.That(packet.IsPingOfDeathAttempt()).IsFalse();
        await Assert.That(packet.IsOversizedIcmpPacket()).IsFalse();
    }
}
