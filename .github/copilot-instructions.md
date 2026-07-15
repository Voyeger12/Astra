# Astra Repository Instructions

Astra ist ein C#/.NET-10-Neuaufbau eines lokal-first Windows-KI-Agenten. Vor Änderungen sind die relevanten Dokumente unter `docs/` und Skills unter `.github/skills/` zu lesen.

## Verbindliche Regeln

- Keine 1:1-Portierung der alten Python-Architektur.
- Ein einzelner Agent vor Multi-Agent-Orchestrierung.
- Microsoft Agent Framework ab dem ersten Agentenpfad.
- WPF ViewModels kennen keine Ollama-, SQLite-, MCP- oder Agent-Framework-Typen.
- Domain bleibt framework- und infrastrukturfrei.
- Tools sind typisierte Adapter auf Application-Services.
- Keine freie Shell im Kern.
- Riskante Aktionen benötigen Anwendungscode-basierte Policies und Freigaben.
- Alle lang laufenden Abläufe unterstützen `CancellationToken`.
- Conversation State, Memory, Tool Runs und Application State bleiben getrennt.
- Keine globalen veränderbaren Singletons.
- Neue tragende Entscheidungen benötigen ein ADR.
- Features werden als vollständige vertikale Abläufe mit Tests umgesetzt.

## Arbeitsweise

1. Scope und betroffenen Use Case identifizieren.
2. Passenden Repository-Skill lesen.
3. Abhängigkeiten und Datenfluss prüfen.
4. Kleinste vollständige Änderung implementieren.
5. Deterministische Tests und bei Agentenverhalten Contract Tests ergänzen.
6. Dokumentation und ADRs aktualisieren, falls sich eine Entscheidung ändert.
