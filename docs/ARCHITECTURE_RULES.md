# Architecture Rules

Diese Regeln sind verbindlich. Abweichungen benötigen eine bewusste Architekturentscheidung und in der Regel ein ADR.

## Schichten

### Astra.Desktop

Verantwortlich für WPF, Views, ViewModels, UI-Zustand und Benutzerinteraktion.

Nicht erlaubt:

- direkter Zugriff auf Ollama, SQLite, MCP oder Microsoft Agent Framework,
- Fachlogik in ViewModels,
- globale UI-Events als Anwendungsarchitektur,
- Datenbank- oder Tool-Policies in der Oberfläche.

### Astra.Application

Verantwortlich für Use Cases, Orchestrierung, Ports und fachliche Ablaufkoordination.

Nicht erlaubt:

- WPF-Typen,
- konkrete SQLite-, Ollama-, MCP- oder Agent-Framework-Typen,
- Infrastrukturdetails in öffentlichen Verträgen.

### Astra.Domain

Verantwortlich für fachliche Modelle, Regeln, Werteobjekte und unveränderliche Verträge.

Nicht erlaubt:

- Referenzen auf Desktop, Infrastructure, AgentFramework oder Tools,
- Frameworkattribute und Persistenzdetails,
- Netzwerk-, Datei- oder UI-Zugriffe.

### Astra.AgentFramework

Adapter zwischen Astra.Application und Microsoft Agent Framework.

Verantwortlich für:

- Agentenerzeugung,
- Session-Adapter,
- Tool-Registrierung,
- Middleware,
- Übersetzung von Frameworkereignissen in Astra-Ereignisse.

Nicht erlaubt:

- fachliche Geschäftsregeln,
- Sicherheitsentscheidungen nur über Prompts,
- direkte UI-Abhängigkeiten,
- Persistenzlogik außerhalb definierter Ports.

### Astra.Infrastructure

Implementiert technische Ports, etwa Modellprovider, SQLite, Logging, MCP und Betriebssystemzugriffe.

Nicht erlaubt:

- fachliche Berechtigungsentscheidungen,
- UI-Verhalten,
- Agentenprompts als Ersatz für Anwendungscode.

### Astra.Tools

Enthält typisierte Agentenadapter auf Application-Services.

Ein Tool darf keine eigene Fachlogik besitzen. Es validiert den Aufruf, delegiert an einen Use Case oder Service und übersetzt das Ergebnis in einen klaren Toolvertrag.

## Abhängigkeitsrichtung

Erlaubt:

```text
Astra.Desktop -> Astra.Application -> Astra.Domain
Astra.AgentFramework -> Astra.Application
Astra.Infrastructure -> Astra.Application
Astra.Tools -> Astra.Application
```

Domain bleibt unabhängig. Application kennt nur Abstraktionen. Implementierungsprojekte dürfen Application-Ports implementieren, aber Application referenziert ihre konkreten Typen nicht.

Neue Projektverweise werden vor Aufnahme gegen diese Richtung geprüft.

## Agentenarchitektur

- Astra startet mit einem einzelnen Agenten.
- Multi-Agent-Orchestrierung benötigt einen konkreten, gemessenen Anwendungsfall und ein ADR.
- Der Agent Loop wird vom Microsoft Agent Framework betrieben.
- Astra kontrolliert Fachlogik, Daten, Policies, Lifecycle, Providerkonfiguration und Freigaben.
- Der normale Dialog beginnt nicht mit einem vollständigen autonomen Harness.
- Ein spezialisiertes Harness oder Workflow-Modell darf später als eigener Modus ergänzt werden.
- Deterministische Regeln werden in Code implementiert, nicht in Prompts ausgelagert.

## Agentenvertrag

Die UI arbeitet gegen einen Astra-eigenen Vertrag, der mindestens unterstützt:

- Streaming-Text,
- Statusänderungen,
- Tool-Anfragen,
- Freigabeanforderungen,
- Tool-Ergebnisse,
- Abschluss,
- Fehler,
- Cancellation.

Kein `string rein, string raus`-Interface, das Tool- und Agentenereignisse versteckt.

## Tool-Regeln

Jedes Tool benötigt:

- eindeutigen Namen und Zweck,
- typisierte Parameter,
- typisiertes Ergebnis,
- definierte Fehler,
- Risikoklasse,
- Policy,
- Tests,
- Logging ohne sensible Daten.

Nicht erlaubt:

- freie Shell im Kern,
- allgemeines `ExecuteCommand`,
- ungeprüfte Übergabe an Dateisystem, SQL oder Prozesse,
- Schreibzugriffe ohne passende Risikoklasse,
- automatische Freigabe durch das Modell,
- Fachlogik im Tool selbst.

Ein Tool ist ein kontrollierter Adapter auf normale Anwendungskomponenten. UI und Agent dürfen denselben Application-Service über unterschiedliche Adapter nutzen.

## Memory und Zustand

Folgende Zustände bleiben getrennt:

- Conversation State: aktueller Gesprächsverlauf und Session.
- Long-Term Memory: bewusst persistierte langfristige Informationen.
- Tool Run Log: nachvollziehbare Aktionen und Ergebnisse.
- Application State: Verfügbarkeit von Modell, Sensoren und Diensten.

Nicht erlaubt:

- alle Zustände in einen Chatverlauf zu kippen,
- Persistenzmodelle ungeprüft als Domainmodelle zu verwenden,
- persönliche Informationen automatisch ohne Regel und Nachvollziehbarkeit zu speichern,
- Tool-Logs als Memory zu behandeln.

## Persistenz

- SQLite ist der erste lokale Speicher.
- Schemaänderungen benötigen Migrationen.
- Transaktionen umfassen fachlich vollständige Operationen.
- Nutzerdaten werden nicht still gelöscht oder überschrieben.
- Tests nutzen isolierte temporäre Datenbanken.
- Repositories und Persistenzadapter dürfen keine Agenten- oder UI-Entscheidungen treffen.

## Lifecycle und Nebenläufigkeit

- Der Generic Host besitzt Start und Shutdown der Dienste.
- Jeder Hintergrunddienst unterstützt Cancellation.
- Keine Endlosschleife ohne Abbruchpfad.
- Keine festen Sleeps zur Synchronisation.
- Keine unkontrollierten Fire-and-Forget-Tasks.
- Ressourcen werden deterministisch freigegeben.
- UI-Aktualisierungen erfolgen im richtigen Dispatcher-Kontext.
- Wiederholungen sind begrenzt und berücksichtigen Idempotenz.

## Abstraktionen

Nicht erlaubt sind spekulative Abstraktionen ohne realen Anwendungsfall.

- Keine Factory, wenn Dependency Injection genügt.
- Kein Event Bus, wenn ein direkter Aufruf klarer ist.
- Keine Basisklasse nur zur Wiederverwendung weniger Zeilen.
- Kein Repository-Pattern aus Gewohnheit, wenn es keinen klaren Port oder Testnutzen bietet.
- Kein Multi-Agent-System als Prestige-Architektur.
- Keine generische Plattform für hypothetische Anforderungen.

Abstraktionen müssen mindestens einen konkreten Entkopplungs-, Test- oder Austauschbedarf lösen.

## Refactoring und Änderungen

- Bugfixes verändern die Root Cause mit kleinstmöglichem Blast Radius.
- Architekturumbau und Featureänderung werden getrennt.
- Keine großflächigen Umbenennungen oder Formatierungen als Nebenprodukt.
- Öffentliche Verträge werden nicht still verändert.
- Neue tragende Technologien benötigen ein ADR.
- Ein Feature wird als vollständiger vertikaler Ablauf umgesetzt und getestet.

## Durchsetzung

Architekturregeln werden durch folgende Maßnahmen abgesichert:

- Projektverweise und Solution-Struktur,
- Architekturtests,
- Analyzer und Nullable Reference Types,
- Pull-Request-Checks,
- Agent Contract Tests,
- Repository Skills und Copilot-Instructions.

Ein grüner Build hebt keine verletzte Architekturregel auf.

## Stop-Regel

Wenn die richtige Schicht, Vertrauensgrenze oder Auswirkung unklar ist, wird nicht spekulativ implementiert. Zuerst werden Datenfluss, Verträge, relevante ADRs und Dokumentation geprüft.