# ADR-0008: Build- und Artefaktstruktur

- Status: Accepted
- Datum: 2026-07-15

## Kontext

Astra benötigt reproduzierbare Builds und klar getrennte Ausgaben. Projektlokale Buildpfade, uneinheitliche SDKs und verstreute Paketversionen erschweren Diagnose, CI und Releases.

## Entscheidung

- Das .NET SDK wird über `global.json` festgelegt.
- Repositoryweite Buildregeln liegen in `Directory.Build.props`.
- NuGet-Versionen werden über `Directory.Packages.props` zentral verwaltet.
- Paket-Lockfiles werden committed und CI verwendet Locked Mode.
- Reproduzierbare Ausgaben werden unter `artifacts/` gesammelt.
- `TreatWarningsAsErrors`, Nullable, Analyzer und deterministische Builds sind standardmäßig aktiv.
- CI wird erst mit einer echten Solution angelegt und muss reale Restore-, Build- und Testschritte ausführen.

## Konsequenzen

- Lokale Entwicklung und CI verwenden denselben Buildvertrag.
- Projektdateien bleiben kleiner und konsistenter.
- Unbeabsichtigte Paketgraphänderungen werden sichtbar.
- Buildausgaben lassen sich vollständig entfernen und reproduzieren.
- Ausnahmen von zentralen Regeln benötigen eine dokumentierte Begründung.

## Nicht entschieden

- konkrete Releaseautomatisierung,
- Code Signing,
- Installer-Technologie,
- Single-File, Trimming oder AOT.
