# Astra Test-First Skill

## Trigger

Vor neuen Features, Bugfixes, Architekturänderungen, Agentenpfaden, Tool-Implementierungen, Persistenzänderungen und sicherheitsrelevanten Änderungen verwenden.

## Pflichtlektüre

- `docs/TEST-STRATEGY.md`
- `CONTRIBUTING.md`
- `docs/DEVELOPMENT.md`
- `docs/ARCHITECTURE_RULES.md`
- betroffene ADRs

## Ziel

Verhalten, Verträge, Sicherheit und Architektur werden vor oder gemeinsam mit der Implementierung spezifiziert. Tests dürfen nicht nachträglich nur den bereits geschriebenen Code spiegeln.

## Vorgehen

1. Akzeptanzkriterien und beobachtbares Verhalten formulieren.
2. Testkategorie bestimmen: Verfassungsvertrag, Produktverhalten oder implementierungsnaher Test.
3. Passende Testebene wählen: Unit Test, Architekturtest, Contract Test, Integrationstest oder Eval.
4. Bei deterministischem Verhalten zuerst einen fehlschlagenden Test schreiben.
5. Nur die kleinste Implementierung erstellen, die den Vertrag erfüllt.
6. Engste Tests, breitere Tests, Analyzer und Build ausführen.
7. Diff auf testbezogene Sonderfälle und unerlaubte Testanpassungen prüfen.

## Test-First-Pflicht

Vor Implementierung oder Bugfix müssen Tests beziehungsweise ausführbare Verträge zuerst entstehen bei:

- Domain- und Application-Regeln,
- Tool-Policies und Freigaben,
- Architekturgrenzen,
- Pfad- und Speicherlogik,
- Persistenz und Migrationen,
- Lifecycle und Cancellation,
- Agent Contract Verhalten mit Fake-`IChatClient`,
- Regressionen aus bestätigten Fehlern.

## Nichtdeterministisches Verhalten

Echte Modellantworten werden nicht als exakter String getestet. Lokale Evals prüfen Eigenschaften wie Tool-Auswahl, verbotene Aktionen, Evidenz, Sicherheitsgrenzen und robuste Reaktion auf Prompt Injection.

Evals ersetzen keine deterministischen Unit-, Contract- oder Integrationstests.

## Änderung bestehender Tests

Ein bestehender Test darf nicht geändert werden, nur damit eine Implementierung grün wird.

Eine Änderung ist nur zulässig, wenn:

- sich eine Anforderung bewusst geändert hat,
- ein Test nachweislich falsches Verhalten fordert,
- ein ADR einen Vertrag ersetzt,
- der Test unnötig an interne Implementierungsdetails gekoppelt ist.

Der Pull Request dokumentiert alte Erwartung, neue Erwartung, Begründung, Risiko und neuen Schutz.

## Verboten

- Assertions abschwächen oder entfernen,
- Tests löschen, überspringen oder deaktivieren,
- Produktionscode für konkrete Testdaten oder Testnamen verzweigen,
- Fakes verwenden, die unabhängig von Eingaben Erfolg liefern,
- Produktionslogik im Test kopieren,
- Integrationstests durch bequemere Unit Tests ersetzen,
- `Task.Delay` als Synchronisationsstrategie,
- exakte Formulierungen eines Sprachmodells als stabilen Vertrag behandeln.

## Abschlussprüfung

- Prüft der Test fachliches Verhalten statt Implementierungsdetails?
- Wäre der Test auch bei einer alternativen korrekten Implementierung grün?
- Schlägt ein Regressionstest ohne den Fix tatsächlich fehl?
- Enthält Produktionscode testbezogene Sonderfälle?
- Wurden bestehende Tests geändert und nachvollziehbar begründet?
- Sind nichtdeterministische Evals von deterministischen Tests getrennt?
- Wurden echte Befehle und Ergebnisse dokumentiert?

## Stop-Regel

Wenn erwartetes Verhalten oder Vertragsgrenze nicht klar ist, wird nicht durch Tests eine zufällige Architektur erfunden. Zuerst Anforderung, ADR, bestehende Verträge und offizielle Dokumentation klären.