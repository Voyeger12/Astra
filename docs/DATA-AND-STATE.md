# Daten- und Zustandslogik

## Grundsatz

Astra trennt Gesprächszustand, langfristiges Gedächtnis, Anwendungszustand und Aktionsprotokolle. Diese Bereiche dürfen nicht in einer gemeinsamen unstrukturierten History verschwimmen.

## Datenbereiche

### Agent Session

Enthält den fortsetzbaren Kontext einer Unterhaltung:

- Session-ID
- Titel
- Erstellungs- und Änderungszeit
- verwendetes Modellprofil
- Framework-spezifischer Sessionzustand
- Status und optionale Zusammenfassung

### Message

- Session-ID
- Rolle
- Inhalt
- Zeitstempel
- Reihenfolge
- optionale strukturierte Teile wie Tool Calls oder Anhänge

### Memory

Langfristige Informationen außerhalb eines einzelnen Gesprächs:

- Inhalt
- Kategorie
- Quelle
- Wichtigkeit
- Vertrauensgrad
- Erstellungs- und Ablaufzeit
- optionaler Embedding-Vektor oder Verweis darauf
- Lösch- und Zustimmungskontext

Automatisches Erinnern persönlicher Daten ist nicht Standard. Memory-Schreibvorgänge benötigen definierte Regeln und müssen nachvollziehbar bleiben.

### Tool Run

- Tool-Name und Call-ID
- validierte Argumente
- Risikoklasse
- Start- und Endzeit
- Ergebnisstatus
- Fehlercode
- Agent Run und Session

Sensible Inhalte werden nicht ungefiltert protokolliert.

### Approval

- zugehöriger Tool Run
- angezeigte Auswirkung
- Entscheidung des Benutzers
- Gültigkeitsbereich
- Zeitstempel

Freigaben gelten standardmäßig nur für den konkreten Aufruf. Dauerhafte Berechtigungen benötigen eine eigene, sichtbare Einstellung.

### Application State

Kurzlebiger Laufzeitzustand wie:

- Online-, Offline- und Fehlerstatus
- aktives Modell
- laufender Agent Run
- Sensorstatus
- verbundene MCP-Server

Dieser Zustand wird nicht automatisch als Langzeit-Memory behandelt.

## Tool-Risikoklassen

- `ReadOnly`: liest lokale oder externe Daten
- `UserData`: verarbeitet persönliche Daten oder Sensorinformationen
- `Modifying`: verändert Daten oder Konfiguration
- `SystemChange`: beeinflusst Anwendungen oder Betriebssystem
- `Destructive`: löscht oder überschreibt schwer wiederherstellbare Daten

Die Risikoklasse ist Metadatum des Tools und wird nicht aus dem erzeugten Befehlstext geschätzt.

## Transaktionen und Lifecycle

- genau ein klarer Eigentümer pro Datenbankverbindung oder DbContext-Scope
- keine globale veränderbare Verbindung
- Schreibvorgänge sind atomar
- Shutdown wartet kontrolliert auf offene Persistenzvorgänge
- Datenbankdateien liegen im Benutzer-AppData-Bereich, nicht im Repository
- Backups verwenden SQLite-kompatible Verfahren

## Datenschutz

Rohbilder und Roh-Audio werden standardmäßig nicht gespeichert. Logs, Tool-Ergebnisse und Traces müssen Redaction unterstützen. Der Benutzer muss Sessions, Memories und Auditdaten getrennt einsehen und löschen können.
