using Atmel.Serial;
using Atmel.Services.Rfcomm;
using Atmel.Services.Models;
using Atmel.Services.Interfaces;
using Atmel.Services.Configuration;
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
using Atmel.Infrastructure;
using Atmel.ViewModels;

namespace Atmel
{
    /// <summary>
    /// Main page - MVVM implementation
    /// Code-behind is minimal, all logic is in ViewModel
    /// Follows Prism MVVM principles
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // ViewModel (MVVM pattern)
        public MainPageViewModel ViewModel { get; private set; }

        public MainPage()
        {
            this.InitializeComponent();

            // Resolve ViewModel from IoC container (Dependency Injection)
            ViewModel = ServiceContainer.Instance.Resolve<MainPageViewModel>();
            
            // Set DataContext for MVVM binding
            this.DataContext = ViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel?.OnNavigatedTo(e.Parameter);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            ViewModel?.OnNavigatedFrom();
        }
    }
}
