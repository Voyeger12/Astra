# ADR-0009: Runtime-Datenpfade

- Status: Accepted
- Datum: 2026-07-15

## Kontext

Astra verarbeitet persönliche Gespräche, Memories, Tool-Abläufe, Konfiguration und später Sensorinformationen. Diese Daten dürfen nicht mit Buildausgaben oder Installationsdateien vermischt werden.

## Entscheidung

- Veränderliche Runtime-Daten liegen pro Benutzer unter `%LOCALAPPDATA%\Astra`.
- Das Stammverzeichnis wird über Betriebssystem-APIs ermittelt und nicht fest codiert.
- Konfiguration, Datenbank, Logs, Cache, Backups, Traces und temporäre Dateien werden getrennt gespeichert.
- Die Application-Schicht verwendet eine Pfadabstraktion; nur Infrastructure kennt echte Betriebssystempfade.
- Tests verwenden isolierte temporäre Pfade.
- Secrets werden über eine getrennte Secret-Store-Abstraktion verwaltet.
- Anwendung, Repository und `artifacts/` enthalten keine produktiven Nutzerdaten.

## Konsequenzen

- Installation und Updates können die Anwendung ersetzen, ohne Nutzerdaten anzutasten.
- Tests können keine reale Benutzer-Datenbank versehentlich verändern.
- Backup-, Lösch- und Diagnosefunktionen erhalten definierte Grenzen.
- Portable Verteilung ist nicht automatisch gegeben und benötigt später eine eigene Entscheidung.
