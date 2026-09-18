# Design

Wire/protocol inspection is a **transport-layer** concern. The live package is `Novolis.Transports.WireFish` in [novolis-transports](https://github.com/Novolis-Platform/novolis-transports).

This repository keeps a public GitHub home and leftover `Frank.WireFish` sources for history. New work does not land here.

## Layer placement

Follow [library-boundaries](https://github.com/Novolis-Platform/novolis-governance/blob/main/docs/library-boundaries.md). Capture adapters sit with other transports (HTTP, IPC, torrent). They must not take Avalonia package references.

## Goals

- Point consumers at `Novolis.Transports.WireFish`.
- Keep the leftover tree buildable for local inspection.

## Non-goals

- Publishing `Frank.WireFish` to GitHub Packages.
- A second live capture implementation beside `novolis-transports`.

## Topics

- `dotnet`
- `networking`
- `novolis`
