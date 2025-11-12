# ? CHECKLIST WERYFIKACJI - Atmel.Services Migration

## ?? Pre-Deployment Checklist

### ?? 1. Pliki projektu Atmel.Services

- [ ] `Atmel.Services.csproj` istnieje
- [ ] ProjectGuid jest prawid³owy (nie placeholder)
- [ ] `Properties/AssemblyInfo.cs` istnieje
- [ ] Wszystkie 14 plików .cs s¹ w projekcie

#### SprawdŸ pliki:
```
Atmel.Services/
??? ? Atmel.Services.csproj
??? ? Properties/AssemblyInfo.cs
??? ? Interfaces/ (4 pliki)
??? ? Implementation/ (3 pliki)
??? ? Models/ (1 plik)
??? ? Configuration/ (1 plik)
??? ? Helpers/ (2 pliki)
??? ? Rfcomm/ (2 pliki)
```

---

### ??? 2. Konfiguracja projektu

- [ ] `OutputType` = `winmdobj`
- [ ] `TargetPlatformIdentifier` = `UAP`
- [ ] `TargetPlatformVersion` = `10.0.26100.0` (lub Twoja wersja)
- [ ] PackageReference do `Microsoft.NETCore.UniversalWindowsPlatform` 6.2.14

#### Weryfikacja w pliku .csproj:
```xml
<PropertyGroup>
  <OutputType>winmdobj</OutputType>
  <TargetPlatformIdentifier>UAP</TargetPlatformIdentifier>
  <ProjectGuid>{B8C6E1A2-4F3D-42B8-9A5E-7C8D9E6F0A1B}</ProjectGuid>
</PropertyGroup>
```

---

### ?? 3. Czyszczenie przed build

- [ ] Usuniêto `Atmel.Services/bin/`
- [ ] Usuniêto `Atmel.Services/obj/`
- [ ] Usuniêto `Atmel/bin/`
- [ ] Usuniêto `Atmel/obj/`

#### Polecenie PowerShell:
```powershell
Remove-Item -Recurse -Force Atmel.Services\bin,Atmel.Services\obj,Atmel\bin,Atmel\obj -ErrorAction SilentlyContinue
```

---

### ?? 4. Visual Studio

- [ ] Solution otwarte w Visual Studio
- [ ] Projekt Atmel.Services za³adowany (nie "(unavailable)")
- [ ] Referencja Atmel ? Atmel.Services dodana
- [ ] Build Configuration zgodny (x86/x64/ARM)

#### SprawdŸ w Solution Explorer:
```
Solution 'AT93C56Tests'
??? Atmel
?   ??? References
?       ??? ? Atmel.Services
??? ? Atmel.Services (za³adowany, nie szary)
??? Atmel.Tests
```

---

### ?? 5. Using statements zaktualizowane

#### Plik: `Atmel/Infrastructure/ServiceContainer.cs`
- [ ] Usuniêto `using Atmel.Interfaces;`
- [ ] Usuniêto `using Atmel.Services;`
- [ ] Usuniêto `using Atmel.Configuration;`
- [ ] Usuniêto `using Atmel.Silnik;`
- [ ] Dodano `using Atmel.Services.Interfaces;`
- [ ] Dodano `using Atmel.Services.Implementation;`
- [ ] Dodano `using Atmel.Services.Configuration;`
- [ ] Dodano `using Atmel.Services.Rfcomm;`

#### Plik: `Atmel/ViewModels/MainPageViewModel.cs`
- [ ] Usuniêto `using Atmel.Interfaces;`
- [ ] Usuniêto `using Atmel.Models;`
- [ ] Usuniêto `using Atmel.Configuration;`
- [ ] Dodano `using Atmel.Services.Interfaces;`
- [ ] Dodano `using Atmel.Services.Models;`
- [ ] Dodano `using Atmel.Services.Configuration;`

#### Plik: `Atmel/MainPage.xaml.cs`
- [ ] Usuniêto `using Atmel.Interfaces;`
- [ ] Usuniêto `using Atmel.Models;`
- [ ] Usuniêto `using Atmel.Configuration;`
- [ ] Dodano `using Atmel.Services.Interfaces;`
- [ ] Dodano `using Atmel.Services.Models;`
- [ ] Dodano `using Atmel.Services.Configuration;`

---

### ?? 6. Build

- [ ] Build ? Rebuild Solution wykonany
- [ ] 0 errors
- [ ] 0 warnings (lub tylko minor warnings)
- [ ] Plik `Atmel.Services.winmd` utworzony

#### SprawdŸ Output:
```
1>------ Rebuild All started: Project: Atmel.Services ------
1>  Atmel.Services -> ...\Atmel.Services\bin\x86\Debug\Atmel.Services.winmd
2>------ Rebuild All started: Project: Atmel ------
2>  Atmel -> ...\Atmel\bin\x86\Debug\Atmel.exe
========== Rebuild All: 2 succeeded, 0 failed, 0 skipped ==========
```

#### SprawdŸ pliki:
- [ ] `Atmel.Services/bin/Debug/Atmel.Services.winmd` istnieje
- [ ] `Atmel.Services/bin/Debug/Atmel.Services.dll` istnieje
- [ ] `Atmel.Services/bin/Debug/Atmel.Services.pri` istnieje

---

### ?? 7. Runtime Test

- [ ] Debug ? Start Debugging (F5) wykonany
- [ ] Aplikacja uruchamia siê bez b³êdów
- [ ] Wszystkie funkcje dzia³aj¹:
  - [ ] Load Devices
  - [ ] Connect to device
  - [ ] LED ON/OFF
  - [ ] Device Discovery
  - [ ] RFCOMM Server/Client

---

### ?? 8. Dokumentacja

- [ ] `QUICK_START_MIGRATION.md` istnieje
- [ ] `MIGRATION_TO_SERVICES_PROJECT.md` istnieje
- [ ] `SERVICES_MIGRATION_COMPLETE.md` istnieje
- [ ] `FIXING_PROJECT_LOAD_ISSUE.md` istnieje
- [ ] `PROJECT_FIX_SUMMARY.md` istnieje
- [ ] `GIT_COMMIT_INSTRUCTIONS.md` zaktualizowany

---

### ?? 9. Code Quality

- [ ] Wszystkie namespace'y zgodne (`Atmel.Services.*`)
- [ ] Brak duplikacji kodu
- [ ] SOLID principles przestrzegane
- [ ] MVVM pattern zachowany
- [ ] Dependency Injection dzia³a

---

### ??? 10. Cleanup (Opcjonalnie - PO weryfikacji)

Po pe³nej weryfikacji dzia³ania, usuñ stare pliki:

- [ ] `Atmel/Interfaces/` (folder)
- [ ] `Atmel/Services/` (folder bez ViewModels)
- [ ] `Atmel/Models/BluetoothLEDeviceInfoModel.cs`
- [ ] `Atmel/Configuration/AppConfiguration.cs`
- [ ] `Atmel/Silnik/ServerRFCOMM.cs`
- [ ] `Atmel/Silnik/ClientRFCOMM.cs`

**UWAGA**: Usuñ dopiero gdy masz 100% pewnoœci ¿e wszystko dzia³a!

---

### ?? 11. Git

- [ ] Wszystkie nowe pliki staged
- [ ] Commit message przygotowany
- [ ] Lokalne zmiany przetestowane
- [ ] Gotowe do push

#### Git commands:
```bash
# Check status
git status

# Stage files
git add Atmel.Services/
git add *.md

# Commit
git commit -m "feat: migrate services to Atmel.Services + fix project config"

# Push
git push origin master
```

---

## ?? Final Verification

### Wszystkie checklisty zakoñczone?
- [ ] 1. Pliki projektu ?
- [ ] 2. Konfiguracja ?
- [ ] 3. Czyszczenie ?
- [ ] 4. Visual Studio ?
- [ ] 5. Using statements ?
- [ ] 6. Build ?
- [ ] 7. Runtime Test ?
- [ ] 8. Dokumentacja ?
- [ ] 9. Code Quality ?
- [ ] 10. Cleanup ? (opcjonalnie)
- [ ] 11. Git ?

---

## ? GOTOWE DO WDRO¯ENIA!

Jeœli wszystkie checklisty s¹ zaznaczone:

**?? PROJEKT JEST GOTOWY!**

Mo¿esz bezpiecznie:
1. Commit changes
2. Push to repository
3. Deploy to production

---

## ?? Szybki Test

Uruchom ten skrypt w PowerShell aby szybko zweryfikowaæ:

```powershell
# Quick verification script
Write-Host "=== Atmel.Services Verification ===" -ForegroundColor Cyan

# Check project file
if (Test-Path "Atmel.Services\Atmel.Services.csproj") {
    Write-Host "? Project file exists" -ForegroundColor Green
} else {
    Write-Host "? Project file missing!" -ForegroundColor Red
}

# Check AssemblyInfo
if (Test-Path "Atmel.Services\Properties\AssemblyInfo.cs") {
    Write-Host "? AssemblyInfo.cs exists" -ForegroundColor Green
} else {
    Write-Host "? AssemblyInfo.cs missing!" -ForegroundColor Red
}

# Count files
$csFiles = (Get-ChildItem -Path "Atmel.Services" -Filter "*.cs" -Recurse -Exclude "obj","bin").Count
Write-Host "?? Found $csFiles .cs files (expected: 15)" -ForegroundColor $(if($csFiles -eq 15){"Green"}else{"Yellow"})

# Check for .winmd
if (Test-Path "Atmel.Services\bin\*\Debug\Atmel.Services.winmd") {
    Write-Host "? .winmd file built successfully" -ForegroundColor Green
} else {
    Write-Host "??  .winmd not found (need to build)" -ForegroundColor Yellow
}

Write-Host "`n=== Verification Complete ===" -ForegroundColor Cyan
```

---

**Status**: Ready for deployment! ??
