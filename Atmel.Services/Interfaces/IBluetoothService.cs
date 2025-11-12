using System.Collections.Generic;
using Windows.Foundation;
using Atmel.Services.Models;

namespace Atmel.Services.Interfaces
{
    /// <summary>
    /// Delegate for connection events
    /// </summary>
    public delegate void ConnectionEventHandler();

    /// <summary>
    /// Delegate for connection error events
    /// </summary>
    public delegate void ConnectionErrorEventHandler(string error);

    /// <summary>
    /// Interface for Bluetooth connection management
    /// Windows Runtime Component compatible
    /// </summary>
    public interface IBluetoothService
    {
        event ConnectionEventHandler ConnectionEstablished;
        event ConnectionErrorEventHandler ConnectionLost;
        event ConnectionErrorEventHandler ConnectionFailed;

        IAsyncOperation<IEnumerable<BluetoothLEDeviceInfoModel>> GetAvailableDevicesAsync();
        IAsyncOperation<bool> ConnectAsync(string deviceName);
        IAsyncAction DisconnectAsync();
        bool IsConnected { get; }
    }
}
