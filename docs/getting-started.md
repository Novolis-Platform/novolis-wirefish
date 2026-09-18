# Getting started

**Packet capture lives in [novolis-transports](https://github.com/Novolis-Platform/novolis-transports)** as `Novolis.Transports.WireFish`.

This repository is a public redirect for the former `novolis-wirefish` / `Frank.WireFish` tree. Do not add a package reference to `Frank.WireFish`.

## Install

```bash
dotnet add package Novolis.Transports.WireFish
```

**Prerequisites:** [.NET 10 SDK](https://dotnet.microsoft.com/download) (`net10.0`), Npcap (Windows) or libpcap for live capture, GitHub Packages auth for `Novolis.*`.

Configure GPR once from a sibling `novolis-governance` checkout:

```powershell
pwsh -File d:\novolis\novolis-governance\scripts\configure-gpr-user-nuget.ps1
```

## Next

- [Novolis.Transports.WireFish README](https://github.com/Novolis-Platform/novolis-transports/blob/main/src/Novolis.Transports.WireFish/README.md)
- [design.md](design.md)
- [release.md](release.md)
