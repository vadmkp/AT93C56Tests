using System;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.Devices.Bluetooth;

namespace Atmel.Services.Helpers
{
    /// <summary>
    /// Validates RFCOMM service compatibility (Single Responsibility)
    /// </summary>
    public sealed class RfcommServiceValidator
    {
        private readonly uint _minimumServiceVersion;
        private readonly uint _serviceVersionAttributeId;
        private readonly byte _serviceVersionAttributeType;

        public RfcommServiceValidator(
            uint minimumServiceVersion,
            uint serviceVersionAttributeId,
            byte serviceVersionAttributeType)
        {
            _minimumServiceVersion = minimumServiceVersion;
            _serviceVersionAttributeId = serviceVersionAttributeId;
            _serviceVersionAttributeType = serviceVersionAttributeType;
        }

        public bool SupportsProtection(RfcommDeviceService service)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            switch (service.ProtectionLevel)
            {
                case SocketProtectionLevel.PlainSocket:
                    return (service.MaxProtectionLevel == SocketProtectionLevel.BluetoothEncryptionWithAuthentication)
                        || (service.MaxProtectionLevel == SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication);
                
                case SocketProtectionLevel.BluetoothEncryptionWithAuthentication:
                case SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication:
                    return true;
                
                default:
                    return false;
            }
        }

        public bool IsCompatibleVersion(RfcommDeviceService service)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            var attributes = service.GetSdpRawAttributesAsync(BluetoothCacheMode.Uncached).GetResults();
            
            if (!attributes.ContainsKey(_serviceVersionAttributeId))
                return false;

            var attribute = attributes[_serviceVersionAttributeId];
            var reader = DataReader.FromBuffer(attribute);

            byte attributeType = reader.ReadByte();
            if (attributeType == _serviceVersionAttributeType)
            {
                uint version = reader.ReadUInt32();
                return version >= _minimumServiceVersion;
            }

            return false;
        }
    }
}
