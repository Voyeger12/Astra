# Architektur

## Architekturziel

Astra wird als modularer Monolith mit klaren Schichtgrenzen aufgebaut. Die Anwendung bleibt in einer deploybaren Desktop-Anwendung, ohne ihre Fachlogik an WPF, Ollama, SQLite oder ein einzelnes Agent Framework zu koppeln.

## Solution-Struktur

```text
Astra.sln

src/
├── Astra.Desktop
├── Astra.Application
├── Astra.Domain
├── Astra.AgentFramework
├── Astra.Infrastructure
└── Astra.Tools

tests/
├── Astra.Application.Tests
├── Astra.AgentFramework.Tests
├── Astra.Infrastructure.Tests
└── Astra.AgentEvals
```

## Verantwortlichkeiten

### Astra.Desktop

- WPF Views und ViewModels
- UI-State, Commands und Darstellung
- keine direkte Modell-, Datenbank- oder MCP-Kommunikation

### Astra.Application

- Use Cases und Ports
- `IAstraAgent`
- `IAstraSessionService`
- `IMemoryService`
- `IToolApprovalService`
- `IAstraStatusService`
- Orchestrierung ohne Infrastrukturdetails

### Astra.Domain

- Agent-, Session-, Memory- und Tool-Modelle
- Value Objects und fachliche Regeln
- keine Abhängigkeit auf UI, Datenbank oder Netzwerk

### Astra.AgentFramework

- Adapter zum Microsoft Agent Framework
- Agent Instructions
- Tool-Registrierung
- Sessions und Context Provider
- Middleware und Eventübersetzung
- keine Fachlogik in Tool-Wrappern

### Astra.Infrastructure

- Ollama und `IChatClient`
- SQLite-Repositories
- Konfiguration, Logging und OpenTelemetry
- MCP-Client und externe Adapter
- Betriebssystemnahe Dienste

### Astra.Tools

- schmale Agentenadapter auf Application-Services
- Tool-Metadaten und Risikoklassen
- keine freie Shell im Kern

## Abhängigkeitsregel

```text
Desktop → Application → Domain
AgentFramework → Application + Domain
Infrastructure → Application + Domain
Tools → Application + Domain
```

`Domain` referenziert keine andere Projektschicht. `Application` kennt nur Domain-Typen und eigene Interfaces.

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

Der Ereignisstrom muss mindestens Text-Deltas, Tool-Anfragen, Freigaben, Tool-Ergebnisse, Statusänderungen, Abschluss und Fehler abbilden.

## Lifecycle

`Microsoft.Extensions.Hosting` besitzt den Application-Lifecycle. Alle lang laufenden Komponenten unterstützen `CancellationToken` und werden über Dependency Injection erzeugt. Globale mutable Singletons und versteckte Initialisierung beim Import beziehungsweise beim Laden einer Klasse sind nicht erlaubt.

## Tools

Tools geben dem Agenten kontrollierten Zugriff auf normale Application-Services. Sie enthalten keine primäre Fachlogik.

Jedes Tool besitzt:

- eindeutigen Namen und Zweck
- typisierte Parameter und Ergebnisse
- Risikoklasse
- definierte Freigaberegel
- Timeout und Cancellation
- Audit-Ereignisse

## Vertikale Entwicklung

Neue Funktionen werden nicht nur als isoliertes Modul angelegt. Ein Feature umfasst immer den vollständigen Weg von UI oder Agenteneingabe über Application und Infrastruktur bis zur Fehlerbehandlung und zu Tests.
