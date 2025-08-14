using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace SerialAPI;

public class BIDCOSNodeCollection
{
	public byte[] GroupIP { get; set; }

	public byte[] GroupIPWsd2 { get; set; }

	[XmlElement]
	public int SequenceCounterWsd2 { get; set; }

	[XmlIgnore]
	public byte SequenceCounterWsd2LowByte => (byte)SequenceCounterWsd2;

	[XmlIgnore]
	public byte SequenceCounterWsd2MidByte => (byte)(SequenceCounterWsd2 >> 8);

	[XmlIgnore]
	public byte SequenceCounterWsd2HighByte => (byte)(SequenceCounterWsd2 >> 16);

	public byte[] Wsd2LocalKey { get; set; }

	public List<BIDCOSNode> Nodes { get; set; }

	public BIDCOSNodeCollection()
	{
		Wsd2LocalKey = new byte[16];
		new Random().NextBytes(Wsd2LocalKey);
		Nodes = new List<BIDCOSNode>();
	}

	public BIDCOSNode getNode(BIDCOSNode node)
	{
		return Nodes.FirstOrDefault((BIDCOSNode x) => x.Equals(node));
	}

	public BIDCOSNode getNode(byte[] address)
	{
		if (address == null)
		{
			return null;
		}
		return Nodes.FirstOrDefault((BIDCOSNode x) => x.address.Take(3).SequenceEqual(address.Take(3)));
	}

	public BIDCOSNode getNodeFromIP(byte[] ip)
	{
		if (ip == null)
		{
			return null;
		}
		return Nodes.FirstOrDefault((BIDCOSNode x) => x.ip.Take(3).SequenceEqual(ip.Take(3)));
	}

	public void Clear()
	{
		Nodes.Clear();
	}

	internal void Merge(BIDCOSNode node)
	{
		int num = Nodes.IndexOf(node);
		if (num >= 0)
		{
			Nodes[num] = node;
		}
		else
		{
			Nodes.Add(node);
		}
	}

	internal bool Contains(BIDCOSNode node)
	{
		return Nodes.Contains(node);
	}

	internal void Add(BIDCOSNode node)
	{
		Nodes.Add(node);
	}

	internal BIDCOSNode GetNodeFromSgtin(byte[] sgtin)
	{
		if (sgtin == null)
		{
			return null;
		}
		return Nodes.FirstOrDefault((BIDCOSNode n) => n.Sgtin != null && n.Sgtin.SequenceEqual(sgtin));
	}

	internal void Remove(BIDCOSNode node)
	{
		Nodes.Remove(node);
	}
}
