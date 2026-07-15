# Astra

Astra ist ein lokal-first persönlicher KI-Agent für Windows. Das Projekt wird als moderne C#/.NET-Anwendung neu aufgebaut und verbindet lokale Sprachmodelle, kontrollierte Tools, persistente Sitzungen, langfristiges Gedächtnis und später optionale Wahrnehmung über Kamera und Sprache.

## Status

Astra befindet sich in der Architektur- und Bootstrap-Phase. Der frühere Python-Prototyp bleibt im separaten Repository `ASTRA-OS` als Ideenarchiv, Fehlerkatalog und Referenz erhalten. Dieses Repository ist kein 1:1-Port, sondern ein kontrollierter Neuaufbau.

## Leitprinzipien

- Agent-first statt später nachgerüsteter Agentenlogik
- Local-first und offline-fähig mit Ollama
- Ein kontrollierter Agent vor Multi-Agent-Orchestrierung
- Tools statt freier Shell-Befehle
- Explizite Freigaben für verändernde und riskante Aktionen
- Klare Schichten und austauschbare Infrastruktur
- Vertikale, vollständig getestete Funktionen statt vieler halb verbundener Module
- Datenschutz als durchgesetzte Systemregel, nicht nur als UI-Schalter
- Korrektheit und begrenzter Blast Radius vor Entwicklungsgeschwindigkeit
- Root-Cause-Fixes statt Symptombehandlung

## Geplanter Techstack

- .NET 10 LTS
- C#
- WPF und MVVM
- CommunityToolkit.Mvvm
- Microsoft.Extensions.Hosting und Dependency Injection
- Microsoft Agent Framework
- Microsoft.Extensions.AI
- OllamaSharp als erster lokaler Modellprovider
- Model Context Protocol für externe Integrationen
- SQLite für lokale Persistenz
- xUnit, FluentAssertions und Test-Doubles für Agent Contract Tests
- OpenTelemetry für Tracing und Diagnose

## Zielarchitektur

```text
Astra.Desktop
    ↓
Astra.Application
    ↓
Astra.Domain

Astra.AgentFramework ─┐
Astra.Infrastructure ├─ implementieren Application-Interfaces
Astra.Tools          ┘
```

Die Oberfläche kennt weder Ollama noch SQLite noch MCP direkt. Der Agent Loop wird vom Agent Framework betrieben. Astra behält die Kontrolle über Fachlogik, Tool-Berechtigungen, Daten, Lifecycle, Sicherheitsrichtlinien und Providerkonfiguration.

## Erster Meilenstein

Der erste vollständige Ablauf umfasst:

1. WPF-Anwendung und Generic Host starten.
2. Ollama-Verfügbarkeit erkennen.
3. Agent Session laden oder anlegen.
4. Nachricht an einen einzelnen Astra-Agenten senden.
5. Optional ein ungefährliches Tool aufrufen.
6. Antwort streamen und darstellen.
7. Sitzung und Nachrichten in SQLite persistieren.
8. Lauf abbrechen und Anwendung sauber herunterfahren.

Noch nicht Bestandteil des ersten Meilensteins sind Kamera, Mikrofon, TTS, freie Shell, Multi-Agent-Orchestrierung und autonome Hintergrundaktivität.

## Repository-Regelwerk

Die Regeln gelten für Menschen, Copilot, Coding-Agenten und automatisierte Änderungen:

- [Mitwirkung und Änderungsprozess](CONTRIBUTING.md)
- [Sicherheitsregeln](SECURITY.md)
- [Technische Entwicklungsstandards](docs/DEVELOPMENT.md)
- [Verbindliche Architekturregeln](docs/ARCHITECTURE_RULES.md)

Korrektheit, Nachvollziehbarkeit und begrenzte Auswirkungen haben Vorrang vor Geschwindigkeit. Warnungen werden nicht versteckt, Bugfixes beheben die identifizierte Root Cause und Tests dürfen nicht manipuliert werden, um einen grünen Zustand zu erzeugen.

## Weitere Dokumentation

- [Vision](docs/VISION.md)
- [Architektur](docs/ARCHITECTURE.md)
- [Techstack](docs/TECH-STACK.md)
- [Daten- und Zustandslogik](docs/DATA-AND-STATE.md)
- [Architecture Decision Records](docs/adr/README.md)
- [Repository Skills](.github/skills/README.md)

## Entwicklungsregel

Jede neue Fähigkeit wird als vollständiger vertikaler Ablauf umgesetzt und getestet. Eine Funktion gilt erst als vorhanden, wenn UI, Application Use Case, Agent- oder Tool-Pfad, Persistenz, Fehlerbehandlung und Tests zusammen funktionieren.