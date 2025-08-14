using System;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;

namespace RWE.SmartHome.SHC.DeviceManager.BidCosFactoryResetHandlers;

public class SirFRHandler
{
	private readonly Action<IDeviceInformation> frCallback;

	public byte[] SourceAdress { get; private set; }

	public SirFRHandler(byte[] sourceAdress, Action<IDeviceInformation> frCallback)
	{
		this.frCallback = frCallback;
		SourceAdress = sourceAdress;
	}

	public void ReceivedInfoStatusFrame(IDeviceInformation device, byte[] destinationAddress)
	{
		if (IsEmptyAddress(destinationAddress) && device != null && device.DeviceInclusionState != DeviceInclusionState.FactoryReset && device.DeviceInclusionState != DeviceInclusionState.InclusionPending && device.DeviceInclusionState != DeviceInclusionState.Excluded && device.DeviceInclusionState != DeviceInclusionState.ExclusionPending && device.DeviceInclusionState != DeviceInclusionState.Found)
		{
			frCallback(device);
		}
	}

	private bool IsEmptyAddress(byte[] address)
	{
		if (address != null && address.Length == 3 && address[0] == 0 && address[1] == 0)
		{
			return address[2] == 0;
		}
		return false;
	}
}
