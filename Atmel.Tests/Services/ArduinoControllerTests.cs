using Atmel.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Atmel.Tests.Services;

/// <summary>
/// Unit tests for Arduino Controller functionality
/// Tests the mock implementation to verify GPIO control patterns
/// </summary>
[TestClass]
public class ArduinoControllerTests
{
    private MockArduinoController? _controller;

    [TestInitialize]
    public void Setup()
    {
        _controller = new MockArduinoController();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _controller = null;
    }

    [TestMethod]
    public void Constructor_ShouldCreateNotReadyController()
    {
        // Arrange & Act
        var controller = new MockArduinoController();

        // Assert
        Assert.IsNotNull(controller);
        Assert.IsFalse(controller.IsReady);
        Assert.AreEqual(0, controller.SetPinStateCallCount);
    }

    [TestMethod]
    public void Initialize_ShouldSetControllerToReady()
    {
        // Act
        _controller!.Initialize();

        // Assert
        Assert.IsTrue(_controller.IsReady);
    }

    [TestMethod]
    public void SetPinState_WhenNotReady_ShouldThrowException()
    {
        // Arrange
        byte pin = 13;
        bool state = true;

        // Act & Assert
        Assert.ThrowsException<InvalidOperationException>(() => 
            _controller!.SetPinState(pin, state)
        );
    }

    [TestMethod]
    public void SetPinState_WhenReady_ShouldSetState()
    {
        // Arrange
        _controller!.Initialize();
        byte pin = 13;
        bool state = true;

        // Act
        _controller.SetPinState(pin, state);

        // Assert
        Assert.AreEqual(pin, _controller.LastPin);
        Assert.AreEqual(state, _controller.LastState);
        Assert.AreEqual(1, _controller.SetPinStateCallCount);
    }

    [TestMethod]
    public void SetPinState_MultipleCallsToSamePin_ShouldUpdateState()
    {
        // Arrange
        _controller!.Initialize();
        byte pin = 13;

        // Act
        _controller.SetPinState(pin, true);
        _controller.SetPinState(pin, false);
        _controller.SetPinState(pin, true);

        // Assert
        Assert.AreEqual(pin, _controller.LastPin);
        Assert.AreEqual(true, _controller.LastState);
        Assert.AreEqual(3, _controller.SetPinStateCallCount);
    }

    [TestMethod]
    public void SetPinState_MultiplePins_ShouldTrackLastPin()
    {
        // Arrange
        _controller!.Initialize();

        // Act
        _controller.SetPinState(13, true);
        _controller.SetPinState(12, false);
        _controller.SetPinState(11, true);

        // Assert
        Assert.AreEqual(11, _controller.LastPin);
        Assert.AreEqual(true, _controller.LastState);
        Assert.AreEqual(3, _controller.SetPinStateCallCount);
    }

    [TestMethod]
    public async Task GetPinStateAsync_WhenNotReady_ShouldThrowException()
    {
        // Arrange
        byte pin = 13;

        // Act & Assert
        await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => 
            await _controller!.GetPinStateAsync(pin)
        );
    }

    [TestMethod]
    public async Task GetPinStateAsync_AfterSetPinState_ShouldReturnCorrectState()
    {
        // Arrange
        _controller!.Initialize();
        byte pin = 13;
        _controller.SetPinState(pin, true);

        // Act
        var state = await _controller.GetPinStateAsync(pin);

        // Assert
        Assert.IsTrue(state);
    }

    [TestMethod]
    public async Task GetPinStateAsync_ForUnsetPin_ShouldReturnFalse()
    {
        // Arrange
        _controller!.Initialize();
        byte pin = 13;

        // Act
        var state = await _controller.GetPinStateAsync(pin);

        // Assert
        Assert.IsFalse(state);
    }

    [TestMethod]
    public async Task GetPinStateAsync_AfterToggle_ShouldReturnLatestState()
    {
        // Arrange
        _controller!.Initialize();
        byte pin = 13;
        _controller.SetPinState(pin, true);
        _controller.SetPinState(pin, false);

        // Act
        var state = await _controller.GetPinStateAsync(pin);

        // Assert
        Assert.IsFalse(state);
    }

    [TestMethod]
    public async Task GetPinStateAsync_MultiplePins_ShouldReturnIndependentStates()
    {
        // Arrange
        _controller!.Initialize();
        _controller.SetPinState(13, true);
        _controller.SetPinState(12, false);
        _controller.SetPinState(11, true);

        // Act
        var state13 = await _controller.GetPinStateAsync(13);
        var state12 = await _controller.GetPinStateAsync(12);
        var state11 = await _controller.GetPinStateAsync(11);

        // Assert
        Assert.IsTrue(state13);
        Assert.IsFalse(state12);
        Assert.IsTrue(state11);
    }

    [TestMethod]
    public void Reset_ShouldClearAllStates()
    {
        // Arrange
        _controller!.Initialize();
        _controller.SetPinState(13, true);
        _controller.SetPinState(12, false);

        // Act
        _controller.Reset();

        // Assert
        Assert.AreEqual(0, _controller.SetPinStateCallCount);
        Assert.AreEqual(0, _controller.LastPin);
        Assert.AreEqual(false, _controller.LastState);
    }

    [TestMethod]
    public async Task Reset_ShouldClearPinStates()
    {
        // Arrange
        _controller!.Initialize();
        _controller.SetPinState(13, true);
        _controller.Reset();

        // Act
        var state = await _controller.GetPinStateAsync(13);

        // Assert
        Assert.IsFalse(state);
    }

    [TestMethod]
    public void SetPinState_WithLEDPin_ShouldWork()
    {
        // Arrange
        _controller!.Initialize();
        byte ledPin = 13; // Standard Arduino LED pin
        var mockConfig = new MockArduinoConfiguration();

        // Act
        _controller.SetPinState(mockConfig.LedPin, true);

        // Assert
        Assert.AreEqual(ledPin, _controller.LastPin);
        Assert.AreEqual(true, _controller.LastState);
    }

    [TestMethod]
    public void SetPinState_ToggleLED_ShouldUpdateCorrectly()
    {
        // Arrange
        _controller!.Initialize();
        byte pin = 13;

        // Act - Simulate LED blinking
        for (int i = 0; i < 5; i++)
        {
            _controller.SetPinState(pin, true);
            _controller.SetPinState(pin, false);
        }

        // Assert
        Assert.AreEqual(10, _controller.SetPinStateCallCount);
        Assert.AreEqual(false, _controller.LastState); // Last state should be OFF
    }

    [TestMethod]
    public async Task FullLifecycle_ShouldWorkCorrectly()
    {
        // Arrange
        byte pin = 13;

        // Act & Assert
        
        // 1. Initial state
        Assert.IsFalse(_controller!.IsReady);

        // 2. Initialize
        _controller.Initialize();
        Assert.IsTrue(_controller.IsReady);

        // 3. Set pin HIGH
        _controller.SetPinState(pin, true);
        var stateHigh = await _controller.GetPinStateAsync(pin);
        Assert.IsTrue(stateHigh);

        // 4. Set pin LOW
        _controller.SetPinState(pin, false);
        var stateLow = await _controller.GetPinStateAsync(pin);
        Assert.IsFalse(stateLow);

        // 5. Reset
        _controller.Reset();
        Assert.AreEqual(0, _controller.SetPinStateCallCount);
    }
}
