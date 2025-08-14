using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

public class ConfigurationChannel
{
	public Dictionary<LinkPartner, ConfigurationLink> Links { get; private set; }

	public ChannelType ChannelType { get; set; }

	public ConfigurationLink DefaultLink { get; set; }

	public ConfigurationLink SetLinkConfigUpdatePendingConfiguration { get; set; }

	public ConfigurationLink ClearLinkConfigUpdatePendingConfiguration { get; set; }

	public ConfigurationChannel()
	{
		Links = new Dictionary<LinkPartner, ConfigurationLink>();
		ChannelType = ChannelType.ActuatorWithoutFlag;
	}

	public ConfigurationChannelDiff CreateDiffAndMark(ConfigurationChannel newChannel)
	{
		ConfigurationChannelDiff configurationChannelDiff = new ConfigurationChannelDiff();
		foreach (KeyValuePair<LinkPartner, ConfigurationLink> link in newChannel.Links)
		{
			ConfigurationLink value = link.Value;
			LinkPartner key = link.Key;
			if (Links.TryGetValue(key, out var value2))
			{
				ConfigurationLink configurationLink = value2.CreateDiffAndMark(value);
				if (value2.IsUnknownState)
				{
					configurationChannelDiff.ToCreate.Add(key, configurationLink);
				}
				else if (configurationLink.ParameterLists.Count > 0)
				{
					configurationChannelDiff.ToChange.Add(key, configurationLink);
				}
			}
			else
			{
				ConfigurationLink configurationLink2 = ((DefaultLink != null) ? DefaultLink.Clone() : new ConfigurationLink());
				configurationLink2.IsUnknownState = true;
				ConfigurationLink value3 = configurationLink2.CreateDiffAndMark(value);
				Links.Add(key, configurationLink2);
				configurationChannelDiff.ToCreate.Add(key, value3);
			}
		}
		foreach (KeyValuePair<LinkPartner, ConfigurationLink> item in Links.Where((KeyValuePair<LinkPartner, ConfigurationLink> pair) => !newChannel.Links.ContainsKey(pair.Key)))
		{
			ConfigurationLink value4 = item.Value;
			value4.IsUnknownState = true;
			if (DefaultLink != null)
			{
				value4.CreateDiffAndMark(DefaultLink);
			}
			configurationChannelDiff.ToDelete.Add(item.Key);
		}
		return configurationChannelDiff;
	}

	public void ApplyDiff(ConfigurationChannelDiff diff)
	{
		if (diff == null)
		{
			return;
		}
		foreach (KeyValuePair<LinkPartner, ConfigurationLink> item in diff.ToCreate)
		{
			if (Links.TryGetValue(item.Key, out var value))
			{
				value.IsUnknownState = false;
			}
			else
			{
				value = ((DefaultLink != null) ? DefaultLink.Clone() : new ConfigurationLink());
				Links.Add(item.Key, value);
			}
			value.ApplyDiff(item.Value);
		}
		foreach (LinkPartner item2 in diff.ToDelete)
		{
			Links.Remove(item2);
		}
		foreach (KeyValuePair<LinkPartner, ConfigurationLink> item3 in diff.ToChange)
		{
			ConfigurationLink configurationLink = Links[item3.Key];
			configurationLink.IsUnknownState = false;
			configurationLink.ApplyDiff(item3.Value);
		}
	}

	public ConfigurationChannel Clone()
	{
		ConfigurationChannel configurationChannel = new ConfigurationChannel();
		configurationChannel.ChannelType = ChannelType;
		configurationChannel.DefaultLink = DefaultLink.Clone();
		configurationChannel.SetLinkConfigUpdatePendingConfiguration = SetLinkConfigUpdatePendingConfiguration;
		configurationChannel.ClearLinkConfigUpdatePendingConfiguration = ClearLinkConfigUpdatePendingConfiguration;
		ConfigurationChannel configurationChannel2 = configurationChannel;
		foreach (KeyValuePair<LinkPartner, ConfigurationLink> link in Links)
		{
			configurationChannel2.Links.Add(link.Key.Clone(), link.Value.Clone());
		}
		return configurationChannel2;
	}

	public ConfigurationChannel(BinaryReader reader)
		: this()
	{
		byte b = reader.ReadByte();
		DefaultLink = new ConfigurationLink(reader);
		for (int i = 0; i < b; i++)
		{
			Links.Add(new LinkPartner(reader), new ConfigurationLink(reader));
		}
	}

	public void Save(BinaryWriter writer)
	{
		writer.Write((byte)Links.Count);
		DefaultLink.Save(writer);
		foreach (KeyValuePair<LinkPartner, ConfigurationLink> link in Links)
		{
			link.Key.Save(writer);
			link.Value.Save(writer);
		}
	}
}
