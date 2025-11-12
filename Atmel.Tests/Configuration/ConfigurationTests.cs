using Atmel.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atmel.Tests.Configuration;

/// <summary>
/// Unit tests for Configuration classes
/// Tests Bluetooth, Arduino, and RFCOMM configuration settings
/// </summary>
[TestClass]
public class ConfigurationTests
{
    [TestMethod]
    public void BluetoothConfiguration_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var config = new MockBluetoothConfiguration();

        // Assert
        Assert.AreEqual("TestDevice", config.DefaultDeviceName);
        Assert.AreEqual("HC-05", config.AlternativeDeviceName);
        Assert.AreEqual(5000, config.ConnectionTimeoutMs);
    }

    [TestMethod]
    public void BluetoothConfiguration_ShouldAllowCustomValues()
    {
        // Arrange & Act
        var config = new MockBluetoothConfiguration
        {
            DefaultDeviceName = "CustomDevice",
            AlternativeDeviceName = "HC-06",
            ConnectionTimeoutMs = 10000
        };

        // Assert
        Assert.AreEqual("CustomDevice", config.DefaultDeviceName);
        Assert.AreEqual("HC-06", config.AlternativeDeviceName);
        Assert.AreEqual(10000, config.ConnectionTimeoutMs);
    }

    [TestMethod]
    public void ArduinoConfiguration_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var config = new MockArduinoConfiguration();

        // Assert
        Assert.AreEqual((byte)13, config.LedPin);
        Assert.AreEqual(100, config.CommandDelayMs);
    }

    [TestMethod]
    public void ArduinoConfiguration_ShouldAllowCustomValues()
    {
        // Arrange & Act
        var config = new MockArduinoConfiguration
        {
            LedPin = 12,
            CommandDelayMs = 200
        };

        // Assert
        Assert.AreEqual((byte)12, config.LedPin);
        Assert.AreEqual(200, config.CommandDelayMs);
    }

    [TestMethod]
    public void ArduinoConfiguration_LedPin_ShouldAcceptValidPinNumbers()
    {
        // Arrange
        var config = new MockArduinoConfiguration();
        byte[] validPins = { 0, 1, 7, 13, 51, 53 }; // Common Arduino pins

        // Act & Assert
        foreach (var pin in validPins)
        {
            config.LedPin = pin;
            Assert.AreEqual(pin, config.LedPin);
        }
    }

    [TestMethod]
    public void BluetoothConfiguration_Timeout_ShouldAcceptPositiveValues()
    {
        // Arrange
        var config = new MockBluetoothConfiguration();
        int[] validTimeouts = { 100, 1000, 5000, 10000, 30000 };

        // Act & Assert
        foreach (var timeout in validTimeouts)
        {
            config.ConnectionTimeoutMs = timeout;
            Assert.AreEqual(timeout, config.ConnectionTimeoutMs);
        }
    }

    [TestMethod]
    public void BluetoothConfiguration_DeviceName_ShouldAcceptEmptyString()
    {
        // Arrange
        var config = new MockBluetoothConfiguration();

        // Act
        config.DefaultDeviceName = "";

        // Assert
        Assert.AreEqual("", config.DefaultDeviceName);
    }
}
