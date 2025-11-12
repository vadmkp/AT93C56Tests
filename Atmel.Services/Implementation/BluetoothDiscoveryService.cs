using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Devices.Enumeration;
using Windows.Devices.Bluetooth;
using Atmel.Services.Interfaces;
using Atmel.Services.Models;
using Atmel.Services.Configuration;

namespace Atmel.Services.Implementation
{
    /// <summary>
    /// Service for managing Bluetooth LE device connections
    /// Implements Single Responsibility and Dependency Inversion principles
    /// </summary>
    public sealed class BluetoothDiscoveryService : IBluetoothService
    {
        private readonly BluetoothConfiguration _configuration;
        
        public event ConnectionEventHandler ConnectionEstablished;
        public event ConnectionErrorEventHandler ConnectionLost;
        public event ConnectionErrorEventHandler ConnectionFailed;

        public bool IsConnected { get; private set; }

        public BluetoothDiscoveryService(BluetoothConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IAsyncOperation<IEnumerable<BluetoothLEDeviceInfoModel>> GetAvailableDevicesAsync()
        {
            return AsyncInfo.Run<IEnumerable<BluetoothLEDeviceInfoModel>>(async (cancellationToken) =>
            {
                try
                {
                    var selector = BluetoothDevice.GetDeviceSelector();
                    var devices = await DeviceInformation.FindAllAsync(selector);
                    
                    return devices.Select(d => new BluetoothLEDeviceInfoModel(d)).ToList();
                }
                catch (Exception ex)
                {
                    ConnectionFailed?.Invoke($"Failed to get devices: {ex.Message}");
                    return Enumerable.Empty<BluetoothLEDeviceInfoModel>();
                }
            });
        }

        public IAsyncOperation<bool> ConnectAsync(string deviceName)
        {
            return AsyncInfo.Run<bool>(async (cancellationToken) =>
            {
                try
                {
                    var devices = await GetAvailableDevicesAsync();
                    var targetDevice = devices.FirstOrDefault(d => d.Name == deviceName);
                    
                    if (targetDevice == null)
                    {
                        ConnectionFailed?.Invoke($"Device '{deviceName}' not found");
                        return false;
                    }

                    // Connection logic would go here
                    IsConnected = true;
                    ConnectionEstablished?.Invoke();
                    return true;
                }
                catch (Exception ex)
                {
                    ConnectionFailed?.Invoke($"Connection failed: {ex.Message}");
                    return false;
                }
            });
        }

        public IAsyncAction DisconnectAsync()
        {
            return AsyncInfo.Run(async (cancellationToken) =>
            {
                if (IsConnected)
                {
                    IsConnected = false;
                    ConnectionLost?.Invoke("Disconnected by user");
                }
                await System.Threading.Tasks.Task.CompletedTask;
            });
        }
    }
}
