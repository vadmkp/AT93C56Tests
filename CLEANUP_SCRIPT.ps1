# ?? Skrypt czyszcz¹cy workspace

# UWAGA: Uruchom z g³ównego folderu AT93C56Tests

Write-Host "=== AT93C56Tests Cleanup Script ===" -ForegroundColor Cyan
Write-Host ""

# Funkcja do bezpiecznego usuwania
function Remove-SafelyWithPrompt {
    param(
        [string]$Path,
        [string]$Description
    )
    
    if (Test-Path $Path) {
        $size = (Get-ChildItem -Path $Path -Recurse -ErrorAction SilentlyContinue | Measure-Object -Property Length -Sum).Sum / 1MB
        Write-Host "Found: $Description" -ForegroundColor Yellow
        Write-Host "  Path: $Path" -ForegroundColor Gray
        Write-Host "  Size: $([math]::Round($size, 2)) MB" -ForegroundColor Gray
        return $true
    }
    return $false
}

# ============================================
# KROK 1: Build Artifacts (bin/obj/.vs)
# ============================================
Write-Host "`n[KROK 1] Czyszczenie build artifacts..." -ForegroundColor Green

$buildFolders = @(
    @{Path=".\.vs"; Desc="Visual Studio cache (root)"},
    @{Path=".\Atmel\.vs"; Desc="Visual Studio cache (Atmel)"},
    @{Path=".\Atmel.Services\.vs"; Desc="Visual Studio cache (Atmel.Services)"},
    @{Path=".\Atmel\bin"; Desc="Build output (Atmel)"},
    @{Path=".\Atmel\obj"; Desc="Intermediate files (Atmel)"},
    @{Path=".\Atmel.Services\bin"; Desc="Build output (Atmel.Services)"},
    @{Path=".\Atmel.Services\obj"; Desc="Intermediate files (Atmel.Services)"},
    @{Path=".\Atmel.Tests\bin"; Desc="Build output (Atmel.Tests)"},
    @{Path=".\Atmel.Tests\obj"; Desc="Intermediate files (Atmel.Tests)"}
)

$totalSize = 0
$foundCount = 0

foreach ($folder in $buildFolders) {
    if (Remove-SafelyWithPrompt -Path $folder.Path -Description $folder.Desc) {
        $foundCount++
    }
}

Write-Host "`nZnaleziono $foundCount folderów build artifacts" -ForegroundColor Cyan

$response = Read-Host "`nCzy usun¹æ wszystkie foldery build? (y/n)"
if ($response -eq 'y') {
    foreach ($folder in $buildFolders) {
        if (Test-Path $folder.Path) {
            Remove-Item -Recurse -Force $folder.Path -ErrorAction SilentlyContinue
            Write-Host "  ? Usuniêto: $($folder.Path)" -ForegroundColor Green
        }
    }
    Write-Host "Build artifacts wyczyszczone!" -ForegroundColor Green
} else {
    Write-Host "Pominiêto czyszczenie build artifacts" -ForegroundColor Yellow
}

# ============================================
# KROK 2: Pusty folder Atmel.Core
# ============================================
Write-Host "`n[KROK 2] Sprawdzanie pustych folderów..." -ForegroundColor Green

if (Test-Path ".\Atmel.Core") {
    $files = Get-ChildItem -Path ".\Atmel.Core" -Recurse
    Write-Host "Found: Folder Atmel.Core" -ForegroundColor Yellow
    Write-Host "  Plików: $($files.Count)" -ForegroundColor Gray
    Write-Host "  Zawartoœæ: $($files.Name -join ', ')" -ForegroundColor Gray
    
    $response = Read-Host "`nCzy usun¹æ folder Atmel.Core? (y/n)"
    if ($response -eq 'y') {
        Remove-Item -Recurse -Force ".\Atmel.Core"
        Write-Host "  ? Usuniêto: Atmel.Core" -ForegroundColor Green
    } else {
        Write-Host "Pominiêto usuwanie Atmel.Core" -ForegroundColor Yellow
    }
} else {
    Write-Host "Folder Atmel.Core nie istnieje" -ForegroundColor Gray
}

# ============================================
# KROK 3: Duplikaty dokumentacji
# ============================================
Write-Host "`n[KROK 3] Analiza duplikatów dokumentacji..." -ForegroundColor Green

$migrationDocs = @(
    ".\MIGRATION_TO_SERVICES_PROJECT.md",
    ".\MIGRATION_FINAL_SUMMARY.md",
    ".\PROBLEM_SOLVED.md"
)

$compilationDocs = @(
    ".\COMPILATION_FIX.md",
    ".\COMPILATION_FINAL_FIX.md",
    ".\PROJECT_FIX_SUMMARY.md"
)

Write-Host "`nDuplikaty migracji (zachowaj: SERVICES_MIGRATION_COMPLETE.md, QUICK_START_MIGRATION.md):" -ForegroundColor Yellow
foreach ($doc in $migrationDocs) {
    if (Test-Path $doc) {
        Write-Host "  - $(Split-Path $doc -Leaf)" -ForegroundColor Gray
    }
}

Write-Host "`nDuplikaty kompilacji (mo¿na scaliæ w 1 plik):" -ForegroundColor Yellow
foreach ($doc in $compilationDocs) {
    if (Test-Path $doc) {
        Write-Host "  - $(Split-Path $doc -Leaf)" -ForegroundColor Gray
    }
}

$response = Read-Host "`nCzy usun¹æ duplikaty dokumentacji? (y/n)"
if ($response -eq 'y') {
    # Usuñ duplikaty migracji
    foreach ($doc in $migrationDocs) {
        if (Test-Path $doc) {
            Remove-Item $doc
            Write-Host "  ? Usuniêto: $(Split-Path $doc -Leaf)" -ForegroundColor Green
        }
    }
    
    # Usuñ duplikaty kompilacji
    foreach ($doc in $compilationDocs) {
        if (Test-Path $doc) {
            Remove-Item $doc
            Write-Host "  ? Usuniêto: $(Split-Path $doc -Leaf)" -ForegroundColor Green
        }
    }
    
    Write-Host "Duplikaty dokumentacji usuniête!" -ForegroundColor Green
} else {
    Write-Host "Pominiêto usuwanie duplikatów" -ForegroundColor Yellow
}

# ============================================
# KROK 4: Podsumowanie
# ============================================
Write-Host "`n=== Cleanup zakoñczony ===" -ForegroundColor Cyan

Write-Host "`nCo zosta³o wyczyszczone:" -ForegroundColor Green
Write-Host "  ? Build artifacts (bin/obj/.vs)" -ForegroundColor Gray
Write-Host "  ? Pusty folder Atmel.Core" -ForegroundColor Gray
Write-Host "  ? Duplikaty dokumentacji" -ForegroundColor Gray

Write-Host "`nNastêpne kroki:" -ForegroundColor Yellow
Write-Host "  1. Utwórz .gitignore (uruchom: CREATE_GITIGNORE.ps1)" -ForegroundColor Gray
Write-Host "  2. Przejrzyj pozosta³¹ dokumentacjê" -ForegroundColor Gray
Write-Host "  3. Commit zmian do Git" -ForegroundColor Gray

Write-Host "`n? Gotowe!" -ForegroundColor Green
