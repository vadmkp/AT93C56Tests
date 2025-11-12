using System;
using Windows.Storage.Streams;
using Windows.Devices.Bluetooth.Rfcomm;

namespace Atmel.Services.Helpers
{
    /// <summary>
    /// Responsible for SDP attributes configuration (Single Responsibility)
    /// </summary>
    public sealed class SdpAttributeConfigurator
    {
        private readonly uint _serviceVersionAttributeId;
        private readonly byte _serviceVersionAttributeType;
        private readonly uint _serviceVersion;

        public SdpAttributeConfigurator(
            uint serviceVersionAttributeId,
            byte serviceVersionAttributeType,
            uint serviceVersion)
        {
            _serviceVersionAttributeId = serviceVersionAttributeId;
            _serviceVersionAttributeType = serviceVersionAttributeType;
            _serviceVersion = serviceVersion;
        }

        public void ConfigureAttributes(RfcommServiceProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            var writer = new DataWriter();
            writer.WriteByte(_serviceVersionAttributeType);
            writer.WriteUInt32(_serviceVersion);

            var data = writer.DetachBuffer();
            provider.SdpRawAttributes.Add(_serviceVersionAttributeId, data);
        }
    }
}
