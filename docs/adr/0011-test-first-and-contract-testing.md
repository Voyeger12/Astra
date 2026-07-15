# ADR-011: Test-first und Contract Testing

## Status

Accepted

## Kontext

Astra wird als C#/.NET-Agentenanwendung mit UI, Agent Runtime, Tools, Policies, Persistenz, lokalen Modellen und späteren externen Integrationen aufgebaut. Viele dieser Bereiche besitzen einen hohen Blast Radius. KI-gestützte Entwicklung erhöht zusätzlich das Risiko, dass Produktionscode und nachträglich passend formulierte Tests gemeinsam ein falsches Verhalten absichern.

Eine vollständig starre Test-Spezifikation vor jeder Implementierung wäre jedoch ebenfalls problematisch. Frühe Tests könnten hypothetische interne Strukturen festschreiben, falsche Annahmen konservieren oder sinnvolles Refactoring verhindern.

Benötigt wird deshalb eine Strategie, die stabile Verhaltens- und Sicherheitsverträge früh festlegt, ohne private Implementierungsdetails vorzeitig einzufrieren.

## Entscheidung

Astra wird test-first, contract-first und eval-orientiert entwickelt.

### Test-first

Deterministische Fachlogik, Policies, Validierung, Pfadlogik, Zustandsübergänge, Migrationen und klar definierte Use Cases werden bevorzugt mit einem roten Verhaltenstest begonnen.

### Contract-first

Architekturgrenzen, Application-Interfaces, Agent Runtime, Providergrenzen, Session-, Memory-, Approval- und Tool-Verträge werden aus Sicht des Aufrufers spezifiziert und durch Contract Tests geschützt.

### Eval-orientiert

Nichtdeterministisches Modellverhalten wird durch getrennte Evals geprüft. Evals ersetzen keine deterministischen Sicherheits-, Policy- oder Persistenztests.

### Outside-in

Tests legen öffentliches Verhalten, Zustände, Fehlerpfade, Cancellation, Persistenz und Sicherheitsgrenzen fest. Private Methoden, interne Klassenaufteilung und konkrete Framework-Aufrufreihenfolgen werden nicht als dauerhafte Verträge behandelt.

### Änderung bestehender Tests

Bestehende Verhaltenstests dürfen nicht geändert werden, nur damit eine Implementierung grün wird. Eine Änderung benötigt eine bewusst geänderte Anforderung, ein ersetzendes ADR, den Nachweis eines fehlerhaften Tests oder die Entfernung unnötiger Kopplung an Implementierungsdetails.

Änderungen an besonders stabilen Architektur-, Sicherheits-, Datenverlust-, Cancellation- und Tool-Policy-Verträgen benötigen ADR, Risikoanalyse und explizite Review.

### Regressionstests

Jeder Bugfix erhält grundsätzlich einen Regressionstest, der den ursprünglichen Fehlerpfad abbildet, vor der Korrektur fehlschlägt und nach der Korrektur besteht.

### Testprojektstruktur

Mit dem Solution-Scaffolding wird folgende Zielstruktur vorgesehen:

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

Leere Testprojekte gelten nicht als Qualitätsnachweis. Projekte werden angelegt, wenn reale Tests oder unmittelbar folgende Testarbeit vorhanden sind.

## Folgen

### Vorteile

- Anforderungen und Verträge werden vor oder gemeinsam mit der Implementierung präzisiert.
- KI-Coding-Agenten erhalten überprüfbare Ziele statt die eigenen Tests nachträglich zu definieren.
- Architektur-, Sicherheits- und Persistenzgrenzen werden früh ausführbar geschützt.
- Implementierungen und Provider können ausgetauscht werden, solange Verträge bestehen bleiben.
- Bugfixes erhalten belastbare Regressionstests.
- Tests bleiben refactoringfreundlich, weil Implementierungsdetails nicht unnötig festgeschrieben werden.

### Nachteile

- Akzeptanzkriterien und Verträge müssen vor der Implementierung sorgfältiger geklärt werden.
- Test- und Contract-Design benötigt initial mehr Zeit.
- Falsche frühe Anforderungen können zu falschen Tests führen und müssen bewusst korrigiert werden.
- Agent Evals benötigen separate Modelle, Daten und Bewertungsregeln und sind nicht vollständig deterministisch.

## Abgelehnte Alternativen

### Produktionscode zuerst, Tests danach

Abgelehnt, weil die Implementierung dann häufig bestimmt, was nachträglich als korrekt getestet wird. Dies ist besonders bei KI-generiertem Code riskant.

### Alle Tests vollständig vor jedem Code und danach unveränderlich

Abgelehnt, weil dadurch hypothetische interne Strukturen, falsche Annahmen und unnötige Kopplung dauerhaft konserviert werden könnten.

### Nur End-to-End-Tests

Abgelehnt, weil Root Causes schwer lokalisierbar wären und Architektur-, Policy- und Fehlerverträge nicht ausreichend isoliert geprüft würden.

### Nur Unit Tests

Abgelehnt, weil Agent Loops, Persistenz, Lifecycle, Provider- und Tool-Grenzen echte Contract- und Integrationstests benötigen.

## Verweise

- `docs/TEST-STRATEGY.md`
- `CONTRIBUTING.md`
- `docs/DEVELOPMENT.md`
- `.github/skills/astra-test-first/SKILL.md`
- `.github/skills/astra-quality-gates/SKILL.md`
