## Ziel

<!-- Welches eine primäre Ziel verfolgt dieser Pull Request? -->

## Änderung

<!-- Was wurde konkret geändert und was bewusst nicht? -->

## Root Cause bei Bugfixes

<!-- Beobachtetes Verhalten, erwartetes Verhalten, Reproduktion, Root Cause und Abgrenzung zu Symptomen. Bei keinem Bugfix: Nicht zutreffend. -->

## Blast Radius

<!-- Welche Schichten, Verträge, Daten, Tools, Pfade und Benutzerabläufe können betroffen sein? -->

## Test- und Vertragsstrategie

- [ ] Akzeptanzkriterien und beobachtbares Verhalten vor der Implementierung definiert
- [ ] Passende Testebene gewählt
- [ ] Deterministisches Verhalten test-first oder contract-first entwickelt
- [ ] Agentenverhalten durch Contract Tests und gegebenenfalls getrennte Evals abgesichert
- [ ] Regressionstest schlägt ohne den Fix fehl
- [ ] Keine Tests für private Implementierungsdetails festgezurrt

### Änderungen bestehender Tests

<!-- Falls Tests verändert wurden: alte Erwartung, neue Erwartung, Begründung, zugehörige Anforderung oder ADR, Risiko und neuer Schutz. Sonst: Keine. -->

## Tests und Nachweise

- [ ] Engste relevante Tests ausgeführt
- [ ] Betroffene Projekttests ausgeführt
- [ ] Architekturtests ausgeführt oder nicht betroffen
- [ ] Integrationstests ausgeführt oder begründet nicht möglich
- [ ] Relevante Evals ausgeführt oder nicht betroffen
- [ ] Release-Build ausgeführt
- [ ] Compiler- und Analyzerwarnungen geprüft
- [ ] Gesamtdiff auf Nebenwirkungen und testbezogene Sonderfälle geprüft

Ausgeführte Befehle und Ergebnisse:

```text

```

## Sicherheit und Daten

- [ ] Keine Secrets oder persönlichen Daten committed oder geloggt
- [ ] Tool-Risiken und Freigaben geprüft
- [ ] Datenmigrationen und Rückfallverhalten geprüft oder nicht betroffen
- [ ] Cancellation, Shutdown und Fehlerpfade geprüft oder nicht betroffen

## Dokumentation

- [ ] Dokumentation weiterhin korrekt
- [ ] Relevante ADRs aktualisiert oder nicht betroffen
- [ ] Neue Abhängigkeiten begründet und geprüft oder nicht vorhanden
- [ ] Teststrategie und Verträge weiterhin korrekt

## Nicht geprüft / offene Risiken

<!-- Ehrlich benennen. Keine leere Erfolgsbehauptung. -->