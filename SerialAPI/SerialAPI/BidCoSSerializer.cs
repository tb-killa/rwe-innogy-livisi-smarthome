using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SerialAPI.BidCoSFrames;

namespace SerialAPI;

internal class BidCoSSerializer
{
	private readonly XmlSerializer serializer = new XmlSerializer(typeof(BIDCOSNodeCollection));

	public BIDCOSNodeCollection Deserialize(string serializedData)
	{
		BIDCOSNodeCollection bIDCOSNodeCollection = null;
		bIDCOSNodeCollection = DeserializeXML(serializedData);
		if (bIDCOSNodeCollection == null)
		{
			bIDCOSNodeCollection = DeserializeByteArray(serializedData);
		}
		return bIDCOSNodeCollection;
	}

	private BIDCOSNodeCollection DeserializeXML(string serializedData)
	{
		BIDCOSNodeCollection bIDCOSNodeCollection = null;
		if (serializedData == null)
		{
			return bIDCOSNodeCollection;
		}
		using (StringReader textReader = new StringReader(serializedData))
		{
			try
			{
				bIDCOSNodeCollection = (BIDCOSNodeCollection)serializer.Deserialize(textReader);
				if (bIDCOSNodeCollection != null)
				{
					bIDCOSNodeCollection.Nodes = bIDCOSNodeCollection.Nodes.Where((BIDCOSNode x) => x.included).ToList();
				}
			}
			catch (Exception)
			{
			}
		}
		return bIDCOSNodeCollection;
	}

	private BIDCOSNodeCollection DeserializeByteArray(string serializedData)
	{
		BIDCOSNodeCollection bIDCOSNodeCollection = new BIDCOSNodeCollection();
		try
		{
			List<byte[]> list = new List<byte[]>();
			if (!string.IsNullOrEmpty(serializedData))
			{
				StringReader stringReader = new StringReader(serializedData);
				string s;
				while ((s = stringReader.ReadLine()) != null)
				{
					list.Add(Convert.FromBase64String(s));
				}
			}
			List<byte[,]> data = FromBidCosMappingPersistence(list);
			load(bIDCOSNodeCollection, data);
		}
		catch (Exception)
		{
		}
		return bIDCOSNodeCollection;
	}

	public string Serialize(BIDCOSNodeCollection bidcosNodes)
	{
		string empty = string.Empty;
		bidcosNodes.Nodes = bidcosNodes.Nodes.Where((BIDCOSNode x) => x.included).ToList();
		using StringWriter stringWriter = new StringWriter();
		serializer.Serialize(stringWriter, bidcosNodes);
		return stringWriter.ToString();
	}

	private List<byte[,]> FromBidCosMappingPersistence(IList<byte[]> bidCosMappings)
	{
		if (bidCosMappings == null)
		{
			return new List<byte[,]>();
		}
		List<byte[,]> list = new List<byte[,]>(bidCosMappings.Count);
		foreach (byte[] bidCosMapping in bidCosMappings)
		{
			list.Add(new byte[2, 3]
			{
				{
					bidCosMapping[0],
					bidCosMapping[1],
					bidCosMapping[2]
				},
				{
					bidCosMapping[3],
					bidCosMapping[4],
					bidCosMapping[5]
				}
			});
		}
		return list;
	}

	private void load(BIDCOSNodeCollection _this, List<byte[,]> data)
	{
		_this.Clear();
		if (data.Count <= 0)
		{
			return;
		}
		if (data[0][0, 0] != 0 || data[0][0, 1] != 0 || data[0][0, 2] != 0)
		{
			byte[] array = new byte[3];
			for (int i = 0; i < 3; i++)
			{
				array[i] = data[0][0, i];
			}
			_this.GroupIP = array;
		}
		if (data[0][1, 0] != 0 || data[0][1, 1] != 0 || data[0][1, 2] != 0)
		{
			byte[] array2 = new byte[3];
			for (int j = 0; j < 3; j++)
			{
				array2[j] = data[0][1, j];
			}
			_this.GroupIPWsd2 = array2;
		}
		for (int k = 1; k < data.Count; k++)
		{
			BIDCOSNode bIDCOSNode = new BIDCOSNode();
			bIDCOSNode.included = true;
			for (int l = 0; l < 3; l++)
			{
				bIDCOSNode.address[l] = data[k][0, l];
				bIDCOSNode.ip[l] = data[k][1, l];
				bIDCOSNode.DeviceType = BIDCOSDeviceType.Eq3BasicSmokeDetector;
			}
			_this.Nodes.Add(bIDCOSNode);
		}
	}

	private string LegacyTransform(BIDCOSNodeCollection nodes)
	{
		List<byte[,]> bidCosNodes = save(nodes);
		List<byte[]> list = ToBidCosMappingPersistence(bidCosNodes);
		StringBuilder stringBuilder = new StringBuilder();
		foreach (byte[] item in list)
		{
			stringBuilder.AppendLine(Convert.ToBase64String(item));
		}
		return stringBuilder.ToString();
	}

	private List<byte[]> ToBidCosMappingPersistence(ICollection<byte[,]> bidCosNodes)
	{
		List<byte[]> list = new List<byte[]>(bidCosNodes.Count);
		foreach (byte[,] bidCosNode in bidCosNodes)
		{
			list.Add(new byte[6]
			{
				bidCosNode[0, 0],
				bidCosNode[0, 1],
				bidCosNode[0, 2],
				bidCosNode[1, 0],
				bidCosNode[1, 1],
				bidCosNode[1, 2]
			});
		}
		return list;
	}

	private List<byte[,]> save(BIDCOSNodeCollection _this)
	{
		List<byte[,]> list = new List<byte[,]>();
		byte[,] array = new byte[2, 3];
		if (_this.GroupIP != null)
		{
			for (int i = 0; i < 3; i++)
			{
				array[0, i] = _this.GroupIP[i];
			}
		}
		if (_this.GroupIPWsd2 != null)
		{
			for (int j = 0; j < 3; j++)
			{
				array[1, j] = _this.GroupIPWsd2[j];
			}
		}
		list.Add(array);
		foreach (BIDCOSNode node in _this.Nodes)
		{
			if (node.included)
			{
				byte[,] array2 = new byte[2, 3];
				for (int k = 0; k < 3; k++)
				{
					array2[0, k] = node.address[k];
					array2[1, k] = node.ip[k];
				}
				list.Add(array2);
			}
		}
		if (list.Count == 1)
		{
			list.Clear();
		}
		return list;
	}
}
