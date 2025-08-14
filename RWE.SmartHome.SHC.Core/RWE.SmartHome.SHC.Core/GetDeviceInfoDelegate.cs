using System;

namespace RWE.SmartHome.SHC.Core;

public delegate void GetDeviceInfoDelegate<T>(Guid deviceId, out T deviceData, out bool isValid);
