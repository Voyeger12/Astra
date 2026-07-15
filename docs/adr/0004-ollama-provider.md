# ADR-004: Ollama als erster Model Provider

Status: Accepted

## Kontext

Astra soll local-first und ohne Cloudpflicht funktionieren. Gleichzeitig darf die Anwendung nicht dauerhaft an einen konkreten Provider gekoppelt werden.

## Entscheidung

Ollama ist der erste Model Provider. Die Integration erfolgt über `Microsoft.Extensions.AI.IChatClient` und einen Application-nahen Providervertrag.

## Folgen

- lokaler Betrieb bleibt der Standard
- Offline- und Nicht-erreichbar-Zustände sind reguläre Anwendungsfälle
- Ollama-Typen bleiben in der Infrastructure-Schicht
- weitere Provider können später ergänzt werden, ohne UI oder Domain umzubauen
