using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Enums;
using RWE.SmartHome.SHC.DomainModel.Actions;
using RWE.SmartHome.SHC.DomainModel.Types;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.Exceptions;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler.ActuatorHandler.SirenHandler;

internal class SirenAlarmActuatorHandler : IActuatorHandlerStringTypes, IActuatorHandler, ILogicalDeviceHandler
{
	private const string ActiveChannelPropertyName = "ActiveChannel";

	private static readonly string[] supportedActions = new string[3] { "SetState", "ActivateSiren", "TestSound" };

	private readonly object sync = new object();

	private readonly IRepository configRepo;

	private readonly IDeviceManager deviceManager;

	private readonly IEventManager eventManager;

	private readonly IDictionary<Guid, ActiveChannels> capabilityActiveChannels;

	private readonly IDictionary<Guid, TestSoundBlocker> testSoundBlockers;

	public IEnumerable<string> SupportedActuatorTypes
	{
		get
		{
			List<string> list = new List<string>();
			list.Add("SirenActuator");
			return list;
		}
	}

	public IEnumerable<byte> StatusInfoChannels => ChannelsMapperBidcos.GetAllChannels();

	public bool IsStatusRequestAllowed => true;

	public int MinStatusRequestPollingIterval => 86400;

	public SirenAlarmActuatorHandler(IRepository configRepo, IDeviceManager deviceManager, IEventManager eventManager)
	{
		this.configRepo = configRepo;
		this.deviceManager = deviceManager;
		this.eventManager = eventManager;
		capabilityActiveChannels = new Dictionary<Guid, ActiveChannels>();
		testSoundBlockers = new Dictionary<Guid, TestSoundBlocker>();
	}

	public bool GetIsPeriodicStatusPollingActive(LogicalDevice logicalDevice)
	{
		return true;
	}

	public LogicalDeviceState CreateLogicalDeviceState(LogicalDevice logicalDevice, SortedList<byte, ChannelState> channelStates)
	{
		List<byte> source = StatusInfoChannels.ToList();
		GenericDeviceState genericDeviceState = null;
		if (logicalDevice != null && source.Any(channelStates.ContainsKey))
		{
			GenericDeviceState genericDeviceState2 = new GenericDeviceState();
			genericDeviceState2.LogicalDeviceId = logicalDevice.Id;
			genericDeviceState = genericDeviceState2;
			if (channelStates.Any())
			{
				KeyValuePair<byte, ChannelState> keyValuePair = channelStates.FirstOrDefault();
				byte key = keyValuePair.Key;
				bool state = keyValuePair.Value.Value == 200;
				ActiveChannels activeChannels = GetCapabilityActiveChannels(logicalDevice.Id);
				activeChannels.ChannelStateChange(key, state);
				byte? activeChannel = activeChannels.GetActiveChannel();
				SIRChannel locgicChannel = ChannelsMapperBidcos.GetLocgicChannel(activeChannel);
				string stringValue = SIRChannelMapper.GetStringValue(locgicChannel);
				genericDeviceState.Properties = new List<Property>
				{
					new StringProperty("ActiveChannel", stringValue)
					{
						UpdateTimestamp = ShcDateTime.UtcNow
					}
				};
				TestSoundBlocker testSoundBlocker = GetTestSoundBlocker(logicalDevice.BaseDeviceId);
				testSoundBlocker.UpdateTestSoundBlock(locgicChannel);
			}
		}
		return genericDeviceState;
	}

	public bool CanExecuteAction(ActionDescription action)
	{
		return Array.IndexOf(supportedActions, action.ActionType) >= 0;
	}

	public IEnumerable<SwitchSettings> CreateCosIpCommand(ActionContext ac, ActionDescription action)
	{
		if (!IsSirenIncludedConfigured(action.Target))
		{
			throw new ExecuteActionException("DeviceNotConfigured");
		}
		return action.ActionType switch
		{
			"ActivateSiren" => ActivateSiren(ac, action), 
			"SetState" => SetState(ac, action), 
			"TestSound" => TestSoundCommands(ac, action), 
			_ => throw new ArgumentException($"Unknown action type ({action.ActionType})"), 
		};
	}

	private IEnumerable<SwitchSettings> ActivateSiren(ActionContext ac, ActionDescription action)
	{
		byte channelFromParameters = GetChannelFromParameters(action.Data);
		decimal? offTimerFromParameter = GetOffTimerFromParameter(action.Data);
		if (offTimerFromParameter.HasValue && (offTimerFromParameter.Value <= 0m || offTimerFromParameter.Value > 1200m))
		{
			throw new ArgumentException("The offtimer value must be between 1 and 120000 ms");
		}
		List<SwitchSettings> list = new List<SwitchSettings>();
		list.Add(new SwitchSettingsDirectExecution(RampMode.RampStart, 0, channelFromParameters, 200, offTimerFromParameter.HasValue ? new int?((int)Math.Ceiling((double)offTimerFromParameter.Value)) : ((int?)null)));
		return list;
	}

	private IEnumerable<SwitchSettings> SetState(ActionContext ac, ActionDescription action)
	{
		byte? activeChannelFromParameters = GetActiveChannelFromParameters(action.Data);
		List<SwitchSettings> list = new List<SwitchSettings>();
		if (activeChannelFromParameters.HasValue)
		{
			byte value = activeChannelFromParameters.Value;
			list.Add(new SwitchSettingsDirectExecution(RampMode.RampStart, 0, value, 200, null));
		}
		else
		{
			ActiveChannels activeChannels = GetCapabilityActiveChannels(new Guid(action.Target.EntityId));
			byte[] allChannelsWithActivePriority = activeChannels.GetAllChannelsWithActivePriority();
			for (int i = 0; i < allChannelsWithActivePriority.Length; i++)
			{
				list.Add(new SwitchSettingsDirectExecution(RampMode.RampStart, 0, allChannelsWithActivePriority[i], 0, null));
			}
		}
		return list;
	}

	private IEnumerable<SwitchSettings> TestSoundCommands(ActionContext ac, ActionDescription action)
	{
		if (IsSirenBusy(action.Target))
		{
			throw new ExecuteActionException("SirenIsBusy");
		}
		BlockTestSound(action.Target);
		byte channelFromParameters = GetChannelFromParameters(action.Data);
		int num = (int)(GetOffTimerFromParameter(action.Data) ?? 2000m);
		byte soundFromParameters = GetSoundFromParameters(action.Data);
		Guid id = action.Target.EntityIdAsGuid();
		LogicalDevice logicalDevice = configRepo.GetLogicalDevice(id);
		string stringValue = logicalDevice.Properties.GetStringValue(ChannelsMapperBidcos.GetChannelPropertyName(channelFromParameters));
		byte soundValue = GetSoundValue(stringValue);
		List<SwitchSettings> list = new List<SwitchSettings>();
		list.Add(new SwitchSettingsTestSound
		{
			Channel = channelFromParameters,
			CurrentSoundId = soundValue,
			Delay = num * 100,
			SoundId = soundFromParameters,
			SourceAddress = deviceManager.DefaultShcAddress
		});
		return list;
	}

	private bool IsSirenBusy(LinkBinding link)
	{
		ActiveChannels activeChannels = GetCapabilityActiveChannels(new Guid(link.EntityId));
		byte? activeChannel = activeChannels.GetActiveChannel();
		SIRChannel locgicChannel = ChannelsMapperBidcos.GetLocgicChannel(activeChannel);
		Guid? baseDeviceId = GetBaseDeviceId(link);
		TestSoundBlocker testSoundBlocker = (baseDeviceId.HasValue ? GetTestSoundBlocker(baseDeviceId.Value) : null);
		if (locgicChannel == SIRChannel.None)
		{
			return testSoundBlocker?.IsBlocked() ?? false;
		}
		return true;
	}

	private void BlockTestSound(LinkBinding link)
	{
		Guid? baseDeviceId = GetBaseDeviceId(link);
		(baseDeviceId.HasValue ? GetTestSoundBlocker(baseDeviceId.Value) : null)?.BlockTestSound();
	}

	private bool IsSirenIncludedConfigured(LinkBinding link)
	{
		Guid? baseDeviceId = GetBaseDeviceId(link);
		if (baseDeviceId.HasValue)
		{
			IDeviceInformation deviceInformation = deviceManager.DeviceList[baseDeviceId.Value];
			if (deviceInformation != null)
			{
				if (deviceInformation.DeviceInclusionState == DeviceInclusionState.Included)
				{
					return deviceInformation.DeviceConfigurationState == DeviceConfigurationState.Complete;
				}
				return false;
			}
		}
		return false;
	}

	private Guid? GetBaseDeviceId(LinkBinding link)
	{
		if (link != null && link.LinkType == EntityType.LogicalDevice)
		{
			Guid guid = new Guid(link.EntityId);
			if (guid != Guid.Empty)
			{
				LogicalDevice logicalDevice = configRepo.GetLogicalDevice(guid);
				if (logicalDevice != null)
				{
					return logicalDevice.BaseDeviceId;
				}
			}
		}
		return null;
	}

	private byte GetChannelFromParameters(IEnumerable<Parameter> parameters)
	{
		Parameter parameter = parameters.FirstOrDefault((Parameter m) => m.Name.Equals("Channel"));
		if (parameter == null)
		{
			throw new ArgumentException(string.Format("{0} parameter is required", "Channel"));
		}
		string value = ((ConstantStringBinding)parameter.Value).Value;
		SIRChannel channel = SIRChannelMapper.GetChannel(value);
		byte? bidcosChannel = ChannelsMapperBidcos.GetBidcosChannel(channel);
		if (!bidcosChannel.HasValue)
		{
			throw new ArgumentException($"Unknown channel {value}");
		}
		return bidcosChannel.Value;
	}

	private decimal? GetOffTimerFromParameter(IEnumerable<Parameter> parameters)
	{
		Parameter parameter = parameters.FirstOrDefault((Parameter m) => m.Name.Equals("OffTimer"));
		decimal? num = null;
		decimal? result = null;
		if (parameter != null)
		{
			num = ((ConstantNumericBinding)parameter.Value).Value;
			if (num.HasValue)
			{
				result = num / (decimal?)100m;
			}
		}
		if (result.HasValue && result.Value < 0m)
		{
			throw new ArgumentException($"OffTimerValue cannot be negative (val:{num})");
		}
		return result;
	}

	private byte GetSoundFromParameters(IEnumerable<Parameter> parameters)
	{
		Parameter parameter = parameters.FirstOrDefault((Parameter m) => m.Name.Equals("SoundId"));
		if (parameter == null)
		{
			throw new ArgumentException($"{parameter} parameter are required");
		}
		string value = ((ConstantStringBinding)parameter.Value).Value;
		return GetSoundValue(value);
	}

	private byte? GetActiveChannelFromParameters(IEnumerable<Parameter> parameters)
	{
		Parameter parameter = parameters.FirstOrDefault((Parameter m) => m.Name.Equals("ActiveChannel"));
		if (parameter == null)
		{
			throw new ArgumentException(string.Format("{0} parameter is required", "ActiveChannel"));
		}
		string value = ((ConstantStringBinding)parameter.Value).Value;
		SIRChannel channel = SIRChannelMapper.GetChannel(value);
		return ChannelsMapperBidcos.GetBidcosChannel(channel);
	}

	private byte GetSoundValue(string sound)
	{
		if (sound == null || sound.Length != 3)
		{
			throw new ArgumentException($"Unknown format for sound value (val:{sound})");
		}
		byte signalValue = GetSignalValue(sound);
		byte toneValue = GetToneValue(sound);
		byte speedValue = GetSpeedValue(sound);
		return (byte)(speedValue * 36 + toneValue * 9 + signalValue);
	}

	private byte GetSignalValue(string sound)
	{
		int num = sound[0] - 48 + 1;
		if (num < 1 || num > 9)
		{
			throw new ArgumentException($"Unknown signal value ({num - 1})");
		}
		return (byte)num;
	}

	private byte GetToneValue(string sound)
	{
		byte b = 0;
		return sound[1] switch
		{
			'L' => 0, 
			'M' => 1, 
			'H' => 2, 
			'V' => 3, 
			_ => throw new ArgumentException($"Unknown tone value ({sound[1]})"), 
		};
	}

	private byte GetSpeedValue(string sound)
	{
		byte b = 0;
		return sound[2] switch
		{
			'S' => 0, 
			'F' => 1, 
			_ => throw new ArgumentException($"Unknown speed value ({sound[2]})"), 
		};
	}

	private TestSoundBlocker GetTestSoundBlocker(Guid id)
	{
		TestSoundBlocker value = null;
		if (!testSoundBlockers.TryGetValue(id, out value))
		{
			value = new TestSoundBlocker(TimeSpan.FromSeconds(30.0));
			testSoundBlockers.Add(id, value);
		}
		return value;
	}

	private ActiveChannels GetCapabilityActiveChannels(Guid id)
	{
		ActiveChannels value = null;
		lock (sync)
		{
			if (!capabilityActiveChannels.TryGetValue(id, out value))
			{
				value = new ActiveChannels(3);
				capabilityActiveChannels.Add(id, value);
			}
		}
		return value;
	}
}
