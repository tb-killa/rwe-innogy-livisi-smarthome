using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Configuration;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.LemonbeatCoreServices;
using SmartHome.SHC.API.Protocols.Lemonbeat;
using SmartHome.SHC.API.Protocols.Lemonbeat.Configuration;
using SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions;
using SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.ValueDescriptions;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Transformation;

internal class PhysicalConfigurationBuilder
{
	private class TransformedPartnerCalculation
	{
		public PartnerCalculation SourcePartnerCalculation { get; set; }

		public RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation ResultedCalculation { get; set; }
	}

	internal static readonly uint SHC_PARTNER_ID = 1u;

	internal static readonly Guid SHC_BASE_DEVICE_ID = Guid.Empty;

	private static IPAddress ShcLemonbeatAddress = null;

	private readonly IDeviceList deviceList;

	private readonly IShcValueRepository shcValueRepository;

	private readonly Dictionary<Guid, ProfileConfigurationData> profileConfigData;

	private uint nextPartnerIndex;

	private uint nextCalculationIndex;

	private Dictionary<Guid, uint> assignedPartnerIdsByDevice;

	private Guid configuredDeviceId;

	private PhysicalConfiguration builtResult;

	internal PhysicalConfigurationBuilder(IShcValueRepository shcValueRepository, IDeviceList deviceList, Dictionary<Guid, ProfileConfigurationData> profileConfigData)
	{
		this.deviceList = deviceList;
		this.profileConfigData = profileConfigData;
		this.shcValueRepository = shcValueRepository;
		ShcLemonbeatAddress = IPAddress.Parse("fc00::6:0100:0000:0000");
	}

	public PhysicalConfiguration BuildConfiguration(Guid deviceId, TransformationResult result)
	{
		configuredDeviceId = deviceId;
		nextPartnerIndex = 2u;
		assignedPartnerIdsByDevice = new Dictionary<Guid, uint>();
		builtResult = new PhysicalConfiguration();
		AddShcPartner();
		AddValueDescriptions(result.ValueDescriptions);
		AddTimers(result.Timers);
		AddCalendars(result.Calendars);
		AddActions(result.Actions);
		AddStateMachines(result.StateMachines);
		AddCalculations(result.Calculations);
		InitializeCalculationIndex(result);
		AddPartnerCalculations(result.PartnerCalculationAllocations);
		return builtResult;
	}

	private void AddValueDescriptions(List<global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.ValueDescriptions.ValueDescription> list)
	{
		builtResult.VirtualValueDescriptions.AddRange(list.Select((global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.ValueDescriptions.ValueDescription vd) => vd.ToCoreValueDescription()));
	}

	private void AddTimers(List<global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.LemonbeatTimer> apiTimers)
	{
		builtResult.Timers.AddRange(apiTimers.Select((global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.LemonbeatTimer t) => t.ToCoreTimer()));
	}

	private void AddCalendars(List<CalendarTask> apiCalendars)
	{
		builtResult.CalendarEntries.AddRange(apiCalendars.Select((CalendarTask c) => c.ToCoreCalendar()));
	}

	private void AddStateMachines(List<StateMachine> apiStateMachines)
	{
		foreach (StateMachine apiStateMachine in apiStateMachines)
		{
			builtResult.StateMachines.Add(apiStateMachine.ToCoreStateMachine());
		}
	}

	private void AddCalculations(IEnumerable<global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Calculation> apiCalculation)
	{
		builtResult.Calculations.AddRange(apiCalculation.Select((global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Calculation apiC) => apiC.ToCoreCalculation()));
	}

	private void InitializeCalculationIndex(TransformationResult result)
	{
		IEnumerable<uint> source = result.Calculations.Select((global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Calculation c) => c.CalculationId).Union(result.PartnerCalculationAllocations.Select((PartnerCalculationAllocation pca) => pca.CalculationId));
		if (source.Any())
		{
			nextCalculationIndex = source.Max() + 1;
		}
		else
		{
			nextCalculationIndex = 1u;
		}
	}

	private uint GetNextCalculationId()
	{
		return nextCalculationIndex++;
	}

	private void AddPartnerCalculations(IEnumerable<PartnerCalculationAllocation> partnerCalculationAllocations)
	{
		foreach (PartnerCalculationAllocation partnerCalculationAllocation in partnerCalculationAllocations)
		{
			List<PartnerCalculationMember> list = new List<PartnerCalculationMember>();
			list.Add(GetShcCalculationMemberForProfileSettings(partnerCalculationAllocation.ActionDescription.Id));
			foreach (Guid item in list.Select((PartnerCalculationMember m) => m.PartnerBaseDeviceId).Distinct())
			{
				AddPartner(item);
			}
			List<RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation> list2 = new List<RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation>();
			List<RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation> list3 = new List<RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation>();
			foreach (PartnerCalculationMember item2 in list)
			{
				uint? partnerId = null;
				if (configuredDeviceId != item2.PartnerBaseDeviceId)
				{
					if (!assignedPartnerIdsByDevice.TryGetValue(item2.PartnerBaseDeviceId, out var value))
					{
						Log.Warning(Module.LemonbeatProtocolAdapter, $"A partner of the device could not be created in the target configuration. Base device Id: {configuredDeviceId} ");
						continue;
					}
					partnerId = value;
				}
				list3.Add(TransformPartnerCalculation(partnerId, item2.PartnerCalculation, out var additionalCalculations));
				list2.AddRange(additionalCalculations);
			}
			if (list3.Count == 1)
			{
				list3.First().Id = partnerCalculationAllocation.CalculationId;
			}
			else
			{
				List<RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation> list4 = new List<RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation>();
				RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation orCalculation = GetOrCalculation(list3, list4, partnerCalculationAllocation.CalculationId);
				builtResult.Calculations.Add(orCalculation);
				builtResult.Calculations.AddRange(list4);
			}
			builtResult.Calculations.AddRange(list3);
			builtResult.Calculations.AddRange(list2);
		}
	}

	private PartnerCalculationMember GetShcCalculationMemberForProfileSettings(Guid profileSettingsId)
	{
		if (!shcValueRepository.TryGetValueId("core", profileSettingsId.ToString(), out var valueId))
		{
			valueId = shcValueRepository.RegisterValue("core", profileSettingsId.ToString());
		}
		return new PartnerCalculationMember(SHC_BASE_DEVICE_ID, new PartnerCalculation(global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.CalculationMethod.Equal, new PartnerCalculationOperand
		{
			ValueId = valueId
		}, new PartnerCalculationOperand
		{
			ConstantNumber = ShcValueContainer.SHC_SWITCH_VALUE
		}));
	}

	private RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation TransformPartnerCalculation(uint? partnerId, PartnerCalculation partnerCalculation, out List<RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation> additionalCalculations)
	{
		additionalCalculations = new List<RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation>();
		List<TransformedPartnerCalculation> list = new List<TransformedPartnerCalculation>();
		RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation result = TransformPartnerCalculationRecursively(partnerId, partnerCalculation, list);
		additionalCalculations.AddRange(list.Select((TransformedPartnerCalculation tc) => tc.ResultedCalculation));
		return result;
	}

	private RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation TransformPartnerCalculationRecursively(uint? partnerId, PartnerCalculation partnerCalculation, List<TransformedPartnerCalculation> transformedCalculations)
	{
		RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation calculation = new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation();
		calculation.Id = GetNextCalculationId();
		calculation.Method = partnerCalculation.Method.ToCoreCalculationMethod();
		calculation.Left = TransformPartnerCalculationOperand(partnerId, partnerCalculation.Left, transformedCalculations);
		calculation.Right = TransformPartnerCalculationOperand(partnerId, partnerCalculation.Right, transformedCalculations);
		return calculation;
	}

	private RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationOperand TransformPartnerCalculationOperand(uint? partnerId, PartnerCalculationOperand partnerCalculationOperand, List<TransformedPartnerCalculation> additionalCalculations)
	{
		RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationOperand calculationOperand = new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationOperand();
		if (partnerCalculationOperand.Calculation == null)
		{
			calculationOperand.IsUpdated = partnerCalculationOperand.IsUpdated;
			calculationOperand.ConstantString = partnerCalculationOperand.ConstantString;
			calculationOperand.ConstantBinary = partnerCalculationOperand.ConstantBinary;
			calculationOperand.ConstantNumber = (partnerCalculationOperand.ConstantNumber.HasValue ? new double?(Convert.ToDouble(partnerCalculationOperand.ConstantNumber.Value)) : ((double?)null));
			calculationOperand.ValueId = partnerCalculationOperand.ValueId;
			if (partnerCalculationOperand.ValueId.HasValue)
			{
				calculationOperand.PartnerId = partnerId;
			}
		}
		else
		{
			TransformedPartnerCalculation transformedPartnerCalculation = additionalCalculations.Where((TransformedPartnerCalculation c) => c.SourcePartnerCalculation == partnerCalculationOperand.Calculation).FirstOrDefault();
			if (transformedPartnerCalculation == null)
			{
				RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation calculation = TransformPartnerCalculationRecursively(partnerId, partnerCalculationOperand.Calculation, additionalCalculations);
				calculationOperand.CalculationId = calculation.Id;
				additionalCalculations.Add(new TransformedPartnerCalculation
				{
					SourcePartnerCalculation = partnerCalculationOperand.Calculation,
					ResultedCalculation = calculation
				});
			}
			else
			{
				calculationOperand.CalculationId = transformedPartnerCalculation.ResultedCalculation.Id;
			}
		}
		return calculationOperand;
	}

	private RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation GetOrCalculation(IEnumerable<RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation> operands, List<RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation> additionalOrCalculations, uint assignedCalculationId)
	{
		if (operands.Count() < 2)
		{
			throw new ArgumentException("The list of operands should contain at least 2 elements");
		}
		RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation calculation = new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation();
		calculation.Id = assignedCalculationId;
		calculation.Method = RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationMethod.Or;
		calculation.Left = new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationOperand
		{
			CalculationId = operands.First().Id
		};
		RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation calculation2 = calculation;
		RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationOperand calculationOperand = new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationOperand();
		if (operands.Count() == 2)
		{
			RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationOperand calculationOperand2 = new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationOperand();
			calculationOperand2.CalculationId = operands.Last().Id;
			calculationOperand = calculationOperand2;
		}
		else
		{
			RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation orCalculation = GetOrCalculation(operands.Skip(1), additionalOrCalculations, GetNextCalculationId());
			additionalOrCalculations.Add(orCalculation);
			RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationOperand calculationOperand3 = new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationOperand();
			calculationOperand3.CalculationId = orCalculation.Id;
			calculationOperand = calculationOperand3;
		}
		calculation2.Right = calculationOperand;
		return calculation2;
	}

	private uint GetNextPartnerId()
	{
		return nextPartnerIndex++;
	}

	private uint AddPartner(Guid partnerBaseDeviceId)
	{
		uint value = 0u;
		if (partnerBaseDeviceId != configuredDeviceId && !assignedPartnerIdsByDevice.TryGetValue(partnerBaseDeviceId, out value))
		{
			DeviceInformation deviceInformation = deviceList[partnerBaseDeviceId];
			if (deviceInformation != null)
			{
				value = GetNextPartnerId();
				builtResult.Partners.Add(CreatePartner(value, deviceInformation));
				assignedPartnerIdsByDevice.Add(partnerBaseDeviceId, value);
			}
			else
			{
				Log.Warning(Module.LemonbeatProtocolAdapter, $"AddPartner() => Device with id {partnerBaseDeviceId} could not be found in the list.");
			}
		}
		return value;
	}

	private void AddShcPartner()
	{
		Partner partner = new Partner();
		partner.Id = SHC_PARTNER_ID;
		partner.Identifier = new DeviceIdentifier(ShcLemonbeatAddress, null, LemonbeatUsbDongle.LemonbeatUSBDongleGatewayID);
		builtResult.Partners.Add(partner);
		assignedPartnerIdsByDevice.Add(SHC_BASE_DEVICE_ID, SHC_PARTNER_ID);
	}

	private Partner CreatePartner(uint partnerId, DeviceInformation partnerDevice)
	{
		Partner partner = new Partner();
		partner.Id = partnerId;
		partner.Identifier = partnerDevice.Identifier;
		partner.WakeupChannel = partnerDevice.DeviceDescription.WakeupChannel;
		partner.WakeupInterval = partnerDevice.DeviceDescription.WakeupInterval;
		partner.WakeupMode = partnerDevice.DeviceDescription.RadioMode;
		partner.WakeupOffset = partnerDevice.DeviceDescription.WakeupOffset;
		return partner;
	}

	private Partner CreateMulticastPartner(uint partnerId, IPAddress multicastAddress)
	{
		Partner partner = new Partner();
		partner.Id = partnerId;
		partner.Identifier = new DeviceIdentifier(multicastAddress, null, LemonbeatUsbDongle.LemonbeatUSBDongleGatewayID);
		return partner;
	}

	private Group CreateGroup(uint groupId, List<uint> partnerIds)
	{
		Group obj = new Group();
		obj.Id = groupId;
		obj.PartnerIds = partnerIds.ToArray();
		return obj;
	}

	private uint UpdatePartnersForTriggerDevice(Guid profileId)
	{
		uint num = 0u;
		if (!profileConfigData.TryGetValue(profileId, out var configDataForProfile))
		{
			Log.Warning(Module.LemonbeatProtocolAdapter, "PhysicalConfigurationBuilder: No config data found for profile with id " + profileId);
			return num;
		}
		List<uint> list = new List<uint>();
		IEnumerable<Guid> enumerable = configDataForProfile.DirectLinkProfileExecuters.Where((Guid id) => id != configuredDeviceId);
		if (enumerable.Count() > 0)
		{
			foreach (Guid item in enumerable)
			{
				uint num2 = AddPartner(item);
				if (num2 != 0)
				{
					list.Add(num2);
				}
			}
			if (list.Count > 0)
			{
				if (list.Count == 1)
				{
					num = list.First();
				}
				else
				{
					Partner multicastPartner = builtResult.Partners.Where((Partner p) => p.Identifier.IPAddress.Equals(configDataForProfile.ProfileMulticastAddress)).FirstOrDefault();
					if (multicastPartner == null)
					{
						uint nextPartnerId = GetNextPartnerId();
						builtResult.Partners.Add(CreateMulticastPartner(nextPartnerId, configDataForProfile.ProfileMulticastAddress));
						list.Add(nextPartnerId);
						num = GetNextPartnerId();
						builtResult.PartnerGroups.Add(new Group
						{
							Id = num,
							PartnerIds = list.ToArray()
						});
					}
					else
					{
						num = builtResult.PartnerGroups.Where((Group pg) => pg.PartnerIds.Contains(multicastPartner.Id)).First().Id;
					}
				}
			}
		}
		return num;
	}

	private void AddActions(List<global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions.LemonbeatAction> apiActions)
	{
		foreach (global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions.LemonbeatAction apiAction in apiActions)
		{
			RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.LemonbeatAction lemonbeatAction = ConvertAction(apiAction);
			if (lemonbeatAction != null)
			{
				builtResult.Actions.Add(lemonbeatAction);
			}
		}
	}

	private RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.LemonbeatAction ConvertAction(global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions.LemonbeatAction action)
	{
		List<RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.ActionItem> list = (from i in action.Items
			select ConvertActionItem(i) into i
			where i != null
			select i).ToList();
		if (list.Count > 0)
		{
			RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.LemonbeatAction lemonbeatAction = new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.LemonbeatAction();
			lemonbeatAction.Id = action.ActionId;
			lemonbeatAction.Items = list.Distinct().ToList();
			return lemonbeatAction;
		}
		return null;
	}

	private RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.ActionItem ConvertActionItem(global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions.ActionItem actionItem)
	{
		if (actionItem is global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions.SetNumberActionItem actionItem2)
		{
			return ConvertActionItem(actionItem2);
		}
		if (actionItem is global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions.SetStringActionItem actionItem3)
		{
			return ConvertActionItem(actionItem3);
		}
		if (actionItem is global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions.SetCalculationActionItem actionItem4)
		{
			return ConvertActionItem(actionItem4);
		}
		if (actionItem is global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions.ValueReportActionItem actionItem5)
		{
			return ConvertActionItem(actionItem5);
		}
		if (actionItem is global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions.TimerActionItem actionItem6)
		{
			return ConvertActionItem(actionItem6);
		}
		throw new ArgumentException("Input does not have a recognized action item type");
	}

	private RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.SetNumberActionItem ConvertActionItem(global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions.SetNumberActionItem actionItem)
	{
		RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.SetNumberActionItem setNumberActionItem = new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.SetNumberActionItem();
		setNumberActionItem.Value = Convert.ToDouble(actionItem.Value);
		setNumberActionItem.ValueId = actionItem.ValueId;
		return setNumberActionItem;
	}

	private RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.SetHexBinaryActionItem ConvertActionItem(global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions.SetHexBinaryActionItem actionItem)
	{
		RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.SetHexBinaryActionItem setHexBinaryActionItem = new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.SetHexBinaryActionItem();
		setHexBinaryActionItem.Value = actionItem.Value.ToArray();
		setHexBinaryActionItem.ValueId = actionItem.ValueId;
		return setHexBinaryActionItem;
	}

	private RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.SetStringActionItem ConvertActionItem(global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions.SetStringActionItem actionItem)
	{
		RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.SetStringActionItem setStringActionItem = new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.SetStringActionItem();
		setStringActionItem.Value = actionItem.Value;
		setStringActionItem.ValueId = actionItem.ValueId;
		return setStringActionItem;
	}

	private RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.SetCalculationActionItem ConvertActionItem(global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions.SetCalculationActionItem actionItem)
	{
		RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.SetCalculationActionItem setCalculationActionItem = new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.SetCalculationActionItem();
		setCalculationActionItem.ValueId = actionItem.ValueId;
		setCalculationActionItem.CalculationId = actionItem.CalculationId;
		return setCalculationActionItem;
	}

	private RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.TimerActionItem ConvertActionItem(global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions.TimerActionItem actionItem)
	{
		RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.TimerActionItem timerActionItem = new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.TimerActionItem();
		timerActionItem.ActionType = ConvertTimerActionType(actionItem.ActionType);
		timerActionItem.TimerId = actionItem.TimerId;
		return timerActionItem;
	}

	private RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.ValueReportActionItem ConvertActionItem(global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions.ValueReportActionItem actionItem)
	{
		uint num = (actionItem.Partner.IsShc ? SHC_PARTNER_ID : UpdatePartnersForTriggerDevice(actionItem.Partner.InteractionId));
		if (num != 0)
		{
			RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.ValueReportActionItem valueReportActionItem = new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.ValueReportActionItem();
			valueReportActionItem.ActionType = ConvertValueReportActionType(actionItem.ActionType);
			valueReportActionItem.ValueId = actionItem.ValueId;
			valueReportActionItem.PartnerId = num;
			valueReportActionItem.TransportMode = ConvertTransportMode(actionItem.TransportMode);
			return valueReportActionItem;
		}
		return null;
	}

	private RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.TimerActionType ConvertTimerActionType(global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions.TimerActionType actionType)
	{
		return actionType switch
		{
			global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions.TimerActionType.StartTimer => RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.TimerActionType.StartTimer, 
			global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions.TimerActionType.StopTimer => RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.TimerActionType.StopTimer, 
			_ => throw new ArgumentException("API timer action type value not supported: " + actionType), 
		};
	}

	private RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.ValueReportActionType ConvertValueReportActionType(global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions.ValueReportActionType actionType)
	{
		return actionType switch
		{
			global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions.ValueReportActionType.Get => RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.ValueReportActionType.Get, 
			global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions.ValueReportActionType.Send => RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions.ValueReportActionType.Send, 
			_ => throw new ArgumentException("API value report action type value not supported: " + actionType), 
		};
	}

	private TransportModeType? ConvertTransportMode(Transport? transportMode)
	{
		if (transportMode.HasValue)
		{
			switch (transportMode)
			{
			case Transport.Udp:
				return TransportModeType.Udp;
			case Transport.Tcp:
				return TransportModeType.Tcp;
			}
		}
		return null;
	}
}
