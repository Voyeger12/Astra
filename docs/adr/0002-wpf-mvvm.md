# ADR-002: WPF und MVVM

Status: Accepted

## Kontext

Astra ist primär für Windows vorgesehen und benötigt eine langfristig wartbare Desktop-Oberfläche mit sauberer Trennung von Darstellung und Logik.

## Entscheidung

Die Desktop-Anwendung verwendet WPF mit MVVM und CommunityToolkit.Mvvm.

## Folgen

- Windows ist das klare Primärziel
- ViewModels sprechen nur mit Application-Interfaces
- Code-behind bleibt auf reine View-Belange begrenzt
- ein späterer UI-Wechsel bleibt durch die Schichtgrenze möglich
