# ADR-006: Tool-basierte Berechtigungen

Status: Accepted

## Kontext

Ein persönlicher Desktop-Agent darf Risiken nicht über verbotene Wörter oder frei erzeugte Shell-Kommandos bewerten.

## Entscheidung

Jedes Tool besitzt eine feste Risikoklasse, typisierte Parameter, definierte Freigaberegeln, Timeout, Cancellation und Audit-Ereignisse. Freie Shell-Ausführung gehört nicht zum Kern.

## Folgen

- Sicherheit wird pro Fähigkeit modelliert
- verändernde und destruktive Aktionen benötigen sichtbare Freigaben
- Tool-Wrapper delegieren an normale Application-Services
- MCP-Tools werden vor Registrierung klassifiziert und begrenzt
