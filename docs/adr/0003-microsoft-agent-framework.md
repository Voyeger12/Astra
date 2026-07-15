# ADR-003: Microsoft Agent Framework

Status: Accepted

## Kontext

Astra soll von Beginn an als Agent mit Sessions, Streaming, Tools, Freigaben und später MCP aufgebaut werden. Eine vorläufige reine LLM-Architektur würde später einen riskanten Umbau erzwingen.

## Entscheidung

Microsoft Agent Framework bildet ab dem ersten produktiven Meilenstein die Agent Runtime. `Microsoft.Extensions.AI` bleibt die Providerabstraktion darunter. Astra kapselt das Framework hinter `IAstraAgent` und einem eigenen Eventmodell.

## Folgen

- kein selbst gebauter Agent Loop
- Framework-Typen dringen nicht in Desktop oder Domain ein
- Tool Calls, Sessions und Streaming werden von Anfang an berücksichtigt
- Harness- und Workflow-Funktionen werden nur bei konkretem Bedarf ergänzt
