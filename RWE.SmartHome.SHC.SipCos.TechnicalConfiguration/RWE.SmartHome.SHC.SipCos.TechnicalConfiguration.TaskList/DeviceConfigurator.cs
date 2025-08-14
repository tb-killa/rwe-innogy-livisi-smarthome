using System;
using System.Collections.Generic;
using System.Resources;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.ErrorHandling;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.TaskList;

internal class DeviceConfigurator
{
	private const string LoggingSource = "TechnicalConfiguration";

	private readonly Guid ownDeviceId;

	private readonly IDeviceManager deviceManager;

	private readonly ISipcosConfigurator sequenceGenerator;

	private readonly byte[] ownAddress;

	private readonly IDictionary<Guid, Guid> changeAssociations;

	private readonly IDictionary<Guid, ConfigurationChange> changes;

	public DeviceConfigurator(IDeviceManager deviceManager, ISipcosConfigurator sequenceGenerator, IDictionary<Guid, Guid> globalChangeAssociations, IDictionary<Guid, ConfigurationChange> pendingChanges, Guid deviceId)
	{
		this.deviceManager = deviceManager;
		changes = pendingChanges;
		changeAssociations = globalChangeAssociations;
		ownDeviceId = deviceId;
		IDeviceInformation deviceInformation = deviceManager.DeviceList[deviceId];
		if (deviceInformation != null)
		{
			ownAddress = deviceInformation.Address;
			this.sequenceGenerator = sequenceGenerator;
		}
		else
		{
			LogError(ErrorCode.DeviceDoesNotExist, deviceId);
		}
	}

	public void ConfigureDevice(DeviceConfigurationDiff diff)
	{
		if (diff == null || sequenceGenerator == null)
		{
			return;
		}
		foreach (KeyValuePair<byte, ConfigurationChannelDiff> channelDiff in diff.ChannelDiffs)
		{
			ConfigureChannel(channelDiff.Key, channelDiff.Value);
		}
	}

	private void ConfigureChannel(byte channelNo, ConfigurationChannelDiff channelDiff)
	{
		if (channelDiff == null || sequenceGenerator == null)
		{
			return;
		}
		foreach (LinkPartner item in channelDiff.ToDelete)
		{
			DeleteLink(item, channelNo);
		}
		foreach (KeyValuePair<LinkPartner, ConfigurationLink> item2 in channelDiff.ToCreate)
		{
			LinkPartner key = item2.Key;
			ConfigureLink(channelNo, key, GetPartnerOperationMode(key), item2.Value, withCreate: true);
		}
		foreach (KeyValuePair<LinkPartner, ConfigurationLink> item3 in channelDiff.ToChange)
		{
			LinkPartner key2 = item3.Key;
			ConfigureLink(channelNo, key2, GetPartnerOperationMode(key2), item3.Value, withCreate: false);
		}
	}

	private byte GetPartnerOperationMode(LinkPartner linkPartner)
	{
		if (linkPartner == LinkPartner.Empty)
		{
			return 0;
		}
		if (linkPartner.DeviceId == Guid.Empty)
		{
			return 1;
		}
		return deviceManager.DeviceList[linkPartner.DeviceId]?.AllOperationModes ?? 0;
	}

	private void CreateLink(byte channelNo, LinkPartner linkPartner, byte operationMode)
	{
		Log.Debug(Module.ConfigurationTransformation, "TechnicalConfiguration", $"DeviceConfigurator.CreateLink: target=[{ownAddress.ToReadable()}:{channelNo}] linkpartner=[{linkPartner.Address.ToReadable()}:{linkPartner.Channel}]");
		Guid key = sequenceGenerator.CreateLink(ownAddress, channelNo, linkPartner.Address, linkPartner.Channel, operationMode);
		changeAssociations.Add(key, ownDeviceId);
		changes.Add(key, new CreateLinkChange(channelNo, linkPartner));
	}

	private void DeleteLink(LinkPartner linkToDelete, byte channelNo)
	{
		Log.Debug(Module.ConfigurationTransformation, "TechnicalConfiguration", $"DeviceConfigurator.DeleteLink: target=[{ownAddress.ToReadable()}:{channelNo}] linkpartner=[{linkToDelete.Address.ToReadable()}:{linkToDelete.Channel}]");
		Guid key = sequenceGenerator.RemoveLink(ownAddress, channelNo, linkToDelete.Address, linkToDelete.Channel);
		changeAssociations.Add(key, ownDeviceId);
		changes.Add(key, new RemoveLinkChange(channelNo, linkToDelete));
	}

	public void ConfigureLink(byte channelNo, LinkPartner linkPartner, byte operationMode, ConfigurationLink configLink, bool withCreate)
	{
		if (sequenceGenerator == null)
		{
			return;
		}
		if (withCreate && configLink.ParameterLists.Count == 0)
		{
			CreateLink(channelNo, linkPartner, operationMode);
			return;
		}
		foreach (KeyValuePair<byte, ParameterList> parameterList in configLink.ParameterLists)
		{
			byte key = parameterList.Key;
			ParameterList value = parameterList.Value;
			byte[] parameters = value.ToArray();
			Log.Debug(Module.ConfigurationTransformation, "TechnicalConfiguration", $"DeviceConfigurator.ConfigureLink: target=[{ownAddress.ToReadable()}:{channelNo}] linkpartner=[{linkPartner.Address.ToReadable()}:{linkPartner.Channel}] parameters:[{key} | {value.ToReadable()}]");
			Guid key2 = sequenceGenerator.ConfigureLink(ownAddress, channelNo, linkPartner.Address, linkPartner.Channel, key, parameters, operationMode, withCreate);
			withCreate = false;
			changeAssociations.Add(key2, ownDeviceId);
			changes.Add(key2, new ConfigureLinkChange(channelNo, linkPartner, key, value));
		}
	}

	private void LogError(ErrorCode errorCode, params object[] values)
	{
		ResourceManager resourceManager = new ResourceManager(typeof(ErrorStrings));
		string text = resourceManager.GetString(errorCode.ToString());
		string message = (string.IsNullOrEmpty(text) ? errorCode.ToString() : string.Format(text, values));
		Log.Error(Module.ConfigurationTransformation, "TechnicalConfiguration", message);
	}
}
