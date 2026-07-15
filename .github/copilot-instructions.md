# Astra Repository Instructions

Astra ist ein C#/.NET-10-Neuaufbau eines lokal-first Windows-KI-Agenten. Diese Regeln gelten für Copilot, Coding-Agenten und automatisierte Änderungen.

## Pflichtlektüre

Vor Änderungen sind mindestens zu lesen:

- `CONTRIBUTING.md`
- `SECURITY.md`
- `docs/DEVELOPMENT.md`
- `docs/ARCHITECTURE_RULES.md`
- `docs/TEST-STRATEGY.md`
- `docs/LOGGING-AND-OBSERVABILITY.md`
- `docs/ERROR-HANDLING.md`
- relevante ADRs unter `docs/adr/`
- der passende Skill unter `.github/skills/`

Für Features, Bugfixes, Agentenpfade, Tools, Persistenz und Sicherheitsänderungen ist `.github/skills/astra-test-first/SKILL.md` verpflichtend.

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
- Features werden als vollständige vertikale Abläufe mit Tests, Fehlerbehandlung und Diagnose umgesetzt.

## Fehler, Warnungen und Root Cause

- Warnungen und Fehler niemals ignorieren oder verstecken.
- Keine Analyzer, Nullable-Prüfungen, Tests oder Assertions deaktivieren, um einen grünen Zustand zu erzeugen.
- Keine leeren `catch`-Blöcke und kein pauschales Schlucken von Exceptions.
- Technische Exceptions an klaren Boundaries in typisierte Astra-Fehler übersetzen.
- Cancellation, Timeout, Ablehnung und unerwartete Fehler nicht miteinander vermischen.
- Exceptions nur dort loggen, wo ausreichend Kontext vorhanden ist, und nicht in jeder Schicht erneut.
- Stacktraces und interne Exceptiondetails niemals ungefiltert an die UI weitergeben.
- Bei Bugs zuerst beobachtetes und erwartetes Verhalten, Reproduktion, Datenfluss, Root Cause und Blast Radius bestimmen.
- Nur die identifizierte Ursache mit der kleinsten geeigneten Änderung beheben.
- Keine zusätzlichen Timeouts, Retries oder `if`-Abfragen, die nur Symptome verdecken.
- Keine benachbarten Refactorings, Paketupdates oder Formatierungswellen als Teil eines Bugfixes.

## Logging und Observability

- `Microsoft.Extensions.Logging` als gemeinsame Abstraktion verwenden.
- Konkrete Logger, lokale Dateien und OpenTelemetry ausschließlich in `Astra.Infrastructure` konfigurieren.
- Strukturierte Event-IDs, Fehlercodes und Korrelations-IDs verwenden.
- Logs, Traces und Audit-Ereignisse fachlich trennen.
- Keine vollständigen Prompts, Antworten, Tool-Argumente, Dateiinhalte, lokalen Pfade, Tokens oder Secrets protokollieren.
- Diagnosefelder bevorzugt über eine Allowlist zulässiger Metadaten bestimmen.
- Cancellation und normale Benutzerablehnung nicht als Systemfehler loggen.
- Ein neuer Logging-Provider benötigt eine bewusste Paketprüfung; keine Bibliothek ohne konkreten Bedarf installieren.

## Test-first und Contract-first

- Deterministische Fachlogik, Policies, Pfade, Persistenz, Migrationen, Lifecycle und Cancellation werden test-first entwickelt.
- Agent Runtime, Provider, Tools, Sessions und Speichergrenzen werden contract-first entwickelt.
- Fehlercodes, Exception-Mapping, Redaction und Korrelation sind Teil der Verträge.
- Nichtdeterministisches Modellverhalten wird mit getrennten Evals geprüft.
- Architektur- und Sicherheitsverträge werden nicht an eine konkrete Implementierung gekoppelt.
- Keine Tests für hypothetische private Methoden oder erfundene Klassenhierarchien.
- Ein Bugfix benötigt einen Regressionstest, der ohne den Fix fehlschlägt.

## Änderung bestehender Tests

- Tests niemals ändern, nur damit die Implementierung grün wird.
- Eine Teständerung benötigt eine geänderte Anforderung, einen nachweislich fehlerhaften Test, ein ersetzendes ADR oder die Entfernung unnötiger Implementierungskopplung.
- Alte und neue Erwartung, Begründung, Risiko und neuer Schutz müssen im Pull Request stehen.
- Keine Testlöschung, kein Skip, keine abgeschwächte Assertion und kein Fake, das unabhängig von Eingaben Erfolg liefert.
- Produktionscode darf nicht auf Testnamen, konkrete Testdaten oder Testumgebungsdetails verzweigen.

## Tests

- Tests müssen fachlich erwartetes Verhalten prüfen.
- Bei Agentenpfaden Contract Tests mit Fake-`IChatClient` und bei relevantem Verhalten lokale Evals vorsehen.
- Evals ersetzen keine Unit-, Contract-, Architektur- oder Integrationstests.
- Logs werden über stabile Event-IDs und erlaubte Felder getestet, nicht über jeden Freitext.
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
3. Betroffene Dateien, Verträge, Datenflüsse und bestehende Tests untersuchen.
4. Testkategorie und passende Testebene bestimmen.
5. Fehler-, Logging- und Datenschutzvertrag des Ablaufs bestimmen.
6. Ursache oder Implementierungsziel formulieren.
7. Blast Radius einschätzen.
8. Bei deterministischem Verhalten zuerst einen fehlschlagenden Test oder Vertrag erstellen.
9. Kleinste geeignete Änderung implementieren.
10. Engste und anschließend breitere Tests ausführen.
11. Build, Analyzer und Warnungen prüfen.
12. Gesamten Diff auf Nebenwirkungen, Architekturverletzungen, sensible Logdaten und testbezogene Sonderfälle prüfen.
13. Ergebnis sowie nicht geprüfte Bereiche ehrlich dokumentieren.

## Stop-Regel

Wenn eine Änderung oder das erwartete Verhalten nicht sicher verstanden wird, nicht durch großflächige Änderungen oder erfundene Tests versuchen, zufällig eine funktionierende Lösung zu erzeugen. Zuerst mehr Kontext sammeln, offizielle Dokumentation und bestehende Verträge prüfen oder die Unsicherheit offen benennen.
