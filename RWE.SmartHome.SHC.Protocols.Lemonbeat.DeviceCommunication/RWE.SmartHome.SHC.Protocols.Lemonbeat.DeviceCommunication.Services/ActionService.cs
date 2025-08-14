using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Action;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Services;

public class ActionService : SenderService<network>, IActionService
{
	private const string DefaultNamespace = "urn:actionxsd";

	public ActionService(ILemonbeatCommunication aggregator)
		: base(aggregator, ServiceType.Action, "urn:actionxsd")
	{
	}

	public IEnumerable<LemonbeatAction> GetAllActions(DeviceIdentifier identifier)
	{
		network network = CreateNetworkMessage();
		network.device[0].ItemsElementName = new ItemsChoiceType1[1] { ItemsChoiceType1.action_get };
		network.device[0].Items = new object[1]
		{
			new actionGetType()
		};
		network.device[0].device_idSpecified = identifier.SubDeviceId.HasValue;
		network.device[0].device_id = identifier.SubDeviceId ?? 0;
		network message = SendRequest(identifier, network);
		return ParseActions(message, identifier.SubDeviceId ?? 0);
	}

	public void SetAndDeleteActions(DeviceIdentifier identifier, ICollection<LemonbeatAction> actionsToSet, IEnumerable<uint> actionsToDelete)
	{
		List<KeyValuePair<ItemsChoiceType1, object>> list = new List<KeyValuePair<ItemsChoiceType1, object>>();
		if (actionsToSet != null)
		{
			List<actionType> list2 = new List<actionType>();
			foreach (LemonbeatAction item in actionsToSet)
			{
				ItemsChoiceType[] array = new ItemsChoiceType[item.Items.Count];
				object[] array2 = new object[item.Items.Count];
				int num = 0;
				foreach (ActionItem item2 in item.Items)
				{
					KeyValuePair<ItemsChoiceType, object> keyValuePair = ToMessage(item2);
					array[num] = keyValuePair.Key;
					array2[num] = keyValuePair.Value;
					num++;
				}
				list2.Add(new actionType
				{
					action_id = (byte)item.Id,
					Items = array2,
					ItemsElementName = array
				});
			}
			if (list2.Count > 0)
			{
				list.Add(new KeyValuePair<ItemsChoiceType1, object>(ItemsChoiceType1.action_set, new actionSetType
				{
					action = list2.ToArray()
				}));
			}
		}
		if (actionsToDelete != null)
		{
			foreach (uint item3 in actionsToDelete)
			{
				list.Add(new KeyValuePair<ItemsChoiceType1, object>(ItemsChoiceType1.action_delete, new actionDeleteType
				{
					action_id = (byte)item3,
					action_idSpecified = true
				}));
			}
		}
		if (list.Count <= 0)
		{
			return;
		}
		network network = CreateNetworkMessage();
		network.device[0].ItemsElementName = new ItemsChoiceType1[list.Count];
		network.device[0].Items = new object[list.Count];
		int num2 = 0;
		foreach (KeyValuePair<ItemsChoiceType1, object> item4 in list)
		{
			network.device[0].ItemsElementName[num2] = item4.Key;
			network.device[0].Items[num2] = item4.Value;
			num2++;
		}
		network.device[0].device_idSpecified = identifier.SubDeviceId.HasValue;
		network.device[0].device_id = identifier.SubDeviceId ?? 0;
		SendMessage(identifier, network, TransportType.Connection);
	}

	private network CreateNetworkMessage()
	{
		network network = new network();
		network.version = 1u;
		network.device = new networkDevice[1]
		{
			new networkDevice
			{
				version = 1u
			}
		};
		return network;
	}

	private IEnumerable<LemonbeatAction> ParseActions(network message, uint subDeviceId)
	{
		List<LemonbeatAction> list = new List<LemonbeatAction>();
		if (message != null && message.device != null)
		{
			networkDevice networkDevice = message.device.Where((networkDevice d) => d.device_id == subDeviceId).FirstOrDefault();
			if (networkDevice != null && networkDevice.Items != null)
			{
				foreach (actionReportType item in networkDevice.Items.OfType<actionReportType>())
				{
					if (item.action == null)
					{
						continue;
					}
					actionType[] action = item.action;
					foreach (actionType actionType in action)
					{
						LemonbeatAction lemonbeatAction = new LemonbeatAction();
						lemonbeatAction.Id = actionType.action_id;
						LemonbeatAction lemonbeatAction2 = lemonbeatAction;
						int num2 = 0;
						object[] items = actionType.Items;
						foreach (object action2 in items)
						{
							lemonbeatAction2.Items.Add(FromMessage(actionType.ItemsElementName[num2], action2));
							num2++;
						}
						list.Add(lemonbeatAction2);
					}
				}
			}
		}
		return list;
	}

	private TransportModeType GetTransportMode(uint LemonbeatTransportMode)
	{
		return LemonbeatTransportMode switch
		{
			0u => TransportModeType.Udp, 
			1u => TransportModeType.Tcp, 
			_ => TransportModeType.Udp, 
		};
	}

	private ActionItem FromMessage(ItemsChoiceType itemName, object action)
	{
		switch (itemName)
		{
		case ItemsChoiceType.get:
		{
			getType getType = (getType)action;
			ValueReportActionItem valueReportActionItem2 = new ValueReportActionItem();
			valueReportActionItem2.ActionType = ValueReportActionType.Get;
			valueReportActionItem2.ValueId = getType.value_id;
			valueReportActionItem2.PartnerId = (getType.partner_idSpecified ? new uint?(getType.partner_id) : ((uint?)null));
			valueReportActionItem2.TransportMode = (getType.transport_modeSpecified ? new TransportModeType?(GetTransportMode(getType.transport_mode)) : ((TransportModeType?)null));
			return valueReportActionItem2;
		}
		case ItemsChoiceType.report:
		{
			reportType reportType = (reportType)action;
			ValueReportActionItem valueReportActionItem = new ValueReportActionItem();
			valueReportActionItem.ActionType = ValueReportActionType.Send;
			valueReportActionItem.ValueId = reportType.my_value_id;
			valueReportActionItem.PartnerId = (reportType.partner_idSpecified ? new uint?(reportType.partner_id) : ((uint?)null));
			valueReportActionItem.TransportMode = (reportType.transport_modeSpecified ? new TransportModeType?(GetTransportMode(reportType.transport_mode)) : ((TransportModeType?)null));
			return valueReportActionItem;
		}
		case ItemsChoiceType.set:
		{
			setType setType = (setType)action;
			if (setType.numberSpecified)
			{
				SetNumberActionItem setNumberActionItem = new SetNumberActionItem();
				setNumberActionItem.ValueId = setType.value_id;
				setNumberActionItem.PartnerId = (setType.partner_idSpecified ? new uint?(setType.partner_id) : ((uint?)null));
				setNumberActionItem.Value = setType.number;
				setNumberActionItem.TransportMode = (setType.transport_modeSpecified ? new TransportModeType?(GetTransportMode(setType.transport_mode)) : ((TransportModeType?)null));
				return setNumberActionItem;
			}
			if (setType.calculation_idSpecified)
			{
				SetCalculationActionItem setCalculationActionItem = new SetCalculationActionItem();
				setCalculationActionItem.CalculationId = setType.calculation_id;
				setCalculationActionItem.ValueId = setType.value_id;
				setCalculationActionItem.PartnerId = (setType.partner_idSpecified ? new uint?(setType.partner_id) : ((uint?)null));
				setCalculationActionItem.TransportMode = (setType.transport_modeSpecified ? new TransportModeType?(GetTransportMode(setType.transport_mode)) : ((TransportModeType?)null));
				return setCalculationActionItem;
			}
			SetStringActionItem setStringActionItem = new SetStringActionItem();
			setStringActionItem.ValueId = setType.value_id;
			setStringActionItem.PartnerId = (setType.partner_idSpecified ? new uint?(setType.partner_id) : ((uint?)null));
			setStringActionItem.Value = setType.@string;
			setStringActionItem.TransportMode = (setType.transport_modeSpecified ? new TransportModeType?(GetTransportMode(setType.transport_mode)) : ((TransportModeType?)null));
			return setStringActionItem;
		}
		case ItemsChoiceType.timer_start:
		case ItemsChoiceType.timer_stop:
		{
			timerType timerType = (timerType)action;
			TimerActionItem timerActionItem = new TimerActionItem();
			timerActionItem.ActionType = ((itemName != ItemsChoiceType.timer_start) ? TimerActionType.StopTimer : TimerActionType.StartTimer);
			timerActionItem.TimerId = timerType.timer_id;
			return timerActionItem;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	private KeyValuePair<ItemsChoiceType, object> ToMessage(ActionItem action)
	{
		if (action is ValueReportActionItem valueReportActionItem)
		{
			if (valueReportActionItem.ActionType == ValueReportActionType.Get)
			{
				return new KeyValuePair<ItemsChoiceType, object>(ItemsChoiceType.get, new getType
				{
					value_id = (byte)valueReportActionItem.ValueId,
					partner_id = (byte)valueReportActionItem.PartnerId.Value,
					partner_idSpecified = valueReportActionItem.PartnerId.HasValue,
					transport_modeSpecified = valueReportActionItem.TransportMode.HasValue,
					transport_mode = (uint)(valueReportActionItem.TransportMode.HasValue ? valueReportActionItem.TransportMode.Value : ((TransportModeType)0))
				});
			}
			return new KeyValuePair<ItemsChoiceType, object>(ItemsChoiceType.report, new reportType
			{
				my_value_id = (byte)valueReportActionItem.ValueId,
				partner_id = (byte)valueReportActionItem.PartnerId.Value,
				partner_idSpecified = valueReportActionItem.PartnerId.HasValue,
				transport_modeSpecified = valueReportActionItem.TransportMode.HasValue,
				transport_mode = (uint)(valueReportActionItem.TransportMode.HasValue ? valueReportActionItem.TransportMode.Value : ((TransportModeType)0))
			});
		}
		if (action is SetNumberActionItem setNumberActionItem)
		{
			return new KeyValuePair<ItemsChoiceType, object>(ItemsChoiceType.set, new setType
			{
				value_id = (byte)setNumberActionItem.ValueId,
				numberSpecified = true,
				partner_id = (byte)setNumberActionItem.ValueId,
				partner_idSpecified = setNumberActionItem.PartnerId.HasValue,
				transport_modeSpecified = setNumberActionItem.TransportMode.HasValue,
				transport_mode = (uint)(setNumberActionItem.TransportMode.HasValue ? setNumberActionItem.TransportMode.Value : ((TransportModeType)0)),
				number = setNumberActionItem.Value
			});
		}
		if (action is SetStringActionItem setStringActionItem)
		{
			return new KeyValuePair<ItemsChoiceType, object>(ItemsChoiceType.set, new setType
			{
				value_id = (byte)setStringActionItem.ValueId,
				partner_id = (byte)setStringActionItem.ValueId,
				partner_idSpecified = setStringActionItem.PartnerId.HasValue,
				transport_modeSpecified = setStringActionItem.TransportMode.HasValue,
				transport_mode = (uint)(setStringActionItem.TransportMode.HasValue ? setStringActionItem.TransportMode.Value : ((TransportModeType)0)),
				@string = setStringActionItem.Value
			});
		}
		if (action is SetCalculationActionItem setCalculationActionItem)
		{
			return new KeyValuePair<ItemsChoiceType, object>(ItemsChoiceType.set, new setType
			{
				value_id = (byte)setCalculationActionItem.ValueId,
				partner_id = (byte)setCalculationActionItem.ValueId,
				partner_idSpecified = setCalculationActionItem.PartnerId.HasValue,
				transport_modeSpecified = setCalculationActionItem.TransportMode.HasValue,
				transport_mode = (uint)(setCalculationActionItem.TransportMode.HasValue ? setCalculationActionItem.TransportMode.Value : ((TransportModeType)0)),
				calculation_id = setCalculationActionItem.CalculationId,
				calculation_idSpecified = true
			});
		}
		if (action is TimerActionItem timerActionItem)
		{
			return new KeyValuePair<ItemsChoiceType, object>((timerActionItem.ActionType == TimerActionType.StartTimer) ? ItemsChoiceType.timer_start : ItemsChoiceType.timer_stop, new timerType
			{
				timer_id = (byte)timerActionItem.TimerId
			});
		}
		throw new ArgumentOutOfRangeException();
	}
}
