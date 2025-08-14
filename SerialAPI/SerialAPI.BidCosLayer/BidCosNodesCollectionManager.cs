using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.Core.Scheduler;
using SerialAPI.BidCoSFrames;
using SerialAPI.BidCosLayer.DevicesSupport;
using SerialAPI.BidCosLayer.DevicesSupport.Sir;
using SerialAPI.BidCosLayer.DevicesSupport.Wsd;
using SerialAPI.BidCosLayer.DevicesSupport.Wsd2;

namespace SerialAPI.BidCosLayer;

internal class BidCosNodesCollectionManager
{
	private BIDCOSNodeCollection nodes = new BIDCOSNodeCollection();

	private readonly List<BidCosDeviceAdapter> adapters = new List<BidCosDeviceAdapter>();

	private readonly IBidCoSPersistence bidcosPersistence;

	private readonly BidCoSSerializer bidcosSerializer = new BidCoSSerializer();

	private readonly BidCosHandlerRef bidCosHandlerRef;

	private readonly IScheduler scheduler;

	public byte[] Wsd1GroupAddress
	{
		get
		{
			return nodes.GroupIP;
		}
		set
		{
			nodes.GroupIP = value;
		}
	}

	public byte[] Wsd2GroupAddress
	{
		get
		{
			return nodes.GroupIPWsd2;
		}
		set
		{
			nodes.GroupIPWsd2 = value;
		}
	}

	public byte SequenceCounterWsd2LowByte => nodes.SequenceCounterWsd2LowByte;

	public byte SequenceCounterWsd2MidByte => nodes.SequenceCounterWsd2MidByte;

	public byte SequenceCounterWsd2HighByte => nodes.SequenceCounterWsd2HighByte;

	public byte[] Wsd2LocalKey
	{
		get
		{
			return nodes.Wsd2LocalKey;
		}
		set
		{
			nodes.Wsd2LocalKey = value;
		}
	}

	public BidCosNodesCollectionManager(BidCosHandlerRef bidCosHandlerRef, IBidCoSPersistence bidCoSPersistence, IScheduler scheduler)
	{
		this.bidCosHandlerRef = bidCosHandlerRef;
		bidcosPersistence = bidCoSPersistence;
		this.scheduler = scheduler;
	}

	public void SetBidcosNodes(string bidcosNodes)
	{
		nodes = bidcosSerializer.Deserialize(bidcosNodes);
		if (nodes != null)
		{
			BuildAdaptersForNodesIfNotExist(nodes);
		}
	}

	public BidCosDeviceAdapter GetAdapter(BIDCOSNode bidCosNode)
	{
		return adapters.FirstOrDefault((BidCosDeviceAdapter x) => bidCosHandlerRef.AddressesEqual(x.Node.address, bidCosNode.address));
	}

	public BIDCOSNode GetNodeFromSgtin(byte[] sgtin)
	{
		return nodes.GetNodeFromSgtin(sgtin);
	}

	public BidCosDeviceAdapter GetAdapterFromIp(byte[] destination)
	{
		return adapters.FirstOrDefault((BidCosDeviceAdapter x) => bidCosHandlerRef.AddressesEqual(x.Node.address, destination));
	}

	internal bool Contains(BIDCOSNode node)
	{
		return nodes.Contains(node);
	}

	internal BIDCOSNode getNode(BIDCOSNode node)
	{
		return nodes.getNode(node);
	}

	public BidCosDeviceAdapter AddNode(BIDCOSNode bidCosNode2)
	{
		nodes.Add(bidCosNode2);
		BidCosDeviceAdapter bidCosDeviceAdapter = BuildAdapter(bidCosNode2);
		adapters.Add(bidCosDeviceAdapter);
		return bidCosDeviceAdapter;
	}

	public void UpdateNode(BIDCOSNode node)
	{
		lock (bidCosHandlerRef.syncRoot)
		{
			nodes.Merge(node);
			BidCosDeviceAdapter bidCosDeviceAdapter = GetAdapter(node);
			if (bidCosDeviceAdapter == null)
			{
				bidCosDeviceAdapter = BuildAdapter(node);
				adapters.Add(bidCosDeviceAdapter);
			}
			bidCosDeviceAdapter.Node = node;
		}
	}

	public void Remove(BIDCOSNode bidCosNode2)
	{
		lock (bidCosHandlerRef.syncRoot)
		{
			nodes.Remove(bidCosNode2);
			BidCosDeviceAdapter adapter = GetAdapter(bidCosNode2);
			if (adapter != null)
			{
				adapters.Remove(adapter);
			}
		}
	}

	public void Persist()
	{
		lock (bidCosHandlerRef.syncRoot)
		{
			bidcosPersistence.Save(bidcosSerializer.Serialize(nodes));
		}
	}

	public void IncreaseSequenceCounter()
	{
		lock (bidCosHandlerRef.syncRoot)
		{
			nodes.SequenceCounterWsd2++;
			Persist();
		}
	}

	private BidCosDeviceAdapter BuildAdapter(BIDCOSNode bidCosNode)
	{
		BidCosDeviceAdapter bidCosDeviceAdapter = null;
		if (bidCosNode.DeviceType == BIDCOSDeviceType.Eq3BasicSmokeDetector)
		{
			WsdComm wsdComm = new WsdComm(bidCosHandlerRef);
			bidCosDeviceAdapter = new WsdAdapter(wsdComm, bidCosHandlerRef);
			bidCosDeviceAdapter.Node = bidCosNode;
		}
		if (bidCosNode.DeviceType == BIDCOSDeviceType.Eq3EncryptedSmokeDetector)
		{
			Wsd2Comm wsdComm2 = new Wsd2Comm(bidCosHandlerRef);
			bidCosDeviceAdapter = new Wsd2Adapter(wsdComm2, bidCosHandlerRef);
			bidCosDeviceAdapter.Node = bidCosNode;
		}
		if (bidCosNode.DeviceType == BIDCOSDeviceType.Eq3EncryptedSiren)
		{
			SirComm sirComm = new SirComm(bidCosHandlerRef);
			bidCosDeviceAdapter = new SirAdapter(sirComm, bidCosHandlerRef, scheduler);
			bidCosDeviceAdapter.Node = bidCosNode;
		}
		return bidCosDeviceAdapter;
	}

	internal Wsd2Adapter FindWsd2Device()
	{
		return adapters.FirstOrDefault((BidCosDeviceAdapter x) => x is Wsd2Adapter && x.Included) as Wsd2Adapter;
	}

	internal WsdAdapter FindWsd1Device()
	{
		return adapters.FirstOrDefault((BidCosDeviceAdapter x) => x is WsdAdapter && x.Included) as WsdAdapter;
	}

	private void BuildAdaptersForNodesIfNotExist(BIDCOSNodeCollection nodes)
	{
		foreach (BIDCOSNode node in nodes.Nodes)
		{
			if (GetAdapter(node) == null)
			{
				BidCosDeviceAdapter item = BuildAdapter(node);
				adapters.Add(item);
			}
		}
	}
}
