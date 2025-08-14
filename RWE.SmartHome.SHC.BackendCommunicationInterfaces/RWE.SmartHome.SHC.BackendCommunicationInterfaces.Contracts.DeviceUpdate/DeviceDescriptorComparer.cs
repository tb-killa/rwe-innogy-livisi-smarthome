using System.Collections.Generic;

namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate;

public class DeviceDescriptorComparer : IEqualityComparer<DeviceDescriptor>
{
	public bool Equals(DeviceDescriptor compare, DeviceDescriptor toCompare)
	{
		if (compare != null && toCompare != null)
		{
			if (compare.AddInVersion == toCompare.AddInVersion && compare.FirmwareVersion == toCompare.FirmwareVersion && compare.HardwareVersion == toCompare.HardwareVersion && compare.Manufacturer == toCompare.Manufacturer)
			{
				return compare.ProductId == toCompare.ProductId;
			}
			return false;
		}
		return false;
	}

	public int GetHashCode(DeviceDescriptor obj)
	{
		return ((obj.AddInVersion == null) ? string.Empty.GetHashCode() : obj.AddInVersion.GetHashCode()) ^ ((obj.FirmwareVersion == null) ? string.Empty.GetHashCode() : obj.FirmwareVersion.GetHashCode()) ^ ((obj.HardwareVersion == null) ? string.Empty.GetHashCode() : obj.HardwareVersion.GetHashCode()) ^ obj.Manufacturer.GetHashCode() ^ obj.ProductId.GetHashCode();
	}
}
