using System;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces.Persistence;

public class DeviceInformationEntity
{
	public Guid DeviceId { get; set; }

	public byte[] Address { get; set; }

	public byte[] SGTIN { get; set; }

	public byte ManufacturerCode { get; set; }

	public uint ManufacturerDeviceType { get; set; }

	public byte ManufacturerDeviceAndFirmware { get; set; }

	public byte OperationModes { get; set; }

	public int DeviceInclusionState { get; set; }

	public DateTime DeviceExclusionTime { get; set; }

	public bool DeviceNotReachable { get; set; }

	public byte ProtocolType { get; set; }

	public int UpdateState { get; set; }

	public int? PendingVersionNumber { get; set; }

	public bool IsRoutedInclusion { get; set; }
}
