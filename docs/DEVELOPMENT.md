# Development Guide

Dieses Dokument beschreibt die technische Arbeitsweise für Astra. Es gilt für Menschen, KI-Coding-Agenten, Copilot und automatisierte Änderungen.

## Quellenhierarchie

Bei technischen Entscheidungen gilt folgende Reihenfolge:

1. aktuelle offizielle Microsoft-Dokumentation,
2. aktuelle offizielle Dokumentation des verwendeten Frameworks oder Pakets,
3. offizieller Quellcode und offizielle Beispiele,
4. anerkannte Standards und Spezifikationen,
5. erst danach Blogposts, Foren oder Stack Overflow.

Bei widersprüchlichen Quellen gilt die aktuelle offizielle Dokumentation. Eine KI muss offen benennen, wenn sie eine API nur aus Erinnerung verwendet, eine Dokumentation nicht prüfen konnte, eine experimentelle API vorschlägt oder bewusst von einer offiziellen Empfehlung abweicht. Methoden, Namespaces, Paketversionen und Konfigurationsoptionen dürfen nicht erfunden werden.

## C#- und .NET-Regeln

- Nullable Reference Types bleiben aktiviert.
- Asynchrone Abläufe verwenden `async` und `await`.
- Kein `.Result`, `.Wait()` oder anderes Sync-over-Async.
- Länger laufende Operationen akzeptieren einen `CancellationToken`.
- `CancellationToken` wird durch alle relevanten Schichten weitergegeben.
- Keine unkontrollierten Fire-and-Forget-Tasks.
- `IDisposable` und `IAsyncDisposable` werden korrekt verwendet.
- Abhängigkeiten werden über Dependency Injection bereitgestellt.
- Globale veränderbare Zustände und unnötige Singletons werden vermieden.
- Öffentliche APIs verwenden klare Typen statt unstrukturierter Dictionaries oder `object`.
- Records werden für unveränderliche Datenmodelle bevorzugt.
- Exceptions dienen nicht als normaler Kontrollfluss.
- Logging verwendet strukturierte Platzhalter statt String-Verkettung.
- Konfiguration wird über Options-Klassen gebunden und validiert.
- Secrets gehören nicht in Quellcode, Logs oder Repository.
- Datenbankzugriffe werden transaktional und abbrechbar umgesetzt.
- UI-Logik bleibt aus Domain- und Application-Schichten heraus.
- Fachlogik gehört nicht in ViewModels, Tools oder Datenbankklassen.

## Compiler, Analyzer und Warnungen

Warnungen und Fehler werden nicht ignoriert.

Nicht erlaubt:

- `#pragma` oder `NoWarn`, nur um Warnungen zu verstecken,
- deaktivierte Analyzer ohne dokumentierte Begründung,
- abgeschaltete Nullable-Prüfung,
- leere `catch`-Blöcke,
- pauschales Schlucken von Exceptions,
- Build-Erfolg als alleiniger Nachweis für korrektes Verhalten.

Ein `catch` behandelt sinnvoll, protokolliert mit ausreichendem Kontext oder wirft kontrolliert weiter. Fehlermeldungen dürfen keine Secrets oder persönlichen Daten enthalten.

## Umgang mit Bugs

Vor einem Bugfix wird:

1. das beobachtete Verhalten beschrieben,
2. das erwartete Verhalten festgehalten,
3. der Fehler möglichst reproduziert,
4. der Daten- und Kontrollfluss verfolgt,
5. die Ursache von Symptomen getrennt,
6. der Wirkungsbereich abgeschätzt,
7. die kleinste geeignete Korrektur geplant.

Nicht erlaubt:

- zusätzliche `if`-Abfragen, die nur Symptome verdecken,
- höhere Timeouts oder zusätzliche Retries ohne Ursachenanalyse,
- das Deaktivieren einer Funktion, damit ein Fehler verschwindet,
- das Umdeuten einer Anforderung, damit das aktuelle Verhalten korrekt wirkt,
- großflächige Änderungen in der Hoffnung, zufällig den Fehler zu beseitigen.

Ein Bugfix erhält grundsätzlich einen Regressionstest, der vor der Korrektur fehlschlägt und danach besteht.

## Testregeln

Tests prüfen fachlich erwartetes Verhalten, nicht nur die aktuelle Implementierung.

Nicht erlaubt:

- Assertions entfernen oder abschwächen, damit Tests grün werden,
- Tests überspringen oder löschen, um einen Build zu retten,
- Fakes, die immer das gewünschte Ergebnis liefern,
- Produktionslogik im Test nachbauen und anschließend nur diese Kopie testen,
- Integrationstests durch leichtere Unit Tests ersetzen, obwohl der echte Ablauf betroffen ist,
- `Task.Delay` als Synchronisationsstrategie,
- unrealistische Mock-Antworten, die reale Fehlerfälle unsichtbar machen.

Prüfreihenfolge:

1. engster betroffener Test,
2. Tests des betroffenen Projekts,
3. Architekturtests,
4. Integrationstests,
5. vollständige Solution.

Relevante Tests decken mindestens ab:

- Normalfall,
- Fehlerfall,
- Abbruch,
- ungültige Eingaben,
- fehlende Abhängigkeiten,
- Wiederanlauf nach Fehlern,
- Persistenz und Neustart, sofern betroffen,
- Tool-Freigaben und abgelehnte Aktionen.

Agentenverhalten benötigt zusätzlich Contract Tests mit Fake-`IChatClient` und gezielte lokale Evals gegen ein konfiguriertes Ollama-Modell.

## Nebenläufigkeit und Lifecycle

- Jeder Hintergrunddienst besitzt definierten Start und Shutdown.
- Keine Endlosschleife ohne Cancellation.
- Keine gemeinsam veränderbaren Collections ohne Synchronisationskonzept.
- Keine UI-Aktualisierung aus einem fremden Thread.
- Keine festen Sleeps zur Koordination.
- Keine Ressourcen, die nur durch den Garbage Collector geschlossen werden.
- Shutdown bricht laufende Operationen kontrolliert ab.
- Cancellation gilt als eigener kontrollierter Zustand, nicht automatisch als Fehler.
- Retries sind begrenzt, nutzen Backoff und protokollieren den Grund.
- Nicht-idempotente Aktionen werden nicht unkontrolliert wiederholt.

## Abhängigkeiten

Neue NuGet-Pakete werden nur eingeführt, wenn Standardbibliothek und vorhandener Stack die Aufgabe nicht angemessen lösen.

Vor Aufnahme eines Pakets wird dokumentiert:

- Welches Problem wird gelöst?
- Warum reicht der bestehende Stack nicht?
- Welche Alternativen wurden betrachtet?
- Welche Risiken, Lizenzen und Transitivabhängigkeiten entstehen?

Keine Pakete für Funktionen, die mit wenigen klaren Zeilen zuverlässig umgesetzt werden können. Preview- und Prerelease-Pakete benötigen ein ADR oder eine ausdrücklich dokumentierte Ausnahme.

## Refactoring

Refactoring und Verhaltensänderung werden möglichst getrennt.

Ein Refactoring benötigt:

- abgesichertes bestehendes Verhalten,
- keine stillen Vertragsänderungen,
- keine neuen Features im selben PR,
- einen konkret benannten Nutzen.

Aussagen wie „cleaner“ oder „moderner“ reichen nicht. Es muss benannt werden, welche Komplexität, Kopplung, Wartungs- oder Testprobleme konkret verbessert werden.

## Pflichtablauf vor einer Änderung

1. Aufgabe und Akzeptanzkriterien verstehen.
2. Relevante Regeln, Skills und ADRs lesen.
3. Betroffene Dateien, Datenflüsse und Verträge untersuchen.
4. Bestehende Tests prüfen.
5. Ursache oder Implementierungsziel formulieren.
6. Blast Radius einschätzen.
7. Kleinste geeignete Änderung planen.
8. Änderung implementieren.
9. Passende Tests ergänzen oder aktualisieren.
10. Build und Tests tatsächlich ausführen.
11. Warnungen und Analyzer prüfen.
12. Gesamten Diff auf Nebenwirkungen überprüfen.
13. Ergebnis und nicht geprüfte Bereiche ehrlich dokumentieren.

## Pflichtprüfung nach einer Änderung

Vor Abschluss muss beantwortet werden:

- Wurde nur der notwendige Bereich verändert?
- Wurden APIs oder Verhalten unbeabsichtigt verändert?
- Gibt es neue Warnungen?
- Gibt es neue unbehandelte Exceptions?
- Funktioniert Cancellation?
- Wurden Sicherheit und Datenschutz berücksichtigt?
- Wurden echte Tests ausgeführt?
- Sind Behauptungen im PR durch Ergebnisse belegt?
- Bleibt die Abhängigkeitsrichtung korrekt?
- Wurde neue technische Schuld eingeführt?

## Stop-Regel

Wenn eine Änderung nicht sicher verstanden wird, darf eine KI nicht durch großflächige Änderungen versuchen, zufällig eine funktionierende Lösung zu erzeugen. Sie sammelt zuerst mehr Kontext, prüft Dokumentation oder benennt die Unsicherheit offen.

Lieber eine Aufgabe begründet nicht abschließen als einen scheinbar funktionierenden Fix liefern, dessen Nebenwirkungen nicht verstanden wurden.