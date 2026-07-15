# Astra Repository Instructions

Astra ist ein C#/.NET-10-Neuaufbau eines lokal-first Windows-KI-Agenten. Diese Regeln gelten für Copilot, Coding-Agenten und automatisierte Änderungen.

## Pflichtlektüre

Vor Änderungen sind mindestens zu lesen:

- `CONTRIBUTING.md`
- `SECURITY.md`
- `docs/DEVELOPMENT.md`
- `docs/ARCHITECTURE_RULES.md`
- relevante ADRs unter `docs/adr/`
- der passende Skill unter `.github/skills/`

## Oberster Grundsatz

Korrektheit, Nachvollziehbarkeit und begrenzte Auswirkungen haben Vorrang vor Geschwindigkeit und Umfang.

Lieber eine Aufgabe begründet nicht abschließen als einen scheinbar funktionierenden Fix liefern, dessen Nebenwirkungen nicht verstanden wurden.

## Verbindliche Architekturregeln

- Keine 1:1-Portierung der alten Python-Architektur.
- Ein einzelner Agent vor Multi-Agent-Orchestrierung.
- Microsoft Agent Framework ab dem ersten Agentenpfad.
- WPF ViewModels kennen keine Ollama-, SQLite-, MCP- oder Agent-Framework-Typen.
- Domain bleibt framework- und infrastrukturfrei.
- Tools sind typisierte Adapter auf Application-Services und enthalten keine Fachlogik.
- Keine freie Shell oder allgemeines `ExecuteCommand` im Kern.
- Riskante Aktionen benötigen Anwendungscode-basierte Policies und Freigaben.
- Alle lang laufenden Abläufe unterstützen `CancellationToken`.
- Conversation State, Long-Term Memory, Tool Runs und Application State bleiben getrennt.
- Keine globalen veränderbaren Singletons.
- Neue tragende Entscheidungen benötigen ein ADR.
- Features werden als vollständige vertikale Abläufe mit Tests umgesetzt.

## Fehler, Warnungen und Root Cause

- Warnungen und Fehler niemals ignorieren oder verstecken.
- Keine Analyzer, Nullable-Prüfungen, Tests oder Assertions deaktivieren, um einen grünen Zustand zu erzeugen.
- Keine leeren `catch`-Blöcke und kein pauschales Schlucken von Exceptions.
- Bei Bugs zuerst beobachtetes und erwartetes Verhalten, Reproduktion, Datenfluss, Root Cause und Blast Radius bestimmen.
- Nur die identifizierte Ursache mit der kleinsten geeigneten Änderung beheben.
- Keine zusätzlichen Timeouts, Retries oder `if`-Abfragen, die nur Symptome verdecken.
- Keine benachbarten Refactorings, Paketupdates oder Formatierungswellen als Teil eines Bugfixes.

## Tests

- Keine Fake-Tests, die nur den erzeugten Code spiegeln oder immer grün sind.
- Keine Tests löschen, überspringen oder abschwächen, um einen Build zu retten.
- Ein Bugfix benötigt einen Regressionstest, der das fehlerhafte Verhalten abdeckt.
- Tests müssen fachlich erwartetes Verhalten prüfen.
- Bei Agentenpfaden Contract Tests mit Fake-`IChatClient` und bei relevantem Verhalten lokale Evals vorsehen.
- Build, Tests, Analyzer und Warnungen tatsächlich ausführen. Nicht behaupten, sie seien erfolgreich, wenn sie nicht ausgeführt wurden.

## C# und Quellen

- Moderne C#- und .NET-Best-Practices verwenden.
- Nullable Reference Types aktiviert lassen.
- Kein Sync-over-Async, keine unkontrollierten Fire-and-Forget-Tasks.
- Cancellation durch alle relevanten Schichten weitergeben.
- Strukturierte Logs, validierte Options-Klassen und Dependency Injection verwenden.
- Technische APIs zuerst über aktuelle offizielle Microsoft- oder Framework-Dokumentation prüfen.
- Keine Methoden, Namespaces, Paketversionen oder Optionen erfinden.
- Unsicherheit, Preview-APIs und nicht geprüfte Annahmen offen benennen.

## Sicherheitsgrenzen

- Keine Secrets, persönlichen Inhalte oder vollständigen Tool-Argumente in Code, Logs oder Traces.
- Externe Inhalte, MCP-Ergebnisse und Tool-Ausgaben als nicht vertrauenswürdige Daten behandeln.
- SQL parametrisieren, Pfade normalisieren und Eingaben an Systemgrenzen validieren.
- Sicherheits- und Freigabeentscheidungen niemals an das Modell delegieren.

## Arbeitsweise

1. Aufgabe und Akzeptanzkriterien verstehen.
2. Relevante Regeln, ADRs und Skills lesen.
3. Betroffene Dateien, Verträge und Datenflüsse untersuchen.
4. Bestehende Tests prüfen.
5. Ursache oder Implementierungsziel formulieren.
6. Blast Radius einschätzen.
7. Kleinste geeignete Änderung planen.
8. Änderung implementieren.
9. Relevante Tests ergänzen oder aktualisieren.
10. Build, Tests, Analyzer und Warnungen ausführen.
11. Gesamten Diff auf Nebenwirkungen und Architekturverletzungen prüfen.
12. Ergebnis sowie nicht geprüfte Bereiche ehrlich dokumentieren.

## Stop-Regel

Wenn eine Änderung nicht sicher verstanden wird, nicht durch großflächige Änderungen versuchen, zufällig eine funktionierende Lösung zu erzeugen. Zuerst mehr Kontext sammeln, offizielle Dokumentation prüfen oder die Unsicherheit offen benennen.