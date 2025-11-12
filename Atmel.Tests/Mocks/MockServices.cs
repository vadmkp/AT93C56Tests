using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atmel.Tests.Mocks
{
    /// <summary>
    /// Mock Bluetooth device info for testing
    /// Simulates Atmel.Services.Models.BluetoothLEDeviceInfoModel
    /// </summary>
    public class MockBluetoothDevice
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsPaired { get; set; }

        public MockBluetoothDevice(string id, string name, bool isEnabled = true, bool isPaired = false)
        {
            Id = id;
            Name = name;
            IsEnabled = isEnabled;
            IsPaired = isPaired;
        }
    }

    /// <summary>
    /// Mock Bluetooth configuration
    /// Simulates Atmel.Services.Configuration.BluetoothConfiguration
    /// </summary>
    public class MockBluetoothConfiguration
    {
        public string DefaultDeviceName { get; set; } = "TestDevice";
        public string AlternativeDeviceName { get; set; } = "HC-05";
        public int ConnectionTimeoutMs { get; set; } = 5000;
    }

    /// <summary>
    /// Mock Arduino configuration
    /// Simulates Atmel.Services.Configuration.ArduinoConfiguration
    /// </summary>
    public class MockArduinoConfiguration
    {
        public byte LedPin { get; set; } = 13;
        public int CommandDelayMs { get; set; } = 100;
    }

    /// <summary>
    /// Mock Bluetooth Service for testing
    /// Simulates Atmel.Services.Interfaces.IBluetoothService
    /// </summary>
    public class MockBluetoothService
    {
        private bool _isConnected;
        private readonly List<MockBluetoothDevice> _availableDevices;

        public event Action? ConnectionEstablished;
        public event Action<string>? ConnectionLost;
        public event Action<string>? ConnectionFailed;

        public bool IsConnected => _isConnected;
        public bool ConnectAsyncCalled { get; private set; }
        public bool DisconnectAsyncCalled { get; private set; }
        public string? LastConnectedDevice { get; private set; }

        public MockBluetoothService()
        {
            _availableDevices = new List<MockBluetoothDevice>
            {
                new MockBluetoothDevice("device-1", "TestDevice", true, true),
                new MockBluetoothDevice("device-2", "HC-05", true, false),
                new MockBluetoothDevice("device-3", "sowaphone", true, true)
            };
        }

        public Task<IEnumerable<MockBluetoothDevice>> GetAvailableDevicesAsync()
        {
            return Task.FromResult<IEnumerable<MockBluetoothDevice>>(_availableDevices);
        }

        public Task<bool> ConnectAsync(string deviceName)
        {
            ConnectAsyncCalled = true;
            LastConnectedDevice = deviceName;

            var device = _availableDevices.FirstOrDefault(d => d.Name == deviceName);
            
            if (device == null)
            {
                ConnectionFailed?.Invoke($"Device '{deviceName}' not found");
                return Task.FromResult(false);
            }

            if (!device.IsEnabled)
            {
                ConnectionFailed?.Invoke($"Device '{deviceName}' is disabled");
                return Task.FromResult(false);
            }

            _isConnected = true;
            ConnectionEstablished?.Invoke();
            return Task.FromResult(true);
        }

        public Task DisconnectAsync()
        {
            DisconnectAsyncCalled = true;
            
            if (_isConnected)
            {
                _isConnected = false;
                ConnectionLost?.Invoke("Disconnected by user");
            }
            
            return Task.CompletedTask;
        }

        public void AddDevice(MockBluetoothDevice device)
        {
            _availableDevices.Add(device);
        }

        public void ClearDevices()
        {
            _availableDevices.Clear();
        }
    }

    /// <summary>
    /// Mock Arduino Controller for testing
    /// Simulates Atmel.Services.Interfaces.IArduinoController
    /// </summary>
    public class MockArduinoController
    {
        private readonly Dictionary<byte, bool> _pinStates = new();
        private bool _isInitialized;

        public bool IsReady => _isInitialized;
        public byte LastPin { get; private set; }
        public bool LastState { get; private set; }
        public int SetPinStateCallCount { get; private set; }

        public void Initialize()
        {
            _isInitialized = true;
        }

        public void SetPinState(byte pin, bool state)
        {
            if (!IsReady)
                throw new InvalidOperationException("Arduino controller is not initialized");

            LastPin = pin;
            LastState = state;
            SetPinStateCallCount++;
            _pinStates[pin] = state;
        }

        public Task<bool> GetPinStateAsync(byte pin)
        {
            if (!IsReady)
                throw new InvalidOperationException("Arduino controller is not initialized");

            return Task.FromResult(_pinStates.ContainsKey(pin) && _pinStates[pin]);
        }

        public void Reset()
        {
            _pinStates.Clear();
            SetPinStateCallCount = 0;
            LastPin = 0;
            LastState = false;
        }
    }

    /// <summary>
    /// Mock Device Discovery Service
    /// Simulates Atmel.Services.Interfaces.IDeviceDiscoveryService
    /// </summary>
    public class MockDeviceDiscoveryService
    {
        private bool _isDiscovering;

        public event EventHandler? EnumerationCompleted;

        public bool IsDiscovering => _isDiscovering;
        public bool StartDiscoveryAsyncCalled { get; private set; }
        public bool StopDiscoveryAsyncCalled { get; private set; }

        public Task StartDiscoveryAsync()
        {
            StartDiscoveryAsyncCalled = true;
            _isDiscovering = true;
            
            // Simulate discovery completion
            Task.Run(async () =>
            {
                await Task.Delay(100);
                EnumerationCompleted?.Invoke(this, EventArgs.Empty);
            });

            return Task.CompletedTask;
        }

        public Task StopDiscoveryAsync()
        {
            StopDiscoveryAsyncCalled = true;
            _isDiscovering = false;
            return Task.CompletedTask;
        }

        public void Reset()
        {
            _isDiscovering = false;
            StartDiscoveryAsyncCalled = false;
            StopDiscoveryAsyncCalled = false;
        }
    }
}
