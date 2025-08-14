using System;
using System.Xml.Serialization;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using SerialAPI.BidCoSFrames;

namespace SerialAPI;

public class BIDCOSNode
{
	public byte[] address;

	public byte[] ip;

	[XmlElement]
	public bool included;

	private BIDCOSSysinfoFrame sysinfo;

	[XmlIgnore]
	public int SequenceCount;

	[XmlIgnore]
	public byte AnswerSequenceCount;

	[XmlIgnore]
	public bool UseAnswerCount;

	[XmlIgnore]
	public bool UseDefaultKey { get; set; }

	[XmlElement]
	public BIDCOSDeviceType DeviceType { get; set; }

	[XmlIgnore]
	internal BIDCOSSysinfoFrame Sysinfo
	{
		get
		{
			return sysinfo;
		}
		set
		{
			sysinfo = value;
			if (sysinfo != null)
			{
				DeviceType = sysinfo.DeviceType;
				Sgtin = sysinfo.generateSGTIN();
				Log.Information(Module.SerialCommunication, $"Found {sysinfo.DeviceType} device with SGTIN: {BitConverter.ToString(Sgtin)}");
			}
		}
	}

	public byte[] Sgtin { get; set; }

	[XmlIgnore]
	public byte[] DefaultKey { get; set; }

	public BIDCOSNode()
	{
		address = new byte[3];
		ip = new byte[3];
		included = false;
		Sysinfo = null;
		SequenceCount = -1;
	}

	public BIDCOSNode(byte[] addr)
		: this()
	{
		address = addr;
		ip = (byte[])addr.Clone();
	}

	public void ValidateIP()
	{
		if (!validIP(ip))
		{
			GenerateNewIP();
		}
	}

	private bool validIP(byte[] ip)
	{
		return ip[0] < 224;
	}

	public void GenerateNewIP()
	{
		Random random = new Random();
		do
		{
			random.NextBytes(ip);
		}
		while (!validIP(ip));
	}

	public override bool Equals(object obj)
	{
		if (obj != null && obj is BIDCOSNode)
		{
			BIDCOSNode bIDCOSNode = (BIDCOSNode)obj;
			if (bIDCOSNode.address != null && address[0] == bIDCOSNode.address[0] && address[1] == bIDCOSNode.address[1] && address[2] == bIDCOSNode.address[2])
			{
				return true;
			}
		}
		return false;
	}

	public override int GetHashCode()
	{
		return address.GetHashCode();
	}
}
