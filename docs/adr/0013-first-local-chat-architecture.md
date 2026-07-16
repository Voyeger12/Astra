# ADR-013: First Local Chat Architecture

## Status

Accepted

## Kontext

Astra benötigt als ersten vertikalen Produktschnitt einen lokal ausgeführten, mehrteiligen Chat. Der Schnitt muss den vollständigen Weg von WPF über Application und Microsoft Agent Framework bis zu Ollama beweisen, ohne Persistenz, Tools oder weitere Kernfunktionen vorwegzunehmen.

## Entscheidung

- Die Benutzeroberfläche kommuniziert ausschließlich über `IAstraAgent` und einen typisierten `IAsyncEnumerable<AstraAgentEvent>`.
- `Astra.AgentFramework` verwendet `Microsoft.Agents.AI` und hält eine flüchtige `AgentSession` für die Laufzeit des Fensters.
- `Astra.Infrastructure` stellt Ollama über OllamaSharp als `IChatClient` bereit und prüft Dienst- sowie Modellverfügbarkeit.
- `Astra.Presentation` bleibt frei von Agent-Framework-, Ollama- und HTTP-Typen.
- Antworten werden als Text-Deltas gestreamt.
- Jeder Lauf endet exakt mit `Completed`, `Cancelled` oder `Failed`.
- Cancellation, Timeout, ProviderUnavailable und ModelNotFound bleiben unterscheidbar.
- Gesprächsinhalte werden nicht in operative Logs geschrieben.

## Folgen

Der erste Slice liefert einen echten, flüchtigen Multi-Turn-Chat. Sessionpersistenz, SQLite, Tools, MCP, Memory, Markdown und Modellinstallation bleiben bewusst späteren Slices vorbehalten.
