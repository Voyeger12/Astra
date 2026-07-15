# Astra Quality Gates Skill

## Trigger

Verwenden vor Abschluss eines Features, bei Tests, Evals, Security Review, Lifecycle-Änderungen und Pull-Request-Vorbereitung.

## Pflichtlektüre

- `docs/ARCHITECTURE.md`
- `docs/DATA-AND-STATE.md`
- betroffene ADRs

## Testebenen

### Deterministische Tests

- Domain-Regeln
- Tool-Policies und Parameter
- Persistenz und Migrationen
- Cancellation und Lifecycle
- ViewModels und Application Use Cases

### Agent Contract Tests

Mit Fake-`IChatClient` prüfen:

- korrekte Tool-Auswahl und Argumente
- Tool-Ergebnis zurück im Agent Loop
- Freigaben und Ablehnungen
- Streaming-Ereignisse und Reihenfolge
- Abbruch, Timeout und Fehlerabbildung
- Session-Fortsetzung

### Lokale Evals

Gegen ein konfiguriertes Ollama-Modell prüfen:

- gewünschte und verbotene Tool Calls
- Antwort bei fehlender Evidenz
- Sicherheits- und Freigabegrenzen
- Regressionen aus realen Fehlerfällen

## Definition of Done

Ein Feature ist nur fertig, wenn:

1. der vollständige vertikale Ablauf funktioniert
2. Fehler und Offline-Zustände kontrolliert behandelt werden
3. Cancellation und Shutdown geprüft sind
4. sensible Daten nicht unkontrolliert geloggt werden
5. relevante Tests grün sind
6. Agentenverhalten durch Contract Tests oder Evals abgesichert ist
7. Dokumentation und ADRs weiterhin stimmen

## Abbruchkriterien

Kein Merge bei:

- fehlenden Tests für neue Tool-Nebenwirkungen
- unklarer Datenhaltung oder Löschlogik
- nicht abbrechbaren Hintergrundabläufen
- stillen Architekturabweichungen
- freier Shell oder destruktivem Zugriff ohne explizite Policy
