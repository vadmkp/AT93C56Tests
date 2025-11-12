using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Devices.Enumeration;
using Atmel.Services.Interfaces;

namespace Atmel.Services.Implementation
{
    /// <summary>
    /// Service for discovering Bluetooth LE devices
    /// Implements Single Responsibility Principle
    /// </summary>
    public sealed class BluetoothLEDiscoveryService : IDeviceDiscoveryService
    {
        private DeviceWatcher _deviceWatcher;
        private readonly string[] _requestedProperties = 
        { 
            "System.Devices.Aep.DeviceAddress", 
            "System.Devices.Aep.IsConnected", 
            "System.Devices.Aep.Bluetooth.Le.IsConnectable" 
        };

        public event EventHandler<DeviceInformation> DeviceAdded;
        public event EventHandler<DeviceInformationUpdate> DeviceUpdated;
        public event EventHandler<DeviceInformationUpdate> DeviceRemoved;
        public event EventHandler EnumerationCompleted;

        public bool IsDiscovering { get; private set; }

        public IAsyncAction StartDiscoveryAsync()
        {
            return AsyncInfo.Run(async (cancellationToken) =>
            {
                if (IsDiscovering)
                    return;

                // Protocol ID for Bluetooth Classic RFCOMM
                const string aqsBluetoothRfcomm = "(System.Devices.Aep.ProtocolId:=\"{E0CBF06C-CD8B-4647-BB8A-263B43F0F974}\")";

                _deviceWatcher = DeviceInformation.CreateWatcher(
                    aqsBluetoothRfcomm,
                    _requestedProperties,
                    DeviceInformationKind.AssociationEndpoint);

                // Register event handlers
                _deviceWatcher.Added += OnDeviceAdded;
                _deviceWatcher.Updated += OnDeviceUpdated;
                _deviceWatcher.Removed += OnDeviceRemoved;
                _deviceWatcher.EnumerationCompleted += OnEnumerationCompleted;
                _deviceWatcher.Stopped += OnStopped;

                _deviceWatcher.Start();
                IsDiscovering = true;

                await System.Threading.Tasks.Task.CompletedTask;
            });
        }

        public IAsyncAction StopDiscoveryAsync()
        {
            return AsyncInfo.Run(async (cancellationToken) =>
            {
                if (!IsDiscovering || _deviceWatcher == null)
                    return;

                _deviceWatcher.Stop();
                
                // Unregister event handlers
                _deviceWatcher.Added -= OnDeviceAdded;
                _deviceWatcher.Updated -= OnDeviceUpdated;
                _deviceWatcher.Removed -= OnDeviceRemoved;
                _deviceWatcher.EnumerationCompleted -= OnEnumerationCompleted;
                _deviceWatcher.Stopped -= OnStopped;

                IsDiscovering = false;

                await System.Threading.Tasks.Task.CompletedTask;
            });
        }

        private void OnDeviceAdded(DeviceWatcher sender, DeviceInformation args)
        {
            DeviceAdded?.Invoke(this, args);
        }

        private void OnDeviceUpdated(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            DeviceUpdated?.Invoke(this, args);
        }

        private void OnDeviceRemoved(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            DeviceRemoved?.Invoke(this, args);
        }

        private void OnEnumerationCompleted(DeviceWatcher sender, object args)
        {
            EnumerationCompleted?.Invoke(this, EventArgs.Empty);
        }

        private void OnStopped(DeviceWatcher sender, object args)
        {
            IsDiscovering = false;
        }
    }
}
