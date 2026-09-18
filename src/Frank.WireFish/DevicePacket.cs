using PacketDotNet;
using SharpPcap.LibPcap;

namespace Frank.WireFish;

/// <summary>
/// Represents a device packet containing information about the captured packet.
/// </summary>
/// <param name="Device">The libpcap device that captured the packet.</param>
/// <param name="Packet">Parsed PacketDotNet payload.</param>
/// <param name="Timestamp">Capture timestamp in local time.</param>
public record DevicePacket(LibPcapLiveDevice Device, Packet Packet, DateTime Timestamp);
