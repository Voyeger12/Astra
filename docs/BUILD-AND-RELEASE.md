# Build and Release

## Ziel

Astra wird reproduzierbar gebaut, getestet und veröffentlicht. Lokale Entwicklung, CI und Releases verwenden dieselbe SDK-Linie, dieselben Paketauflösungen und dieselben Qualitätsregeln.

## SDK

- Die SDK-Version wird in `global.json` festgelegt.
- Preview-SDKs sind nicht erlaubt.
- Ein SDK-Wechsel erfolgt bewusst und mit geprüftem Build.

## Zentrale Build-Konfiguration

- `Directory.Build.props` enthält repositoryweite Compiler-, Analyzer- und Ausgaberegeln.
- `Directory.Packages.props` verwaltet NuGet-Versionen zentral.
- Projekte dürfen Paketversionen nicht lokal überschreiben.
- `packages.lock.json` wird für ausführbare Anwendungen und Tests committed.
- CI verwendet `dotnet restore --locked-mode`.

## Artefaktstruktur

Alle reproduzierbaren Ausgaben liegen unter `artifacts/`:

```text
artifacts/
├── bin/
├── obj/
├── publish/
├── package/
├── test-results/
└── logs/
```

`artifacts/` enthält niemals Nutzerdaten, Einstellungen, Datenbanken, Memories, Secrets oder produktive Logs.

## Build-Konfigurationen

- `Debug` dient lokaler Entwicklung und Diagnose.
- `Release` ist für Tests des Veröffentlichungsstands und Releases verbindlich.
- Ein Release darf nicht ausschließlich in `Debug` validiert werden.

## Standardbefehle

```powershell
dotnet restore --locked-mode
dotnet build Astra.sln -c Release --no-restore
dotnet test Astra.sln -c Release --no-build
```

Sobald die Solution existiert, bündeln Skripte unter `scripts/` diese Befehle. Skripte dürfen offizielle `dotnet`-Befehle nur orchestrieren, nicht durch eine eigene Build-Engine ersetzen.

## Qualität

Ein Release-Build muss:

1. ohne Compiler- oder Analyzerwarnungen bestehen,
2. alle deterministischen Tests bestehen,
3. Architekturtests bestehen,
4. relevante Integrationstests bestehen,
5. keine ungeprüften Paketgraphänderungen enthalten,
6. reproduzierbare Artefakte erzeugen.

## Veröffentlichung

Die erste Distributionsform ist:

- Windows x64,
- self-contained,
- ordnerbasierte Veröffentlichung,
- Release-Konfiguration,
- keine Trimming-, AOT- oder Single-File-Optimierung ohne eigenen Spike.

Beispiel nach dem Scaffolding:

```powershell
dotnet publish src/Astra.Desktop/Astra.Desktop.csproj -c Release -r win-x64 --self-contained true --no-restore
```

## Versionierung

Astra verwendet Semantic Versioning. Vor Version 1.0 dürfen sich öffentliche Verträge kontrolliert ändern.

- Patch: kompatible Fehlerkorrektur
- Minor: neue Fähigkeit oder bewusst dokumentierte Vertragsänderung
- Major: inkompatible Änderung nach 1.0

Git-Tags verwenden das Format `v0.1.0`.

## CI

Ein echter GitHub-Actions-Workflow wird zusammen mit der Solution angelegt. Er muss mindestens Restore im Locked Mode, Release-Build, Tests, Format- und Analyzerprüfung ausführen. Ein No-op-Workflow ohne Solution ist nicht erlaubt, weil er falsche Sicherheit erzeugt.

## Release-Gate

Kein Release bei:

- versteckten Warnungen,
- fehlgeschlagenen oder deaktivierten Tests,
- ungeklärten Migrationen,
- nicht dokumentierten Paketänderungen,
- nicht reproduzierbaren Buildschritten,
- ungeprüften Runtime-Datenpfaden,
- fehlender Rückfallstrategie bei Datenmigrationen.
