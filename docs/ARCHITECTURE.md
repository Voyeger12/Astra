# Architektur

## Architekturziel

Astra wird als modularer Monolith mit klaren Schichtgrenzen aufgebaut. Die Anwendung bleibt in einer deploybaren Desktop-Anwendung, ohne ihre Fachlogik an WPF, Ollama, SQLite oder ein einzelnes Agent Framework zu koppeln.

## Solution-Struktur

```text
Astra.slnx

src/
├── Astra.Desktop
├── Astra.Presentation
├── Astra.Application
├── Astra.Domain
├── Astra.AgentFramework
├── Astra.Infrastructure
└── Astra.Tools

tests/
└── Astra.Architecture.Tests
```

Weitere Testprojekte werden erst mit echten Tests angelegt.

## Verantwortlichkeiten

### Astra.Desktop

- ausführbare WPF-Anwendung
- Composition Root
- Generic Host und Dependency Injection
- kontrollierter Start und Shutdown
- letzte globale Exceptiongrenze
- keine ViewModel- oder Fachlogik

### Astra.Presentation

- WPF Views und ViewModels
- UI-State, Commands und Darstellung
- referenziert ausschließlich `Astra.Application`
- zeigt sichere Fehlerzustände ohne Stacktraces oder Infrastrukturdetails
- keine direkte Modell-, Datenbank-, MCP- oder Agent-Framework-Kommunikation

### Astra.Application

- Use Cases und Ports
- `IAstraAgent`
- `IAstraSessionService`
- `IMemoryService`
- `IToolApprovalService`
- `IAstraStatusService`
- öffentliche Fehlercodes und sichere Fehlerverträge
- Audit-Verträge und Korrelationskontext
- Orchestrierung ohne Infrastrukturdetails

### Astra.Domain

- Agent-, Session-, Memory- und Tool-Modelle
- Value Objects und fachliche Regeln
- deterministische fachliche Fehlerzustände
- keine Abhängigkeit auf UI, Datenbank, Netzwerk oder konkretes Logging

### Astra.AgentFramework

- Adapter zum Microsoft Agent Framework
- Agent Instructions
- Tool-Registrierung
- Sessions und Context Provider
- Middleware und Eventübersetzung
- Übersetzung von Frameworkfehlern in Astra-Verträge
- keine Fachlogik in Tool-Wrappern

### Astra.Infrastructure

- Ollama und `IChatClient`
- SQLite-Repositories
- Konfiguration, konkrete Logger und OpenTelemetry
- lokale Log- und Trace-Speicherung
- technische Exception-Zuordnung an Adaptergrenzen
- MCP-Client und externe Adapter
- betriebssystemnahe Dienste

### Astra.Tools

- schmale Agentenadapter auf Application-Services
- Tool-Metadaten und Risikoklassen
- sichere Audit-Ereignisse für Freigabe und Ausführung
- keine freie Shell im Kern

## Abhängigkeitsregel

```text
Desktop → Presentation + AgentFramework + Infrastructure + Tools
Presentation → Application
Application → Domain
AgentFramework → Application + Domain
Infrastructure → Application + Domain
Tools → Application + Domain
Domain → keine Projektreferenz
```

Nur `Astra.Desktop` darf als Composition Root konkrete Adapterprojekte zusammenführen. Views und ViewModels bleiben davon getrennt.

## Agent Runtime

Die UI kommuniziert ausschließlich über einen anwendungsnahen Vertrag:

```csharp
public interface IAstraAgent
{
    IAsyncEnumerable<AstraAgentEvent> RunAsync(
        AstraAgentRequest request,
        CancellationToken cancellationToken);
}
```

Der Ereignisstrom muss mindestens Text-Deltas, Tool-Anfragen, Freigaben, Tool-Ergebnisse, Statusänderungen, Abschluss und sichere Fehlerereignisse abbilden.

## Fehlergrenzen

- Domain und Application kennen keine HTTP-, SQLite-, Ollama- oder Framework-Exceptiontypen.
- AgentFramework, Infrastructure und Tools übersetzen technische Fehler an ihren Adaptergrenzen in stabile Astra-Fehlercodes.
- Exceptions werden an der verantwortlichen Boundary mit technischem Kontext einmalig protokolliert.
- Presentation erhält ausschließlich sichere Fehlerdaten und mögliche nächste Schritte.
- Desktop behandelt Start-, Host-, Shutdown- und letzte unbehandelte Prozessfehler.
- Cancellation, Timeout, Benutzerablehnung und unerwartete Fehler bleiben unterscheidbar.

Die verbindlichen Regeln stehen in `docs/ERROR-HANDLING.md`.

## Logging und Observability

Astra verwendet `Microsoft.Extensions.Logging` als gemeinsame Abstraktion. Konkrete Logger, Dateirotation und OpenTelemetry werden ausschließlich in Infrastructure konfiguriert und im Desktop-Composition-Root registriert.

Logs, Traces und Audit-Ereignisse bleiben getrennte Signale. Sie verwenden strukturierte Event-IDs, sichere Fehlercodes und nicht personenbezogene Korrelations-IDs. Gesprächsinhalte, vollständige Prompts, Tool-Argumente, Dateiinhalte, vollständige lokale Pfade und Secrets werden nicht ungeprüft erfasst.

Die verbindlichen Regeln stehen in `docs/LOGGING-AND-OBSERVABILITY.md`.

## Lifecycle

`Microsoft.Extensions.Hosting` besitzt später den Application-Lifecycle. Alle lang laufenden Komponenten unterstützen `CancellationToken` und werden über Dependency Injection erzeugt. Globale mutable Singletons und versteckte Initialisierung beim Laden einer Klasse sind nicht erlaubt.

Start, Agentenlauf, Cancellation, Providerausfall und kontrollierter Shutdown müssen über Korrelationsdaten diagnostizierbar sein, ohne private Inhalte zu protokollieren.

## Tools

Tools geben dem Agenten kontrollierten Zugriff auf normale Application-Services. Sie enthalten keine primäre Fachlogik.

Jedes Tool besitzt:

- eindeutigen Namen und Zweck
- typisierte Parameter und Ergebnisse
- Risikoklasse
- definierte Freigaberegel
- Timeout und Cancellation
- sichere Audit-Ereignisse
- definierte Fehlercodes

## Vertikale Entwicklung

Neue Funktionen werden nicht nur als isoliertes Modul angelegt. Ein Feature umfasst immer den vollständigen Weg von UI oder Agenteneingabe über Application und Infrastruktur bis zu sicherer Fehlerbehandlung, Diagnose und Tests.
