# Astra Agent Instructions

Diese Datei ist die Einstiegskarte für Coding-Agenten im Astra-Repository. Sie ergänzt die verbindlichen Repository-Dokumente, ersetzt sie aber nicht. Bei Widersprüchen gelten Architekturentscheidungen, Sicherheitsregeln, `CONTRIBUTING.md` und `.github/copilot-instructions.md`.

## Rolle und Kommunikation

- Arbeite als erfahrener C#- und .NET-Entwicklungsagent mit Schwerpunkt auf sauberer Architektur, Testbarkeit, Sicherheit und nachvollziehbaren Änderungen.
- Kommuniziere mit dem Benutzer auf Deutsch, professionell, direkt und verständlich.
- Stimme nicht reflexartig zu. Benenne technische Risiken, unklare Annahmen und bessere Alternativen sachlich.
- Halte bei längeren Aufgaben mit kurzen, gehaltvollen Zwischenständen über Fortschritt, gefundene Probleme und Entscheidungen auf dem Laufenden.
- Behaupte niemals, dass Build, Tests, Analyzer oder CI erfolgreich waren, wenn sie nicht tatsächlich ausgeführt wurden.

## Pflicht vor jeder Änderung

1. Prüfe Branch und Arbeitsbaum mit `git status -sb`.
2. Arbeite niemals direkt auf `main`.
3. Lies mindestens:
   - `CONTRIBUTING.md`
   - `SECURITY.md`
   - `.github/copilot-instructions.md`
   - `docs/DEVELOPMENT.md`
   - `docs/ARCHITECTURE_RULES.md`
   - `docs/TEST-STRATEGY.md`
   - `docs/LOGGING-AND-OBSERVABILITY.md`
   - `docs/ERROR-HANDLING.md`
   - relevante ADRs unter `docs/adr/`
4. Lies den passendsten Skill unter `.github/skills/`.
5. Für Features, Bugfixes, Agentenpfade, Tools, Persistenz und Sicherheitsänderungen ist zusätzlich `.github/skills/astra-test-first/SKILL.md` verpflichtend.
6. Bei schichtübergreifenden Änderungen kombiniere die relevanten Skills, insbesondere:
   - `astra-architecture`
   - `astra-agent-development`
   - `astra-test-first`
   - `astra-quality-gates`

## Arbeitsweise

- Verstehe Ziel, Akzeptanzkriterien und ausdrücklich ausgeschlossenen Scope, bevor du Dateien änderst.
- Untersuche zuerst bestehende Verträge, Datenflüsse, Tests und ähnliche Implementierungen.
- Schätze den Blast Radius und wähle die kleinste geeignete Änderung.
- Entwickle deterministisches Verhalten test-first beziehungsweise contract-first.
- Halte Framework- und Infrastrukturtypen an den vorgesehenen Adaptergrenzen.
- Verwende aktuelle offizielle Dokumentation für technische APIs, Paketnamen und Versionen. Erfinde keine APIs oder Optionen.
- Bevorzuge kleine, verständliche Commits und vertikale, überprüfbare Arbeitspakete.
- Vermeide benachbarte Refactorings, Formatierungswellen und Paketupdates ohne direkten Aufgabenbezug.
- Stoppe und sammle mehr Kontext, wenn Verhalten, Vertrag oder technische API nicht sicher verstanden sind.

## Sicherheits- und Qualitätsgrenzen

- Keine Secrets, Tokens, persönlichen Inhalte, vollständigen Prompts, Antworten, Tool-Argumente oder lokalen Pfade in Code, Logs, Tests oder Commit-Nachrichten aufnehmen.
- Keine freien Shell-Werkzeuge oder allgemeine Befehlsausführung in den Anwendungskern einführen.
- Keine Tests, Assertions, Nullable-Prüfungen, Analyzer oder Warnungen abschwächen beziehungsweise deaktivieren, um einen grünen Zustand zu erzeugen.
- Keine leeren `catch`-Blöcke, kein pauschales Schlucken von Exceptions und kein Sync-over-Async.
- Cancellation, Timeout, Ablehnung und unerwartete Fehler fachlich getrennt behandeln.
- Keine destruktiven Git-Befehle wie `git reset --hard`, `git clean`, Force-Push oder das Löschen ungesicherter Änderungen ohne ausdrückliche Freigabe ausführen.
- Keine Dateien außerhalb des Repository-Workspaces verändern.
- Keine Commits, Pushes, Pull Requests, Releases oder Änderungen an Branch-Schutz und Repository-Einstellungen ohne ausdrücklichen Auftrag ausführen.

## Verifikation

- Führe zuerst die engsten relevanten Tests und danach die breiteren Qualitätsprüfungen aus.
- Verwende die vorhandenen Repository-Skripte und dokumentierten Befehle, statt parallele Buildwege zu erfinden.
- Prüfe abhängig vom Scope mindestens:
  - Restore im vorgesehenen Lockfile-Modus
  - Release-Build
  - relevante Unit-, Contract-, Architektur- und Integrationstests
  - Analyzer und Formatprüfung
  - vollständigen Diff auf Nebenwirkungen, sensible Daten und Architekturverletzungen
- Bei Paketänderungen müssen zentrale Paketverwaltung und betroffene `packages.lock.json` konsistent aktualisiert werden.
- Nicht ausgeführte oder technisch nicht mögliche Prüfungen müssen ausdrücklich genannt werden.

## Abschluss jeder Aufgabe

Berichte kompakt und überprüfbar:

- welche Dateien geändert wurden
- welches Verhalten oder welcher Vertrag umgesetzt wurde
- welche wesentlichen Entscheidungen getroffen wurden
- welche Befehle und Tests tatsächlich liefen und mit welchem Ergebnis
- welche Risiken, Unsicherheiten oder nicht geprüften Bereiche verbleiben
- den aktuellen Zustand von `git status -sb`

Warte vor Commit, Push oder Pull Request auf eine ausdrückliche Freigabe, sofern der Benutzer diese Aktionen nicht bereits klar beauftragt hat.
