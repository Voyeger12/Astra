# Paths and Storage

## Grundsatz

Build-Ausgaben, Installationsdateien und Nutzerdaten bleiben strikt getrennt. Astra speichert keine veränderlichen Daten im Repository, unter `artifacts/`, neben der ausführbaren Datei oder im Installationsverzeichnis.

## Runtime-Stammverzeichnis

Unter Windows verwendet Astra den vom Betriebssystem gelieferten Pfad für lokale Anwendungsdaten:

```text
%LOCALAPPDATA%\Astra\
├── config\
│   └── settings.json
├── data\
│   └── astra.db
├── logs\
├── cache\
├── backups\
├── traces\
└── temp\
```

Der konkrete Pfad wird über .NET-Betriebssystem-APIs ermittelt und niemals mit einem Benutzer- oder Laufwerksnamen fest codiert.

## Pfadabstraktion

Die Application-Schicht arbeitet ausschließlich gegen eine Abstraktion wie `IAstraPaths`. Nur Infrastructure bestimmt echte Betriebssystempfade.

Die Abstraktion stellt mindestens bereit:

- Root-Verzeichnis,
- Konfigurationsverzeichnis,
- Datenbankpfad,
- Logverzeichnis,
- Cacheverzeichnis,
- Backupverzeichnis,
- Traceverzeichnis,
- temporäres Verzeichnis.

## Tests

Tests verwenden für jeden Lauf ein isoliertes temporäres Stammverzeichnis. Sie dürfen niemals auf die reale Astra-Konfiguration oder Nutzerdatenbank zugreifen.

Temporäre Testdaten werden nach dem Lauf kontrolliert entfernt. Bei fehlgeschlagenen Tests kann eine bewusste Diagnoseoption die Daten erhalten, sie darf aber nicht standardmäßig aktiv sein.

## Konfiguration

Konfiguration wird in drei Ebenen getrennt:

1. mitgelieferte sichere Standardwerte,
2. lokale Benutzereinstellungen unter `config/`,
3. Secrets über eine separate Secret-Store-Abstraktion.

Normale Einstellungen sind beispielsweise Ollama-Endpunkt, Modellwahl, Sprache, Theme, Logging-Level und erlaubte lokale Verzeichnisse.

API-Schlüssel, OAuth-Tokens und externe Zugangsdaten gehören nicht in `settings.json`.

## Datenbank

- Die produktive SQLite-Datenbank liegt ausschließlich unter `data/`.
- WAL-, SHM- und Journaldateien bleiben im Runtime-Datenverzeichnis.
- Schemaänderungen verwenden versionierte Migrationen.
- Vor risikobehafteten Migrationen wird ein konsistentes Backup unter `backups/` erzeugt.
- Eine fehlgeschlagene Migration darf kein halb migriertes Schema hinterlassen.

## Logs und Traces

- Logs und Traces werden getrennt gespeichert.
- Persönliche Inhalte, vollständige Prompts, Dateiinhalte, Tokens und Secrets werden nicht ungeprüft protokolliert.
- Aufbewahrung und Größenbegrenzung werden konfigurierbar und erhalten sichere Standardwerte.
- Diagnoseexporte benötigen eine bewusste Benutzeraktion.

## Cache und temporäre Dateien

Cache und temporäre Dateien dürfen jederzeit rekonstruierbar beziehungsweise entbehrlich sein. Fachlich relevante Daten dürfen nicht ausschließlich dort gespeichert werden.

Temporäre Dateien werden atomar geschrieben, soweit ein unvollständiger Schreibvorgang sonst zu einem inkonsistenten Zustand führen könnte.

## Deinstallation und Datenlöschung

Die Deinstallation entfernt standardmäßig nur die Anwendung. Nutzerdaten bleiben erhalten.

Eine getrennte, explizite Aktion kann alle Astra-Daten löschen. Sie muss:

- den betroffenen Pfad sichtbar machen,
- den Umfang konkret benennen,
- eine explizite Bestätigung verlangen,
- laufende Dienste vorher kontrolliert beenden,
- Fehler beim Löschen transparent melden.

Ollama-Modelle gehören nicht zu Astras Datenverzeichnis und werden nicht ungefragt gelöscht.
