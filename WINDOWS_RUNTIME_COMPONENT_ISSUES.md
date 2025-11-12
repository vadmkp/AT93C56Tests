# ?? Atmel.Services - Problem z Windows Runtime Component

## ?? Problem

Projekt Atmel.Services ma liczne b³êdy kompilacji zwi¹zane z ograniczeniami **Windows Runtime Component** (WinMD):

### B³êdy:
1. ? **Custom delegates** nie s¹ wspierane ? `ConnectionEventHandler`, `ConnectionErrorEventHandler`
2. ? **Task** zamiast **IAsyncAction/IAsyncOperation**
3. ? **Unsealed classes** zamiast **sealed**
4. ? **IEnumerable<T>** zamiast **IVector<T>** / **IVectorView<T>**

---

## ? Co zosta³o naprawione

### 1. Wszystkie klasy oznaczone jako `sealed` ?
- `BluetoothConfiguration`
- `ArduinoConfiguration`
- `RfcommConfiguration`
- `BluetoothDiscoveryService`
- `BluetoothLEDiscoveryService`
- `ArduinoController`
- `ServerRFCOMM`
- `ClientRFCOMM`
- `SdpAttributeConfigurator`
- `RfcommServiceValidator`
- `BluetoothLEDeviceInfoModel`

### 2. Task ? IAsync* ?
- `Task` ? `IAsyncAction`
- `Task<T>` ? `IAsyncOperation<T>`
- Dodano `using System.Runtime.InteropServices.WindowsRuntime;`
- U¿ywamy `AsyncInfo.Run()` do konwersji

### 3. Interfaces zaktualizowane ?
- `IBluetoothService` - IAsyncOperation/IAsyncAction
- `IArduinoController` - IAsyncOperation
- `IRfcommService` - IAsyncAction
- `IDeviceDiscoveryService` - IAsyncAction

---

## ? Co NADAL NIE DZIA£A

### Problem: Custom Delegates
Windows Runtime Component **NIE WSPIERA** custom delegates typu:
```csharp
public delegate void ConnectionEventHandler();
public delegate void ConnectionErrorEventHandler(string error);
```

### Rozwi¹zanie 1: U¿ywaj EventHandler<T>
```csharp
// Zamiast:
public event ConnectionEventHandler ConnectionEstablished;
public event ConnectionErrorEventHandler ConnectionLost;

// U¿yj:
public event EventHandler ConnectionEstablished;
public event EventHandler<string> ConnectionLost;
```

### Rozwi¹zanie 2: Utwórz EventArgs class
```csharp
public sealed class ConnectionErrorEventArgs
{
    public string Error { get; set; }
}

public event EventHandler<ConnectionErrorEventArgs> ConnectionLost;
```

### Rozwi¹zanie 3: U¿yj TypedEventHandler
```csharp
using Windows.Foundation;

public event TypedEventHandler<object, string> ConnectionLost;
```

---

## ?? REKOMENDACJA: NIE U¯YWAJ Windows Runtime Component

### Problem:
Windows Runtime Component ma **BARDZO RESTRYKCYJNE** wymagania:
- ? Nie wspiera wielu standardowych typów .NET
- ? Wymaga sealed classes
- ? Nie wspiera Task (tylko IAsync*)
- ? Nie wspiera custom delegates
- ? Nie wspiera IEnumerable<T> (tylko IVector<T>)
- ? Trudny w utrzymaniu i testowaniu

### Lepsze rozwi¹zanie:

**OPCJA A: .NET Standard 2.0 Library**
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
  </PropertyGroup>
</Project>
```
? Pe³ne wsparcie .NET  
? Task/async/await  
? Custom delegates  
? £atwe testowanie  
? **Problem**: Nie mo¿e u¿ywaæ UWP-specific APIs (Windows.Devices.Bluetooth)

**OPCJA B: UWP Class Library (nie WinRT Component)**
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>uap10.0.17763</TargetFramework>
    <TargetPlatformMinVersion>10.0.17763.0</TargetPlatformMinVersion>
  </PropertyGroup>
</Project>
```
? Dostêp do UWP APIs  
? Normalne .NET typy  
? Task/async/await  
? Custom delegates  
? **NAJLEPSZE ROZWI¥ZANIE**

**OPCJA C: Zostaw w g³ównym projekcie Atmel**
? Najprostsze  
? Wszystko dzia³a  
? Brak separacji warstw

---

## ?? ZALECANE KROKI

### Krok 1: Zmieñ typ projektu Atmel.Services

```xml
<!-- Atmel.Services.csproj -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>uap10.0.17763</TargetFramework>
    <TargetPlatformMinVersion>10.0.17763.0</TargetPlatformMinVersion>
    <TargetPlatformVersion>10.0.19041.0</TargetPlatformVersion>
    <RootNamespace>Atmel.Services</RootNamespace>
    <AssemblyName>Atmel.Services</AssemblyName>
  </PropertyGroup>
  
  <ItemGroup>
    <PackageReference Include="Microsoft.NETCore.UniversalWindowsPlatform" Version="6.2.14" />
  </ItemGroup>
</Project>
```

### Krok 2: Przywróæ normalne typy .NET

```csharp
// Interfaces/IBluetoothService.cs
public interface IBluetoothService
{
    event Action ConnectionEstablished;              // Normalne delegates
    event Action<string> ConnectionLost;
    event Action<string> ConnectionFailed;

    Task<IEnumerable<BluetoothLEDeviceInfoModel>> GetAvailableDevicesAsync();  // Task zamiast IAsync*
    Task<bool> ConnectAsync(string deviceName);
    Task DisconnectAsync();
    bool IsConnected { get; }
}
```

### Krok 3: Usuñ `sealed` gdzie nie potrzebne

```csharp
// Configuration classes mog¹ byæ unsealed
public class BluetoothConfiguration  // Nie sealed
{
    // ...
}
```

### Krok 4: Rebuild

```powershell
Remove-Item -Recurse -Force bin,obj
dotnet restore
dotnet build
```

---

## ?? PODSUMOWANIE

| Rozwi¹zanie | UWP APIs | .NET Types | Testability | Difficulty |
|-------------|----------|------------|-------------|------------|
| **Windows Runtime Component** | ? | ? | ? | ?? Hard |
| **.NET Standard 2.0** | ? | ? | ? | ?? Easy |
| **UWP Class Library** | ? | ? | ? | ?? Easy |
| **W g³ównym projekcie** | ? | ? | ?? | ?? Easy |

**REKOMENDACJA**: **UWP Class Library** (nie Windows Runtime Component)

---

## ?? Nastêpne kroki

1. **Zmieñ typ projektu** na UWP Class Library
2. **Przywróæ normalne typy .NET** (Task, Action, unsealed classes)
3. **Rebuild i test**
4. **Dodaj referencjê** z Atmel do Atmel.Services
5. **Zaktualizuj using statements** w g³ównym projekcie

---

## ?? Dokumentacja

- [UWP vs Windows Runtime Component](https://docs.microsoft.com/en-us/windows/uwp/winrt-components/)
- [Windows Runtime type restrictions](https://docs.microsoft.com/en-us/windows/uwp/winrt-components/creating-windows-runtime-components-in-csharp-and-visual-basic)

---

**Status**: ?? **WYMAGA ZMIANY TYPU PROJEKTU**

Aktualne podejœcie (Windows Runtime Component) jest **zbyt restrykcyjne**.  
Zalecam zmianê na **UWP Class Library**.
