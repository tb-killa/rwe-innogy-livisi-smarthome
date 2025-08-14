namespace SerialAPI.BidCosLayer.DevicesSupport.Wsd2;

internal class AdapterFrameHandlerUpdateInfo : AdapterFrameHandler<Wsd2Adapter>
{
	public AdapterFrameHandlerUpdateInfo(Wsd2Adapter deviceAdapter)
		: base(deviceAdapter)
	{
	}

	public override bool CanHandle(BidCosMessageForSend message)
	{
		return message.message[0] == 2;
	}

	public override SendStatus Handle(BidCosMessageForSend message)
	{
		SendStatus result = SendStatus.MODE_ERROR;
		if (!base.DeviceAdapter.Included)
		{
			result = SendStatus.ERROR;
			if (message.message[1] == 0 && base.DeviceAdapter.Sysinfo != null)
			{
				byte[] array = base.DeviceAdapter.Sysinfo.generateSGTIN();
				for (int i = 0; i < 12; i++)
				{
					if (message.message[2 + i] != array[i])
					{
						return SendStatus.MODE_ERROR;
					}
				}
				result = SendStatus.ACK;
				base.DeviceAdapter.GenerateNewIp();
				base.DeviceAdapter.UpdateNode();
				base.DeviceAdapter.SendSysInfoToSipcosHandler();
			}
		}
		return result;
	}
}
