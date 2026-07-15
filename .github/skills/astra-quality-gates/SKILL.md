# Astra Quality Gates Skill

## Trigger

Verwenden vor Abschluss eines Features, bei Bugfixes, Tests, Evals, Security Review, Lifecycle-Änderungen und Pull-Request-Vorbereitung.

## Pflichtlektüre

- `CONTRIBUTING.md`
- `SECURITY.md`
- `docs/DEVELOPMENT.md`
- `docs/ARCHITECTURE_RULES.md`
- `docs/TEST-STRATEGY.md`
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

## Test-First-Gate

Vor der Implementierung muss geklärt sein:

- Welche fachliche Erwartung oder welcher Vertrag gilt?
- Ist das Verhalten deterministisch testbar?
- Welche Testebene ist passend?
- Muss ein bestehender Test unverändert bleiben?
- Welche nichtdeterministischen Aspekte gehören in Evals?

Test-first ist verpflichtend für Domain- und Application-Regeln, Tool-Policies, Pfade, Persistenz, Migrationen, Lifecycle, Cancellation und bestätigte Regressionen.

Contract-first ist verpflichtend für Agent Runtime, Provider, Tools, Sessions und Speichergrenzen.

Evals ersetzen keine deterministischen Tests.

## Testebenen

### Deterministische Tests

- Domain-Regeln
- Tool-Policies und Parameter
- Persistenz und Migrationen
- Cancellation und Lifecycle
- ViewModels und Application Use Cases

### Architekturtests

- erlaubte Projektabhängigkeiten
- verbotene Frameworkreferenzen
- Trennung von UI, Domain, Infrastructure, AgentFramework und Tools
- Schutz unverhandelbarer Architekturverträge

### Agent Contract Tests

Mit Fake-`IChatClient` prüfen:

- Text- und Streaming-Ereignisse
- korrekte Tool-Auswahl und Argumente
- Tool-Ergebnis zurück im Agent Loop
- Freigaben und Ablehnungen
- Abbruch, Timeout und Fehlerabbildung
- Session-Fortsetzung

### Integrationstests

- echte Persistenzgrenzen mit isolierter temporärer Datenbank
- Migrationen, Rollback und Neustart
- Host-Lifecycle und Shutdown
- Adaptergrenzen, die durch Unit Tests nicht ausreichend geprüft werden

### Lokale Evals

Gegen ein dokumentiertes Ollama-Modell prüfen:

- gewünschte und verbotene Tool Calls
- Antwort bei fehlender Evidenz
- Sicherheits- und Freigabegrenzen
- Prompt-Injection-Resistenz
- Regressionen aus realen Fehlerfällen

## Änderung bestehender Tests

Kein Merge, wenn Tests nur geändert wurden, damit Produktionscode grün wird.

Eine Teständerung benötigt:

1. alte Erwartung,
2. neue Erwartung,
3. fachliche oder architektonische Begründung,
4. zugehörige Anforderung oder ADR,
5. Risikoanalyse,
6. neuen Schutz gegen Regressionen.

## Verbotene Testmanipulationen

- Assertions entfernen oder abschwächen, damit Tests grün werden
- Tests löschen, überspringen oder deaktivieren
- Fakes verwenden, die unabhängig vom Eingang immer Erfolg liefern
- Produktionslogik im Test kopieren und nur die Kopie prüfen
- notwendige Integrationstests durch bequemere Unit Tests ersetzen
- `Task.Delay` als Synchronisationsstrategie verwenden
- unrealistische Mock-Antworten nutzen, die reale Fehlerzustände verdecken
- Produktionscode auf Testnamen, Testdaten oder Testumgebungsdetails verzweigen
- exakte Modellformulierungen als stabilen Vertrag behandeln

Ein Bugfix benötigt einen Regressionstest, der ohne die Korrektur fehlschlägt und danach besteht.

## Build- und Analyzer-Gate

Vor Abschluss tatsächlich ausführen:

1. engste relevante Tests,
2. Tests der betroffenen Projekte,
3. Architekturtests,
4. relevante Integrationstests,
5. vollständige Solution, sofern möglich,
6. Analyzer und Compilerwarnungen.

Keine Behauptung über erfolgreiche Prüfungen, die nicht ausgeführt wurden. Fehlende Umgebung, Modelle oder Geräte werden ausdrücklich als nicht geprüft dokumentiert.

## Diff- und Blast-Radius-Gate

Prüfen:

- Wurden nur notwendige Dateien verändert?
- Enthält der Bugfix unbeteiligte Refactorings, Paketupdates oder Formatierungen?
- Wurden öffentliche Verträge oder Tests still verändert?
- Sind neue Abhängigkeiten oder Projektverweise entstanden?
- Bleibt die dokumentierte Abhängigkeitsrichtung erhalten?
- Entstehen neue Warnungen, unbehandelte Exceptions oder technische Schulden?
- Funktionieren Cancellation und Shutdown weiterhin?
- Werden sensible Daten oder Tool-Argumente neu geloggt?
- Enthält Produktionscode testbezogene Sonderfälle?

## Definition of Done

Ein Feature oder Fix ist nur fertig, wenn:

1. der vollständige betroffene Ablauf funktioniert,
2. die Root Cause bei Bugs nachvollziehbar identifiziert wurde,
3. die Änderung den kleinstmöglichen sinnvollen Umfang hat,
4. passende Tests oder Verträge vor oder gemeinsam mit der Implementierung entstanden sind,
5. Fehler und Offline-Zustände kontrolliert behandelt werden,
6. Cancellation und Shutdown geprüft sind,
7. sensible Daten nicht unkontrolliert geloggt werden,
8. relevante echte Tests grün sind,
9. Agentenverhalten durch Contract Tests und gegebenenfalls Evals abgesichert ist,
10. keine neuen unbegründeten Warnungen existieren,
11. Teständerungen nachvollziehbar begründet sind,
12. Dokumentation und ADRs weiterhin stimmen,
13. nicht geprüfte Bereiche ehrlich benannt sind.

## Abbruchkriterien

Kein Merge bei:

- unklarer Root Cause eines Bugfixes,
- fehlendem Test- oder Vertragsziel bei deterministischem Verhalten,
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

Wenn Ursache, erwartetes Verhalten, Architekturgrenze oder Nebenwirkung nicht verstanden sind, wird nicht weiter implementiert und kein Test erfunden, der eine zufällige Lösung legitimiert. Zuerst Kontext, offizielle Dokumentation und bestehende Verträge prüfen.