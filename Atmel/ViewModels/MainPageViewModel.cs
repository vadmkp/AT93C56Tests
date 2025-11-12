using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Enumeration;
using Atmel.Services.Interfaces;
using Atmel.Services.Models;
using Atmel.Services.Configuration;
using Atmel.ViewModels.Commands;

namespace Atmel.ViewModels
{
    /// <summary>
    /// ViewModel for MainPage - follows MVVM pattern with Prism principles
    /// Separates UI logic from business logic
    /// </summary>
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IBluetoothService _bluetoothService;
        private readonly IArduinoController _arduinoController;
        private readonly IDeviceDiscoveryService _discoveryService;
        private readonly BluetoothConfiguration _bluetoothConfig;
        private readonly ArduinoConfiguration _arduinoConfig;

        // Observable collections for data binding
        private ObservableCollection<BluetoothLEDeviceInfoModel> _devices;
        public ObservableCollection<BluetoothLEDeviceInfoModel> Devices
        {
            get => _devices;
            set => SetProperty(ref _devices, value);
        }

        private BluetoothLEDeviceInfoModel _selectedDevice;
        public BluetoothLEDeviceInfoModel SelectedDevice
        {
            get => _selectedDevice;
            set
            {
                SetProperty(ref _selectedDevice, value);
                ((RelayCommand)ConnectToDeviceCommand).RaiseCanExecuteChanged();
            }
        }

        // UI State Properties
        private bool _isLedOn;
        public bool IsLedOn
        {
            get => _isLedOn;
            set => SetProperty(ref _isLedOn, value);
        }

        private bool _isConnected;
        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                SetProperty(ref _isConnected, value);
                OnPropertyChanged(nameof(IsNotConnected));
                ((RelayCommand)TurnLedOnCommand).RaiseCanExecuteChanged();
                ((RelayCommand)TurnLedOffCommand).RaiseCanExecuteChanged();
                ((RelayCommand)DisconnectCommand).RaiseCanExecuteChanged();
            }
        }

        public bool IsNotConnected => !IsConnected;

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        // Commands (Prism DelegateCommand pattern)
        public ICommand LoadDevicesCommand { get; }
        public ICommand ConnectToDeviceCommand { get; }
        public ICommand DisconnectCommand { get; }
        public ICommand TurnLedOnCommand { get; }
        public ICommand TurnLedOffCommand { get; }
        public ICommand StartDeviceDiscoveryCommand { get; }
        public ICommand StartServerCommand { get; }
        public ICommand StartClientCommand { get; }

        public MainPageViewModel(
            IBluetoothService bluetoothService,
            IArduinoController arduinoController,
            IDeviceDiscoveryService discoveryService,
            BluetoothConfiguration bluetoothConfig,
            ArduinoConfiguration arduinoConfig)
        {
            _bluetoothService = bluetoothService ?? throw new ArgumentNullException(nameof(bluetoothService));
            _arduinoController = arduinoController ?? throw new ArgumentNullException(nameof(arduinoController));
            _discoveryService = discoveryService ?? throw new ArgumentNullException(nameof(discoveryService));
            _bluetoothConfig = bluetoothConfig ?? throw new ArgumentNullException(nameof(bluetoothConfig));
            _arduinoConfig = arduinoConfig ?? throw new ArgumentNullException(nameof(arduinoConfig));

            Title = "Atmel Bluetooth Controller";
            Devices = new ObservableCollection<BluetoothLEDeviceInfoModel>();

            // Initialize Commands
            LoadDevicesCommand = new RelayCommand(async () => await LoadDevicesAsync(), () => !IsBusy);
            ConnectToDeviceCommand = new RelayCommand(async () => await ConnectToDeviceAsync(), CanConnect);
            DisconnectCommand = new RelayCommand(async () => await DisconnectAsync(), () => IsConnected);
            TurnLedOnCommand = new RelayCommand(TurnLedOn, () => IsConnected && !IsLedOn);
            TurnLedOffCommand = new RelayCommand(TurnLedOff, () => IsConnected && IsLedOn);
            StartDeviceDiscoveryCommand = new RelayCommand(async () => await StartDeviceDiscoveryAsync());
            StartServerCommand = new RelayCommand(async () => await StartServerAsync());
            StartClientCommand = new RelayCommand(async () => await StartClientAsync());

            // Subscribe to service events
            InitializeEventHandlers();
        }

        private void InitializeEventHandlers()
        {
            _bluetoothService.ConnectionEstablished += OnConnectionEstablished;
            _bluetoothService.ConnectionLost += OnConnectionLost;
            _bluetoothService.ConnectionFailed += OnConnectionFailed;

            _discoveryService.DeviceAdded += OnDeviceAdded;
            _discoveryService.DeviceUpdated += OnDeviceUpdated;
            _discoveryService.DeviceRemoved += OnDeviceRemoved;
            _discoveryService.EnumerationCompleted += OnEnumerationCompleted;
        }

        private async Task LoadDevicesAsync()
        {
            try
            {
                IsBusy = true;
                StatusMessage = "Loading devices...";

                var devices = await _bluetoothService.GetAvailableDevicesAsync();
                
                await RunOnUiThreadAsync(() =>
                {
                    Devices.Clear();
                    foreach (var device in devices)
                    {
                        Devices.Add(device);
                    }
                });

                StatusMessage = $"Found {Devices.Count} devices";
                
                // Auto-select configured device
                var defaultDevice = Devices.FirstOrDefault(d => d.Name == _bluetoothConfig.DefaultDeviceName);
                if (defaultDevice != null)
                {
                    SelectedDevice = defaultDevice;
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading devices: {ex.Message}";
                Debug.WriteLine($"LoadDevicesAsync error: {ex}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanConnect()
        {
            return !IsBusy && !IsConnected && SelectedDevice != null;
        }

        private async Task ConnectToDeviceAsync()
        {
            if (SelectedDevice == null) return;

            try
            {
                IsBusy = true;
                StatusMessage = $"Connecting to {SelectedDevice.Name}...";

                var success = await _bluetoothService.ConnectAsync(SelectedDevice.Name);
                
                if (!success)
                {
                    StatusMessage = "Connection failed";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error connecting: {ex.Message}";
                Debug.WriteLine($"ConnectToDeviceAsync error: {ex}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DisconnectAsync()
        {
            try
            {
                IsBusy = true;
                StatusMessage = "Disconnecting...";

                await _bluetoothService.DisconnectAsync();
                IsConnected = false;
                StatusMessage = "Disconnected";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error disconnecting: {ex.Message}";
                Debug.WriteLine($"DisconnectAsync error: {ex}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void TurnLedOn()
        {
            try
            {
                _arduinoController.SetPinState(_arduinoConfig.LedPin, true);
                IsLedOn = true;
                StatusMessage = "LED turned ON";
                
                ((RelayCommand)TurnLedOnCommand).RaiseCanExecuteChanged();
                ((RelayCommand)TurnLedOffCommand).RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error turning LED on: {ex.Message}";
                Debug.WriteLine($"TurnLedOn error: {ex}");
            }
        }

        private void TurnLedOff()
        {
            try
            {
                _arduinoController.SetPinState(_arduinoConfig.LedPin, false);
                IsLedOn = false;
                StatusMessage = "LED turned OFF";
                
                ((RelayCommand)TurnLedOnCommand).RaiseCanExecuteChanged();
                ((RelayCommand)TurnLedOffCommand).RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error turning LED off: {ex.Message}";
                Debug.WriteLine($"TurnLedOff error: {ex}");
            }
        }

        private async Task StartDeviceDiscoveryAsync()
        {
            try
            {
                IsBusy = true;
                StatusMessage = "Starting device discovery...";
                await _discoveryService.StartDiscoveryAsync();
                StatusMessage = "Device discovery started";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error starting discovery: {ex.Message}";
                Debug.WriteLine($"StartDeviceDiscoveryAsync error: {ex}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task StartServerAsync()
        {
            try
            {
                IsBusy = true;
                StatusMessage = "Starting RFCOMM server...";
                
                var serverService = Infrastructure.ServiceContainer.Instance.Resolve<IRfcommService>("ServerRFCOMM");
                await serverService.InitializeAsync();
                await serverService.StartAsync();
                
                StatusMessage = "RFCOMM server started";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error starting server: {ex.Message}";
                Debug.WriteLine($"StartServerAsync error: {ex}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task StartClientAsync()
        {
            try
            {
                IsBusy = true;
                StatusMessage = "Starting RFCOMM client...";
                
                var clientService = Infrastructure.ServiceContainer.Instance.Resolve<IRfcommService>("ClientRFCOMM");
                await clientService.InitializeAsync();
                await clientService.StartAsync();
                
                StatusMessage = "RFCOMM client connected";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error starting client: {ex.Message}";
                Debug.WriteLine($"StartClientAsync error: {ex}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Event Handlers
        private void OnConnectionEstablished()
        {
            RunOnUiThreadAsync(() =>
            {
                IsConnected = true;
                StatusMessage = $"Connected to {SelectedDevice?.Name ?? "device"}";
            }).ConfigureAwait(false);
        }

        private void OnConnectionLost(string message)
        {
            RunOnUiThreadAsync(() =>
            {
                IsConnected = false;
                StatusMessage = $"Connection lost: {message}";
            }).ConfigureAwait(false);
        }

        private void OnConnectionFailed(string message)
        {
            RunOnUiThreadAsync(() =>
            {
                StatusMessage = $"Connection failed: {message}";
            }).ConfigureAwait(false);
        }

        private void OnDeviceAdded(object sender, DeviceInformation args)
        {
            Debug.WriteLine($"Device added: {args.Name}");
            RunOnUiThreadAsync(() =>
            {
                var device = new BluetoothLEDeviceInfoModel(args);
                if (!Devices.Any(d => d.Id == device.Id))
                {
                    Devices.Add(device);
                }
            }).ConfigureAwait(false);
        }

        private void OnDeviceUpdated(object sender, DeviceInformationUpdate args)
        {
            Debug.WriteLine($"Device updated: {args.Id}");
        }

        private void OnDeviceRemoved(object sender, DeviceInformationUpdate args)
        {
            Debug.WriteLine($"Device removed: {args.Id}");
            RunOnUiThreadAsync(() =>
            {
                var device = Devices.FirstOrDefault(d => d.Id == args.Id);
                if (device != null)
                {
                    Devices.Remove(device);
                }
            }).ConfigureAwait(false);
        }

        private void OnEnumerationCompleted(object sender, EventArgs e)
        {
            Debug.WriteLine("Device enumeration completed");
            RunOnUiThreadAsync(() =>
            {
                StatusMessage = $"Found {Devices.Count} devices";
            }).ConfigureAwait(false);
        }

        public override void OnNavigatedFrom()
        {
            // Cleanup
            _bluetoothService.ConnectionEstablished -= OnConnectionEstablished;
            _bluetoothService.ConnectionLost -= OnConnectionLost;
            _bluetoothService.ConnectionFailed -= OnConnectionFailed;

            _discoveryService.DeviceAdded -= OnDeviceAdded;
            _discoveryService.DeviceUpdated -= OnDeviceUpdated;
            _discoveryService.DeviceRemoved -= OnDeviceRemoved;
            _discoveryService.EnumerationCompleted -= OnEnumerationCompleted;
        }
    }
}
