using System.Collections.Generic;

namespace RWE.SmartHome.SHC.DeviceManager.SwitchDelegate;

public class SwitchDelegateEntry
{
	public IEnumerable<byte[]> DestinationAddresses { get; private set; }

	public byte KeyPressCounter { get; set; }

	public SwitchDelegateEntry(IEnumerable<byte[]> destinationAddresses)
	{
		DestinationAddresses = destinationAddresses;
	}
}
