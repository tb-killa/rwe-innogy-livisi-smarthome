using System.Collections.Generic;
using System.Globalization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.ErrorHandling;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Channels;

public class BaseChannel<T> where T : BaseLink, new()
{
	public byte ChannelIndex { get; private set; }

	public byte MaxLinkCount { get; private set; }

	public IDictionary<LinkPartner, T> Links { get; private set; }

	public BaseChannel(byte channelIndex, byte maxLinkCount)
	{
		ChannelIndex = channelIndex;
		MaxLinkCount = maxLinkCount;
		Links = new SortedList<LinkPartner, T>();
	}

	public virtual ConfigurationChannel SaveConfiguration(IDictionary<byte, ConfigurationChannel> channels)
	{
		ConfigurationChannel configurationChannel = new ConfigurationChannel();
		configurationChannel.DefaultLink = new ConfigurationLink();
		ConfigurationChannel configurationChannel2 = configurationChannel;
		T val = new T();
		val.SaveConfiguration(configurationChannel2.DefaultLink);
		ConfigurationLink configurationLink = CreateLinkWithGlobalSettings();
		if (configurationLink != null)
		{
			configurationChannel2.Links[LinkPartner.Empty] = configurationLink;
		}
		foreach (KeyValuePair<LinkPartner, T> link in Links)
		{
			ConfigurationLink configurationLink2 = new ConfigurationLink();
			T value = link.Value;
			value.SaveConfiguration(configurationLink2);
			configurationChannel2.Links.Add(link.Key, configurationLink2);
		}
		if (Links.Count > MaxLinkCount)
		{
			throw GetChannelLinkCountExceededException();
		}
		channels.Add(ChannelIndex, configurationChannel2);
		return configurationChannel2;
	}

	protected virtual ConfigurationLink CreateLinkWithGlobalSettings()
	{
		return null;
	}

	protected TransformationException GetChannelLinkCountExceededException()
	{
		ErrorEntry errorEntry = new ErrorEntry();
		errorEntry.ErrorCode = ValidationErrorCode.ChannelLinkCountExceeded;
		ErrorEntry errorEntry2 = errorEntry;
		errorEntry2.ErrorParameters.Add(new ErrorParameter
		{
			Key = ErrorParameterKey.ParameterValue,
			Value = Links.Count.ToString(CultureInfo.InvariantCulture)
		});
		errorEntry2.ErrorParameters.Add(new ErrorParameter
		{
			Key = ErrorParameterKey.ParameterValue,
			Value = ChannelIndex.ToString(CultureInfo.InvariantCulture)
		});
		errorEntry2.ErrorParameters.Add(new ErrorParameter
		{
			Key = ErrorParameterKey.ParameterValue,
			Value = MaxLinkCount.ToString(CultureInfo.InvariantCulture)
		});
		return new TransformationException(errorEntry2);
	}

	public virtual void AddLink(LinkPartner linkPartner, T config)
	{
		if (Links.ContainsKey(linkPartner))
		{
			throw new LinkAlreadyExistsException();
		}
		Links.Add(linkPartner, config);
	}
}
