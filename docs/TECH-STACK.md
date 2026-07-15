# Techstack

## Plattform

- .NET 10 LTS
- C# mit Nullable Reference Types
- Windows als primäres Zielsystem
- WPF für die Desktop-Oberfläche
- CommunityToolkit.Mvvm für MVVM, Commands und Observable State

## Anwendung und Lifecycle

- Microsoft.Extensions.Hosting
- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.Configuration
- Microsoft.Extensions.Logging
- CancellationToken für alle abbrechbaren Abläufe

## Agent und KI

- Microsoft Agent Framework als Agent Runtime
- Microsoft.Extensions.AI als Providerabstraktion
- OllamaSharp als erster lokaler `IChatClient`
- Ollama als lokaler Model Provider
- ein einzelner Astra-Agent in der ersten Ausbaustufe
- Model Context Protocol für externe Tools und Datenquellen

## Daten

- SQLite als lokale persistente Datenbank
- Entity Framework Core nur, wenn Migrationen und Mapping den Mehraufwand rechtfertigen
- alternativ Microsoft.Data.Sqlite mit expliziten Repositories
- getrennte Speicherung von Sessions, Nachrichten, Memories, Tool Runs und Approvals

## Qualität

- xUnit
- FluentAssertions
- NSubstitute oder Fake-Implementierungen für deterministische Tests
- Agent Contract Tests mit Fake-`IChatClient`
- echte lokale Evals gegen konfigurierte Ollama-Modelle
- OpenTelemetry für Traces, Metriken und Tool-Abläufe
- EditorConfig, Roslyn Analyzer und TreatWarningsAsErrors

## Paketregeln

- zentrale Paketversionen über `Directory.Packages.props`
- keine Bibliothek ohne dokumentierten Zweck
- keine parallelen Frameworks für dieselbe Aufgabe
- neue Agent-, Persistence- oder UI-Frameworks benötigen ein ADR
- experimentelle Pakete bleiben aus dem produktiven Kern

## Noch nicht festgelegt

Folgende Entscheidungen werden erst durch einen Spike oder konkreten Bedarf getroffen:

- EF Core oder direkte SQLite-Repositories
- genaue lokale Embedding-Strategie
- Vision- und Speech-Bibliotheken
- Installer und Updatekanal
- Nutzung des HarnessAgent für spezialisierte Arbeitsmodi
