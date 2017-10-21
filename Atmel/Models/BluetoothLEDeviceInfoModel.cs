﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;

namespace Atmel.Models
{
    public class BluetoothLEDeviceInfoModel
    {
        public BluetoothLEDeviceInfoModel(DeviceInformation deviceInformation)
        {
            this.Id = deviceInformation.Id;
            this.Name = deviceInformation.Name;
            this.IsEnabled = deviceInformation.IsEnabled;
            this.IsPaired = deviceInformation.Pairing.IsPaired;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsPaired { get; set; }

        public override string ToString()
        {
            return String.Format("Id: {0}\nName: {1}\nIsEnabled: {2}\nIsPaired: {3}",
                this.Id, this.Name, this.IsEnabled, this.IsPaired);
        }
    }
}
