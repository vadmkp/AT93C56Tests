using System;
using System.Collections.Generic;
using Atmel.Services.Interfaces;
using Atmel.Services.Implementation;
using Atmel.Services.Configuration;
using Atmel.Services.Rfcomm;
using Atmel.ViewModels;

namespace Atmel.Infrastructure
{
    /// <summary>
    /// Simple IoC container for dependency injection
    /// Implements Dependency Inversion Principle
    /// Now includes ViewModels following MVVM pattern
    /// </summary>
    public class ServiceContainer
    {
        private static ServiceContainer _instance;
        private readonly Dictionary<Type, object> _services;
        private readonly Dictionary<string, object> _namedServices;

        private ServiceContainer()
        {
            _services = new Dictionary<Type, object>();
            _namedServices = new Dictionary<string, object>();
            RegisterServices();
        }

        public static ServiceContainer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServiceContainer();
                }
                return _instance;
            }
        }

        private void RegisterServices()
        {
            // Register configurations
            var bluetoothConfig = new BluetoothConfiguration();
            var arduinoConfig = new ArduinoConfiguration();
            var rfcommConfig = new RfcommConfiguration();

            Register(bluetoothConfig);
            Register(arduinoConfig);
            Register(rfcommConfig);

            // Register services with interfaces (Dependency Inversion Principle)
            Register<IBluetoothService>(new BluetoothDiscoveryService(bluetoothConfig));
            Register<IArduinoController>(new ArduinoController(arduinoConfig));
            Register<IDeviceDiscoveryService>(new BluetoothLEDiscoveryService());
            
            // Register RFCOMM services with names
            RegisterNamed<IRfcommService>("ServerRFCOMM", new ServerRFCOMM(rfcommConfig));
            RegisterNamed<IRfcommService>("ClientRFCOMM", new ClientRFCOMM(rfcommConfig));

            // Register ViewModels (MVVM pattern)
            RegisterViewModel<MainPageViewModel>();
        }

        private void RegisterViewModel<TViewModel>() where TViewModel : ViewModelBase
        {
            // ViewModels are created on-demand with dependency resolution
            _services[typeof(TViewModel)] = null; // Placeholder for lazy creation
        }

        public void Register<T>(T service)
        {
            _services[typeof(T)] = service;
        }

        public void RegisterNamed<T>(string name, T service)
        {
            var key = $"{typeof(T).FullName}_{name}";
            _namedServices[key] = service;
        }

        public T Resolve<T>()
        {
            var type = typeof(T);
            
            if (_services.ContainsKey(type))
            {
                var service = _services[type];
                
                // Lazy creation for ViewModels
                if (service == null && typeof(ViewModelBase).IsAssignableFrom(type))
                {
                    service = CreateViewModel<T>();
                    _services[type] = service;
                }
                
                return (T)service;
            }
            
            throw new InvalidOperationException($"Service of type {type.Name} is not registered");
        }

        public T Resolve<T>(string name)
        {
            var key = $"{typeof(T).FullName}_{name}";
            
            if (_namedServices.ContainsKey(key))
            {
                return (T)_namedServices[key];
            }
            
            throw new InvalidOperationException($"Service of type {typeof(T).Name} with name '{name}' is not registered");
        }

        private T CreateViewModel<T>()
        {
            var type = typeof(T);
            
            // MainPageViewModel requires specific dependencies
            if (type == typeof(MainPageViewModel))
            {
                return (T)(object)new MainPageViewModel(
                    Resolve<IBluetoothService>(),
                    Resolve<IArduinoController>(),
                    Resolve<IDeviceDiscoveryService>(),
                    Resolve<BluetoothConfiguration>(),
                    Resolve<ArduinoConfiguration>()
                );
            }
            
            throw new InvalidOperationException($"Don't know how to create ViewModel of type {type.Name}");
        }

        /// <summary>
        /// Reset the container (useful for testing)
        /// </summary>
        public static void Reset()
        {
            _instance = null;
        }
    }
}
