# ADR-013: First Local Chat als erster vertikaler Slice

## Status

Accepted

## Kontext

Astra benötigt als ersten echten Produktdurchstich einen lokalen Chat, der die vollständige Strecke von WPF über Application-Verträge und Microsoft Agent Framework bis zu Ollama abbildet. Der Slice soll Streaming, Cancellation, sichere Fehler, Providerstatus und flüchtigen Multi-Turn-Kontext beweisen, ohne Persistenz, Tools oder weitere Kernfunktionen vorwegzunehmen.

## Entscheidung

- `Astra.Application` definiert einen frameworkunabhängigen Ereignisstrom über `IAstraAgent`.
- Jeder Lauf besitzt `OperationId` und `AgentRunId`; providerbezogene Ereignisse können zusätzlich eine `ProviderCallId` tragen.
- Jeder Lauf endet genau fachlich mit Completed, Cancelled oder Failed.
- Microsoft Agent Framework wird ausschließlich in `Astra.AgentFramework` adaptiert.
- OllamaSharp wird ausschließlich in `Astra.Infrastructure` als `IChatClient`-Provider integriert.
- Eine Agent-Session bleibt während der laufenden Desktop-Anwendung erhalten, wird in diesem Slice aber nicht persistiert.
- Presentation kennt ausschließlich Application-Verträge.
- Prompts und Antworten werden weder in Logs noch in Diagnosemetadaten geschrieben.

## Bewusst nicht enthalten

- SQLite und Sessionpersistenz
- Memory
- Tools, Freigaben und MCP
- Markdown-Rendering
- Modellinstallation oder automatischer Modelldownload
- OpenTelemetry-Exporter

## Folgen

Der Slice beweist einen vollständigen lokalen Chatpfad mit kleinem Fehlersuchraum. Persistenz, Tools und weitere Fähigkeiten werden als eigene vertikale Slices auf einem getesteten Chatkern aufgebaut.
