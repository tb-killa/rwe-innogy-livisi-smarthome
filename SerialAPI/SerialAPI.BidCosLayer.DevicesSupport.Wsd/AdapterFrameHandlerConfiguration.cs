using System.Collections.Generic;

namespace SerialAPI.BidCosLayer.DevicesSupport.Wsd;

internal class AdapterFrameHandlerConfiguration : AdapterFrameHandler<WsdAdapter>
{
	public AdapterFrameHandlerConfiguration(WsdAdapter deviceAdapter)
		: base(deviceAdapter)
	{
	}

	public override bool CanHandle(BidCosMessageForSend message)
	{
		return message.header.FrameType == SIPcosFrameType.CONFIGURATION;
	}

	public override SendStatus Handle(BidCosMessageForSend message)
	{
		SendStatus result = SendStatus.NO_REPLY;
		byte channel = message.message[0];
		byte[] array = new byte[3]
		{
			message.message[2],
			message.message[3],
			message.message[4]
		};
		SendMode mode = message.mode;
		switch (message.message[1])
		{
		case 1:
			if (base.DeviceAdapter.wsdComm.ConfigRegistration(base.DeviceAdapter.Address, channel, array, message.message[5], message.message[6], mode))
			{
				result = SendStatus.ACK;
			}
			break;
		case 2:
			if (base.DeviceAdapter.wsdComm.ConfigDeregistration(base.DeviceAdapter.Address, channel, array, message.message[5], message.message[6], mode))
			{
				result = SendStatus.ACK;
			}
			break;
		case 3:
			if (base.DeviceAdapter.wsdComm.ConfigOrderLinkList(base.DeviceAdapter.Address, channel, mode))
			{
				result = SendStatus.ACK;
			}
			break;
		case 5:
			if (base.DeviceAdapter.wsdComm.ConfigBegin(base.DeviceAdapter.Address, channel, array, message.message[5], message.message[6], mode))
			{
				result = SendStatus.ACK;
			}
			break;
		case 6:
			if (base.DeviceAdapter.wsdComm.ConfigEnd(base.DeviceAdapter.Address, channel, mode))
			{
				result = SendStatus.ACK;
			}
			break;
		case 8:
		{
			List<byte> list = new List<byte>();
			for (int i = 2; i < message.message.Length; i++)
			{
				list.Add(message.message[i]);
			}
			if (base.DeviceAdapter.wsdComm.ConfigData(base.DeviceAdapter.Address, channel, list.ToArray(), mode))
			{
				result = SendStatus.ACK;
			}
			break;
		}
		default:
			result = SendStatus.ERROR;
			break;
		}
		return result;
	}
}
