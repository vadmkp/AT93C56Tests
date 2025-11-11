# AT93C56Tests

> **Projekt edukacyjny z 2017 roku** - Nauka Bluetooth communication, Arduino remote control, oraz Windows UWP development.

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Platform](https://img.shields.io/badge/platform-Windows%20UWP-brightgreen.svg)]()
[![Arduino](https://img.shields.io/badge/Arduino-Compatible-00979D.svg)]()

---

## 📖 O Projekcie

**AT93C56Tests** to eksperymentalny projekt łączący świat **Windows UWP** z **Arduino** przez **Bluetooth**. Aplikacja pozwala na zdalne sterowanie urządzeniem Arduino z komputera Windows, wykorzystując moduły Bluetooth HC-05 oraz Microsoft Maker RemoteWiring library.

Projekt powstał jako platforma do nauki:
- 🔵 **Bluetooth programming** (BLE + Classic RFCOMM)
- 🤖 **Arduino remote control** przez Bluetooth Serial
- 💻 **Windows UWP development** z XAML i C#
- 📝 **GitHub workflow** i zarządzanie repozytorium

---

## ✨ Główne Funkcje

### 🎮 Zdalne Sterowanie Arduino
- **ON/OFF LED** - Włączanie/wyłączanie LED na pinie 13 Arduino z aplikacji Windows
- **digitalWrite** - Zdalna kontrola pinów GPIO przez Bluetooth
- **Real-time communication** - Natychmiastowa reakcja urządzenia

### 📡 Bluetooth Integration
- **Bluetooth Classic** - Komunikacja przez HC-05/HC-06 moduły
- **Bluetooth Low Energy (BLE)** - Skanowanie i połączenia z urządzeniami BLE
- **RFCOMM Client/Server** - Peer-to-peer komunikacja między urządzeniami Windows
- **Device Discovery** - Automatyczne wykrywanie urządzeń w zasięgu

### 🛠️ Biblioteki dla Chipów Pamięci
Projekt zawiera szkielety bibliotek dla popularnych chipów pamięci:
- **AT93C56** - 2Kb Microwire Serial EEPROM
- **24LC512** - 512Kb I2C Serial EEPROM  
- **23LC1024** - 1Mb SPI Serial RAM

*(Uwaga: Biblioteki nie są zaimplementowane - tylko struktura projektów)*

---

## 🏗️ Architektura

```
┌─────────────────────────────────────────────────────────────┐
│                    Windows 10 Computer                      │
│  ┌───────────────────────────────────────────────────────┐  │
│  │           Atmel UWP Application (C# + XAML)           │  │
│  │  ┌─────────────────────────────────────────────────┐  │  │
│  │  │  MainPage.xaml - User Interface                 │  │  │
│  │  │  [List] [Start] [ON] [OFF] [BLE Scan]          │  │  │
│  │  └─────────────────────────────────────────────────┘  │  │
│  │                        │                               │  │
│  │  ┌─────────────────────▼─────────────────────────┐    │  │
│  │  │  BluetoothSerial ("HC-05" / "sowaphone")      │    │  │
│  │  │  Microsoft.Maker.RemoteWiring                 │    │  │
│  │  └─────────────────────┬─────────────────────────┘    │  │
│  └────────────────────────┼─────────────────────────────┘  │
└─────────────────────────┼─────────────────────────────────┘
                          │ Bluetooth Connection
                          │ (Serial Protocol)
┌─────────────────────────▼─────────────────────────────────┐
│                     Arduino Board                          │
│  ┌───────────────────────────────────────────────────┐    │
│  │  Pin 13 - LED (digitalWrite HIGH/LOW)             │    │
│  │  Bluetooth Module HC-05/HC-06                     │    │
│  │  Arduino Firmata (or Custom Sketch)               │    │
│  └───────────────────────────────────────────────────┘    │
└────────────────────────────────────────────────────────────┘
```

---

## 🚀 Quick Start

### Wymagania

- **Windows 10** lub nowszy
- **Visual Studio 2017+** z workloadami:
  - Universal Windows Platform development
  - .NET desktop development
- **Arduino** (Uno, Mega, Nano, itp.)
- **Moduł Bluetooth** HC-05 lub HC-06
- **Kabel USB** do programowania Arduino

### Instalacja

1. **Sklonuj repozytorium:**
   ```bash
   git clone https://github.com/vadmkp/AT93C56Tests.git
   cd AT93C56Tests
   ```

2. **Otwórz projekt w Visual Studio:**
   ```bash
   AT93C56.sln
   ```

3. **Skonfiguruj Arduino:**
   - Podłącz moduł Bluetooth HC-05 do Arduino (TX→RX, RX→TX, VCC→5V, GND→GND)
   - Załaduj sketch obsługujący RemoteWiring (np. StandardFirmata)
   - Sparuj moduł Bluetooth z komputerem Windows

4. **Zmień nazwę urządzenia w kodzie:**
   
   W pliku `Atmel/MainPage.xaml.cs` (linia ~67):
   ```csharp
   _bluetooth = new BluetoothSerial("TWOJA_NAZWA_BT");
   ```

5. **Build & Run:**
   - Ustaw projekt **Atmel** jako StartUp Project
   - Wybierz platformę (x86/x64/ARM)
   - Naciśnij F5 (Debug) lub Ctrl+F5 (Run)

### Użycie

1. Uruchom aplikację UWP
2. Kliknij **"Start"** - nawiąże połączenie z Arduino przez Bluetooth
3. Kliknij **"ON"** - LED na pinie 13 Arduino zapali się 💡
4. Kliknij **"OFF"** - LED zgaśnie
5. Kliknij **"List"** - wyświetli dostępne urządzenia Bluetooth (w Output window)

---

## 📂 Struktura Projektu

```
AT93C56Tests/
├── 📱 Atmel/                              # Główna aplikacja UWP
│   ├── MainPage.xaml                      # UI aplikacji
│   ├── MainPage.xaml.cs                   # Logika - Bluetooth + Arduino control
│   ├── Silnik/
│   │   ├── ClientRFCOMM.cs                # Bluetooth RFCOMM Client
│   │   └── ServerRFCOMM.cs                # Bluetooth RFCOMM Server
│   ├── Models/
│   │   └── BluetoothLEDeviceInfoModel.cs  # Model danych BLE device
│   └── Assets/                            # Obrazy i zasoby
│
├── 🤖 Arduino01/                          # Projekt Arduino
│   └── Arduino01.ino                      # Pusty sketch (do uzupełnienia)
│
├── 📚 Atmel.AT93C56/                      # Biblioteka dla AT93C56 EEPROM
├── 📚 Atmel.Microchip24LC512/             # Biblioteka dla 24LC512 EEPROM
├── 📚 Atmel.Microchip23LC1024/            # Biblioteka dla 23LC1024 RAM
│
├── AT93C56.sln                            # Visual Studio Solution
├── README.md                              # Ten plik
├── DOCUMENTATION.md                       # Szczegółowa dokumentacja techniczna
└── LICENSE                                # Licencja projektu
```

---

## 🔧 Technologie

### Frontend (UWP Application)
- **C# 7.0+**
- **XAML** - UI markup language
- **Universal Windows Platform (UWP)** APIs
- **.NET Framework** / UWP Runtime

### Libraries & Dependencies
- **Microsoft.Maker.RemoteWiring** - Arduino remote control library
- **Microsoft.Maker.Serial** - Serial communication abstractions
- **Windows.Devices.Bluetooth** - Bluetooth APIs (BLE + Classic)
- **Windows.Devices.Enumeration** - Device discovery APIs
- **Windows.Networking.Sockets** - RFCOMM socket communication

### Hardware
- **Arduino** (Uno/Mega/Nano)
- **HC-05 / HC-06** Bluetooth Serial Module
- **LEDs, sensors, actuators** (opcjonalnie)

---

## 📸 Screenshots

### Główna Aplikacja
```
┌──────────────────────────────────────────┐
│  AT93C56Tests - Arduino Remote Control  │
├──────────────────────────────────────────┤
│                                          │
│  [List]  - List Bluetooth Devices       │
│  [Start] - Connect to Arduino           │
│                                          │
│  ┌────────────────────────────────┐     │
│  │  LED Control                   │     │
│  │  [ON]  [OFF]                   │     │
│  └────────────────────────────────┘     │
│                                          │
│  ┌────────────────────────────────┐     │
│  │  BLE Testing                   │     │
│  │  [BLE Scan 1]  [BLE Scan 2]    │     │
│  │  [RFCOMM Server] [RFCOMM Client] │  │
│  └────────────────────────────────┘     │
│                                          │
└──────────────────────────────────────────┘
```

---

## 🎯 Use Cases

### 1. Home Automation
Zdalne sterowanie urządzeniami domowymi (światła, żaluzje, otwieranie drzwi) przez Bluetooth z aplikacji Windows.

### 2. IoT Prototyping
Szybkie prototypowanie projektów IoT - kontrola sensorów i aktuatorów bez pisania skomplikowanego kodu Arduino.

### 3. Educational Projects
Nauka komunikacji Bluetooth, programowania Arduino, oraz tworzenia aplikacji UWP w jednym projekcie.

### 4. Robotics Control
Pilot do sterowania robotem - kontrola motorów, serwomechanizmów, odczyt sensorów dystansu.

---

## 🐛 Znane Ograniczenia

- ⚠️ **Hardcoded device names** - Nazwa urządzenia Bluetooth jest wpisana na sztywno w kodzie
- ⚠️ **Brak UI dla listy urządzeń** - Lista urządzeń wyświetla się tylko w Debug Output
- ⚠️ **Arduino sketch pusty** - Wymaga załadowania Firmata lub custom protocol
- ⚠️ **Biblioteki EEPROM nie zaimplementowane** - Tylko szkielety projektów
- ⚠️ **Single connection** - Aplikacja obsługuje jedno urządzenie na raz
- ⚠️ **Windows only** - Projekt jest UWP, działa tylko na Windows 10+

---

## 🔮 Roadmap & Możliwe Rozszerzenia

### v1.1 (Near Future)
- [ ] UI do wyboru urządzenia Bluetooth z listy
- [ ] Zapisywanie ostatnio używanego urządzenia
- [ ] Toast notifications dla zdarzeń Bluetooth
- [ ] Arduino StandardFirmata sketch w projekcie

### v2.0 (Future)
- [ ] Obsługa wielu pinów GPIO (sliders, switches)
- [ ] PWM control dla LED/motorów
- [ ] Odczyt danych z sensorów (temperatura, wilgotność)
- [ ] Charts/graphs dla danych real-time
- [ ] Implementacja biblioteki 24LC512 (I2C EEPROM)

### v3.0 (Long-term)
- [ ] Migracja do WinUI 3 / .NET MAUI
- [ ] Cross-platform support (Android/iOS)
- [ ] Cloud integration (Azure IoT Hub)
- [ ] Voice control (Cortana / Windows Speech)
- [ ] Machine learning na danych sensorów

---

## 📚 Dokumentacja

### Szczegółowa Dokumentacja
Przeczytaj [DOCUMENTATION.md](DOCUMENTATION.md) dla:
- Szczegółowej architektury systemu
- Opisu wszystkich klas i metod
- Przykładów kodu
- Troubleshooting guide
- FAQ

### Przydatne Linki
- [Microsoft Maker RemoteWiring Documentation](https://github.com/ms-iot/remote-wiring)
- [UWP Bluetooth APIs](https://docs.microsoft.com/en-us/windows/uwp/devices-sensors/bluetooth)
- [Arduino Firmata Protocol](https://github.com/firmata/protocol)
- [HC-05 Bluetooth Module Datasheet](https://www.electronicwings.com/sensors-modules/hc-05-bluetooth-module)

---

## 🤝 Contributing

Projekt jest otwarty na wkład! Jeśli chcesz pomóc:

1. **Fork** repozytorium
2. Stwórz **feature branch** (`git checkout -b feature/AmazingFeature`)
3. **Commit** zmiany (`git commit -m 'Add some AmazingFeature'`)
4. **Push** do brancha (`git push origin feature/AmazingFeature`)
5. Otwórz **Pull Request**

### Pomysły na Wkład
- 🐛 Zgłaszanie bugów i issues
- 💡 Propozycje nowych funkcji
- 📝 Poprawki dokumentacji
- 🧪 Dodawanie testów jednostkowych
- 🎨 Ulepszenia UI/UX
- 🔧 Implementacja bibliotek EEPROM

---

## 📜 Licencja

Projekt udostępniony na licencji MIT. Zobacz plik [LICENSE](LICENSE) dla szczegółów.

```
MIT License

Copyright (c) 2017-2025 vadmkp

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files...
```

---

## 👨‍💻 Autor

**vadmkp**
- GitHub: [@vadmkp](https://github.com/vadmkp)
- Repozytorium: [AT93C56Tests](https://github.com/vadmkp/AT93C56Tests)

---

## 🏷️ Tags

`#Bluetooth` `#Arduino` `#UWP` `#IoT` `#RemoteWiring` `#CSharp` `#XAML` `#Windows10` `#HC05` `#BLE` `#RFCOMM` `#Firmata` `#MakerMovement` `#EmbeddedSystems` `#Learning`

---

## ⭐ Star History

Jeśli projekt Ci się podoba, zostaw gwiazdkę! ⭐

[![Star History Chart](https://api.star-history.com/svg?repos=vadmkp/AT93C56Tests&type=Date)](https://github.com/vadmkp/AT93C56Tests)

---

## 📊 Project Status

| Komponent | Status | Uwagi |
|-----------|--------|-------|
| UWP Application | ✅ Działająca | Podstawowe funkcje zaimplementowane |
| Bluetooth Serial | ✅ Działający | HC-05 support |
| BLE Scanning | ✅ Działający | Device discovery |
| RFCOMM Client/Server | ⚠️ Częściowy | Brak data transfer |
| Arduino Sketch | ❌ Pusty | Wymaga Firmata |
| AT93C56 Library | ❌ Nie rozpoczęte | Tylko szkielet |
| 24LC512 Library | ❌ Nie rozpoczęte | Tylko szkielet |
| 23LC1024 Library | ❌ Nie rozpoczęte | Tylko szkielet |
| Unit Tests | ❌ Brak | Do zaimplementowania |

**Ostatnia aktualizacja:** 2025-11-11

---

<p align="center">
  <sub>Zbudowane z ❤️ dla społeczności Maker i IoT</sub>
</p>

<p align="center">
  <sub>Projekt edukacyjny - Learning by Doing 🚀</sub>
</p>
