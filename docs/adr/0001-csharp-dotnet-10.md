# ADR-001: C# und .NET 10

Status: Accepted

## Kontext

Der Python-Prototyp müsste für Lifecycle, Agent Runtime, Tooling und UI tiefgreifend umgebaut werden. Astra ist realistisch eine Windows-Desktop-Anwendung und der langfristige Entwicklungsschwerpunkt liegt auf C# und .NET.

## Entscheidung

Astra wird neu in C# auf .NET 10 LTS aufgebaut. Der Python-Prototyp wird nicht portiert, sondern als Referenz und Anforderungssammlung genutzt.

## Folgen

- moderner Generic Host, Dependency Injection und `async`/`await`
- gute Windows-Integration
- Python bleibt nur für später klar abgegrenzte ML-Services möglich
- keine direkte Übernahme der alten Modulstruktur
