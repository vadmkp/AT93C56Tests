using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;

namespace Atmel.Models
{
    public class BtLe
    {


        private void DeviceWatcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {
            Debug.WriteLine("EnumerationCompleted");
        }

        private void DeviceWatcher_Stopped(DeviceWatcher sender, object args)
        {
            Debug.WriteLine("Zatrzymano uzadzenie");
        }

        private void DeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            Debug.WriteLine("Usunieto uzadzenie");
        }

        private void DeviceWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            Debug.WriteLine("Uaktualniono uzadzenie");
        }

        private void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation args)
        {
            ConnectDevice(args);

            Debug.WriteLine(String.Format("Dodano urzadzenie: {0}", args.Id));
        }

        async void ConnectDevice(DeviceInformation deviceInfo)
        {
            // Note: BluetoothLEDevice.FromIdAsync must be called from a UI thread because it may prompt for consent.
            BluetoothLEDevice bluetoothLeDevice = await BluetoothLEDevice.FromIdAsync(deviceInfo.Id);
            // ...

            GattDeviceServicesResult result = await bluetoothLeDevice.GetGattServicesAsync();

            if (result.Status == GattCommunicationStatus.Success)
            {
                var services = result.Services;
                // ...
            }
        }
    }
}
