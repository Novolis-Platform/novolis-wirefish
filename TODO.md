# Frank.WireFish — usability backlog

Migrated from [frankhaugen/Frank.WireFish](https://github.com/frankhaugen/Frank.WireFish) into `novolis-wirefish`. Package and namespace stay **Frank.WireFish**; channel wiring uses **Novolis.Messaging.Channels**.

## Before first stable release

- [ ] **CI without sibling checkout** — switch `Frank.WireFish.csproj` to `PackageReference` on `Novolis.Messaging.Channels` once `0.1.0-preview.1` is published from `novolis-messaging` (today uses a workspace `ProjectReference`).
- [ ] **Capture configuration** — options for interface filter, BPF/display filter, promiscuous mode toggle, and “capture one device” vs all devices (today opens every adapter).
- [ ] **Graceful degradation** — when no capture devices exist or libpcap/Npcap is missing, log clearly and fail startup without hanging hosted services.
- [ ] **Handler pipeline** — replace `GetAwaiter().GetResult()` in `PacketHandler` with async channel writes; consider bounded channel + backpressure.
- [ ] **Integration tests** — port the skipped xUnit capture test behind `[Explicit]` / environment guard (`WIREFISH_INTEGRATION=1`) and document Npcap install for Windows CI.
- [ ] **Samples** — optional `samples/` IRC + file-writer demos from the original repo (not required for the library pack).

## Nice to have

- [ ] **Readme** — link to Novolis registry entry after first NuGet publish.
- [ ] **Personal repo note** — partial-migration banner on `frankhaugen/Frank.WireFish` pointing here (see `novolis-governance/docs/frank-partial-migration.md`).
