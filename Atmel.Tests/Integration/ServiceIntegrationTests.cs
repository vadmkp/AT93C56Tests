using Atmel.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atmel.Tests.Integration;

/// <summary>
/// Integration tests combining multiple services
/// Tests realistic usage scenarios
/// </summary>
[TestClass]
public class ServiceIntegrationTests
{
    private MockBluetoothService? _bluetoothService;
    private MockArduinoController? _arduinoController;
    private MockDeviceDiscoveryService? _discoveryService;

    [TestInitialize]
    public void Setup()
    {
        _bluetoothService = new MockBluetoothService();
        _arduinoController = new MockArduinoController();
        _discoveryService = new MockDeviceDiscoveryService();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _bluetoothService = null;
        _arduinoController = null;
        _discoveryService = null;
    }

    [TestMethod]
    public async Task FullWorkflow_DiscoverConnectControl_ShouldWorkCorrectly()
    {
        // Arrange
        var config = new MockArduinoConfiguration();

        // Act & Assert

        // 1. Start device discovery
        await _discoveryService!.StartDiscoveryAsync();
        Assert.IsTrue(_discoveryService.IsDiscovering);

        // 2. Get available devices
        var devices = await _bluetoothService!.GetAvailableDevicesAsync();
        Assert.IsTrue(devices.Any());

        // 3. Connect to device
        var targetDevice = devices.First();
        var connected = await _bluetoothService.ConnectAsync(targetDevice.Name);
        Assert.IsTrue(connected);
        Assert.IsTrue(_bluetoothService.IsConnected);

        // 4. Initialize Arduino controller
        _arduinoController!.Initialize();
        Assert.IsTrue(_arduinoController.IsReady);

        // 5. Control LED
        _arduinoController.SetPinState(config.LedPin, true);
        var ledState = await _arduinoController.GetPinStateAsync(config.LedPin);
        Assert.IsTrue(ledState);

        // 6. Stop discovery
        await _discoveryService.StopDiscoveryAsync();
        Assert.IsFalse(_discoveryService.IsDiscovering);

        // 7. Disconnect
        await _bluetoothService.DisconnectAsync();
        Assert.IsFalse(_bluetoothService.IsConnected);
    }

    [TestMethod]
    public async Task LedBlinkScenario_ShouldWorkCorrectly()
    {
        // Arrange
        await _bluetoothService!.ConnectAsync("TestDevice");
        _arduinoController!.Initialize();
        byte ledPin = 13;

        // Act - Simulate LED blinking 3 times
        for (int i = 0; i < 3; i++)
        {
            _arduinoController.SetPinState(ledPin, true);
            await Task.Delay(10); // Simulate delay
            _arduinoController.SetPinState(ledPin, false);
            await Task.Delay(10);
        }

        // Assert
        Assert.AreEqual(6, _arduinoController.SetPinStateCallCount); // 3 ON + 3 OFF
        Assert.AreEqual(false, _arduinoController.LastState); // Last state should be OFF
    }

    [TestMethod]
    public async Task MultipleDeviceSwitch_ShouldHandleCorrectly()
    {
        // Act
        
        // Connect to first device
        await _bluetoothService!.ConnectAsync("TestDevice");
        Assert.IsTrue(_bluetoothService.IsConnected);
        Assert.AreEqual("TestDevice", _bluetoothService.LastConnectedDevice);

        // Disconnect
        await _bluetoothService.DisconnectAsync();
        Assert.IsFalse(_bluetoothService.IsConnected);

        // Connect to second device
        await _bluetoothService.ConnectAsync("HC-05");
        Assert.IsTrue(_bluetoothService.IsConnected);
        Assert.AreEqual("HC-05", _bluetoothService.LastConnectedDevice);

        // Assert
        Assert.IsTrue(_bluetoothService.ConnectAsyncCalled);
        Assert.IsTrue(_bluetoothService.DisconnectAsyncCalled);
    }

    [TestMethod]
    public async Task ConnectionFailure_ShouldPreventArduinoControl()
    {
        // Arrange
        bool connectionFailed = false;
        _bluetoothService!.ConnectionFailed += (msg) => connectionFailed = true;

        // Act
        var connected = await _bluetoothService.ConnectAsync("InvalidDevice");

        // Assert
        Assert.IsFalse(connected);
        Assert.IsTrue(connectionFailed);
        Assert.IsFalse(_bluetoothService.IsConnected);

        // Verify Arduino controller is not initialized
        Assert.IsFalse(_arduinoController!.IsReady);
    }

    [TestMethod]
    public async Task ArduinoNotInitialized_ShouldThrowException()
    {
        // Arrange
        await _bluetoothService!.ConnectAsync("TestDevice");
        // Note: NOT initializing Arduino controller

        // Act & Assert
        Assert.ThrowsException<InvalidOperationException>(() =>
            _arduinoController!.SetPinState(13, true)
        );
    }

    [TestMethod]
    public async Task DiscoveryShouldNotAffectExistingConnection()
    {
        // Arrange
        await _bluetoothService!.ConnectAsync("TestDevice");
        Assert.IsTrue(_bluetoothService.IsConnected);

        // Act
        await _discoveryService!.StartDiscoveryAsync();
        await Task.Delay(200);
        await _discoveryService.StopDiscoveryAsync();

        // Assert
        Assert.IsTrue(_bluetoothService.IsConnected); // Connection should remain
    }

    [TestMethod]
    public async Task MultipleServiceReset_ShouldWorkCorrectly()
    {
        // Arrange
        await _bluetoothService!.ConnectAsync("TestDevice");
        _arduinoController!.Initialize();
        _arduinoController.SetPinState(13, true);
        await _discoveryService!.StartDiscoveryAsync();

        // Act
        await _bluetoothService.DisconnectAsync();
        _arduinoController.Reset();
        _discoveryService.Reset();

        // Assert
        Assert.IsFalse(_bluetoothService.IsConnected);
        Assert.AreEqual(0, _arduinoController.SetPinStateCallCount);
        Assert.IsFalse(_discoveryService.IsDiscovering);
    }

    [TestMethod]
    public async Task EventSequence_ShouldOccurInCorrectOrder()
    {
        // Arrange
        var events = new List<string>();
        _bluetoothService!.ConnectionEstablished += () => events.Add("BT_Connected");
        _bluetoothService.ConnectionLost += (msg) => events.Add("BT_Disconnected");
        _discoveryService!.EnumerationCompleted += (s, e) => events.Add("Discovery_Complete");

        // Act
        await _discoveryService.StartDiscoveryAsync();
        await _bluetoothService.ConnectAsync("TestDevice");
        await Task.Delay(200); // Wait for discovery completion event
        await _bluetoothService.DisconnectAsync();

        // Assert
        Assert.IsTrue(events.Contains("BT_Connected"));
        Assert.IsTrue(events.Contains("BT_Disconnected"));
        Assert.IsTrue(events.Contains("Discovery_Complete"));
        Assert.AreEqual(3, events.Count);
    }

    [TestMethod]
    public async Task Configuration_ShouldAffectServiceBehavior()
    {
        // Arrange
        var btConfig = new MockBluetoothConfiguration 
        { 
            DefaultDeviceName = "TestDevice",
            ConnectionTimeoutMs = 1000
        };
        
        var arduinoConfig = new MockArduinoConfiguration 
        { 
            LedPin = 13,
            CommandDelayMs = 100
        };

        // Act
        await _bluetoothService!.ConnectAsync(btConfig.DefaultDeviceName);
        _arduinoController!.Initialize();
        _arduinoController.SetPinState(arduinoConfig.LedPin, true);

        // Assert
        Assert.IsTrue(_bluetoothService.IsConnected);
        Assert.AreEqual(btConfig.DefaultDeviceName, _bluetoothService.LastConnectedDevice);
        Assert.AreEqual(arduinoConfig.LedPin, _arduinoController.LastPin);
    }
}
