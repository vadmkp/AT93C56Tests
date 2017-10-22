using Atmel.Serial;
using Atmel.Silnik;
using Microsoft.Maker.RemoteWiring;
using Microsoft.Maker.Serial;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Atmel.Models;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Atmel
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private BluetoothSerial _bluetooth;

        private RemoteDevice _arduino;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void btnList_Click(object sender,
                                           RoutedEventArgs e)
        {
            var a = await BluetoothSerial.
                             listAvailableDevicesAsync();

            //var b = a.First();
            //var c = a.First(x => x.Name == "HC-05");
            if (a.Count < 1)
            {
                Debug.WriteLine("Bad pobierania listy");
                return;
            }
            var c = a.First(x => x.Name == "sowaphone");
            //var d = b.Name;
        }

        private void btnStart_Click(object sender,
                                           RoutedEventArgs e)
        {
            //_bluetooth = new BluetoothSerial("HC-05");
            _bluetooth = new BluetoothSerial("sowaphone");
            _arduino = new RemoteDevice(_bluetooth);
            _bluetooth.ConnectionLost +=
                         _bluetooth_ConnectionLost;
            _bluetooth.ConnectionFailed +=
                         _bluetooth_ConnectionFailed;
            _bluetooth.ConnectionEstablished +=
                         OnConnectionEstablished;
            _bluetooth.begin(0, SerialConfig.SERIAL_8N1);
        }

        private void _bluetooth_ConnectionLost(string message)
        {
            Debug.WriteLine(String.Format("Utracono płączenie z użądzeniem: {0}", message));
        }

        private void _bluetooth_ConnectionFailed(string message)
        {
            Debug.WriteLine(String.Format("Błąd połączenia z użądzeniem: {0}", message));
        }

        private void OnConnectionEstablished()
        {
            var action = Dispatcher.RunAsync(
                                CoreDispatcherPriority.Normal,
                                new DispatchedHandler(() =>
                                {
                                    btnOn.IsEnabled = true;
                                    btnOff.IsEnabled = false;
                                }));
        }

        private void btnOn_Click(object sender, RoutedEventArgs e)
        {
            //turn the LED, connected to pin 13, ON
            _arduino.digitalWrite(13, PinState.HIGH);

            var action = Dispatcher.RunAsync(
                               CoreDispatcherPriority.Normal,
                               new DispatchedHandler(() =>
                               {
                                   btnOn.IsEnabled = false;
                                   btnOff.IsEnabled = true;
                               }));
        }

        private void btnOff_Click(object sender, RoutedEventArgs e)
        {
            //turn the LED connected to pin 13 OFF
            _arduino.digitalWrite(13, PinState.LOW);

            var action = Dispatcher.RunAsync(
                               CoreDispatcherPriority.Normal,
                               new DispatchedHandler(() =>
                               {
                                   btnOn.IsEnabled = true;
                                   btnOff.IsEnabled = false;
                               }));
        }

        private void btnSerial01_Click(object sender, RoutedEventArgs e)
        {
            string deviceSelector2 = Windows.Devices.SerialCommunication.SerialDevice.GetDeviceSelectorFromUsbVidPid(
                                                                    ArduinoDevice.Vid, ArduinoDevice.Pid);
            //Atmel.Serial.DeviceListEntry listaModel = new Serial.DeviceListEntry(deviceSelector2, "Com01");
        }

        private async void btnBlueLE01_Click(object sender, RoutedEventArgs e)
        {
            // Query for extra properties you want returned
            string[] requestedProperties = { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected", "System.Devices.Aep.Bluetooth.Le.IsConnectable" };

            //string aqsAllBluetoothLEDevices = "(System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\")";
            string aqsAllBluetoothLEDevices = "(System.Devices.Aep.ProtocolId:=\"{E0CBF06C-CD8B-4647-BB8A-263B43F0F974}\")";

            DeviceWatcher deviceWatcher;
            //DeviceInformation.CreateWatcher(
            //        BluetoothLEDevice.GetDeviceSelectorFromPairingState(false),
            //        requestedProperties,
            //        DeviceInformationKind.AssociationEndpoint);

            deviceWatcher = DeviceInformation.CreateWatcher(
                aqsAllBluetoothLEDevices,
                requestedProperties,
                DeviceInformationKind.AssociationEndpoint
                );

            // Register event handlers before starting the watcher.
            // Added, Updated and Removed are required to get all nearby devices
            deviceWatcher.Added += DeviceWatcher_Added;
            deviceWatcher.Updated += DeviceWatcher_Updated;
            deviceWatcher.Removed += DeviceWatcher_Removed;

            // EnumerationCompleted and Stopped are optional to implement.
            deviceWatcher.EnumerationCompleted += DeviceWatcher_EnumerationCompleted;
            deviceWatcher.Stopped += DeviceWatcher_Stopped;

            // Start the watcher.
            deviceWatcher.Start();
        }

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

        private async void btnBlueLE02_Click(object sender, RoutedEventArgs e)
        {
            var selector = BluetoothDevice.GetDeviceSelector();
            //var con = selector.Where
            Debug.WriteLine(selector);
            var devices = await DeviceInformation.FindAllAsync(selector);
            if (devices.Count > 0)
            {
                Debug.WriteLine("Found {0} devices", devices.Count);
            }
            foreach (var item in devices)
            {
                var result = new BluetoothLEDeviceInfoModel(item);
                Debug.WriteLine(result.ToString());
            }
        }


        private void StartServer()
        {
            ServerRFCOMM _sr = new ServerRFCOMM();
            _sr.Initialize();
        }

        private void StartClient()
        {
            ClientRFCOMM _cl = new ClientRFCOMM();
            _cl.Initialize();
        }

        private void BtnBlueLE03_Click(object sender, RoutedEventArgs e)
        {
            StartServer();
        }

        private void BtnBlueLE04_Click(object sender, RoutedEventArgs e)
        {
            StartClient();
        }
    }
}
