using System;
using System.Collections;
using System.Collections.Generic;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces;

public interface IDeviceList : IEnumerable<IDeviceInformation>, IEnumerable
{
	object SyncRoot { get; }

	bool ContainsRouter { get; set; }

	bool? ForceDetectionOfRouters { set; }

	IDeviceInformation this[byte[] address] { get; }

	IDeviceInformation this[Guid deviceId] { get; }

	bool Contains(byte[] address);

	bool ContainsSGTIN(byte[] sgtin);

	bool Contains(Guid deviceId);

	IDeviceInformation GetBySGTIN(byte[] sgtin);

	string LogInfoByAddress(byte[] address);

	string LogInfoBySGTIN(byte[] sgtin);

	string LogInfoByGuid(Guid guid);

	string LogInfoByDeviceInfo(IDeviceInformation deviceInfo);
}
