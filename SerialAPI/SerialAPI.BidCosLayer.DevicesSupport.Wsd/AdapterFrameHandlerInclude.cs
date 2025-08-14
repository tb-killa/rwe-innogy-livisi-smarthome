using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace SerialAPI.BidCosLayer.DevicesSupport.Wsd;

internal class AdapterFrameHandlerInclude : AdapterFrameHandler<WsdAdapter>
{
	public AdapterFrameHandlerInclude(WsdAdapter deviceAdapter)
		: base(deviceAdapter)
	{
	}

	public override bool CanHandle(BidCosMessageForSend message)
	{
		return message.message[0] == 4;
	}

	public override SendStatus Handle(BidCosMessageForSend message)
	{
		SendStatus sendStatus = SendStatus.NO_REPLY;
		if (message.message[1] == 8)
		{
			if (!base.DeviceAdapter.EnsureCurrentNodeDefaultKey())
			{
				Log.Error(Module.SerialCommunication, "Could not retrieve original BidCos device key");
				return sendStatus;
			}
			base.DeviceAdapter.bidCosHandler.m_block_answer = true;
			base.DeviceAdapter.Included = false;
			sendStatus = base.DeviceAdapter.TryInclusionForWsd();
			base.DeviceAdapter.Included = sendStatus == SendStatus.ACK;
			base.DeviceAdapter.UpdateNode();
			base.DeviceAdapter.bidCosHandler.m_block_answer = false;
		}
		return sendStatus;
	}
}
