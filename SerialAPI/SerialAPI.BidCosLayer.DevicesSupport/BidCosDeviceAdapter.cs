using System.Collections.Generic;
using SerialAPI.BidCoSFrames;

namespace SerialAPI.BidCosLayer.DevicesSupport;

internal abstract class BidCosDeviceAdapter
{
	public readonly BidCosHandlerRef bidCosHandler;

	public BIDCOSNode Node { get; set; }

	public bool Included
	{
		get
		{
			return Node.included;
		}
		set
		{
			Node.included = value;
		}
	}

	public bool Removed { get; set; }

	public byte[] Address => Node.address;

	public BIDCOSSysinfoFrame Sysinfo => Node.Sysinfo;

	protected BidCosDeviceAdapter(BidCosHandlerRef bidCosHandler)
	{
		this.bidCosHandler = bidCosHandler;
	}

	public virtual void Remove()
	{
		Removed = true;
		Included = false;
	}

	public abstract bool EnsureCurrentNodeDefaultKey();

	public abstract void UpdateNode();

	public abstract SendMode GetBurstType();

	public abstract byte GetCounter();

	public virtual void GenerateNewIp()
	{
		Node.GenerateNewIP();
	}

	public void SendToSipcosHandler(BIDCOSMessage message)
	{
		if (message != null)
		{
			CORESTACKHeader header = new CORESTACKHeader();
			List<byte> data = new List<byte>();
			message.GenerateSIPCOSMessage(ref header, ref data);
			header.MacSecurity = false;
			data[0] = (data[0] &= 127);
			BIDCOSNode bIDCOSNode = (Removed ? null : Node);
			if (bIDCOSNode != null)
			{
				header.MacSource = bIDCOSNode.ip;
				header.IpSource = bIDCOSNode.ip;
				bidCosHandler.m_sipcos.Handle(header, data);
			}
		}
	}

	public abstract SendStatus SendCosipMessage(BidCosMessageForSend message);

	public abstract BIDCOSMessage HandleFrame(ReceiveFrameData frameData);
}
