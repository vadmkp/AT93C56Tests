using Windows.Foundation;

namespace Atmel.Services.Interfaces
{
    /// <summary>
    /// Interface for RFCOMM communication
    /// Windows Runtime Component compatible
    /// </summary>
    public interface IRfcommService
    {
        IAsyncAction InitializeAsync();
        IAsyncAction StartAsync();
        IAsyncAction StopAsync();
        bool IsRunning { get; }
    }
}
