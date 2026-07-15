# Security Policy

Dieses Dokument definiert die Sicherheitsregeln für Astra. Sie gelten für Menschen, Coding-Agenten, Tools, externe Integrationen und alle automatisierten Änderungen.

## Sicherheitsgrundsatz

Sicherheitsentscheidungen werden deterministisch durch Anwendungscode getroffen. Das Sprachmodell darf Berechtigungen, Freigaben oder Schutzgrenzen weder selbst festlegen noch umgehen.

## Schwachstellen melden

Sicherheitsprobleme sollen nicht als öffentliches Issue mit ausnutzbaren Details veröffentlicht werden. Bis ein privater Meldeweg eingerichtet ist, sollen sensible Details direkt an den Repository-Eigentümer übermittelt werden.

## Secrets und Zugangsdaten

- Keine API-Schlüssel, Tokens, Passwörter oder Zertifikate im Quellcode.
- Keine Secrets in Tests, Beispielen, Logs, Traces, Screenshots oder Commit-Historie.
- Lokale Geheimnisse werden über sichere Konfiguration oder Betriebssystemmechanismen bereitgestellt.
- Beispielkonfigurationen enthalten ausschließlich Platzhalter.
- Ein versehentlich veröffentlichtes Secret gilt als kompromittiert und muss ersetzt werden.

## Logging und Telemetrie

- Keine vollständigen Tokens, Prompts, persönlichen Inhalte oder Dateiinhalte in Logs.
- Logs verwenden strukturierte Felder und notwendige technische Metadaten.
- Sensible Werte werden redigiert oder vollständig ausgelassen.
- OpenTelemetry-Traces dürfen keine privaten Gesprächsinhalte oder Tool-Argumente ungeprüft erfassen.
- Fehlerdiagnose darf Datenschutzgrenzen nicht aufweichen.

## Eingaben und Systemgrenzen

- Externe Eingaben werden an jeder Vertrauensgrenze validiert.
- SQL wird ausschließlich parametrisiert ausgeführt.
- Dateipfade werden normalisiert und gegen explizit erlaubte Bereiche geprüft.
- Prozesse werden nicht über frei zusammengesetzte Kommandozeilen gestartet.
- URLs, MCP-Ergebnisse, Dokumente und Tool-Ausgaben gelten als nicht vertrauenswürdige Daten.
- Externe Inhalte können Prompt Injection enthalten und dürfen keine Systemanweisungen ersetzen.

## Tool-Sicherheit

Jedes Tool benötigt:

- einen eindeutigen Zweck,
- typisierte Parameter,
- ein typisiertes Ergebnis,
- eine Risikoklasse,
- definierte Fehler,
- Logging ohne sensible Daten,
- Tests für erlaubte und abgelehnte Abläufe.

Risikoklassen:

- `ReadOnly`: keine dauerhafte Veränderung, automatische Ausführung kann erlaubt sein.
- `UserData`: Zugriff auf persönliche Daten, kontextabhängige Freigabe erforderlich.
- `SystemChange`: verändert Anwendung, Dateien oder Betriebssystem, explizite Freigabe erforderlich.
- `Destructive`: kann Daten löschen oder schwer rückgängig zu machende Folgen haben, konkrete Warnung und explizite Bestätigung erforderlich.
- `SensorAccess`: Kamera, Mikrofon oder andere Sensoren, eigener einmaliger oder dauerhafter Consent erforderlich.
- `ExternalData`: sendet Daten an externe Dienste, Zweck und übertragene Daten müssen sichtbar sein.

Nicht erlaubt:

- ein allgemeines `ExecuteShell(string command)`,
- ungeprüfte Benutzereingaben an Shell, SQL oder Dateisystem,
- Schreibzugriffe als Read-only zu deklarieren,
- automatische Umgehung von Freigaben,
- Tools ohne Policy zu registrieren,
- Tool-Ausgaben als vertrauenswürdige Instruktionen zu behandeln.

## Agentenverhalten

- Der Agent darf Sicherheitsentscheidungen nicht selbst überstimmen.
- Tool-Freigaben werden außerhalb des Prompts durch Anwendungscode erzwungen.
- Der Agent erhält nur die Tools, die für den aktuellen Kontext erforderlich sind.
- Ein Modellfehler darf nicht zu einer impliziten Berechtigungserweiterung führen.
- Bei unklarer Aktion wird nicht ausgeführt, sondern abgebrochen oder eine Freigabe angefordert.

## Daten und Persistenz

- Persönliche Daten werden lokal gespeichert, sofern keine bewusste externe Übertragung freigegeben wurde.
- Schemaänderungen benötigen einen reproduzierbaren Migrationspfad.
- Nutzerdaten dürfen nicht still gelöscht, überschrieben oder migriert werden.
- Tests verwenden isolierte temporäre Datenbanken.
- Löschvorgänge müssen fachlich vollständig, nachvollziehbar und testbar sein.
- Conversation State, Long-Term Memory, Tool Runs und Application State bleiben getrennt.

## Nebenläufigkeit und Ressourcen

- Jeder Hintergrunddienst besitzt definierten Start und Shutdown.
- Keine Endlosschleife ohne `CancellationToken`.
- Keine unkontrollierten Fire-and-Forget-Tasks.
- Ressourcen werden deterministisch über `IDisposable` oder `IAsyncDisposable` freigegeben.
- Retries sind begrenzt, verwenden Backoff und wiederholen keine nicht-idempotenten Aktionen unkontrolliert.

## Abhängigkeiten und Lieferkette

Vor einer neuen NuGet-Abhängigkeit werden geprüft:

- offizieller Ursprung,
- aktive Wartung,
- Lizenz,
- bekannte Sicherheitsrisiken,
- Notwendigkeit gegenüber Standardbibliothek und vorhandenem Stack,
- Auswirkungen auf Transitivabhängigkeiten.

Preview- und Prerelease-Pakete benötigen eine bewusste Architekturentscheidung.

## Sicherheitsrelevante Änderungen

Änderungen an Tool-Policies, Freigaben, Persistenz, Sensoren, externen Integrationen, Prozessausführung oder Authentifizierung benötigen:

1. dokumentierten Bedrohungs- und Wirkungsbereich,
2. Tests für erlaubte und verbotene Abläufe,
3. Prüfung auf Datenabfluss,
4. Prüfung von Cancellation und Fehlerzuständen,
5. einen klar abgegrenzten Pull Request.

## Stop-Regel

Wenn die Sicherheitsauswirkung einer Änderung nicht verstanden ist, darf sie nicht durch breite oder spekulative Änderungen umgesetzt werden. Zuerst werden Kontext, Dokumentation und Vertrauensgrenzen geklärt.