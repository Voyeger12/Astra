# Contributing to Astra

Dieses Dokument definiert, wie Änderungen an Astra geplant, umgesetzt, geprüft und eingereicht werden. Es gilt für Menschen, KI-Coding-Agenten, Copilot und automatisierte Änderungen.

## Grundsatz

Korrektheit, Nachvollziehbarkeit und begrenzte Auswirkungen haben Vorrang vor Geschwindigkeit und Umfang.

Ein kleiner, verstandener und belegter Fix ist wertvoller als eine große Änderung, deren Nebenwirkungen nicht verstanden wurden.

## Verbindliche Dokumente

Vor einer Änderung sind zu lesen:

- `SECURITY.md`
- `docs/DEVELOPMENT.md`
- `docs/ARCHITECTURE_RULES.md`
- relevante ADRs unter `docs/adr/`
- der passende Repository-Skill unter `.github/skills/`

## Branches und Pull Requests

- Keine direkte Arbeit auf `main`.
- Jede Änderung erfolgt in einem klar benannten Branch.
- Ein Pull Request verfolgt ein primäres Ziel.
- Bugfix, Refactoring, Paketupdates und neue Features werden möglichst getrennt.
- Keine unbeteiligten Formatierungs- oder Umbenennungsänderungen.
- Der PR beschreibt Zweck, Root Cause bei Bugs, Wirkungsbereich, Tests und nicht geprüfte Bereiche.
- Architekturänderungen benötigen ein ADR oder die Aktualisierung eines bestehenden ADRs.

## Änderungsumfang

Vor der Implementierung wird der Blast Radius eingeschätzt.

Nicht erlaubt:

- komplette Dateien ohne Notwendigkeit neu schreiben,
- benachbarte Komponenten nebenbei modernisieren,
- Abhängigkeiten aktualisieren, obwohl sie nicht zur Aufgabe gehören,
- neue Architekturmuster als Nebenprodukt eines kleinen Fixes einführen,
- große Änderungen in der Hoffnung durchführen, dass der Fehler dadurch verschwindet.

Die Anzahl geänderter Zeilen ist kein Qualitätsmerkmal.

## Root-Cause-Regel für Bugs

Vor jedem Bugfix werden dokumentiert:

1. beobachtetes Verhalten,
2. erwartetes Verhalten,
3. Reproduktion oder nachvollziehbarer Fehlerpfad,
4. betroffener Daten- und Kontrollfluss,
5. identifizierte Root Cause,
6. Abgrenzung zwischen Ursache und Symptomen,
7. geschätzter Blast Radius,
8. kleinste geeignete Korrektur.

Nicht erlaubt:

- Symptome durch zusätzliche Bedingungen zu verdecken,
- Timeouts oder Retries ohne Ursachenanalyse zu erhöhen,
- Exceptions pauschal zu schlucken,
- Funktionen zu deaktivieren, damit ein Fehler nicht mehr sichtbar ist,
- Anforderungen umzudeuten, damit das bestehende Verhalten korrekt erscheint.

## Warnungen und Fehler

- Compiler-, Analyzer- und Laufzeitwarnungen werden nicht ignoriert.
- Keine `#pragma`, `NoWarn`, abgeschalteten Analyzer oder deaktivierten Nullable-Prüfungen nur für einen grünen Build.
- Keine leeren `catch`-Blöcke.
- Exceptions werden behandelt, mit geeignetem Kontext protokolliert oder kontrolliert weitergeworfen.
- Ein grüner Build allein beweist kein korrektes Verhalten.
- Es darf nicht behauptet werden, ein Fehler sei behoben, wenn Build und relevante Tests nicht tatsächlich ausgeführt wurden.

## Testintegrität

Tests prüfen fachlich erwartetes Verhalten.

Nicht erlaubt:

- Assertions entfernen oder abschwächen, damit Tests grün werden,
- Tests löschen, überspringen oder deaktivieren,
- Fakes, die unabhängig von Eingaben immer Erfolg liefern,
- Produktionslogik im Test kopieren und nur die Kopie prüfen,
- erforderliche Integrationstests durch leichtere Unit Tests ersetzen,
- `Task.Delay` als Synchronisationsstrategie,
- unrealistische Mock-Antworten, die reale Fehlerzustände verdecken.

Ein Bugfix benötigt einen Regressionstest, der den ursprünglichen Fehler abdeckt.

## Abhängigkeiten

Neue NuGet-Pakete werden nur eingeführt, wenn Standardbibliothek und vorhandener Stack nicht ausreichen.

Vor Aufnahme wird dokumentiert:

- welches Problem gelöst wird,
- warum vorhandene Möglichkeiten nicht ausreichen,
- welche Alternativen geprüft wurden,
- welche Risiken, Lizenzen und Transitivabhängigkeiten entstehen.

Preview- und Prerelease-Pakete benötigen eine bewusste Architekturentscheidung.

## C# und Dokumentation

- Moderne C#- und .NET-Best-Practices verwenden.
- Technische APIs zuerst über aktuelle offizielle Microsoft- oder Framework-Dokumentation prüfen.
- Keine Methoden, Namespaces, Paketversionen oder Optionen erfinden.
- Unsicherheit, Preview-APIs und nicht geprüfte Annahmen offen benennen.
- Nullable Reference Types, Cancellation, Dependency Injection und strukturierte Logs bleiben verbindlich.

## Pflichtablauf

1. Aufgabe und Akzeptanzkriterien verstehen.
2. Regeln, Skills und ADRs lesen.
3. Betroffene Dateien, Verträge, Datenflüsse und Tests untersuchen.
4. Ursache oder Implementierungsziel formulieren.
5. Blast Radius einschätzen.
6. Kleinste geeignete Änderung planen.
7. Implementieren.
8. Passende Tests ergänzen oder aktualisieren.
9. Build, Tests, Analyzer und Warnungen tatsächlich ausführen.
10. Gesamten Diff auf Nebenwirkungen prüfen.
11. Ergebnis und nicht geprüfte Bereiche ehrlich dokumentieren.

## Definition of Done

Eine Änderung ist erst fertig, wenn:

- das beabsichtigte Verhalten nachgewiesen ist,
- die Root Cause bei Bugs identifiziert wurde,
- relevante Tests bestehen,
- keine Warnungen versteckt wurden,
- Cancellation und Fehlerpfade berücksichtigt sind,
- Sicherheits- und Datenschutzregeln eingehalten werden,
- die Architekturgrenzen bestehen bleiben,
- Dokumentation und ADRs weiterhin stimmen,
- nicht geprüfte Bereiche offen benannt sind.

## Stop-Regel

Wenn eine Änderung nicht sicher verstanden wird, darf eine KI nicht durch großflächige Änderungen versuchen, zufällig eine funktionierende Lösung zu erzeugen. Zuerst werden Kontext, Verträge und offizielle Dokumentation geprüft oder die Unsicherheit offen benannt.

Lieber eine Aufgabe begründet nicht abschließen als einen scheinbar funktionierenden Fix liefern, dessen Nebenwirkungen nicht verstanden wurden.