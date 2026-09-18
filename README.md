<!-- novolis-marketing:start -->
<p align="center">
  <a href="https://github.com/Novolis-Platform">
    <img src="https://raw.githubusercontent.com/Novolis-Platform/.github/main/brand/logo-brand-transparent.svg" width="360" alt="Novolis"/>
  </a>
</p>

<p align="center">
  <img src="https://raw.githubusercontent.com/Novolis-Platform/.github/main/brand/banners/novolis-wirefish.svg" width="100%" alt="novolis-wirefish"/>
</p>

<p align="center">
  <strong>Wire inspection tooling</strong><br/>
  Wire/protocol inspection helpers for Novolis debugging.
</p>

<p align="center">
  <a href="https://github.com/Novolis-Platform/novolis-wirefish/actions"><img src="https://img.shields.io/github/actions/workflow/status/Novolis-Platform/novolis-wirefish/merge.yml?branch=main&label=merge&logo=github" alt="merge"/></a>
  <a href="https://github.com/orgs/Novolis-Platform/packages?repo_name=novolis-wirefish"><img src="https://img.shields.io/badge/packages-GitHub%20Packages-0a7ea3?logo=nuget" alt="packages"/></a>
  <a href="https://github.com/Novolis-Platform"><img src="https://img.shields.io/badge/org-Novolis--Platform-111827" alt="org"/></a>
</p>

<p align="center">
  <a href="https://nuget.pkg.github.com/Novolis-Platform/index.json"><code>https://nuget.pkg.github.com/Novolis-Platform/index.json</code></a>
  ·
  <a href="https://github.com/Novolis-Platform/.github/blob/main/profile/README.md">Org landing</a>
  ·
  <a href="https://github.com/Novolis-Platform/novolis-governance">Governance</a>
</p>

---
<!-- novolis-marketing:end -->
# WireFish moved to novolis-transports

**`Frank.WireFish` / `novolis-wirefish` is consolidated into [novolis-transports](https://github.com/Novolis-Platform/novolis-transports).**

Use package **`Novolis.Transports.WireFish`** (namespace `Novolis.Transports.WireFish`) instead of maintaining a separate repo.

## Install

```bash
dotnet add package Novolis.Transports.WireFish
```

**Prerequisites:** [.NET 10 SDK](https://dotnet.microsoft.com/download) (`net10.0`), Npcap (Windows) or libpcap for live capture.

## Quick start

```csharp
using Novolis.Transports.WireFish;

builder.Services.AddNovolisWireFish(
    w => w.AddPacketHandler<MyPacketHandler>(),
    o => o.BpfFilter = "tcp port 443");
```

Handler template:

```csharp
public sealed class MyPacketHandler : IPacketHandler
{
    public bool CanHandle(DevicePacket packet) => packet.IsTcp();
    public Task HandleAsync(DevicePacket packet, CancellationToken cancellationToken) => Task.CompletedTask;
}
```

Device discovery:

```csharp
var devices = WireFishCaptureDevices.List();
var health = WireFishCaptureHealthChecks.Check();
```

## API (in `Novolis.Transports.WireFish`)

| Type | Role |
|------|------|
| `AddNovolisWireFish` | DI registration + hosted capture |
| `IPacketHandler` | `CanHandle`, `HandleAsync` |
| `DevicePacket` | Captured packet unit |
| `WireFishOptions` | Devices, BPF filter, promiscuous mode |
| `WireFishCaptureDevices` | Enumerate capture interfaces |
| `WireFishCaptureHealthChecks` | Driver/service readiness |
| `DevicePacketExtensions` | TCP/IP formatting, security heuristics |

Obsolete `Frank.WireFish` type aliases ship in the same package for one preview cycle.

## Dogfooding / apps

**WireFishViewer** in `novolis-dogfooding` uses this package for live capture UI.

## Related

| Package / repo | Role |
|----------------|------|
| [Novolis.Transports.WireFish README](https://github.com/Novolis-Platform/novolis-transports/blob/main/src/Novolis.Transports.WireFish/README.md) | Full package documentation |
| `Novolis.Messaging.Channels` | `Channel<DevicePacket>` pipeline |
| `novolis-transports` | Active development repo |

This repository is retained only as a redirect; new work happens in **novolis-transports**.

