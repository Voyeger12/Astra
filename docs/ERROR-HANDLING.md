# Error Handling

## Ziel

Astra behandelt Fehler als explizite, typisierte Zustände. Technische Exceptions dürfen nicht ungefiltert durch Schichten oder bis in die Benutzeroberfläche wandern. Gleichzeitig dürfen Ursachen nicht verschluckt, pauschal umgedeutet oder nur durch generische Meldungen verborgen werden.

## Fehlerkategorien

Astra unterscheidet mindestens:

- `Validation`: Eingabe oder Konfiguration ist ungültig.
- `Cancelled`: Benutzer oder Lifecycle hat die Operation kontrolliert abgebrochen.
- `Timeout`: Eine Operation hat ihr definiertes Zeitbudget überschritten.
- `Unavailable`: Provider oder externe Abhängigkeit ist nicht erreichbar.
- `NotFound`: Erwartete Ressource, Sitzung, Modell oder Datei fehlt.
- `Conflict`: Zustand erlaubt die angeforderte Aktion aktuell nicht.
- `Unauthorized`: Eine erforderliche Berechtigung oder Freigabe fehlt.
- `Rejected`: Benutzer oder Policy hat eine Aktion bewusst abgelehnt.
- `RateLimited`: Eine externe Grenze verhindert die aktuelle Ausführung.
- `Persistence`: Speichern, Laden oder Migration ist fehlgeschlagen.
- `Integrity`: Daten oder Zustand sind inkonsistent oder beschädigt.
- `ToolFailure`: Ein kontrolliertes Tool konnte seine Aufgabe nicht abschließen.
- `ProviderFailure`: Ein Modellprovider ist unerwartet fehlgeschlagen.
- `Unexpected`: Kein definierter Fehlervertrag passt sicher.

Die konkrete C#-Darstellung wird im ersten vertikalen Slice contract-first festgelegt. Die Kategorie ist stabiler als eine konkrete Exceptionklasse oder UI-Formulierung.

## Fehlercode

Jeder öffentliche Fehlerzustand besitzt einen stabilen maschinenlesbaren Code, beispielsweise:

```text
ASTRA-AGENT-PROVIDER-UNAVAILABLE
ASTRA-AGENT-TIMEOUT
ASTRA-TOOL-APPROVAL-REJECTED
ASTRA-STORAGE-INTEGRITY
ASTRA-UNEXPECTED
```

Fehlercodes:

- sind unabhängig vom UI-Text,
- enthalten keine Datenwerte oder IDs,
- werden dokumentiert und getestet,
- dürfen nicht still für eine andere Bedeutung wiederverwendet werden,
- können zusammen mit einer Korrelations-ID angezeigt werden.

## Öffentliche Fehlerdaten

Ein an UI oder Agentenvertrag weitergegebener Fehler enthält nur sichere Informationen:

- Fehlercode,
- Kategorie,
- verständliche Kurzbeschreibung,
- Hinweis, ob Wiederholen sinnvoll sein kann,
- optionale Korrelations-ID,
- optionale sichere Handlungsaufforderung.

Nicht öffentlich weitergegeben werden:

- Stacktrace,
- innere Exceptions,
- vollständige lokale Pfade,
- SQL oder Datenbankinhalte,
- Prompts, Antworten oder Toolargumente,
- Secrets oder Provider-Header,
- interne Klassennamen, sofern sie keinen Benutzerwert haben.

## Exception-Mapping

Technische Exceptions werden ausschließlich an klaren Grenzen in Astra-Fehler übersetzt, zum Beispiel:

- Ollama- oder HTTP-Adapter zu Providerfehlern,
- SQLite-Repository zu Persistenz- oder Integritätsfehlern,
- Tooladapter zu Toolfehlern,
- Composition Root zu Start- oder Shutdownfehlern.

Application und Domain sollen nicht von provider- oder datenbankspezifischen Exceptiontypen abhängen.

Ein unbekannter Fehler wird nicht als Timeout, Offlinezustand oder Validierungsproblem geraten. Er wird als `Unexpected` behandelt, mit technischer Ursache geloggt und sicher an den Aufrufer übersetzt.

## Cancellation

`OperationCanceledException` wird nur dann als kontrollierte Cancellation behandelt, wenn der zugehörige `CancellationToken` tatsächlich abgebrochen wurde.

Ein fremder Timeout, Providerabbruch oder interner Fehler darf nicht pauschal als Benutzer-Cancellation erscheinen. Cancellation wird normalerweise als `Information` protokolliert und erzeugt keinen Fehleralarm.

## Retry

Ein Fehler ist nicht automatisch wiederholbar.

Retries benötigen:

- eine explizit als transient eingestufte Fehlerkategorie,
- begrenzte Anzahl,
- Cancellation-Unterstützung,
- Backoff,
- Protokollierung des Grundes,
- Schutz vor Wiederholung nicht idempotenter Aktionen.

Validierungsfehler, Ablehnungen, fehlende Freigaben und Integritätsfehler werden nicht automatisch wiederholt.

## Fehlergrenzen

### Domain

Domainregeln liefern deterministische Ergebnisse oder domänenspezifische Fehlerzustände. Technische Infrastruktur-Exceptions gehören nicht in Domain.

### Application

Application orchestriert Fehlerverträge, entscheidet aber nicht anhand konkreter HTTP-, SQLite- oder Framework-Exceptions.

### Infrastructure und AgentFramework

Adapter übersetzen externe Exceptions in Astra-Verträge und liefern den technischen Logging-Kontext.

### Presentation

Die UI stellt sichere Meldungen und mögliche nächste Schritte dar. Sie entscheidet nicht anhand von Exceptiontypen über Fachverhalten.

### Desktop

Der Composition Root behandelt Start-, Host- und Shutdownfehler sowie die letzte globale Exceptiongrenze. Eine globale Grenze ist kein Ersatz für lokale, fachlich sinnvolle Behandlung.

## Unbehandelte Exceptions

Unbehandelte Exceptions werden am Prozessrand mit `Critical`, Korrelationsdaten und Stacktrace protokolliert. Astra versucht nur dann einen kontrollierten Zustand oder Shutdown, wenn dies ohne zusätzliche Datengefährdung möglich ist.

Nicht erlaubt sind:

- stilles Weiterlaufen nach unbekannt beschädigtem Zustand,
- leere globale Handler,
- Dialoge mit vollständigem Stacktrace,
- automatisches Löschen von Daten als vermeintliche Reparatur,
- mehrfaches Loggen derselben Exception in jeder Schicht.

## Benutzererlebnis

Eine gute Fehlermeldung beantwortet möglichst:

- Was konnte nicht durchgeführt werden?
- Ist der Zustand sicher?
- Was kann die Person als Nächstes tun?
- Welche Fehler- oder Korrelations-ID hilft bei der Diagnose?

Beispiel:

```text
Der lokale KI-Dienst ist nicht erreichbar. Prüfe, ob Ollama gestartet ist, und versuche es erneut.
Fehlercode: ASTRA-AGENT-PROVIDER-UNAVAILABLE
Diagnose-ID: 4f2c…
```

## Testverträge

Mindestens zu testen sind:

- technische Exceptions werden in die richtige Kategorie übersetzt,
- unbekannte Exceptions bleiben `Unexpected`,
- Cancellation und Timeout werden nicht verwechselt,
- sichere UI-Fehler enthalten weder Stacktrace noch sensible Daten,
- Retry wird nur für ausdrücklich transiente und idempotente Abläufe angeboten,
- eine Exception wird nicht mehrfach geloggt,
- Fehlercode und Korrelations-ID bleiben über Eventstream und UI-Zustand erhalten,
- Offline- und Fehlerzustände führen nicht zu einem Prozessabsturz,
- Integritätsfehler führen nicht zu stiller Datenlöschung oder automatischer Neuerstellung.
