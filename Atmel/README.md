# Atmel UWP Application

> **Windows Universal Platform application** do zdalnego sterowania urządzeniami Arduino przez Bluetooth.

![Platform](https://img.shields.io/badge/platform-UWP-blue.svg)
![Language](https://img.shields.io/badge/language-C%23-brightgreen.svg)
![Framework](https://img.shields.io/badge/framework-XAML-orange.svg)
![.NET](https://img.shields.io/badge/.NET-UWP%206.2.14-512BD4.svg)

---

## 📱 O Aplikacji

**Atmel** to aplikacja Windows UWP umożliwiająca zdalne sterowanie Arduino przez połączenie Bluetooth. Aplikacja wykorzystuje bibliotekę **Windows-Remote-Arduino** (Microsoft Maker RemoteWiring) do komunikacji z urządzeniem poprzez moduły Bluetooth Serial (HC-05/HC-06).

Projekt wykorzystuje architekturę **MVVM** (Model-View-ViewModel) z separacją logiki biznesowej do osobnego projektu `Atmel.Services`.

### 🎯 Główne Funkcjonalności

- 🔵 **Bluetooth Classic** - Połączenie z Arduino przez HC-05/HC-06
- 💡 **LED Control** - Zdalne włączanie/wyłączanie LED na pinie 13
- 📡 **BLE Scanning** - Wyszukiwanie urządzeń Bluetooth Low Energy
- 🔌 **RFCOMM Server/Client** - Komunikacja peer-to-peer między urządzeniami Windows
- 📋 **Device Discovery** - Lista dostępnych urządzeń Bluetooth
- 🎨 **MVVM Architecture** - Separacja UI i logiki biznesowej

---

## 🏗️ Architektura Aplikacji

```
┌─────────────────────────────────────────────────────┐
│              Atmel (UWP App)                        │
│  ┌──────────────────────────────────────────────┐   │
│  │ MainPage.xaml (View)                         │   │
│  │  • XAML UI Bindings                          │   │
│  │  • Value Converters                          │   │
│  └─────────────┬────────────────────────────────┘   │
│                │ Data Binding                       │
│  ┌─────────────▼────────────────────────────────┐   │
│  │ MainPageViewModel (ViewModel)                │   │
│  │  • INotifyPropertyChanged                    │   │
│  │  • RelayCommand                              │   │
│  │  • Service orchestration                     │   │
│  └─────────────┬────────────────────────────────┘   │
│                │                                    │
│  ┌─────────────▼────────────────────────────────┐   │
│  │ Infrastructure                               │   │
│  │  • ServiceContainer (DI)                     │   │
│  └──────────────────────────────────────────────┘   │
└────────────────┬────────────────────────────────────┘
                 │
┌────────────────▼────────────────────────────────────┐
│          Atmel.Services (Class Library)             │
│  ┌──────────────────────────────────────────────┐   │
│  │ Interfaces                                   │   │
│  │  • IBluetoothService                         │   │
│  │  • IArduinoController                        │   │
│  │  • IDeviceDiscoveryService                   │   │
│  │  • IRfcommService                            │   │
│  └──────────────────────────────────────────────┘   │
│  ┌──────────────────────────────────────────────┐   │
│  │ Implementation                               │   │
│  │  • BluetoothDiscoveryService                 │   │
│  │  • BluetoothLEDiscoveryService               │   │
│  │  • ArduinoController                         │   │
│  └──────────────────────────────────────────────┘   │
│  ┌──────────────────────────────────────────────┐   │
│  │ RFCOMM                                       │   │
│  │  • ServerRFCOMM                              │   │
│  │  • ClientRFCOMM                              │   │
│  └──────────────────────────────────────────────┘   │
│  ┌──────────────────────────────────────────────┐   │
│  │ Helpers                                      │   │
│  │  • SdpAttributeConfigurator                  │   │
│  │  • RfcommServiceValidator                    │   │
│  └──────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────┘
```

---

## 🔧 Komponenty Techniczne

### 📂 Struktura Projektu

#### Atmel (Główna aplikacja UWP)
```
Atmel/
├── 📄 App.xaml                   # Definicja aplikacji
├── 📄 App.xaml.cs                # Entry point aplikacji
├── 📄 MainPage.xaml              # UI - interfejs użytkownika
├── 📄 MainPage.xaml.cs           # Code-behind (legacy code)
│
├── 📁 ViewModels/                # MVVM ViewModels
│   ├── ViewModelBase.cs          # Bazowa klasa z INotifyPropertyChanged
│   ├── MainPageViewModel.cs      # ViewModel dla MainPage
│   └── Commands/
│       └── RelayCommand.cs       # ICommand implementation
│
├── 📁 Infrastructure/            # Infrastruktura aplikacji
│   └── ServiceContainer.cs       # Dependency Injection container
│
├── 📁 Converters/                # XAML Value Converters
│   └── ValueConverters.cs        # Konwertery dla data binding
│
├── 📁 Serial/                    # Serial communication (legacy)
│   ├── Constants.cs              # Stałe dla komunikacji serial
│   └── DeviceListEntry.cs        # Entry dla listy urządzeń
│
├── 📁 Assets/                    # Zasoby graficzne
│   ├── Utilities.cs              # Funkcje pomocnicze
│   ├── *.png                     # Ikony aplikacji
│   └── ...
│
└── 📁 Properties/
    ├── AssemblyInfo.cs           # Informacje o assembly
    └── Default.rd.xml            # Runtime directives
```

#### Atmel.Services (Biblioteka biznesowa)
```
Atmel.Services/
├── 📁 Interfaces/                # Kontrakty serwisów
│   ├── IBluetoothService.cs
│   ├── IArduinoController.cs
│   ├── IDeviceDiscoveryService.cs
│   └── IRfcommService.cs
│
├── 📁 Implementation/            # Implementacje serwisów
│   ├── BluetoothDiscoveryService.cs
│   ├── BluetoothLEDiscoveryService.cs
│   └── ArduinoController.cs
│
├── 📁 Rfcomm/                    # RFCOMM Communication
│   ├── ServerRFCOMM.cs           # RFCOMM Server
│   └── ClientRFCOMM.cs           # RFCOMM Client
│
├── 📁 Helpers/                   # Klasy pomocnicze
│   ├── SdpAttributeConfigurator.cs
│   └── RfcommServiceValidator.cs
│
├── 📁 Models/                    # Modele danych
│   └── BluetoothLEDeviceInfoModel.cs
│
├── 📁 Configuration/             # Konfiguracja aplikacji
│   └── AppConfiguration.cs
│
└── 📁 Properties/
    └── AssemblyInfo.cs
```

---

## 🎮 Interfejs Użytkownika

### Główny Ekran (MainPage.xaml)

```
┌────────────────────────────────────────────────┐
│  Aplikacja używa bluetooth i coś robi.        │
├────────────────────────────────────────────────┤
│                                                │
│  [List]  [Start]  [Led On]  [Led Off]        │
│  [Serial 01]  [BT LE 01]  [BT LE 02]         │
│                                                │
│  [BT LE 03 - Server]  [BT LE 03 - Client]    │
│                                                │
└────────────────────────────────────────────────┘
```

### Przyciski i ich Funkcje

| Przycisk | Funkcja | Status |
|----------|---------|--------|
| **List** | Lista dostępnych urządzeń Bluetooth | ✅ Działający |
| **Start** | Nawiązanie połączenia z Arduino (HC-05/"sowaphone") | ✅ Działający |
| **Led On** | Włączenie LED na pinie 13 Arduino | ✅ Włącza się po Start |
| **Led Off** | Wyłączenie LED na pinie 13 Arduino | ✅ Włącza się po Led On |
| **Serial 01** | Test komunikacji Serial (USB) | ⚠️ Częściowy |
| **BT LE 01** | Skanowanie BLE z DeviceWatcher | ✅ Działający |
| **BT LE 02** | Skanowanie BLE alternatywne | ✅ Działający |
| **BT LE 03 - Server** | Uruchomienie RFCOMM Server | ✅ Działający |
| **BT LE 03 - Client** | Uruchomienie RFCOMM Client | ✅ Działający |

---

## 📦 Zależności (NuGet Packages)

### Atmel (UWP App)

#### Microsoft.NETCore.UniversalWindowsPlatform `6.2.14`
- Podstawowe frameworki UWP
- APIs Windows Runtime
- ⬆️ **Zaktualizowane** z wersji 5.1.0

#### Windows-Remote-Arduino `1.4.0`
- **Microsoft.Maker.RemoteWiring** - Arduino remote control
- **Microsoft.Maker.Serial** - Serial communication abstractions
- Biblioteka do zdalnego sterowania Arduino przez różne protokoły

### Atmel.Services (Class Library)

#### Microsoft.NETCore.UniversalWindowsPlatform `6.2.14`
- Podstawowe frameworki UWP dla biblioteki

### Built-in UWP APIs
- **Windows.Devices.Bluetooth** - Bluetooth APIs (Classic + BLE)
- **Windows.Devices.Enumeration** - Device discovery
- **Windows.Networking.Sockets** - RFCOMM socket communication
- **Windows.UI.Xaml** - UI framework

---

## 🎨 Wzorce Projektowe

### MVVM (Model-View-ViewModel)

Aplikacja wykorzystuje wzorzec MVVM do separacji UI i logiki:

#### ViewModelBase
```csharp
public class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

#### RelayCommand
```csharp
public class RelayCommand : ICommand
{
    private readonly Action<object> _execute;
    private readonly Func<object, bool> _canExecute;
    
    public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }
    
    public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;
    public void Execute(object parameter) => _execute(parameter);
}
```

### Dependency Injection

Aplikacja używa `ServiceContainer` do zarządzania zależnościami:

```csharp
public class ServiceContainer
{
    private static ServiceContainer _instance;
    public static ServiceContainer Instance => _instance ??= new ServiceContainer();
    
    private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
    
    public void Register<TInterface, TImplementation>() where TImplementation : TInterface, new()
    {
        _services[typeof(TInterface)] = new TImplementation();
    }
    
    public TInterface Resolve<TInterface>()
    {
        return (TInterface)_services[typeof(TInterface)];
    }
}
```

---

## 🚀 Jak Używać

### Krok 1: Konfiguracja Arduino

1. **Podłącz moduł Bluetooth HC-05/HC-06:**
   ```
   HC-05  →  Arduino
   TX     →  RX (pin 0)
   RX     →  TX (pin 1)
   VCC    →  5V
   GND    →  GND
   ```

2. **Załaduj StandardFirmata:**
   - File → Examples → Firmata → StandardFirmata
   - Upload do Arduino

3. **Sparuj Bluetooth:**
   - Windows Settings → Devices → Bluetooth
   - Dodaj urządzenie "HC-05" (domyślne PIN: 1234 lub 0000)

### Krok 2: Uruchomienie Aplikacji

1. **Zmień nazwę urządzenia** (jeśli potrzeba):
   
   W `MainPage.xaml.cs` linia ~67:
   ```csharp
   _bluetooth = new BluetoothSerial("sowaphone"); // lub "HC-05"
   ```

2. **Build & Deploy:**
   - Visual Studio → Set as StartUp Project
   - Platform: x86/x64/ARM
   - F5 (Debug) lub Ctrl+F5 (Run)

### Krok 3: Test Połączenia

```
1. Kliknij [List]
   → W Output window pojawi się lista urządzeń Bluetooth
   
2. Kliknij [Start]
   → Aplikacja łączy się z Arduino
   → Przyciski [Led On] i [Led Off] stają się aktywne
   
3. Kliknij [Led On]
   → LED na pinie 13 Arduino zapala się 💡
   
4. Kliknij [Led Off]
   → LED gaśnie
```

---

## 🔌 Kluczowe Klasy i Metody

### MainPage.xaml.cs - Główna Logika (Legacy)

#### Bluetooth Serial Connection
```csharp
// Inicjalizacja połączenia
_bluetooth = new BluetoothSerial("sowaphone");
_arduino = new RemoteDevice(_bluetooth);

// Event handlers
_bluetooth.ConnectionEstablished += OnConnectionEstablished;
_bluetooth.ConnectionLost += _bluetooth_ConnectionLost;
_bluetooth.ConnectionFailed += _bluetooth_ConnectionFailed;

// Start połączenia
_bluetooth.begin(0, SerialConfig.SERIAL_8N1);
```

#### LED Control
```csharp
// Włączenie LED (pin 13)
_arduino.digitalWrite(13, PinState.HIGH);

// Wyłączenie LED
_arduino.digitalWrite(13, PinState.LOW);
```

#### BLE Device Scanning
```csharp
// Skanowanie urządzeń BLE
DeviceWatcher deviceWatcher = DeviceInformation.CreateWatcher(
    aqsAllBluetoothLEDevices,
    requestedProperties,
    DeviceInformationKind.AssociationEndpoint
);

// Event handlers
deviceWatcher.Added += DeviceWatcher_Added;
deviceWatcher.Updated += DeviceWatcher_Updated;
deviceWatcher.Removed += DeviceWatcher_Removed;

// Start skanowania
deviceWatcher.Start();
```

---

### Atmel.Services - Serwisy Biznesowe

#### IArduinoController Interface
```csharp
public interface IArduinoController
{
    Task<bool> ConnectAsync(string deviceName);
    Task DisconnectAsync();
    Task SetPinStateAsync(byte pin, bool state);
    Task<bool> GetPinStateAsync(byte pin);
}
```

#### IBluetoothService Interface
```csharp
public interface IBluetoothService
{
    Task<IEnumerable<BluetoothDevice>> DiscoverDevicesAsync();
    Task<bool> PairDeviceAsync(string deviceId);
}
```

#### ServerRFCOMM.cs - RFCOMM Server

**Funkcja:** Nasłuchiwanie połączeń RFCOMM

**Kluczowe metody:**
- `Initialize()` - Start serwera RFCOMM
- `InitializeServiceSdpAttributes()` - Konfiguracja SDP attributes
- `OnConnectionReceived()` - Handler dla nowych połączeń

**Użycie:** Komunikacja peer-to-peer między urządzeniami Windows

---

#### ClientRFCOMM.cs - RFCOMM Client

**Funkcja:** Połączenie z RFCOMM Server jako klient

**Kluczowe metody:**
- `Initialize()` - Wyszukiwanie i połączenie z usługą RFCOMM
- `SupportsProtection()` - Weryfikacja poziomu szyfrowania
- `IsCompatibleVersion()` - Sprawdzanie wersji usługi (min. 2.0)

**Protokół:** ObexObjectPush (RFCOMM standard)

---

#### BluetoothLEDeviceInfoModel.cs

**Model danych dla urządzeń BLE:**
- Device ID
- Device Name
- Connection Status
- Signal Strength (RSSI)
- Pairing Status

---

## 🎯 Przypadki Użycia

### Use Case 1: Smart Home Control
Zdalne sterowanie oświetleniem, żaluzjami, lub innymi urządzeniami domowymi podłączonymi do Arduino przez Bluetooth.

### Use Case 2: IoT Monitoring
Odczyt danych z sensorów (temperatura, wilgotność, ruch) podłączonych do Arduino i wyświetlanie w aplikacji Windows.

### Use Case 3: Robotics Control
Pilot do sterowania robotem - kontrola motorów DC, serwomechanizmów, czujników dystansu.

### Use Case 4: Educational Platform
Nauka programowania Arduino i aplikacji UWP w jednym projekcie - idealne dla studentów i hobbystów.

---

## 🐛 Znane Ograniczenia

1. **Hardcoded Device Names**
   - Nazwa urządzenia Bluetooth ("sowaphone", "HC-05") jest na sztywno w kodzie
   - **Fix:** UI do wyboru z listy dostępnych urządzeń

2. **Brak UI dla Output**
   - Lista urządzeń wyświetla się tylko w Debug Output
   - **Fix:** ListView w UI z dynamiczną listą

3. **Single Connection**
   - Aplikacja obsługuje jedno urządzenie Arduino na raz
   - **Fix:** Multi-device support z tab view

4. **Brak Error Handling UI**
   - Błędy połączenia wyświetlają się tylko w Debug
   - **Fix:** Toast notifications i error dialogs

5. **UI/UX Przestarzałe**
   - Interfejs jest bardzo prosty
   - **Fix:** Modern Fluent Design, animations

6. **Mixed Code Patterns**
   - Część kodu używa MVVM, część legacy code-behind
   - **Fix:** Pełna migracja do MVVM

---

## 🔮 Plany Rozwoju

### v1.1 - Complete MVVM Migration
- [ ] Przeniesienie całej logiki z MainPage.xaml.cs do ViewModels
- [ ] Pełne wykorzystanie Data Binding w XAML
- [ ] Command pattern dla wszystkich akcji użytkownika
- [ ] Dependency Injection dla wszystkich serwisów

### v1.2 - UI Improvements
- [ ] ListView dla dostępnych urządzeń Bluetooth
- [ ] Toast notifications dla zdarzeń połączenia
- [ ] Error dialogs dla użytkownika
- [ ] Zapisywanie ostatnio używanego urządzenia
- [ ] Fluent Design (Acrylic, Reveal)

### v1.3 - Extended GPIO Control
- [ ] Kontrola wielu pinów GPIO (nie tylko pin 13)
- [ ] Sliders dla PWM control
- [ ] Toggle switches dla digital pins
- [ ] Analog input reading (A0-A5)

### v2.0 - Sensor Dashboard
- [ ] Real-time charts dla danych sensorów
- [ ] Temperature/humidity monitoring
- [ ] Motion detection alerts
- [ ] Data logging do pliku

### v3.0 - Advanced Features
- [ ] File transfer przez RFCOMM
- [ ] Custom Arduino commands
- [ ] Multi-device dashboard
- [ ] Cloud sync (Azure IoT Hub)

---

## 🛠️ Wymagania Systemowe

### Development
- **OS:** Windows 10 version 1809 (October 2018 Update) lub nowszy
- **Visual Studio:** 2019 lub nowszy z UWP workload
- **SDK:** Windows 10 SDK (10.0.26100.0)
- **Min Version:** Windows 10 Fall Creators Update (10.0.17763.0)
- **.NET:** UWP 6.2.14

### Runtime
- **OS:** Windows 10 Fall Creators Update (17763) lub nowszy
- **Bluetooth:** Bluetooth 2.0+ (Classic) lub Bluetooth 4.0+ (BLE)
- **Permissions:** Bluetooth capability w Package.appxmanifest

### Hardware
- **Arduino:** Uno, Mega, Nano, lub kompatybilny
- **Bluetooth Module:** HC-05, HC-06, HM-10, lub kompatybilny
- **Computer:** PC z Bluetooth adapter (built-in lub USB dongle)

---

## 📝 Package.appxmanifest - Capabilities

Aplikacja wymaga następujących uprawnień:

```xml
<Capabilities>
  <Capability Name="internetClient" />
  <DeviceCapability Name="bluetooth.rfcomm">
    <Device Id="any">
      <Function Type="serviceId:00001101-0000-1000-8000-00805F9B34FB" />
    </Device>
  </DeviceCapability>
  <DeviceCapability Name="bluetooth.genericAttributeProfile">
    <Device Id="any">
      <Function Type="name:genericAccess" />
      <Function Type="name:genericAttribute" />
    </Device>
  </DeviceCapability>
</Capabilities>
```

---

## 🧪 Testing

### Test Scenario 1: LED Control
1. Uruchom aplikację
2. Kliknij [Start] → sprawdź czy połączenie nawiązane
3. Kliknij [Led On] → sprawdź czy LED świeci
4. Kliknij [Led Off] → sprawdź czy LED zgasł

### Test Scenario 2: BLE Scanning
1. Uruchom aplikację
2. Kliknij [BT LE 01] → sprawdź Output dla listy urządzeń
3. Sprawdź czy aplikacja wykrywa urządzenia BLE w zasięgu

### Test Scenario 3: RFCOMM Communication
1. Uruchom dwie instancje aplikacji (lub dwa komputery)
2. Na pierwszym: [BT LE 03 - Server]
3. Na drugim: [BT LE 03 - Client]
4. Sprawdź czy połączenie nawiązane (Output window)

---

## 🔧 Projekt Powiązany: Atmel.Services

Projekt `Atmel.Services` jest biblioteką klas UWP zawierającą logikę biznesową aplikacji. Kluczowe komponenty:

### Interfaces (Kontrakty)
- `IBluetoothService` - Serwis Bluetooth
- `IArduinoController` - Kontroler Arduino
- `IDeviceDiscoveryService` - Wykrywanie urządzeń
- `IRfcommService` - Serwis RFCOMM

### Implementation (Implementacje)
- `BluetoothDiscoveryService` - Wykrywanie urządzeń Bluetooth Classic
- `BluetoothLEDiscoveryService` - Wykrywanie urządzeń BLE
- `ArduinoController` - Kontrola Arduino przez Firmata

### Helpers (Pomocnicze)
- `SdpAttributeConfigurator` - Konfiguracja atrybutów SDP
- `RfcommServiceValidator` - Walidacja serwisów RFCOMM

---

## 📚 Najważniejsze Zmiany

### ✅ Co zostało zaktualizowane:

1. **NuGet Packages** - Microsoft.NETCore.UniversalWindowsPlatform: 5.1.0 → 6.2.14
2. **Architektura** - Dodano projekt Atmel.Services z separacją logiki
3. **MVVM Pattern** - Wprowadzono ViewModels i Commands
4. **Dependency Injection** - Dodano ServiceContainer
5. **Target Platform** - Windows 10 SDK 10.0.26100.0
6. **Min Platform Version** - 10.0.17763.0 (Fall Creators Update)

### 🔄 Aktualny Stan Projektu:

- ✅ **Projekt kompiluje się** - UWP 6.2.14 działa poprawnie
- ✅ **MVVM częściowo wdrożone** - ViewModels i Commands gotowe
- ⚠️ **Legacy code** - MainPage.xaml.cs zawiera jeszcze starą logikę
- ✅ **Modułowa struktura** - Atmel.Services oddziela logikę biznesową

---

## 🔗 Powiązane Dokumentacje

- [Główna dokumentacja projektu](../DOCUMENTATION.md)
- [Microsoft Maker RemoteWiring GitHub](https://github.com/ms-iot/remote-wiring)
- [UWP Bluetooth APIs](https://docs.microsoft.com/en-us/windows/uwp/devices-sensors/bluetooth)
- [Arduino Firmata Protocol](https://github.com/firmata/protocol)
- [MVVM Pattern in UWP](https://docs.microsoft.com/en-us/windows/uwp/data-binding/data-binding-and-mvvm)

---

## 📄 Licencja

Projekt AT93C56Tests (włącznie z aplikacją Atmel) jest dostępny na licencji MIT. Zobacz [LICENSE](../LICENSE) dla szczegółów.

---

## 👨‍💻 Autor

**vadmkp**
- GitHub: [@vadmkp](https://github.com/vadmkp)
- Projekt: [AT93C56Tests](https://github.com/vadmkp/AT93C56Tests)

---

## 🏷️ Tags

`#UWP` `#Bluetooth` `#Arduino` `#RemoteWiring` `#CSharp` `#XAML` `#IoT` `#HC05` `#BLE` `#RFCOMM` `#Windows10` `#MVVM` `#DependencyInjection`

---

*Dokumentacja aplikacji Atmel - część projektu AT93C56Tests*  
*Ostatnia aktualizacja: 2025-01-XX*
