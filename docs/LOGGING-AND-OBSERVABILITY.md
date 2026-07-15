# Logging and Observability

## Ziel

Astra verwendet Logging, Tracing und Audit-Ereignisse als kontrollierte Diagnoseinstrumente. Sie sollen Fehlerpfade, Laufzeiten und sicherheitsrelevante Entscheidungen nachvollziehbar machen, ohne Gesprächsinhalte, Secrets oder persönliche Daten als Nebenprodukt zu sammeln.

Logging ersetzt weder kontrollierte Fehlerbehandlung noch Benutzerfeedback. Die UI zeigt verständliche und sichere Meldungen. Logs und Traces enthalten die zusätzliche technische Diagnose.

## Verantwortlichkeiten

- Anwendungscode verwendet `Microsoft.Extensions.Logging` und strukturierte Ereignisse.
- `Astra.Infrastructure` konfiguriert konkrete Logger, lokale Dateien, Aufbewahrung und OpenTelemetry.
- `Astra.Application` definiert fachlich relevante Fehlercodes, Operationen und Audit-Verträge.
- `Astra.Desktop` konfiguriert Logging ausschließlich im Composition Root.
- `Astra.Presentation`, `Astra.Domain` und Agentenmodelle kennen keinen konkreten Logging-Provider.

Vor der Auswahl eines zusätzlichen File-Logging-Providers werden Lizenz, Wartung, Datenschutz, Dateirotation und Transitivabhängigkeiten geprüft. Die Auswahl benötigt eine dokumentierte Paketentscheidung, aber kein neues Projekt.

## Drei getrennte Signale

### Logs

Logs beschreiben technische und fachliche Ereignisse einer Komponente. Sie sind für lokale Diagnose, Support und Root-Cause-Analyse bestimmt.

### Traces

Traces verbinden Operationen über Schicht- und Adaptergrenzen. Sie messen Ablauf, Dauer und Ergebnis. Trace-Attribute dürfen keine vollständigen Prompts, Tool-Argumente oder Dateiinhalte enthalten.

### Audit-Ereignisse

Audit-Ereignisse dokumentieren sicherheitsrelevante Entscheidungen und Aktionen. Beispiele:

- Tool angefordert,
- Risikoklasse bestimmt,
- Freigabe verlangt,
- Freigabe erteilt oder abgelehnt,
- Tool gestartet,
- Tool erfolgreich beendet oder fehlgeschlagen,
- externe Datenübertragung bewusst bestätigt.

Audit-Ereignisse sind keine normalen Debug-Logs und keine vollständige Aufzeichnung persönlicher Inhalte.

## Log-Level

### Trace

Nur für eng begrenzte lokale Diagnose. Darf nicht standardmäßig dauerhaft aktiviert sein.

### Debug

Interne Zustands- und Ablaufdetails für Entwicklung. Keine vollständigen Nutzereingaben oder Providerantworten.

### Information

Erwartete erfolgreiche oder kontrollierte Abläufe, zum Beispiel:

- Anwendung gestartet oder beendet,
- Agentenlauf gestartet oder abgeschlossen,
- Benutzerabbruch verarbeitet,
- Toolfreigabe abgelehnt,
- Konfiguration erfolgreich geladen.

### Warning

Ein unerwarteter oder beeinträchtigter Zustand, den Astra kontrolliert behandeln kann, zum Beispiel:

- Ollama nicht erreichbar,
- Modell nicht verfügbar,
- begrenzter Retry,
- Timeout mit kontrollierter Benutzerreaktion,
- beschädigter Cache wird verworfen.

### Error

Eine Operation ist unerwartet fehlgeschlagen, der Prozess bleibt jedoch kontrolliert, zum Beispiel:

- Datenbankoperation fehlgeschlagen,
- Agentenereignis konnte nicht übersetzt werden,
- Toolausführung endete mit unerwarteter Exception.

### Critical

Astra kann einen sicheren Zustand nicht aufrechterhalten, zum Beispiel:

- Startkonfiguration kann nicht geladen werden,
- Datenbankintegrität ist gefährdet,
- unbehandelte Exception am Prozessrand,
- kontrollierter Shutdown schlägt fehl.

Cancellation, Benutzerablehnung und normale Validierungsfehler sind nicht automatisch `Error`.

## Einmaliges Exception-Logging

Eine Exception wird an der Grenze protokolliert, an der genügend Kontext für eine sinnvolle Diagnose vorhanden ist. Sie wird nicht in jeder Schicht erneut geloggt.

Typischer Ablauf:

1. Ein technischer Adapter wirft eine konkrete Exception.
2. Die zuständige Boundary ordnet sie einem Astra-Fehlercode zu.
3. Diese Boundary loggt Exception, Operation und sichere Metadaten genau einmal.
4. Der öffentliche Vertrag liefert einen kontrollierten Fehlerzustand weiter.
5. Die UI zeigt eine verständliche Meldung ohne Stacktrace oder interne Details.

Ein `catch` darf behandeln, kontrolliert übersetzen oder mit zusätzlichem Kontext weiterwerfen. Leere Catch-Blöcke und mehrfaches Log-and-Rethrow ohne neue Verantwortung sind nicht erlaubt.

## Korrelation

Jeder relevante Ablauf erhält stabile, nicht personenbezogene Korrelationswerte:

- `OperationId` für eine allgemeine Operation,
- `SessionId` für eine Astra-Sitzung,
- `AgentRunId` für einen Agentenlauf,
- `ProviderCallId` für einen Modellprovider-Aufruf,
- `ToolRunId` für eine Toolausführung,
- `TraceId` und `SpanId` für OpenTelemetry.

IDs werden als strukturierte Felder weitergegeben. Sie dürfen nicht aus Prompttext, Dateipfaden oder anderen persönlichen Inhalten abgeleitet werden.

## Strukturierte Ereignisse

Logeinträge verwenden stabile Event-IDs und benannte Felder statt String-Verkettung. Geeignete Felder sind beispielsweise:

- Komponente,
- Operation,
- Ergebnis,
- Fehlercode,
- Dauer in Millisekunden,
- Providername,
- Modellkennung,
- Anzahl Nachrichten,
- Anzahl Anhänge,
- Risikoklasse,
- Freigabeentscheidung,
- Exceptiontyp.

Freitext bleibt knapp und enthält keine dynamischen persönlichen Inhalte.

## Datenschutz und Redaction

Standardmäßig nicht protokolliert werden:

- vollständige Prompts oder Antworten,
- Gesprächsinhalte,
- Tool-Argumente und Tool-Ergebnisse,
- Dateiinhalte,
- vollständige lokale Pfade,
- API-Schlüssel, Tokens, Cookies oder Zertifikate,
- Datenbankinhalte,
- Kamera-, Mikrofon- oder Sensordaten,
- personenbezogene Namen, Adressen oder Kontaktdaten.

Wo Diagnosemetadaten notwendig sind, werden sichere Ersatzwerte verwendet, zum Beispiel Anzahl, Typ, Länge, Hash einer nicht sensiblen technischen Kennung oder eine redigierte Kategorie. Redaction verwendet bevorzugt eine Allowlist zulässiger Felder statt einer unvollständigen Verbotsliste.

## Lokale Speicherung

- Logs liegen unter `%LOCALAPPDATA%\Astra\logs`.
- Traces liegen getrennt unter `%LOCALAPPDATA%\Astra\traces`.
- Die Installation und das Repository bleiben schreibgeschützt gegenüber Laufzeitdiagnosen.
- Dateigröße und Aufbewahrungsdauer erhalten sichere Standardwerte und sind begrenzt.
- Alte Dateien werden kontrolliert rotiert und entfernt.
- Ein Absturz darf nicht zu unbegrenztem Wachstum oder blockiertem Start führen.

Konkrete Standardwerte werden mit der File-Logger-Implementierung festgelegt und getestet.

## Diagnoseexport

Ein späterer eigener vertikaler Slice kann einen bewussten Diagnoseexport bereitstellen. Er darf nur nach Benutzeraktion entstehen und soll vor dem Export anzeigen, welche Kategorien enthalten sind.

Ein Diagnosepaket kann enthalten:

- ausgewählte Logs und Traces,
- Astra-Version und Commit,
- .NET- und Windows-Version,
- Provider- und Modellkennung ohne Secrets,
- Konfigurationsschema und aktivierte Fähigkeiten,
- Fehler- und Korrelations-IDs.

Der Export muss sensible Werte erneut redigieren. Er darf keine vollständigen Gespräche oder Dateien still beilegen.

## Testverträge

Mindestens zu testen sind:

- Cancellation wird nicht als Systemfehler geloggt,
- Providerfehler, Timeout und Validierungsfehler bleiben unterscheidbar,
- eine Korrelations-ID bleibt über einen Agentenlauf erhalten,
- unerwartete Exceptions werden genau an der verantwortlichen Boundary protokolliert,
- Benutzerfehler enthalten keine internen Stacktraces,
- Prompts, Secrets, vollständige Pfade und Tool-Argumente gelangen nicht in Logs oder Traces,
- Audit-Ereignisse bilden Freigaben und Toolausgänge nachvollziehbar ab,
- Rotation und Größenbegrenzung verhindern unbegrenztes Wachstum, sobald ein File-Logger existiert.

Tests prüfen stabile Event-IDs, Fehlercodes und erlaubte Felder. Sie sollen nicht jeden Freitext oder die exakte Reihenfolge interner Debug-Einträge festschreiben.
