# ADR-0010: Erste Distributionsstrategie

- Status: Accepted
- Datum: 2026-07-15

## Kontext

Astra benötigt früh einen reproduzierbaren ausführbaren Stand, ohne vorschnell Installer, Auto-Updater, Trimming oder Native AOT in den Kern einzubauen.

## Entscheidung

Die erste Distributionsform ist:

- Windows x64,
- self-contained .NET-Veröffentlichung,
- ordnerbasiert,
- Release-Konfiguration,
- ohne Single-File, Trimming und Native AOT,
- ohne automatische Installation von Ollama oder Modellen.

Der konkrete Installer und Updatekanal werden erst nach einem eigenen Spike ausgewählt. Der normale Betrieb soll pro Benutzer und ohne Administratorrechte möglich sein.

## Konsequenzen

- Frühe Releases sind größer, aber unabhängig von einer vorinstallierten .NET Runtime.
- Runtime-Sicherheitsupdates erfordern eine neue Astra-Veröffentlichung.
- Kompatibilitätsprobleme mit Reflection, WPF, Agent Framework und nativen Bibliotheken werden nicht durch vorzeitige Optimierungen verschärft.
- Ein späterer Installer muss Programm- und Nutzerdaten strikt trennen.

## Spätere Prüfpunkte

- Code Signing,
- Integritätsprüfung,
- atomare Updates und Rollback,
- MSIX oder alternative Installer,
- ARM64,
- Single-File,
- Trimming oder AOT,
- Releasekanäle.
