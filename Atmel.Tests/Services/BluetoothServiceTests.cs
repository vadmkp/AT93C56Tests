using Atmel.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atmel.Tests.Services;

/// <summary>
/// Unit tests for Bluetooth Service functionality
/// Tests the mock implementation to verify correct behavior patterns
/// </summary>
[TestClass]
public class BluetoothServiceTests
{
    private MockBluetoothService? _service;

    [TestInitialize]
    public void Setup()
    {
        _service = new MockBluetoothService();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _service = null;
    }

    [TestMethod]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Arrange & Act
        var service = new MockBluetoothService();

        // Assert
        Assert.IsNotNull(service);
        Assert.IsFalse(service.IsConnected);
        Assert.IsFalse(service.ConnectAsyncCalled);
    }

    [TestMethod]
    public async Task GetAvailableDevicesAsync_ShouldReturnDevices()
    {
        // Act
        var devices = await _service!.GetAvailableDevicesAsync();

        // Assert
        Assert.IsNotNull(devices);
        Assert.IsTrue(devices.Any());
        Assert.AreEqual(3, devices.Count());
    }

    [TestMethod]
    public async Task GetAvailableDevicesAsync_ShouldContainTestDevice()
    {
        // Act
        var devices = await _service!.GetAvailableDevicesAsync();

        // Assert
        var testDevice = devices.FirstOrDefault(d => d.Name == "TestDevice");
        Assert.IsNotNull(testDevice);
        Assert.AreEqual("device-1", testDevice.Id);
        Assert.IsTrue(testDevice.IsEnabled);
        Assert.IsTrue(testDevice.IsPaired);
    }

    [TestMethod]
    public async Task ConnectAsync_WithValidDevice_ShouldSucceed()
    {
        // Arrange
        string deviceName = "TestDevice";
        bool eventRaised = false;
        _service!.ConnectionEstablished += () => eventRaised = true;

        // Act
        var result = await _service.ConnectAsync(deviceName);

        // Assert
        Assert.IsTrue(result);
        Assert.IsTrue(_service.IsConnected);
        Assert.IsTrue(_service.ConnectAsyncCalled);
        Assert.AreEqual(deviceName, _service.LastConnectedDevice);
        Assert.IsTrue(eventRaised);
    }

    [TestMethod]
    public async Task ConnectAsync_WithInvalidDevice_ShouldFail()
    {
        // Arrange
        string deviceName = "NonExistentDevice";
        string? failureMessage = null;
        _service!.ConnectionFailed += (msg) => failureMessage = msg;

        // Act
        var result = await _service.ConnectAsync(deviceName);

        // Assert
        Assert.IsFalse(result);
        Assert.IsFalse(_service.IsConnected);
        Assert.IsNotNull(failureMessage);
        Assert.IsTrue(failureMessage.Contains(deviceName));
    }

    [TestMethod]
    public async Task ConnectAsync_WithDisabledDevice_ShouldFail()
    {
        // Arrange
        var disabledDevice = new MockBluetoothDevice("device-4", "DisabledDevice", false);
        _service!.AddDevice(disabledDevice);
        string? failureMessage = null;
        _service.ConnectionFailed += (msg) => failureMessage = msg;

        // Act
        var result = await _service.ConnectAsync("DisabledDevice");

        // Assert
        Assert.IsFalse(result);
        Assert.IsFalse(_service.IsConnected);
        Assert.IsNotNull(failureMessage);
        Assert.IsTrue(failureMessage.Contains("disabled"));
    }

    [TestMethod]
    public async Task DisconnectAsync_WhenConnected_ShouldDisconnect()
    {
        // Arrange
        await _service!.ConnectAsync("TestDevice");
        string? lostMessage = null;
        _service.ConnectionLost += (msg) => lostMessage = msg;

        // Act
        await _service.DisconnectAsync();

        // Assert
        Assert.IsFalse(_service.IsConnected);
        Assert.IsTrue(_service.DisconnectAsyncCalled);
        Assert.IsNotNull(lostMessage);
        Assert.IsTrue(lostMessage.Contains("Disconnected by user"));
    }

    [TestMethod]
    public async Task DisconnectAsync_WhenNotConnected_ShouldNotRaiseEvent()
    {
        // Arrange
        bool eventRaised = false;
        _service!.ConnectionLost += (_) => eventRaised = true;

        // Act
        await _service.DisconnectAsync();

        // Assert
        Assert.IsFalse(_service.IsConnected);
        Assert.IsTrue(_service.DisconnectAsyncCalled);
        Assert.IsFalse(eventRaised);
    }

    [TestMethod]
    public async Task ConnectAsync_MultipleDevices_ShouldTrackLastConnected()
    {
        // Act
        await _service!.ConnectAsync("TestDevice");
        await _service.DisconnectAsync();
        await _service.ConnectAsync("HC-05");

        // Assert
        Assert.AreEqual("HC-05", _service.LastConnectedDevice);
        Assert.IsTrue(_service.IsConnected);
    }

    [TestMethod]
    public void AddDevice_ShouldIncreaseDeviceCount()
    {
        // Arrange
        var newDevice = new MockBluetoothDevice("device-99", "NewDevice");

        // Act
        _service!.AddDevice(newDevice);
        var devices = _service.GetAvailableDevicesAsync().Result;

        // Assert
        Assert.AreEqual(4, devices.Count());
        Assert.IsTrue(devices.Any(d => d.Name == "NewDevice"));
    }

    [TestMethod]
    public void ClearDevices_ShouldRemoveAllDevices()
    {
        // Act
        _service!.ClearDevices();
        var devices = _service.GetAvailableDevicesAsync().Result;

        // Assert
        Assert.AreEqual(0, devices.Count());
    }

    [TestMethod]
    public async Task ConnectionEvents_ShouldFireInCorrectOrder()
    {
        // Arrange
        var events = new List<string>();
        _service!.ConnectionEstablished += () => events.Add("Established");
        _service.ConnectionLost += (msg) => events.Add("Lost");
        _service.ConnectionFailed += (msg) => events.Add("Failed");

        // Act
        await _service.ConnectAsync("TestDevice");
        await _service.DisconnectAsync();

        // Assert
        Assert.AreEqual(2, events.Count);
        Assert.AreEqual("Established", events[0]);
        Assert.AreEqual("Lost", events[1]);
    }
}
