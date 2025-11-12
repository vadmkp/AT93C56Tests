using System.Threading.Tasks;
using Windows.Foundation;

namespace Atmel.Services.Interfaces
{
    /// <summary>
    /// Interface for Arduino device control
    /// Windows Runtime Component compatible
    /// </summary>
    public interface IArduinoController
    {
        void SetPinState(byte pin, bool state);
        IAsyncOperation<bool> GetPinStateAsync(byte pin);
        bool IsReady { get; }
    }
}
