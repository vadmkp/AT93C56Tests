using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Networking.Sockets;
using Atmel.Services.Interfaces;
using Atmel.Services.Configuration;
using Atmel.Services.Helpers;

namespace Atmel.Services.Rfcomm
{
    /// <summary>
    /// RFCOMM Client implementation following SOLID principles
    /// </summary>
    public sealed class ClientRFCOMM : IRfcommService
    {
        private RfcommDeviceService _service;
        private StreamSocket _socket;
        private readonly RfcommConfiguration _configuration;
        private readonly RfcommServiceValidator _validator;

        public bool IsRunning { get; private set; }

        public ClientRFCOMM(RfcommConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _validator = new RfcommServiceValidator(
                _configuration.MinimumServiceVersion,
                _configuration.ServiceVersionAttributeId,
                _configuration.ServiceVersionAttributeType);
        }

        public IAsyncAction InitializeAsync()
        {
            return AsyncInfo.Run(async (cancellationToken) =>
            {
                var services = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(
                    RfcommDeviceService.GetDeviceSelector(RfcommServiceId.ObexObjectPush));

                if (services.Count == 0)
                    throw new InvalidOperationException("No RFCOMM services found");

                var service = await RfcommDeviceService.FromIdAsync(services.First().Id);

                if (!_validator.SupportsProtection(service))
                    throw new InvalidOperationException("Service does not support required protection level");

                if (!_validator.IsCompatibleVersion(service))
                    throw new InvalidOperationException("Service version is not compatible");

                _service = service;
            });
        }

        public IAsyncAction StartAsync()
        {
            return AsyncInfo.Run(async (cancellationToken) =>
            {
                if (_service == null)
                    throw new InvalidOperationException("Service must be initialized before starting");

                _socket = new StreamSocket();
                await _socket.ConnectAsync(
                    _service.ConnectionHostName,
                    _service.ConnectionServiceName,
                    SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication);

                IsRunning = true;
            });
        }

        public IAsyncAction StopAsync()
        {
            return AsyncInfo.Run(async (cancellationToken) =>
            {
                _socket?.Dispose();
                _socket = null;
                
                _service?.Dispose();
                _service = null;
                
                IsRunning = false;
                
                await System.Threading.Tasks.Task.CompletedTask;
            });
        }
    }
}
