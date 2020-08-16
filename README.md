# Produktionsplanungs- und Steuerungssystem IBSYS 2

## Table of Contents
- Einleitung
- Aufsetzen des Projektes
- Starten des Projektes

### Einleitung

### Aufsetzen des Projektes
Nach dem initialen Klonen in die Rider IDE bzw. Visual Studio IDE kann das Projekt verwendet werden.
<br> Um die lokale Datenbank zu erstellen, muss zu Beginn unter *Tools* > *NuGet Package Manager* > *Package Manager Console* <br>
mittels dem Befehl `Update-Database` die vorhandene Migration in die Datenbank eingespielt werden. Ansonsten ist das Seeding der Daten <br>
nicht problemlos möglich.

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
