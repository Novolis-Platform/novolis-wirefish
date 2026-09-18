using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using PacketDotNet;
using PacketDotNet.Utils;

namespace Frank.WireFish.Tests;

internal static class WireFishTestPackets
{
    public static DevicePacket Create(EthernetPacket ethernet)
    {
        var packet = Packet.ParsePacket(LinkLayers.Ethernet, ethernet.Bytes);
        return new DevicePacket(null!, packet, new DateTime(2024, 6, 1, 12, 0, 0, DateTimeKind.Utc));
    }

    public static DevicePacket Tcp(
        IPAddress source,
        IPAddress destination,
        int sourcePort,
        int destinationPort,
        Action<TcpPacket>? configure = null,
        byte[]? payload = null)
    {
        var tcp = new TcpPacket((ushort)sourcePort, (ushort)destinationPort);
        configure?.Invoke(tcp);
        if (payload is not null)
            tcp.PayloadData = payload;

        var eth = new EthernetPacket(
            PhysicalAddress.Parse("001122334455"),
            PhysicalAddress.Parse("AABBCCDDEEFF"),
            EthernetType.IPv4)
        {
            PayloadPacket = new IPv4Packet(source, destination)
            {
                PayloadPacket = tcp,
            },
        };

        return Create(eth);
    }

    public static DevicePacket Udp(
        IPAddress source,
        IPAddress destination,
        int sourcePort,
        int destinationPort,
        byte[]? payload = null)
    {
        var udp = new UdpPacket((ushort)sourcePort, (ushort)destinationPort);
        if (payload is not null)
            udp.PayloadData = payload;

        var eth = new EthernetPacket(
            PhysicalAddress.Parse("001122334455"),
            PhysicalAddress.Parse("AABBCCDDEEFF"),
            EthernetType.IPv4)
        {
            PayloadPacket = new IPv4Packet(source, destination)
            {
                PayloadPacket = udp,
            },
        };

        return Create(eth);
    }

    public static DevicePacket TcpWithOptions(byte[] options)
    {
        var tcp = new TcpPacket((ushort)4000, (ushort)4001)
        {
            Options = options,
        };

        var eth = new EthernetPacket(
            PhysicalAddress.Parse("001122334455"),
            PhysicalAddress.Parse("AABBCCDDEEFF"),
            EthernetType.IPv4)
        {
            PayloadPacket = new IPv4Packet(IPAddress.Parse("192.168.0.1"), IPAddress.Parse("192.168.0.2"))
            {
                PayloadPacket = tcp,
            },
        };

        return Create(eth);
    }

    public static DevicePacket WithRootPayload(byte[] payload)
    {
        var eth = new EthernetPacket(
            PhysicalAddress.Parse("001122334455"),
            PhysicalAddress.Parse("AABBCCDDEEFF"),
            EthernetType.IPv4)
        {
            PayloadData = payload,
        };

        return new DevicePacket(null!, eth, new DateTime(2024, 6, 1, 12, 0, 0, DateTimeKind.Utc));
    }

    public static DevicePacket WithIpPayload(byte[] payload)
    {
        var eth = new EthernetPacket(
            PhysicalAddress.Parse("001122334455"),
            PhysicalAddress.Parse("AABBCCDDEEFF"),
            EthernetType.IPv4)
        {
            PayloadPacket = new IPv4Packet(IPAddress.Parse("192.168.0.1"), IPAddress.Parse("192.168.0.2"))
            {
                PayloadData = payload,
            },
        };

        return Create(eth);
    }

    public static DevicePacket WithPayload(byte[] payload) =>
        Tcp(
            IPAddress.Parse("192.168.0.1"),
            IPAddress.Parse("192.168.0.2"),
            4000,
            4001,
            payload: payload);

    public static byte[] AsciiPayload(string text) => Encoding.ASCII.GetBytes(text);

    public static DevicePacket Arp()
    {
        var arp = new ArpPacket(ArpOperation.Request,
            PhysicalAddress.Parse("001122334455"),
            IPAddress.Parse("192.168.0.1"),
            PhysicalAddress.Parse("000000000000"),
            IPAddress.Parse("192.168.0.2"));

        var eth = new EthernetPacket(
            PhysicalAddress.Parse("001122334455"),
            PhysicalAddress.Parse("FFFFFFFFFFFF"),
            EthernetType.Arp)
        {
            PayloadPacket = arp,
        };

        return Create(eth);
    }

    public static DevicePacket MulticastEthernetUdp()
    {
        return Udp(
            IPAddress.Parse("192.168.0.1"),
            IPAddress.Parse("224.0.0.1"),
            5000,
            5001,
            payload: [1, 2, 3],
            destinationMac: PhysicalAddress.Parse("01005E000001"));
    }

    private static DevicePacket Udp(
        IPAddress source,
        IPAddress destination,
        int sourcePort,
        int destinationPort,
        byte[]? payload,
        PhysicalAddress destinationMac)
    {
        var udp = new UdpPacket((ushort)sourcePort, (ushort)destinationPort);
        if (payload is not null)
            udp.PayloadData = payload;

        var eth = new EthernetPacket(
            PhysicalAddress.Parse("001122334455"),
            destinationMac,
            EthernetType.IPv4)
        {
            PayloadPacket = new IPv4Packet(source, destination)
            {
                PayloadPacket = udp,
            },
        };

        return Create(eth);
    }
}
