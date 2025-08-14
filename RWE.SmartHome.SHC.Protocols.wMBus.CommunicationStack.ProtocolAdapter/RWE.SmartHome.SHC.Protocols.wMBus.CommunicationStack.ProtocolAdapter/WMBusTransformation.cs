using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DomainModel.Rules;
using RWE.SmartHome.SHC.Protocols.wMBus.CommunicationStack.ProtocolAdapter.Interfaces;

namespace RWE.SmartHome.SHC.Protocols.wMBus.CommunicationStack.ProtocolAdapter;

internal class WMBusTransformation : BaseProtocolSpecificTransformation
{
	private const int UI_DEVICE_KEY_LENGTH = 8;

	private readonly IWMBusManager iwmBusManager;

	private IEnumerable<BaseDevice> devicesToBeAdded;

	private IEnumerable<Guid> devicesToBeRemoved;

	public override ProtocolIdentifier ProtocolId => ProtocolIdentifier.wMBus;

	public WMBusTransformation(IWMBusManager iwmBusManager, IRepository configRepository)
		: base(configRepository)
	{
		this.iwmBusManager = iwmBusManager;
	}

	public override bool PrepareTransformation(IElementaryRuleRepository elementaryRuleRepository)
	{
		try
		{
			IDeviceInformation[] deviceInformations;
			lock (iwmBusManager.DeviceList.SyncRoot)
			{
				deviceInformations = iwmBusManager.DeviceList.ToArray();
			}
			devicesToBeAdded = from pd in configRepository.GetBaseDevices()
				where pd.ProtocolId == ProtocolIdentifier.wMBus && deviceInformations.Any((IDeviceInformation deviceInformation) => deviceInformation.DeviceId == pd.Id && !Included(deviceInformation))
				select pd;
			ValidateDeviceKeys(devicesToBeAdded);
			devicesToBeRemoved = from deviceInformation in deviceInformations
				where Included(deviceInformation) && configRepository.GetBaseDevice(deviceInformation.DeviceId) == null
				select deviceInformation.DeviceId;
		}
		catch (TransformationException ex)
		{
			base.Errors.Add(ex.Error);
		}
		ProcessElementaryRules(elementaryRuleRepository);
		return base.Errors.Count > 0;
	}

	public override void CommitTransformationResults(IEnumerable<Guid> devicesToDelete)
	{
		base.DiscardTransformationResults();
		foreach (Guid item in devicesToBeRemoved)
		{
			iwmBusManager.ExcludeDevice(item);
		}
		foreach (BaseDevice item2 in devicesToBeAdded)
		{
			iwmBusManager.IncludeDevice(item2.Id, GetDecryptionKey(item2));
		}
	}

	public override void DiscardTransformationResults()
	{
		base.DiscardTransformationResults();
		devicesToBeAdded = null;
		devicesToBeRemoved = null;
	}

	protected override bool AccelerateRule(ElementaryRule rule)
	{
		return false;
	}

	private void ValidateDeviceKeys(IEnumerable<BaseDevice> devicesToInclude)
	{
		foreach (BaseDevice item in devicesToInclude)
		{
			IDeviceInformation deviceInformation;
			lock (iwmBusManager.DeviceList.SyncRoot)
			{
				deviceInformation = iwmBusManager.DeviceList[item.Id];
			}
			if (deviceInformation.DecryptionKey == null)
			{
				Log.Information(Module.wMBusProtocolAdapter, "The saved device key on the device is null. Check the stored value in backend " + deviceInformation.DeviceIdentifier.ToReadable());
				base.Errors.Add(new ErrorEntry
				{
					AffectedEntity = new EntityMetadata
					{
						EntityType = EntityType.BaseDevice,
						Id = item.Id
					},
					ErrorCode = ValidationErrorCode.InvalidDeviceKey
				});
			}
			else if (!deviceInformation.DecryptionKey.Matches(GetDecryptionKey(item), 0, 0, 8))
			{
				Log.Information(Module.wMBusProtocolAdapter, "The device key entered by the user is invalid for device with id: " + deviceInformation.DeviceIdentifier.ToReadable());
				base.Errors.Add(new ErrorEntry
				{
					AffectedEntity = new EntityMetadata
					{
						EntityType = EntityType.BaseDevice,
						Id = item.Id
					},
					ErrorCode = ValidationErrorCode.InvalidDeviceKey
				});
			}
		}
	}

	private byte[] GetDecryptionKey(BaseDevice pd)
	{
		try
		{
			if (pd.VolatileProperties.FirstOrDefault((Property kvp) => kvp.Name == ProtocolPropertyKey.MBusEncryptionKey.ToString()) is StringProperty stringProperty)
			{
				if (stringProperty.Value != null)
				{
					return stringProperty.Value.ToByteArray();
				}
				return null;
			}
		}
		catch (Exception ex)
		{
			throw new TransformationException("Error retrieving wMBus device key: " + ex.ToString(), new ErrorEntry
			{
				AffectedEntity = new EntityMetadata
				{
					EntityType = EntityType.BaseDevice,
					Id = pd.Id
				},
				ErrorCode = ValidationErrorCode.InvalidDeviceKey
			});
		}
		return null;
	}

	public byte[] ConvertHexStringToByteArray(string hexString)
	{
		if (hexString.Length % 2 != 0)
		{
			throw new TransformationException("The binary key cannot have an odd number of digits: " + hexString, new ErrorEntry
			{
				ErrorCode = ValidationErrorCode.InvalidDeviceKey
			});
		}
		byte[] array = new byte[hexString.Length / 2];
		for (int i = 0; i < array.Length; i++)
		{
			string s = hexString.Substring(i * 2, 2);
			array[i] = byte.Parse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
		}
		return array;
	}

	private bool Included(IDeviceInformation devInfo)
	{
		bool result = false;
		switch (devInfo.DeviceInclusionState)
		{
		case DeviceInclusionState.Included:
			result = true;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case DeviceInclusionState.None:
		case DeviceInclusionState.Found:
		case DeviceInclusionState.InclusionPending:
		case DeviceInclusionState.FactoryReset:
		case DeviceInclusionState.FactoryResetWithAddressCollision:
		case DeviceInclusionState.FoundWithAddressCollision:
		case DeviceInclusionState.ExclusionPending:
		case DeviceInclusionState.Excluded:
			break;
		}
		return result;
	}
}
