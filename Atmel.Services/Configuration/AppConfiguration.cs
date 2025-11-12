namespace Atmel.Services.Configuration
{
    /// <summary>
    /// Configuration settings for Bluetooth connections
    /// </summary>
    public sealed class BluetoothConfiguration
    {
        public string DefaultDeviceName { get; set; } = "sowaphone";
        public string AlternativeDeviceName { get; set; } = "HC-05";
        public int ConnectionTimeoutMs { get; set; } = 5000;
    }

    /// <summary>
    /// Configuration settings for Arduino control
    /// </summary>
    public sealed class ArduinoConfiguration
    {
        public byte LedPin { get; set; } = 13;
        public int CommandDelayMs { get; set; } = 100;
    }

    /// <summary>
    /// Configuration settings for RFCOMM services
    /// </summary>
    public sealed class RfcommConfiguration
    {
        public uint ServiceVersion { get; set; } = 200;
        public uint MinimumServiceVersion { get; set; } = 200;
        public uint ServiceVersionAttributeId { get; set; } = 0x0300;
        public byte ServiceVersionAttributeType { get; set; } = 0x0A;
    }
}
