using System;
using UnityEngine;

namespace telescope
{
    interface IIDeviceIdentifiersInternal
    {
        string Idfv { get; }
    }

    class DeviceIdentifiersInternal : IIDeviceIdentifiersInternal
    {
        public string Idfv => SystemInfo.deviceUniqueIdentifier;
    }
}
