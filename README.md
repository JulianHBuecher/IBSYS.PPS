# Produktionsplanungs- und Steuerungssystem IBSYS 2

## Table of Contents
- Einleitung
- Aufsetzen des Projektes
- Starten des Projektes

### Einleitung

### Aufsetzen des Projektes
Nach dem initialen Klonen in die Rider IDE kann das Projekt verwendet werden.

### Starten des Projektes
Zum Starten des Projektes wird eine Console im Projektordner geöffnet und mit folgenden Kommandos das Projekt laufbereit gemacht.
Bauen des Projektes und Herunterladen aller NuGet-Abhängigkeiten:
```
dotnet build
```
Nach dem initialen Build und keiner neuen Installation eines NuGet-Packages reicht auch:
```
dotnet build --no-restore
```
Um das Projekt mit seiner integrierten `ClientApp` zu starten wird dies verwendet:
```
dotnet run
```
Somit wird die React-App und der zugehörige Service gestartet.
