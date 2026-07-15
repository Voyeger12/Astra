# Installation and First Run

## Ziel

Installation und Erststart sind nachvollziehbar, wiederholbar und abbrechbar. Astra verändert das System nicht still und installiert weder Modelle noch externe Dienste ohne bewusste Zustimmung.

## Erste Distributionsstufe

Die erste lauffähige Veröffentlichung ist eine self-contained Windows-x64-Ordnerverteilung. Ein klassischer Installer und ein automatischer Updatekanal folgen erst nach einem gesonderten technischen Spike.

Nicht Bestandteil der ersten Stufe:

- Single-File-Publishing,
- Native AOT,
- Trimming,
- ARM64-Paket,
- automatische Ollama-Installation,
- automatische Modell-Downloads,
- stiller Auto-Updater.

## Installationsvertrag

- Astra wird pro Benutzer betrieben.
- Der normale Betrieb benötigt keine Administratorrechte.
- Das Installationsverzeichnis enthält Anwendung und unveränderliche Ressourcen.
- Veränderliche Daten liegen unter `%LOCALAPPDATA%\Astra`.
- Ollama und installierte Modelle werden als externe, gemeinsam nutzbare Ressourcen behandelt.
- Astra führt keine stillen Paketmanager-, Prozess- oder Netzwerkaktionen aus.

## Erststart

Der Erststart wird als wiederholbarer Setup-Zustand modelliert. Er prüft später in dieser Reihenfolge:

1. Runtime-Datenverzeichnis kann erstellt und beschrieben werden.
2. Konfiguration kann geladen oder mit sicheren Standardwerten erzeugt werden.
3. SQLite-Datenbank kann geöffnet und migriert werden.
4. Ollama-Endpunkt ist erreichbar oder Astra wechselt kontrolliert in den Offline-Zustand.
5. Verfügbare Modelle werden ermittelt.
6. Ein empfohlenes Modell wird vorgeschlagen, aber nicht ungefragt heruntergeladen.
7. Optionale Integrationen werden einzeln erklärt und aktiviert.
8. Datenschutz-, Sensor- und Datenfreigaben werden getrennt erfasst.

Ein Abbruch darf keine halb konfigurierte Anwendung hinterlassen. Bereits erfolgreiche Schritte müssen entweder idempotent wiederholt oder sauber fortgesetzt werden können.

## Offline-Zustand

Fehlendes oder nicht erreichbares Ollama verhindert nicht den Start der Oberfläche. Astra zeigt den Zustand verständlich an und bietet Diagnose beziehungsweise erneute Prüfung an.

Offline-Zustände werden nicht durch Endlosschleifen oder aggressive Retries behandelt. Wiederholungen sind begrenzt und abbrechbar.

## Modellbereitstellung

- Modelle sind nicht Bestandteil des Astra-Installationspakets.
- Vor einem Download werden Modellname, geschätzter Speicherbedarf und Quelle angezeigt.
- Der Benutzer bestätigt den Download ausdrücklich.
- Ein abgebrochener Download wird kontrolliert behandelt.
- Astra löscht keine Modelle, die auch von anderen Anwendungen verwendet werden könnten.

## Updates

Ein Updatekanal wird erst eingeführt, wenn folgende Punkte geklärt und getestet sind:

- Signierung und Integritätsprüfung,
- atomare oder rückrollbare Aktualisierung,
- Datenbankmigrationen,
- Verhalten bei laufender Anwendung,
- Abbruch und Wiederaufnahme,
- Releasekanäle,
- Datenschutz und Telemetrie.

Bis dahin erfolgen Releases manuell und transparent über versionierte Artefakte.

## Deinstallation

Standardverhalten:

- Anwendung entfernen,
- Nutzerdaten behalten,
- Ollama und Modelle nicht verändern.

Eine separate Option darf alle Astra-Daten entfernen. Sie muss den Datenpfad, den Umfang und die Folgen deutlich anzeigen und eine explizite Bestätigung verlangen.

## Installer-Auswahl

Die konkrete Installer-Technologie wird nicht vorweggenommen. Sie wird anhand folgender Kriterien ausgewählt:

- per-user ohne Administratorrechte,
- verlässliche Updates und Deinstallation,
- Signierbarkeit,
- Support für WPF und self-contained .NET,
- keine Vermischung von Programm- und Nutzerdaten,
- geringe Wartungskomplexität.
