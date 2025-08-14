using System.Collections.Generic;

namespace SmartHome.SHC.API.Protocols.wMBus;

public class WMBusDeviceState
{
	public List<VariableDataEntry> VariableDataEntries { get; set; }

	public DeviceDescription DeviceDescription { get; set; }

	public IDataHeader DataHeader { get; set; }

	public byte[] ManufacturerSpecificData { get; set; }

	public WMBusDeviceState()
	{
		VariableDataEntries = new List<VariableDataEntry>();
	}

	public bool Equals(WMBusDeviceState other)
	{
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		if (VariableDataEntries.Count != other.VariableDataEntries.Count)
		{
			return false;
		}
		for (int i = 0; i < VariableDataEntries.Count; i++)
		{
			if (!VariableDataEntries[i].Equals(other.VariableDataEntries[i]))
			{
				return false;
			}
		}
		if (!ManufacturerSpecificData.Compare(other.ManufacturerSpecificData))
		{
			return false;
		}
		return object.Equals(other.DeviceDescription, DeviceDescription) && object.Equals(other.DataHeader, DataHeader);
	}

	public override bool Equals(object obj)
	{
		if (object.ReferenceEquals(null, obj))
		{
			return false;
		}
		if (object.ReferenceEquals(this, obj))
		{
			return true;
		}
		if ((object)obj.GetType() != typeof(WMBusDeviceState))
		{
			return false;
		}
		return Equals((WMBusDeviceState)obj);
	}

	public override int GetHashCode()
	{
		int num = ((VariableDataEntries != null) ? VariableDataEntries.GetHashCode() : 0);
		return (num * 397) ^ ((DeviceDescription != null) ? DeviceDescription.GetHashCode() : 0);
	}
}
