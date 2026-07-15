# ADR-007: Ein Agent vor Multi-Agent

Status: Accepted

## Kontext

Astra benötigt zunächst verlässliche Tool-Nutzung, Sessions, Memory und Policies. Mehrere Agenten erhöhen Koordinationsaufwand, Kosten und Fehlermöglichkeiten, ohne automatisch bessere Ergebnisse zu liefern.

## Entscheidung

Astra startet mit einem einzelnen Agenten. Weitere Agenten werden nur eingeführt, wenn ein konkreter Anwendungsfall nicht sinnvoll durch ein Tool, einen Service, einen deterministischen Workflow oder den bestehenden Agenten gelöst werden kann.

## Folgen

- geringere Komplexität im ersten Produktkern
- Fähigkeiten bleiben zunächst Tools und Services
- Multi-Agent-Orchestrierung benötigt später ein eigenes ADR und Evals
