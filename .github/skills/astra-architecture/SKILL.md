# Astra Architecture Skill

## Trigger

Verwenden bei neuen Projekten, Schichten, Abhängigkeiten, Services, Datenflüssen oder größeren Refactorings.

## Pflichtlektüre

- `docs/ARCHITECTURE.md`
- `docs/adr/README.md`
- alle ADRs, die den betroffenen Bereich berühren

## Regeln

1. `Astra.Domain` bleibt frei von UI-, Datenbank-, Netzwerk- und Frameworkabhängigkeiten.
2. `Astra.Application` definiert Use Cases und Ports, aber keine Infrastrukturdetails.
3. WPF ViewModels sprechen nur mit Application-Interfaces.
4. Agent-Framework-, Ollama-, SQLite- und MCP-Typen bleiben in ihren Adapterprojekten.
5. Keine globalen veränderbaren Singletons oder versteckte Initialisierung.
6. Lang laufende Vorgänge unterstützen `CancellationToken`.
7. Neue Fähigkeiten werden als vertikale Funktion umgesetzt, nicht als unverbundene Modulinsel.

## Arbeitsablauf

1. Betroffenen Use Case und Datenfluss benennen.
2. Bestehende Interfaces und ADRs prüfen.
3. Abhängigkeitsrichtung vor dem Coden festlegen.
4. Kleinste vollständige Änderung planen.
5. Architekturtests oder Referenzprüfungen ergänzen.
6. Bei einer neuen tragenden Entscheidung zuerst ein ADR erstellen.

## Abbruchkriterien

Stoppen und Architekturentscheidung klären, wenn:

- eine tiefere Schicht eine höhere Schicht referenzieren müsste
- Frameworktypen in Domain oder Desktop-Verträge gelangen
- dieselbe Verantwortung in mehreren Projekten dupliziert würde
- ein neues Framework eine bestehende Kernaufgabe parallel übernimmt
