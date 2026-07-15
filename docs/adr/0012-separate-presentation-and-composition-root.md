# ADR-012: Präsentation und Composition Root trennen

- Status: Accepted
- Datum: 2026-07-16

## Kontext

Die ausführbare WPF-Anwendung muss konkrete Implementierungen aus AgentFramework, Infrastructure und Tools im Dependency-Injection-Container zusammensetzen. Views und ViewModels dürfen diese Infrastrukturtypen jedoch nicht kennen.

Würde `Astra.Desktop` gleichzeitig UI-Schicht und Composition Root enthalten, wäre diese Grenze nur durch Konvention geschützt und könnte bei späteren Änderungen leicht verletzt werden.

## Entscheidung

Astra verwendet zwei getrennte Windows-Projekte:

- `Astra.Desktop` ist die ausführbare WPF-Anwendung und Composition Root.
- `Astra.Presentation` enthält Views, ViewModels, Commands und UI-State.

`Astra.Presentation` referenziert ausschließlich `Astra.Application`. `Astra.Desktop` darf die konkreten Adapterprojekte referenzieren, um den Host und Dependency Injection zusammenzusetzen.

## Folgen

- UI-Code bleibt von Ollama, SQLite, MCP und Agent-Framework-Typen getrennt.
- Der Composition Root ist explizit und klein.
- Architekturtests können die Grenze direkt prüfen.
- Die Solution enthält ein zusätzliches Projekt, bevor erste Features entstehen.
