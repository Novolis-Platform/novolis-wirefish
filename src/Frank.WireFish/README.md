<!-- novolis-pkg-brand:start -->
[![Novolis](https://raw.githubusercontent.com/Novolis-Platform/.github/main/brand/logo-icon.png)](https://novolis-platform.github.io/.github/novolis-wirefish/)

[Novolis](https://github.com/Novolis-Platform) · [Docs](https://novolis-platform.github.io/.github/novolis-wirefish/) · [Source](https://github.com/Novolis-Platform/novolis-wirefish)
<!-- novolis-pkg-brand:end -->

# Frank.WireFish

Live packet capture for .NET using SharpPcap, with DI-friendly handlers and Novolis channel integration.

## Install

```bash
dotnet add package Frank.WireFish
```

**Prerequisites:** [.NET 10 SDK](https://dotnet.microsoft.com/download), libpcap-compatible driver (Npcap on Windows).

## Quick start

```csharp
services.AddWireFish(b => b.AddPacketHandler<MyPacketHandler>());
```

```csharp
public sealed class MyPacketHandler : IPacketHandler
{
    public bool CanHandle(DevicePacket packet) => packet.IsTcp();
    public Task HandleAsync(DevicePacket packet, CancellationToken cancellationToken) => Task.CompletedTask;
}
```

## Related packages

| Package | When to use |
|---------|-------------|
| `Novolis.Messaging.Channels` | Channel primitives (pulled in by WireFish) |

## More documentation

- [Getting started](https://github.com/Novolis-Platform/novolis-wirefish/blob/main/docs/getting-started.md)
- [Design](https://github.com/Novolis-Platform/novolis-wirefish/blob/main/docs/design.md)

## Support

Requires elevated permissions or capture capabilities on many OS configurations.

