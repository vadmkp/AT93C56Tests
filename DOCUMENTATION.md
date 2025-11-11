# AT93C56Tests - Dokumentacja Projektu

## 📋 Przegląd

**AT93C56Tests** to projekt edukacyjny z 2017 roku, stworzony do nauki obsługi GitHub oraz eksperymentów z komunikacją Bluetooth między urządzeniami Arduino/Atmel a aplikacjami Windows UWP (Universal Windows Platform).

**Status projektu:** Archiwum edukacyjne (Learning/Portfolio project)  
**Repozytorium:** https://github.com/vadmkp/AT93C56Tests  
**Autor:** pknut (vadmkp)  
**Licencja:** Zobacz plik LICENSE

---

## 🎯 Cele Projektu

1. **Nauka GitHub** - Eksperymentowanie z Markdown, Git workflow, i zarządzaniem repozytorium
2. **Bluetooth Communication** - Implementacja komunikacji Bluetooth RFCOMM między urządzeniami
3. **Arduino Integration** - Zdalne sterowanie Arduino przez Bluetooth z aplikacji Windows
4. **UWP Development** - Budowanie aplikacji Universal Windows Platform z XAML

---

## 🏗️ Architektura Rozwiązania

Projekt składa się z **dwóch głównych komponentów** zorganizowanych w Visual Studio Solution:

### 📂 Struktura Projektu

```
AT93C56Tests/
├── 📱 ATMEL Solutions/              # Aplikacje Windows UWP
│   ├── Atmel/                       # Główna aplikacja UWP
│   ├── Atmel.AT93C56/              # Biblioteka dla pamięci AT93C56 EEPROM
│   ├── Atmel.Microchip24LC512/     # Biblioteka dla pamięci 24LC512 I2C EEPROM
│   └── Atmel.Microchip23LC1024/    # Biblioteka dla pamięci 23LC1024 SPI RAM
│
├── 🤖 Arduino Solutions/            # Kod dla Arduino
│   └── Arduino01/                   # Projekt Arduino (pusty szablon)
│
└── 📄 Solution Items/
    └── README.md                    # Markdown syntax examples
```

---

## 🔧 Komponenty Techniczne

### 1. Aplikacja UWP (Atmel)

**Technologie:**
- C# + XAML (Universal Windows Platform)
- .NET Framework / UWP APIs
- Visual Studio 2017

**Główne funkcjonalności:**

#### 📡 Bluetooth Low Energy (BLE)
- **Skanowanie urządzeń BLE** (`btnBlueLE01_Click`, `btnBlueLE02_Click`)
- **Device Watcher** - Automatyczne wykrywanie urządzeń
- **GATT Services** - Pobieranie usług Bluetooth Generic Attribute Profile
- **Pairing** - Parowanie z urządzeniami BLE

#### 📻 Bluetooth RFCOMM
- **Client RFCOMM** (`ClientRFCOMM.cs`) - Połączenie jako klient
- **Server RFCOMM** (`ServerRFCOMM.cs`) - Nasłuchiwanie połączeń
- **Service Discovery Protocol (SDP)** - Wyszukiwanie usług Bluetooth
- **Socket Protection Levels** - Szyfrowanie BluetoothEncryptionAllowNullAuthentication

#### 🔌 Arduino Remote Wiring
- **Microsoft.Maker.RemoteWiring** - Biblioteka do zdalnego sterowania Arduino
- **BluetoothSerial** - Komunikacja przez Bluetooth Serial (HC-05, "sowaphone")
- **RemoteDevice** - Obiekt reprezentujący zdalne Arduino
- **digitalWrite** - Zdalne sterowanie pinami GPIO (pin 13 - LED)

**Interfejs użytkownika (MainPage.xaml):**
- Przycisk "List" - Lista dostępnych urządzeń Bluetooth
- Przycisk "Start" - Nawiązanie połączenia z Arduino
- Przycisk "ON" - Włączenie LED (pin 13)
- Przycisk "OFF" - Wyłączenie LED (pin 13)
- Przyciski testowe BLE (btnBlueLE01-04)

#### 🔧 Kluczowe klasy:

**MainPage.xaml.cs**
```csharp
// Połączenie z Arduino przez Bluetooth
_bluetooth = new BluetoothSerial("sowaphone");
_arduino = new RemoteDevice(_bluetooth);

// Sterowanie LED
_arduino.digitalWrite(13, PinState.HIGH);  // ON
_arduino.digitalWrite(13, PinState.LOW);   // OFF
```

**ClientRFCOMM.cs**
- `Initialize()` - Wyszukiwanie i połączenie z RFCOMM server
- `SupportsProtection()` - Weryfikacja poziomu bezpieczeństwa
- `IsCompatibleVersion()` - Sprawdzanie wersji usługi (min. 2.0)

**ServerRFCOMM.cs**
- `Initialize()` - Start serwera RFCOMM
- `InitializeServiceSdpAttributes()` - Konfiguracja SDP attributes
- `OnConnectionReceived()` - Handler dla nowych połączeń

---

### 2. Projekt Arduino (Arduino01)

**Plik:** `Arduino01.ino`

**Status:** Pusty szkielet projektu

```cpp
void setup() {
    // Inicjalizacja (puste)
}

void loop() {
    // Główna pętla (puste)
}
```

**Przeznaczenie:**
- Docelowo miał zawierać kod obsługi Firmata lub custom Bluetooth protocol
- Współpraca z aplikacją UWP przez Bluetooth Serial
- Sterowanie GPIO Arduino zdalnie

---

### 3. Biblioteki dla pamięci EEPROM/RAM

Trzy puste projekty bibliotek dla różnych chipów pamięci:

#### Atmel.AT93C56
- **Chip:** AT93C56 - 2Kb (256x8) Microwire Serial EEPROM
- **Interfejs:** Microwire (SPI-like)
- **Status:** Tylko szkielet projektu (`Class1.cs` pusta)

#### Atmel.Microchip24LC512
- **Chip:** 24LC512 - 512Kb (64KB) I2C Serial EEPROM
- **Interfejs:** I2C
- **Status:** Tylko szkielet projektu

#### Atmel.Microchip23LC1024
- **Chip:** 23LC1024 - 1Mb (128KB) SPI Serial SRAM
- **Interfejs:** SPI
- **Status:** Tylko szkielet projektu

---

## 📦 Zależności i NuGet Packages

**Projekt Atmel (UWP):**
- `Microsoft.Maker.RemoteWiring` - Arduino remote control
- `Microsoft.Maker.Serial` - Serial communication abstractions
- Windows.Devices.Bluetooth APIs (built-in UWP)
- Windows.Devices.Enumeration APIs (built-in UWP)
- Windows.Networking.Sockets APIs (built-in UWP)

**Wymagania systemowe:**
- Windows 10 (UWP target)
- Visual Studio 2017+
- Arduino compatible board z Bluetooth (HC-05/HC-06 module)
- .NET Framework / UWP SDK

---

## 🚀 Jak Uruchomić

### Krok 1: Środowisko

1. **Zainstaluj Visual Studio 2017+** z workloadem:
   - Universal Windows Platform development
   - .NET desktop development
   - C++ desktop development (dla Arduino)

2. **Arduino Setup:**
   - Załaduj firmata lub custom sketch obsługujący RemoteWiring
   - Sparuj moduł Bluetooth HC-05 z komputerem

### Krok 2: Budowanie projektu

```powershell
# Sklonuj repozytorium
git clone https://github.com/vadmkp/AT93C56Tests.git
cd AT93C56Tests

# Otwórz solution
start AT93C56.sln

# W Visual Studio:
# - Ustaw projekt "Atmel" jako StartUp Project
# - Wybierz platformę (x86/x64/ARM)
# - Build Solution (Ctrl+Shift+B)
```

### Krok 3: Uruchomienie

1. **Deploy aplikacji UWP:**
   - Debug → Local Machine (lub Remote Device)
   
2. **Konfiguracja Bluetooth:**
   - W kodzie MainPage.xaml.cs zmień nazwę urządzenia:
     ```csharp
     _bluetooth = new BluetoothSerial("TWOJA_NAZWA_BT");
     ```

3. **Test połączenia:**
   - Kliknij "Start" → nawiązuje połączenie
   - Kliknij "ON" → LED na Arduino zapala się
   - Kliknij "OFF" → LED gaśnie

---

## 🔍 Funkcje i Przypadki Użycia

### Use Case 1: Zdalne Sterowanie LED Arduino

**Cel:** Włączanie/wyłączanie LED na Arduino z aplikacji Windows

**Przepływ:**
1. User klika "Start" w aplikacji UWP
2. Aplikacja łączy się z modułem Bluetooth HC-05
3. User klika "ON"
4. `_arduino.digitalWrite(13, PinState.HIGH)` wysyła komendę
5. LED na pinie 13 Arduino zapala się

### Use Case 2: Skanowanie Urządzeń BLE

**Cel:** Wykrycie wszystkich urządzeń Bluetooth w zasięgu

**Przepływ:**
1. User klika "btnBlueLE01" lub "btnBlueLE02"
2. DeviceWatcher rozpoczyna skanowanie
3. Zdarzenia `DeviceWatcher_Added` logują znalezione urządzenia
4. Aplikacja wyświetla listę w Output (Debug)

### Use Case 3: RFCOMM Server/Client

**Cel:** Komunikacja peer-to-peer między dwoma urządzeniami Windows

**Przepływ:**
1. Urządzenie A: `StartServer()` → nasłuchuje na RFCOMM
2. Urządzenie B: `StartClient()` → łączy się z A
3. Po połączeniu: wymiana danych przez StreamSocket

---

## 🧪 Stan Implementacji

### ✅ Zaimplementowane:

- [x] Struktura projektu UWP
- [x] Bluetooth serial connection (HC-05)
- [x] Arduino RemoteWiring integration
- [x] Sterowanie GPIO (digitalWrite)
- [x] BLE device scanning
- [x] RFCOMM client/server podstawy
- [x] DeviceWatcher dla BLE
- [x] Connection event handlers

### ⚠️ Częściowo zaimplementowane:

- [~] RFCOMM data transfer (brak handlera danych)
- [~] BLE GATT characteristics (tylko discovery)
- [~] UI dla listy urządzeń (tylko debug output)
- [~] Error handling (podstawowy)

### ❌ Nie zaimplementowane:

- [ ] Biblioteki dla AT93C56/24LC512/23LC1024
- [ ] Arduino sketch (pusty plik)
- [ ] Obsługa wielu urządzeń jednocześnie
- [ ] Persist connection settings
- [ ] Advanced BLE features (notifications, indications)
- [ ] UART/Serial communication (poza Bluetooth)
- [ ] Testy jednostkowe
- [ ] Dokumentacja API

---

## 🎓 Wartość Edukacyjna

### Czego Można się Nauczyć:

1. **Bluetooth Programming:**
   - BLE vs Classic Bluetooth
   - RFCOMM socket programming
   - SDP attributes
   - Pairing i security levels

2. **UWP Development:**
   - XAML UI design
   - Async/await patterns
   - Device enumeration APIs
   - Event-driven architecture

3. **IoT & Embedded:**
   - Arduino remote control
   - GPIO manipulation
   - Serial communication protocols
   - Hardware abstraction layers

4. **Git & GitHub:**
   - Markdown formatting (README.md pełen przykładów)
   - Repository management
   - Code organization

---

## 📚 Dokumentacja Dodatkowa

### README.md
Główny README to **przewodnik po składni Markdown** zawierający przykłady:
- Headers (H1-H6)
- Bold, Italic, Strikethrough
- Code blocks (JS, C, Python, C#)
- Links (inline, reference-style)
- Images (inline, reference, z linkami)
- Blockquotes
- Lists (unordered, ordered, nested)
- Tables
- Inline HTML
- YouTube embeds

### Atmel/README.md
Krótki opis: "Projekt w trakcie rozwoju, nauka GitHub"

---

## 🐛 Znane Problemy

1. **Hardcoded device names:**
   - "HC-05", "sowaphone" w kodzie
   - Rozwiązanie: UI do wyboru urządzenia z listy

2. **Brak obsługi błędów połączenia:**
   - ConnectionFailed/ConnectionLost tylko logują do Debug
   - Rozwiązanie: User-facing error messages

3. **Puste biblioteki EEPROM:**
   - Projekty Atmel.AT93C56 etc. nie mają implementacji
   - Rozwiązanie: Implementacja I2C/SPI protokołów

4. **Arduino sketch pusty:**
   - Arduino01.ino nie ma kodu
   - Rozwiązanie: StandardFirmata lub custom protocol

5. **UWP APIs przestarzałe:**
   - Projekt z 2017, niektóre APIs deprecated
   - Rozwiązanie: Migracja do WinUI 3 / .NET 8+

---

## 🔮 Potencjalne Rozszerzenia

### Krótkoterminowe:
- [ ] Implementacja listy urządzeń w UI (ListView)
- [ ] Zapisywanie ostatnio używanego urządzenia
- [ ] Toast notifications dla event handlers
- [ ] Arduino Firmata sketch
- [ ] Obsługa wielu pinów GPIO (sliders/switches)

### Średnioterminowe:
- [ ] BLE GATT notifications (sensor data streaming)
- [ ] Implementacja jednej z bibliotek EEPROM (np. 24LC512)
- [ ] Charts/graphs dla danych sensorów
- [ ] File transfer przez RFCOMM
- [ ] PWM control (servos, motors)

### Długoterminowe:
- [ ] Migracja do WinUI 3 / .NET MAUI
- [ ] Cross-platform support (Android/iOS)
- [ ] Cloud integration (Azure IoT Hub)
- [ ] Machine learning na danych sensorów
- [ ] Multi-device dashboard

---

## 🤝 Contributing

Projekt jest archiwalny (learning project z 2017), ale możesz:
- Fork i eksperymentować
- Otworzyć Issues z pytaniami
- Zaproponować Pull Requesty z poprawkami/ulepszeniami

---

## 📄 Licencja

Zobacz plik `LICENSE` w repozytorium.

---

## 📧 Kontakt

**Autor:** vadmkp  
**GitHub:** https://github.com/vadmkp  
**Repozytorium:** https://github.com/vadmkp/AT93C56Tests

---

## 🏷️ Tags

`Bluetooth` `BLE` `Arduino` `UWP` `IoT` `RFCOMM` `RemoteWiring` `C#` `XAML` `Windows10` `EEPROM` `Learning` `Portfolio`

---

## 📊 Statystyki Projektu

- **Język główny:** C# (UWP), C++ (Arduino)
- **Linie kodu:** ~500-1000 (szacunkowo, bez pustych projektów)
- **Pliki:** ~50
- **Commits:** Zobacz historię Git
- **Rok utworzenia:** 2017
- **Status:** Archived / Educational

---

*Dokumentacja wygenerowana: 2025-11-11*
