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
- keine ViewModel- oder Fachlogik

### Astra.Presentation

- WPF Views und ViewModels
- UI-State, Commands und Darstellung
- referenziert ausschließlich `Astra.Application`
- keine direkte Modell-, Datenbank-, MCP- oder Agent-Framework-Kommunikation

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
- betriebssystemnahe Dienste

### Astra.Tools

- schmale Agentenadapter auf Application-Services
- Tool-Metadaten und Risikoklassen
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

Der Ereignisstrom muss mindestens Text-Deltas, Tool-Anfragen, Freigaben, Tool-Ergebnisse, Statusänderungen, Abschluss und Fehler abbilden.

## Lifecycle

`Microsoft.Extensions.Hosting` besitzt später den Application-Lifecycle. Alle lang laufenden Komponenten unterstützen `CancellationToken` und werden über Dependency Injection erzeugt. Globale mutable Singletons und versteckte Initialisierung beim Laden einer Klasse sind nicht erlaubt.

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
