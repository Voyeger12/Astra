# Astra Quality Gates Skill

## Trigger

Verwenden vor Abschluss eines Features, bei Bugfixes, Tests, Evals, Security Review, Lifecycle-Änderungen und Pull-Request-Vorbereitung.

## Pflichtlektüre

- `CONTRIBUTING.md`
- `SECURITY.md`
- `docs/DEVELOPMENT.md`
- `docs/ARCHITECTURE_RULES.md`
- `docs/ARCHITECTURE.md`
- `docs/DATA-AND-STATE.md`
- betroffene ADRs

## Root-Cause-Gate für Bugs

Vor einer Korrektur müssen dokumentiert sein:

1. beobachtetes Verhalten,
2. erwartetes Verhalten,
3. Reproduktion oder nachvollziehbarer Fehlerpfad,
4. identifizierte Root Cause,
5. Abgrenzung zu Symptomen,
6. geschätzter Blast Radius,
7. kleinste geeignete Änderung.

Kein Abschluss, wenn lediglich Symptome durch zusätzliche Bedingungen, Timeouts, Retries, deaktivierte Funktionen oder geschluckte Exceptions verdeckt werden.

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

## Verbotene Testmanipulationen

- Assertions entfernen oder abschwächen, damit Tests grün werden
- Tests löschen, überspringen oder deaktivieren
- Fakes verwenden, die unabhängig vom Eingang immer Erfolg liefern
- Produktionslogik im Test kopieren und nur die Kopie prüfen
- notwendige Integrationstests durch bequemere Unit Tests ersetzen
- `Task.Delay` als Synchronisationsstrategie verwenden
- unrealistische Mock-Antworten nutzen, die reale Fehlerzustände verdecken

Ein Bugfix benötigt einen Regressionstest, der vor der Korrektur fehlschlägt und danach besteht.

## Build- und Analyzer-Gate

Vor Abschluss tatsächlich ausführen:

1. Build der betroffenen Projekte,
2. betroffene Unit- und Contract-Tests,
3. Architekturtests,
4. relevante Integrationstests,
5. vollständige Solution, sofern möglich,
6. Analyzer und Compilerwarnungen.

Keine Behauptung über erfolgreiche Prüfungen, die nicht ausgeführt wurden. Fehlende Umgebung, Modelle oder Geräte werden ausdrücklich als nicht geprüft dokumentiert.

## Diff- und Blast-Radius-Gate

Prüfen:

- Wurden nur notwendige Dateien verändert?
- Enthält der Bugfix unbeteiligte Refactorings, Paketupdates oder Formatierungen?
- Wurden öffentliche Verträge still verändert?
- Sind neue Abhängigkeiten oder Projektverweise entstanden?
- Bleibt die dokumentierte Abhängigkeitsrichtung erhalten?
- Entstehen neue Warnungen, unbehandelte Exceptions oder technische Schulden?
- Funktionieren Cancellation und Shutdown weiterhin?
- Werden sensible Daten oder Tool-Argumente neu geloggt?

## Definition of Done

Ein Feature oder Fix ist nur fertig, wenn:

1. der vollständige betroffene Ablauf funktioniert,
2. die Root Cause bei Bugs nachvollziehbar identifiziert wurde,
3. die Änderung den kleinstmöglichen sinnvollen Umfang hat,
4. Fehler und Offline-Zustände kontrolliert behandelt werden,
5. Cancellation und Shutdown geprüft sind,
6. sensible Daten nicht unkontrolliert geloggt werden,
7. relevante echte Tests grün sind,
8. Agentenverhalten durch Contract Tests oder Evals abgesichert ist,
9. keine neuen unbegründeten Warnungen existieren,
10. Dokumentation und ADRs weiterhin stimmen,
11. nicht geprüfte Bereiche ehrlich benannt sind.

## Abbruchkriterien

Kein Merge bei:

- unklarer Root Cause eines Bugfixes,
- Fake-Tests oder manipulierten Tests,
- ignorierten Warnungen oder Exceptions,
- fehlenden Tests für neue Tool-Nebenwirkungen,
- unklarer Datenhaltung oder Löschlogik,
- nicht abbrechbaren Hintergrundabläufen,
- stillen Architekturabweichungen,
- freier Shell oder destruktivem Zugriff ohne explizite Policy,
- breiten Änderungen ohne verstandenem Wirkungsbereich,
- unbelegten Behauptungen über Build, Tests oder Funktionsfähigkeit.

## Stop-Regel

Wenn Ursache, Architekturgrenze oder Nebenwirkung nicht verstanden sind, wird nicht weiter implementiert. Zuerst Kontext, offizielle Dokumentation und bestehende Verträge prüfen.