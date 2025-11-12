using System;
using Windows.Foundation;
using Windows.Devices.Enumeration;

namespace Atmel.Services.Interfaces
{
    /// <summary>
    /// Interface for device discovery operations
    /// Windows Runtime Component compatible
    /// </summary>
    public interface IDeviceDiscoveryService
    {
        event EventHandler<DeviceInformation> DeviceAdded;
        event EventHandler<DeviceInformationUpdate> DeviceUpdated;
        event EventHandler<DeviceInformationUpdate> DeviceRemoved;
        event EventHandler EnumerationCompleted;

        IAsyncAction StartDiscoveryAsync();
        IAsyncAction StopDiscoveryAsync();
        bool IsDiscovering { get; }
    }
}
