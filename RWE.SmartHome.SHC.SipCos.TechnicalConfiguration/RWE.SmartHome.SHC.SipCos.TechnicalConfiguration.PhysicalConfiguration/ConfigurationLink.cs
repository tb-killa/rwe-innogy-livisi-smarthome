using System.Collections.Generic;
using System.IO;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

public class ConfigurationLink
{
	private readonly SortedList<byte, ParameterList> parameterLists = new SortedList<byte, ParameterList>();

	public bool IsUnknownState { get; set; }

	public SortedList<byte, ParameterList> ParameterLists => parameterLists;

	public ParameterList this[byte key]
	{
		get
		{
			if (!ParameterLists.ContainsKey(key))
			{
				ParameterLists.Add(key, new ParameterList());
			}
			return ParameterLists[key];
		}
	}

	public byte this[KeyValuePair<byte, byte> indices]
	{
		get
		{
			return ParameterLists[indices.Key][indices.Value];
		}
		set
		{
			if (!ParameterLists.ContainsKey(indices.Key))
			{
				ParameterLists.Add(indices.Key, new ParameterList());
			}
			ParameterLists[indices.Key][indices.Value] = value;
		}
	}

	public ConfigurationLink()
	{
	}

	public ConfigurationLink CreateDiffAndMark(ConfigurationLink newLink)
	{
		ConfigurationLink configurationLink = new ConfigurationLink();
		foreach (KeyValuePair<byte, ParameterList> parameterList3 in newLink.ParameterLists)
		{
			ParameterList parameterList = this[parameterList3.Key];
			ParameterList parameterList2 = parameterList.CreateDiffAndMark(parameterList3.Value);
			if (parameterList2.Count > 0)
			{
				configurationLink.ParameterLists.Add(parameterList3.Key, parameterList2);
			}
		}
		return configurationLink;
	}

	public void ApplyDiff(ConfigurationLink diff)
	{
		if (diff == null)
		{
			return;
		}
		foreach (KeyValuePair<byte, ParameterList> parameterList2 in diff.ParameterLists)
		{
			ParameterList parameterList = this[parameterList2.Key];
			parameterList.ApplyDiff(parameterList2.Value);
		}
	}

	public ConfigurationLink Clone()
	{
		ConfigurationLink configurationLink = new ConfigurationLink();
		configurationLink.IsUnknownState = IsUnknownState;
		ConfigurationLink configurationLink2 = configurationLink;
		foreach (KeyValuePair<byte, ParameterList> parameterList in parameterLists)
		{
			configurationLink2.ParameterLists.Add(parameterList.Key, parameterList.Value.Clone());
		}
		return configurationLink2;
	}

	public ConfigurationLink(BinaryReader reader)
		: this()
	{
		byte b = reader.ReadByte();
		IsUnknownState = reader.ReadBoolean();
		for (int i = 0; i < b; i++)
		{
			ParameterLists.Add(reader.ReadByte(), new ParameterList(reader));
		}
	}

	public void Save(BinaryWriter writer)
	{
		writer.Write((byte)ParameterLists.Count);
		writer.Write(IsUnknownState);
		foreach (KeyValuePair<byte, ParameterList> parameterList in ParameterLists)
		{
			writer.Write(parameterList.Key);
			parameterList.Value.Save(writer);
		}
	}
}
