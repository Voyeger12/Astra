# Vision

## Zweck

Astra soll ein persönlicher, lokal-first KI-Agent für Windows werden, der Gespräche versteht, Werkzeuge kontrolliert verwendet, sich an relevante Zusammenhänge erinnert und später auf Wunsch sehen, hören und sprechen kann.

Astra ist weder ein allgemeiner autonomer Bot noch ein Chatfenster mit dekorativen Zusatzfunktionen. Das Produktziel ist ein verlässlicher persönlicher Agent, dessen Fähigkeiten nachvollziehbar, begrenzt und durch den Benutzer steuerbar bleiben.

## Produktversprechen

Astra soll:

- auch ohne Cloudkonto sinnvoll lokal funktionieren
- lokale Modelle über Ollama verwenden
- Gespräche und Sitzungen fortsetzen können
- langfristige Erinnerungen getrennt vom Chatverlauf verwalten
- über typisierte Tools auf Anwendungen und Daten zugreifen
- vor verändernden oder riskanten Aktionen um Freigabe bitten
- externe Integrationen über MCP anbinden
- Kamera, Mikrofon und TTS nur explizit und sichtbar aktivieren
- Fehler verständlich anzeigen und kontrolliert degradieren

## Nicht-Ziele der ersten Version

- kein unkontrollierter Desktop-Vollzugriff
- keine freie Shell als Standardwerkzeug
- keine dauerhafte Kamera- oder Mikrofonüberwachung
- kein Multi-Agent-System ohne belegten Bedarf
- keine Cloudpflicht
- keine automatische Speicherung jeder persönlichen Information
- keine periodischen Modellaufrufe ohne konkreten Trigger

## Entwicklungsphasen

### Phase 1: Astra Core

Lokaler Agent, Streaming, Sessions, SQLite, saubere Fehlerbehandlung, Abbruch und kontrollierter Lifecycle.

### Phase 2: Tools und Memory

Read-only-Tools, Tool-Policies, Freigaben und semantisches Langzeitgedächtnis.

### Phase 3: MCP-Integrationen

Zunächst read-only, beginnend mit GitHub. Schreibfähigkeiten folgen nur mit eigener Policy und Bestätigungsfluss.

### Phase 4: Wahrnehmung

On-Demand-Vision, Spracheingabe und TTS. Sensorzugriffe bleiben opt-in und sichtbar.

### Phase 5: Kontrollierte Proaktivität

Deterministische Trigger wie fällige Erinnerungen oder abgeschlossene Aufgaben. Modellgestützte Autonomie folgt erst nach stabilen Policies, Tools und Evals.

## Erfolgskriterium

Astra ist erfolgreich, wenn es nicht nur beeindruckend wirkt, sondern im Alltag zuverlässig Aufgaben versteht, passende Werkzeuge verwendet, Grenzen einhält und nach einem Neustart nachvollziehbar weiterarbeiten kann.
