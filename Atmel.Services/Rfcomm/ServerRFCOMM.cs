using System;
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
    /// RFCOMM Server implementation following SOLID principles
    /// </summary>
    public sealed class ServerRFCOMM : IRfcommService
    {
        private RfcommServiceProvider _provider;
        private StreamSocketListener _listener;
        private StreamSocket _socket;
        private readonly RfcommConfiguration _configuration;
        private readonly SdpAttributeConfigurator _sdpConfigurator;

        public bool IsRunning { get; private set; }

        public ServerRFCOMM(RfcommConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _sdpConfigurator = new SdpAttributeConfigurator(
                _configuration.ServiceVersionAttributeId,
                _configuration.ServiceVersionAttributeType,
                _configuration.ServiceVersion);
        }

        public IAsyncAction InitializeAsync()
        {
            return AsyncInfo.Run(async (cancellationToken) =>
            {
                _provider = await RfcommServiceProvider.CreateAsync(RfcommServiceId.ObexObjectPush);
                _listener = new StreamSocketListener();
                _listener.ConnectionReceived += OnConnectionReceived;
            });
        }

        public IAsyncAction StartAsync()
        {
            return AsyncInfo.Run(async (cancellationToken) =>
            {
                if (_provider == null)
                    throw new InvalidOperationException("Service must be initialized before starting");

                await _listener.BindServiceNameAsync(
                    _provider.ServiceId.AsString(),
                    SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication);

                _sdpConfigurator.ConfigureAttributes(_provider);
                _provider.StartAdvertising(_listener);
                
                IsRunning = true;
            });
        }

        public IAsyncAction StopAsync()
        {
            return AsyncInfo.Run(async (cancellationToken) =>
            {
                if (_provider != null)
                {
                    _provider.StopAdvertising();
                }

                if (_listener != null)
                {
                    await _listener.CancelIOAsync();
                }

                IsRunning = false;
            });
        }

        private async void OnConnectionReceived(
            StreamSocketListener listener,
            StreamSocketListenerConnectionReceivedEventArgs args)
        {
            await StopAsync();
            _socket = args.Socket;
        }
    }
}
