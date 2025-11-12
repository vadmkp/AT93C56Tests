using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Atmel.Services.Interfaces;
using Atmel.Services.Configuration;

namespace Atmel.Services.Implementation
{
    /// <summary>
    /// Controller for Arduino device operations
    /// Implements Single Responsibility and Dependency Inversion principles
    /// </summary>
    public sealed class ArduinoController : IArduinoController
    {
        private readonly ArduinoConfiguration _configuration;
        private object _remoteDevice;

        public bool IsReady => _remoteDevice != null;

        public ArduinoController(ArduinoConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void Initialize(object remoteDevice)
        {
            _remoteDevice = remoteDevice ?? throw new ArgumentNullException(nameof(remoteDevice));
        }

        public void SetPinState(byte pin, bool state)
        {
            if (!IsReady)
                throw new InvalidOperationException("Arduino controller is not initialized");

            System.Diagnostics.Debug.WriteLine($"Setting pin {pin} to {(state ? "HIGH" : "LOW")}");
        }

        public IAsyncOperation<bool> GetPinStateAsync(byte pin)
        {
            return AsyncInfo.Run<bool>(async (cancellationToken) =>
            {
                if (!IsReady)
                    throw new InvalidOperationException("Arduino controller is not initialized");

                await System.Threading.Tasks.Task.CompletedTask;
                return false;
            });
        }
    }
}
