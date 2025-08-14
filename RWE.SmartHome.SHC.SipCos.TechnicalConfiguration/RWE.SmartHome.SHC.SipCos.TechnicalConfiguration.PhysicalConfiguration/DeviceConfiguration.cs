using System.Collections.Generic;
using System.IO;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

public class DeviceConfiguration
{
	public IDictionary<byte, ConfigurationChannel> Channels { get; private set; }

	public DeviceConfiguration()
	{
		Channels = new Dictionary<byte, ConfigurationChannel>();
	}

	public DeviceConfiguration(BinaryReader reader)
		: this()
	{
		byte b = reader.ReadByte();
		for (byte b2 = 0; b2 < b; b2++)
		{
			byte key = reader.ReadByte();
			Channels.Add(key, new ConfigurationChannel(reader));
		}
	}

	public DeviceConfigurationDiff CreateDiffAndMark(DeviceConfiguration newConfig)
	{
		DeviceConfigurationDiff deviceConfigurationDiff = new DeviceConfigurationDiff();
		foreach (KeyValuePair<byte, ConfigurationChannel> channel in Channels)
		{
			ConfigurationChannel value = null;
			if (newConfig.Channels.TryGetValue(channel.Key, out value))
			{
				deviceConfigurationDiff.ChannelDiffs.Add(channel.Key, channel.Value.CreateDiffAndMark(value));
			}
			else
			{
				Log.Warning(Module.TechnicalConfiguration, $"The channel with index {channel.Key} was removed from the new config. Ignoring.");
			}
		}
		return deviceConfigurationDiff;
	}

	public void ApplyDiff(DeviceConfigurationDiff diff)
	{
		if (diff == null)
		{
			return;
		}
		foreach (KeyValuePair<byte, ConfigurationChannel> channel in Channels)
		{
			channel.Value.ApplyDiff(diff.ChannelDiffs[channel.Key]);
		}
	}

	public DeviceConfiguration Clone()
	{
		DeviceConfiguration deviceConfiguration = new DeviceConfiguration();
		foreach (KeyValuePair<byte, ConfigurationChannel> channel in Channels)
		{
			deviceConfiguration.Channels.Add(channel.Key, channel.Value.Clone());
		}
		return deviceConfiguration;
	}

	public void Save(BinaryWriter writer)
	{
		writer.Write((byte)Channels.Count);
		foreach (KeyValuePair<byte, ConfigurationChannel> channel in Channels)
		{
			writer.Write(channel.Key);
			channel.Value.Save(writer);
		}
	}
}
