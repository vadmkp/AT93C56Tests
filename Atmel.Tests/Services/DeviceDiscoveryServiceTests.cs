using Atmel.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Atmel.Tests.Services;

/// <summary>
/// Unit tests for Device Discovery Service functionality
/// Tests Bluetooth device scanning and enumeration patterns
/// </summary>
[TestClass]
public class DeviceDiscoveryServiceTests
{
    private MockDeviceDiscoveryService? _service;

    [TestInitialize]
    public void Setup()
    {
        _service = new MockDeviceDiscoveryService();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _service = null;
    }

    [TestMethod]
    public void Constructor_ShouldCreateNotDiscoveringService()
    {
        // Arrange & Act
        var service = new MockDeviceDiscoveryService();

        // Assert
        Assert.IsNotNull(service);
        Assert.IsFalse(service.IsDiscovering);
        Assert.IsFalse(service.StartDiscoveryAsyncCalled);
        Assert.IsFalse(service.StopDiscoveryAsyncCalled);
    }

    [TestMethod]
    public async Task StartDiscoveryAsync_ShouldStartDiscovery()
    {
        // Act
        await _service!.StartDiscoveryAsync();

        // Assert
        Assert.IsTrue(_service.IsDiscovering);
        Assert.IsTrue(_service.StartDiscoveryAsyncCalled);
    }

    [TestMethod]
    public async Task StartDiscoveryAsync_ShouldRaiseEnumerationCompletedEvent()
    {
        // Arrange
        bool eventRaised = false;
        _service!.EnumerationCompleted += (sender, args) => eventRaised = true;

        // Act
        await _service.StartDiscoveryAsync();
        await Task.Delay(200); // Wait for async event

        // Assert
        Assert.IsTrue(eventRaised);
    }

    [TestMethod]
    public async Task StopDiscoveryAsync_ShouldStopDiscovery()
    {
        // Arrange
        await _service!.StartDiscoveryAsync();

        // Act
        await _service.StopDiscoveryAsync();

        // Assert
        Assert.IsFalse(_service.IsDiscovering);
        Assert.IsTrue(_service.StopDiscoveryAsyncCalled);
    }

    [TestMethod]
    public async Task StopDiscoveryAsync_WhenNotDiscovering_ShouldNotThrow()
    {
        // Act & Assert (should not throw)
        await _service!.StopDiscoveryAsync();
        
        Assert.IsFalse(_service.IsDiscovering);
        Assert.IsTrue(_service.StopDiscoveryAsyncCalled);
    }

    [TestMethod]
    public async Task StartStopCycle_ShouldWorkCorrectly()
    {
        // Act
        await _service!.StartDiscoveryAsync();
        Assert.IsTrue(_service.IsDiscovering);

        await _service.StopDiscoveryAsync();
        Assert.IsFalse(_service.IsDiscovering);

        await _service.StartDiscoveryAsync();
        Assert.IsTrue(_service.IsDiscovering);

        // Assert
        Assert.IsTrue(_service.StartDiscoveryAsyncCalled);
        Assert.IsTrue(_service.StopDiscoveryAsyncCalled);
    }

    [TestMethod]
    public void Reset_ShouldClearAllFlags()
    {
        // Arrange
        _service!.StartDiscoveryAsync().Wait();
        _service.StopDiscoveryAsync().Wait();

        // Act
        _service.Reset();

        // Assert
        Assert.IsFalse(_service.IsDiscovering);
        Assert.IsFalse(_service.StartDiscoveryAsyncCalled);
        Assert.IsFalse(_service.StopDiscoveryAsyncCalled);
    }

    [TestMethod]
    public async Task EnumerationCompleted_ShouldFireOnlyAfterStart()
    {
        // Arrange
        int eventCount = 0;
        _service!.EnumerationCompleted += (sender, args) => eventCount++;

        // Act
        await _service.StopDiscoveryAsync(); // Should not raise event
        await Task.Delay(200);

        // Assert
        Assert.AreEqual(0, eventCount);
    }

    [TestMethod]
    public async Task EnumerationCompleted_ShouldFireAfterEachStart()
    {
        // Arrange
        int eventCount = 0;
        _service!.EnumerationCompleted += (sender, args) => eventCount++;

        // Act
        await _service.StartDiscoveryAsync();
        await Task.Delay(200);
        
        await _service.StopDiscoveryAsync();
        await _service.StartDiscoveryAsync();
        await Task.Delay(200);

        // Assert
        Assert.AreEqual(2, eventCount);
    }
}
