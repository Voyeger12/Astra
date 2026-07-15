# ADR-005: SQLite für lokale Persistenz

Status: Accepted

## Kontext

Astra benötigt lokale Sessions, Nachrichten, Memories, Tool Runs und Freigaben. Die Datenbasis soll offline-fähig, portabel und wartbar bleiben.

## Entscheidung

SQLite ist die lokale Persistenztechnologie. Der konkrete Zugriff über EF Core oder direkte Repositories wird in einem separaten Spike entschieden.

## Folgen

- Daten liegen im Benutzer-AppData-Bereich
- keine Datenbankdateien im Repository
- klare Repository-Interfaces in der Application-Schicht
- Migrationen, Backups und Shutdown-Verhalten werden getestet
