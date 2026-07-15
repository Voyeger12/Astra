# Test Strategy

Dieses Dokument definiert die Teststrategie für Astra. Es gilt für Menschen, KI-Coding-Agenten, Copilot, lokale Automatisierung und CI.

## Ziel

Astra wird verhaltens-, vertrags- und risikoorientiert entwickelt. Tests sollen nicht nachträglich bestätigen, was bereits implementiert wurde, sondern vor und während der Implementierung festlegen, welches Verhalten, welche Sicherheitsgrenzen und welche Architekturverträge gelten.

Der Grundsatz lautet:

> Verhalten, Verträge, Sicherheit und Architektur werden zuerst spezifiziert und möglichst vor der Produktionsimplementierung getestet. Implementierungsdetails werden erst getestet, wenn sie tatsächlich entstehen.

Tests sind keine unveränderlichen Steintafeln. Sie dürfen jedoch niemals nur deshalb geändert werden, weil eine Implementierung sonst nicht grün wird.

## Test-First-Grundsätze

- Akzeptanzkriterien werden vor der Implementierung formuliert.
- Für deterministische Fachlogik wird bevorzugt Test-Driven Development verwendet.
- Öffentliche Verträge und Integrationsgrenzen werden Contract-first entwickelt.
- Agentenverhalten wird mit deterministischen Contract Tests und getrennten Evals abgesichert.
- Ein roter Test ist nur sinnvoll, wenn er fachlich erwartetes Verhalten ausdrückt.
- Tests dürfen keine hypothetische interne Architektur erzwingen.
- Produktionscode darf keine Kenntnis von Testfällen, Testdaten oder Testumgebungsdetails besitzen, außer über ausdrücklich vorgesehene Abstraktionen.
- Die Implementierung richtet sich nach den vereinbarten Verträgen. Tests werden nicht passend zur Implementierung umgeschrieben.

## Outside-in statt Implementierungsdetails

Vor der Implementierung werden bevorzugt von außen sichtbare Eigenschaften festgelegt:

- Eingaben und Ergebnisse,
- Zustandsübergänge,
- Fehler- und Offline-Verhalten,
- Cancellation und Shutdown,
- Tool-Freigaben,
- Persistenz und Neustart,
- Architekturgrenzen,
- Datenschutz- und Sicherheitsregeln.

Nicht vorab festgelegt werden:

- private Methoden,
- interne Hilfsklassen,
- konkrete Collection-Typen,
- exakte Anzahl interner Services,
- konkrete Framework-Aufrufreihenfolgen,
- Pixelpositionen und rein visuelle UI-Details,
- unnötige Mock-Interaktionen ohne fachliche Bedeutung.

Tests sollen Refactoring erlauben, solange das vereinbarte Verhalten erhalten bleibt.

## Testarten

### 1. Domain- und Unit Tests

Geeignet für vollständig deterministische Logik:

- Domain-Regeln,
- Validierung,
- Zustandsübergänge,
- Tool-Risikoklassen,
- Policy-Entscheidungen,
- Mapping mit fachlicher Bedeutung,
- Pfad- und Konfigurationslogik,
- Datenmigrationsregeln.

Ablauf:

1. Erwartetes Verhalten formulieren.
2. Engsten aussagekräftigen roten Test schreiben.
3. Kleinste korrekte Implementierung erstellen.
4. Test grün machen.
5. Refactoring nur bei weiterhin grünen Verhaltenstests.

### 2. Application Tests

Prüfen Anwendungsfälle und Orchestrierung über echte Application-Komponenten mit kontrollierten Test-Doubles an Infrastrukturgrenzen.

Beispiele:

- Session laden und fortsetzen,
- Agentenlauf starten und abbrechen,
- Freigabe anfordern und auswerten,
- Tool-Ergebnis in den Ablauf zurückführen,
- kontrollierte Reaktion auf fehlende Abhängigkeiten,
- Persistenz eines fachlich vollständigen Use Cases.

### 3. Architekturtests

Architekturtests prüfen die verbindlichen Regeln aus `docs/ARCHITECTURE_RULES.md`.

Mindestens zu schützen sind:

- `Astra.Domain` referenziert weder WPF, SQLite, Ollama, MCP noch das Agent Framework.
- `Astra.Desktop` greift nicht direkt auf SQLite, Ollama oder MCP zu.
- `Astra.Tools` enthält keine Fachlogik und hängt von Application-Verträgen ab.
- Infrastructure implementiert Application-Interfaces und bestimmt keine fachlichen Policies.
- ViewModels referenzieren keine Provider- oder Persistenztypen.
- Projektabhängigkeiten folgen ausschließlich der dokumentierten Richtung.

Architekturtests werden mit dem Solution-Scaffolding angelegt und sind verpflichtender CI-Bestandteil.

### 4. Agent Contract Tests

Agent Contract Tests verwenden einen kontrollierten Fake-`IChatClient` oder gleichwertigen Provider-Test-Double. Sie dürfen nicht von einem echten Ollama-Modell abhängen.

Zu prüfen sind mindestens:

- Text-Streaming wird in korrekter Reihenfolge als Astra-Events weitergegeben.
- Tool Calls werden mit typisierten Argumenten erkannt und validiert.
- Tool-Ergebnisse gelangen zurück in den Agent Loop.
- Freigaben werden angefordert, akzeptiert und abgelehnt.
- Ein abgelehntes Tool wird nicht ausgeführt.
- Tool-Fehler werden strukturiert abgebildet.
- Cancellation beendet den Lauf kontrolliert.
- Timeouts und Providerfehler werden unterschieden.
- Sessions können fortgesetzt werden.
- Offline-Zustände erzeugen kontrollierte Fehler statt Abstürze.

Der Fake muss Eingaben auswerten und realistische Zustandsfolgen simulieren. Ein Fake, der unabhängig von Eingang und Zustand immer Erfolg liefert, ist unzulässig.

### 5. Integrationstests

Integrationstests prüfen reale technische Grenzen in isolierter Umgebung:

- SQLite mit temporärer Datenbank,
- Migrationen und Rollback,
- Serialisierung und Deserialisierung,
- Dateisystemzugriffe in temporären Verzeichnissen,
- Generic-Host-Lifecycle,
- kontrollierte Provideradapter,
- MCP-Adapter, sobald vorhanden.

Integrationstests dürfen niemals echte Benutzerdaten, `%LOCALAPPDATA%\Astra`, reale Produktivdatenbanken oder persönliche Secrets verwenden.

### 6. UI- und ViewModel-Tests

UI-nahe Tests prüfen Verhalten, nicht Pixelgenauigkeit.

Beispiele:

- Senden startet genau einen Agentenlauf.
- Abbrechen propagiert Cancellation.
- Offline-Zustand wird sichtbar dargestellt.
- Approval Requested ermöglicht Zustimmung oder Ablehnung.
- Während eines laufenden Requests gilt ein klar definierter Zustand.
- Fehler werden ohne Absturz und ohne sensible Details angezeigt.

Reine Layout-, Farb- oder Animationsdetails werden nur getestet, wenn daraus ein konkreter Bedien- oder Barrierefreiheitsvertrag entsteht.

### 7. Agent Evals

Evals prüfen nichtdeterministisches Verhalten mit einem realen konfigurierten Modell. Sie sind von deterministischen Tests getrennt.

Geeignete Eval-Gruppen:

- grundlegende Konversation,
- gewünschte Tool-Auswahl,
- verbotene Tool Calls,
- Verhalten bei fehlender Evidenz,
- Memory Recall,
- Prompt Injection,
- Sicherheits- und Freigabegrenzen,
- Regressionen aus realen Fehlerfällen.

Evals prüfen Eigenschaften und Grenzen, nicht starre Wortlaute.

Schlecht:

```text
Die Antwort muss exakt "Hallo Duncan" lauten.
```

Besser:

```text
Die Antwort ist nicht leer, enthält keinen Tool Call und verletzt keine Sicherheitsregel.
```

Evals dokumentieren mindestens:

- Modell und Version,
- Modellparameter,
- Prompt- und Tool-Konfiguration,
- Testdatensatz,
- Bewertungsregeln,
- Anzahl Wiederholungen,
- bekannte Varianz.

Lokale Evals blockieren nicht automatisch jeden normalen Build, solange Hardware und Modellverfügbarkeit nicht reproduzierbar sind. Sicherheitskritische deterministische Verträge dürfen jedoch niemals ausschließlich durch Evals abgesichert werden.

## Testprojektstruktur

Mit dem Solution-Scaffolding wird folgende Zielstruktur angelegt:

```text
tests/
├── Astra.Domain.Tests
├── Astra.Application.Tests
├── Astra.Architecture.Tests
├── Astra.AgentFramework.Tests
├── Astra.Infrastructure.Tests
├── Astra.IntegrationTests
└── Astra.AgentEvals
```

Die Projekte werden nur angelegt, wenn sie echte Tests oder unmittelbar folgende Testarbeit enthalten. Leere Projekte dürfen nicht als Qualitätsnachweis dargestellt werden.

## Testkategorien und Änderungsstabilität

### Kategorie A: Verfassungsverträge

Sehr stabile Verträge:

- Architekturgrenzen,
- Tool-Policies und Freigaben,
- Schutz vor Datenverlust,
- Datenschutz und Secret-Regeln,
- Cancellation und Shutdown,
- keine freie Shell,
- Trennung von Conversation State, Memory, Tool Runs und Application State.

Änderungen benötigen:

1. ein neues oder aktualisiertes ADR,
2. explizite Risikoanalyse,
3. eigene Begründung im Pull Request,
4. Tests für alten und neuen Wirkungsbereich,
5. bewusste Review.

### Kategorie B: Produktverhalten

Stabile, aber fachlich veränderbare Verträge:

- Session-Verhalten,
- Memory-Regeln,
- Offline-Reaktion,
- Erststart,
- Benutzerfreigaben,
- UI-Workflows.

Änderungen benötigen aktualisierte Akzeptanzkriterien und eine Erklärung, welches frühere Verhalten ersetzt wird.

### Kategorie C: Implementierungsnahe Tests

Flexiblere Tests:

- Mapper,
- interne Adapter,
- Serialisierungsdetails,
- technische Hilfsklassen.

Diese Tests dürfen beim Refactoring angepasst werden, solange Kategorie A und B unverändert bleiben und der Test weiterhin sinnvolles Verhalten prüft.

## Regeln für Änderungen an bestehenden Tests

Ein bestehender Test darf geändert werden, wenn mindestens einer dieser Gründe nachgewiesen ist:

- eine fachliche Anforderung wurde bewusst geändert,
- ein ADR ersetzt den bisherigen Vertrag,
- der Test fordert nachweislich falsches Verhalten,
- der Test ist unnötig an Implementierungsdetails gekoppelt,
- eine frühere Annahme wurde durch dokumentierte Evidenz widerlegt.

Nicht zulässige Gründe:

- die Implementierung ist sonst schwieriger,
- der Test ist rot,
- die KI hat einen anderen Lösungsweg gewählt,
- eine Abhängigkeit verhält sich unbequem,
- Zeitdruck,
- der Test verhindert einen schnellen Merge.

Jede Änderung eines bestehenden Verhaltenstests muss im Pull Request beantworten:

1. Welche Erwartung wurde geändert?
2. Warum ist die bisherige Erwartung nicht mehr gültig?
3. Welche Anforderung oder welches ADR begründet die Änderung?
4. Welcher Blast Radius entsteht?
5. Welcher Test schützt das neue Verhalten?

## Regressionstests bei Bugs

Ein Bugfix benötigt grundsätzlich einen Regressionstest.

Der Regressionstest muss:

- das beobachtete fehlerhafte Verhalten reproduzieren oder den Fehlerpfad eindeutig abbilden,
- vor der Korrektur fehlschlagen,
- nach der Korrektur bestehen,
- die Root Cause und nicht nur ein sichtbares Symptom abdecken,
- mindestens eine nahe Variante prüfen, wenn eine testbezogene Sonderbehandlung möglich wäre.

Kann ein Test technisch nicht vor der Korrektur ausgeführt werden, muss der PR erklären, warum und wie der Fehlerfall anderweitig belegt wurde.

## Verbotene Testmanipulationen

Nicht erlaubt sind:

- Assertions entfernen oder abschwächen, um grün zu werden,
- Tests löschen, überspringen, ausfiltern oder deaktivieren,
- Fakes, die immer Erfolg liefern,
- Produktionslogik im Test kopieren und nur diese Kopie prüfen,
- erforderliche Integrationstests durch bequemere Unit Tests ersetzen,
- `Task.Delay` als Synchronisationsstrategie,
- Testreihenfolge als Voraussetzung,
- gemeinsam veränderbarer Zustand zwischen Tests,
- Nutzung realer Benutzerdaten,
- unrealistische Mock-Antworten, die reale Fehlerfälle verdecken,
- Sonderfälle im Produktionscode für konkrete Testdaten,
- Prüfen ausschließlich privater Methoden statt des relevanten Verhaltens,
- Snapshot-Updates ohne inhaltliche Prüfung,
- pauschales Erhöhen von Timeouts gegen flakey Tests.

Flakey Tests werden als Fehler behandelt. Ihre Root Cause wird untersucht. Sie werden nicht dauerhaft mit Retries, größeren Zeitfenstern oder Quarantäne verborgen.

## Testdaten und Isolation

- Jeder Test ist unabhängig und wiederholbar.
- Testdaten sind klein, verständlich und fachlich benannt.
- Temporäre Dateien und Datenbanken werden pro Test oder Testgruppe isoliert erstellt.
- Tests bereinigen Ressourcen deterministisch.
- Zeit wird über abstrahierte Zeitquellen kontrolliert, nicht über echte Sleeps.
- Zufall wird mit festen Seeds oder kontrollierten Generatoren reproduzierbar gemacht.
- Netzwerkzugriffe sind in Unit- und Contract-Tests verboten.
- Externe Dienste werden nur in ausdrücklich markierten Integrationstests verwendet.
- Secrets und persönliche Inhalte gehören nicht in Testdaten oder Snapshots.

## Reihenfolge der Prüfung

Nach einer Änderung wird in dieser Reihenfolge geprüft:

1. engster betroffener Test,
2. Tests des betroffenen Projekts,
3. Agent Contract Tests, sofern betroffen,
4. Architekturtests,
5. relevante Integrationstests,
6. vollständige Solution,
7. lokale Evals, sofern für das Verhalten erforderlich.

Ein grüner enger Test ersetzt keine breitere Prüfung, wenn der Blast Radius mehrere Schichten umfasst.

## CI-Vertrag

Mit dem Solution-Scaffolding wird eine echte CI-Pipeline angelegt. Sie muss mindestens ausführen:

1. SDK-Auflösung über `global.json`,
2. `dotnet restore --locked-mode`,
3. Format- und Analyzerprüfung,
4. Release-Build mit Warnungen als Fehler,
5. deterministische Unit-, Application-, Contract- und Architekturtests,
6. relevante Integrationstests, soweit ohne lokale Geheimnisse und Geräte möglich.

Eine CI, die keine Solution baut oder keine echten Tests ausführt, gilt nicht als Qualitätsnachweis.

Lokale Modell-Evals, Sensorprüfungen und geräteabhängige Tests laufen in getrennten, ausdrücklich gekennzeichneten Abläufen.

## Definition of Done für Tests

Eine Änderung ist testseitig erst abgeschlossen, wenn:

- Akzeptanzkriterien und erwartetes Verhalten klar sind,
- die passende Testart gewählt wurde,
- kritische Verträge möglichst vor der Implementierung getestet wurden,
- relevante Tests tatsächlich ausgeführt wurden,
- keine Tests manipuliert oder still deaktiviert wurden,
- Regressionstests bei Bugs vorhanden sind,
- Testdaten isoliert und frei von Benutzerdaten sind,
- Architektur- und Sicherheitsgrenzen weiterhin geschützt sind,
- geänderte Tests begründet wurden,
- nicht ausgeführte Evals oder geräteabhängige Prüfungen offen benannt sind.

## Stop-Regel

Wenn nicht klar ist, welches Verhalten fachlich korrekt ist, wird weder Produktionscode noch ein festschreibender Test auf Basis einer Vermutung erstellt. Zuerst werden Anforderung, Vertrag, ADR oder offizielle Dokumentation geklärt.

Lieber ein offenes Akzeptanzkriterium dokumentieren als einen falschen Test als dauerhafte Wahrheit in das Repository einzubauen.
