using System;
using System.Collections.Generic;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class ConfigurationDevicesDeletedEventArgs
{
	public List<Guid> DeletedDeviceList;
}
