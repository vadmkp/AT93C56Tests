# ?? Quick Start Guide - Migracja Serwisów

## ? Co zosta³o zrobione automatycznie

Wszystkie serwisy zosta³y **przeniesione** z projektu `Atmel` do nowego projektu `Atmel.Services`:

```
? 4 Interfaces
? 3 Service Implementations  
? 1 Model
? 3 Configuration classes
? 2 Helper classes
? 2 RFCOMM services
???????????????????????
? 14 plików gotowych
? ProjectGuid naprawiony
? AssemblyInfo.cs dodany
? Compile list naprawiona (jawna lista zamiast wildcard)
```

---

## ?? WA¯NE: Problem z kompilacj¹ zosta³ naprawiony!

Plik `Atmel.Services.csproj` zosta³ zaktualizowany - **wildcard `**\*.cs` zosta³ zamieniony na jawn¹ listê plików** aby zapewniæ poprawn¹ kompilacjê w projekcie UWP.

**Zobacz**: `COMPILATION_FIX.md` jeœli masz problemy z buildem.

---

## ?? CO MUSISZ ZROBIÆ (10 minut)

### ? Krok 0: Wyczyœæ projekty (WA¯NE!)
```powershell
# W PowerShell w folderze g³ównym projektu:
Remove-Item -Recurse -Force Atmel.Services\bin,Atmel.Services\obj -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force Atmel\bin,Atmel\obj -ErrorAction SilentlyContinue
```

Lub rêcznie usuñ foldery:
- `Atmel.Services/bin/`
- `Atmel.Services/obj/`
- `Atmel/bin/`
- `Atmel/obj/`

### ? Krok 1: Otwórz Solution w Visual Studio
```
1. Zamknij Visual Studio jeœli jest otwarty
2. Otwórz: AT93C56Tests.sln
3. Jeœli pojawi siê prompt "Reload projects" ? kliknij "Reload All"
4. SprawdŸ czy Atmel.Services siê za³adowa³ (nie pokazuje "(unavailable)")
```

**Jeœli projekt nie ³aduje siê**: Zobacz `FIXING_PROJECT_LOAD_ISSUE.md`  
**Jeœli s¹ b³êdy kompilacji**: Zobacz `COMPILATION_FIX.md`

### ? Krok 2: Dodaj Project Reference
```
1. W Solution Explorer ? Projekt "Atmel"
2. Prawy przycisk na "References"
3. "Add Reference..."
4. Zak³adka "Projects"
5. ? Zaznacz "Atmel.Services"
6. Kliknij "OK"
```

### ? Krok 3: Zaktualizuj 3 pliki

#### **Plik 1: `Atmel/Infrastructure/ServiceContainer.cs`**
Na pocz¹tku pliku ZMIEÑ:
```csharp
// STARE (usuñ):
using Atmel.Interfaces;
using Atmel.Services;
using Atmel.Configuration;
using Atmel.Silnik;

// NOWE (dodaj):
using Atmel.Services.Interfaces;
using Atmel.Services.Implementation;
using Atmel.Services.Configuration;
using Atmel.Services.Rfcomm;
```

#### **Plik 2: `Atmel/ViewModels/MainPageViewModel.cs`**
Na pocz¹tku pliku ZMIEÑ:
```csharp
// STARE (usuñ):
using Atmel.Interfaces;
using Atmel.Models;
using Atmel.Configuration;

// NOWE (dodaj):
using Atmel.Services.Interfaces;
using Atmel.Services.Models;
using Atmel.Services.Configuration;
```

#### **Plik 3: `Atmel/MainPage.xaml.cs`**
Na pocz¹tku pliku ZMIEÑ:
```csharp
// STARE (usuñ):
using Atmel.Interfaces;
using Atmel.Models;
using Atmel.Configuration;

// NOWE (dodaj):
using Atmel.Services.Interfaces;
using Atmel.Services.Models;
using Atmel.Services.Configuration;
```

### ? Krok 4: Build & Run
```
1. Build ? Rebuild Solution (Ctrl+Shift+B)
2. SprawdŸ Output window: 0 errors, 0 warnings
3. Debug ? Start Debugging (F5)
4. SprawdŸ czy wszystko dzia³a
```

### ? Krok 5: Cleanup (opcjonalnie)
Po weryfikacji ¿e wszystko dzia³a, usuñ stare pliki z projektu Atmel:
```
? Atmel/Interfaces/ (ca³y folder)
? Atmel/Services/ (ca³y folder z wyj¹tkiem jeœli s¹ inne pliki)
? Atmel/Models/BluetoothLEDeviceInfoModel.cs
? Atmel/Configuration/AppConfiguration.cs
? Atmel/Silnik/ServerRFCOMM.cs
? Atmel/Silnik/ClientRFCOMM.cs
```

---

## ?? To wszystko!

Po wykonaniu tych kroków:
- ? Projekt kompiluje siê bez b³êdów
- ? Aplikacja dzia³a tak samo jak wczeœniej
- ? Serwisy s¹ w osobnym projekcie
- ? £atwiejsze testowanie i rozwój

---

## ?? Troubleshooting

### Problem: "Cannot resolve Assembly or Windows Metadata file"
**Rozwi¹zanie**: 
1. Usuñ foldery bin/obj (Krok 0)
2. Zamknij i otwórz ponownie Visual Studio
3. Zobacz pe³n¹ instrukcjê: `FIXING_PROJECT_LOAD_ISSUE.md`

### Problem: "Compile errors in Atmel.Services"
**Rozwi¹zanie**: 
1. Projekt zosta³ naprawiony - wildcard zamieniony na jawn¹ listê
2. Zobacz: `COMPILATION_FIX.md`

### Problem: "The type or namespace name 'IBluetoothService' could not be found"
**Rozwi¹zanie**: SprawdŸ czy doda³eœ referencjê do Atmel.Services (Krok 2)

### Problem: "Ambiguous reference"
**Rozwi¹zanie**: Usuñ stare `using Atmel.Interfaces;` i dodaj nowe `using Atmel.Services.Interfaces;`

### Problem: Projekt Atmel.Services nie ³aduje siê
**Rozwi¹zanie**: 
1. Zamknij Visual Studio
2. Usuñ folder Atmel.Services/bin i Atmel.Services/obj
3. Otwórz ponownie Visual Studio
4. Solution Explorer ? Prawy przycisk na Atmel.Services ? "Reload Project"

### Problem: "Project is targeting a different platform"
**Rozwi¹zanie**:
1. Solution ? Configuration Manager
2. Dla Atmel.Services ustaw tê sam¹ platformê co Atmel (x86/x64/ARM)

### Problem: ".NETCore,Version=v5.0" w Output
**Rozwi¹zanie**:
To jest plik generowany - po rebuild z poprawion¹ konfiguracj¹ powinien byæ OK.
Zobacz: `COMPILATION_FIX.md`

---

## ?? Potrzebujesz pomocy?

Zobacz dokumentacjê:
- `COMPILATION_FIX.md` - **NOWY** - Rozwi¹zywanie problemów z kompilacj¹
- `FIXING_PROJECT_LOAD_ISSUE.md` - Rozwi¹zywanie problemów z ³adowaniem
- `MIGRATION_TO_SERVICES_PROJECT.md` - Szczegó³owa instrukcja
- `SERVICES_MIGRATION_COMPLETE.md` - Kompletne podsumowanie

---

**Powodzenia!** ??
