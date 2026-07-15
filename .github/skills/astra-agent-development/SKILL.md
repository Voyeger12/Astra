# Astra Agent Development Skill

## Trigger

Verwenden bei Änderungen an Agent Runtime, Modellen, Sessions, Tools, Memory-Kontext, MCP oder Agent Instructions.

## Pflichtlektüre

- `docs/VISION.md`
- `docs/ARCHITECTURE.md`
- `docs/DATA-AND-STATE.md`
- ADR-003, ADR-004, ADR-006 und ADR-007

## Grundsätze

1. Astra startet mit einem Agenten.
2. Der Agent Loop wird nicht selbst nachgebaut.
3. Tools sind typisierte Adapter auf Application-Services.
4. Keine freie Shell als Standardtool.
5. Conversation State, Long-Term Memory, Tool Runs und Application State bleiben getrennt.
6. Providerzugriffe erfolgen über `IChatClient` oder eigene Application-Ports.
7. Jede Tool-Ausführung erzeugt nachvollziehbare Ereignisse.
8. Verändernde oder riskante Tools benötigen Policy- und Freigabeprüfung.

## Mindestvertrag

Der Agentenpfad muss Text-Deltas, Tool-Anfragen, Freigaben, Tool-Ergebnisse, Statusänderungen, Abschluss, Fehler und Cancellation abbilden.

## Arbeitsablauf

1. Nutzerziel und erwartetes Agentenverhalten formulieren.
2. Prüfen, ob ein deterministischer Service oder Workflow genügt.
3. Nur bei tatsächlichem Agentenbedarf Tool oder Agentenlogik ergänzen.
4. Tool-Schema, Risikoklasse, Timeout und Fehlercodes definieren.
5. Contract Tests mit Fake-`IChatClient` ergänzen.
6. Mindestens einen positiven und einen negativen Eval-Fall festlegen.

## Abbruchkriterien

Stoppen, wenn:

- ein Tool unbeschränkte Parameter oder unklare Nebenwirkungen besitzt
- ein Frameworktyp in Domain-Modelle übernommen werden müsste
- ein zweiter Agent ohne belegten Use Case eingeführt werden soll
- eine Freigabe nur durch Prompttext statt durch Anwendungscode erzwungen wird
