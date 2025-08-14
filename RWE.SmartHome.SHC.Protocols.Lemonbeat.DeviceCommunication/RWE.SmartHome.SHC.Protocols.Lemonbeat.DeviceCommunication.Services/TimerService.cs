using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Timer;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Services;

public class TimerService : SenderService<network>, ITimerService
{
	private const string DefaultNamespace = "urn:timerxsd";

	public TimerService(ILemonbeatCommunication aggregator)
		: base(aggregator, ServiceType.Timer, "urn:timerxsd")
	{
	}

	public IEnumerable<LemonbeatTimer> GetAllTimers(DeviceIdentifier identifier)
	{
		network network = CreateNetworkMessage();
		network.device[0].ItemsElementName = new ItemsChoiceType[1] { ItemsChoiceType.timer_get };
		network.device[0].Items = new object[1]
		{
			new timerGetType()
		};
		network.device[0].device_idSpecified = identifier.SubDeviceId.HasValue;
		network.device[0].device_id = identifier.SubDeviceId ?? 0;
		network message = SendRequest(identifier, network);
		return ParseTimers(message, identifier.SubDeviceId ?? 0);
	}

	public void SetAndDeleteTimers(DeviceIdentifier identifier, IEnumerable<LemonbeatTimer> timersToSet, IEnumerable<uint> timersToDelete)
	{
		List<KeyValuePair<ItemsChoiceType, object>> list = new List<KeyValuePair<ItemsChoiceType, object>>();
		if (timersToSet != null)
		{
			executeType[] array = timersToSet.Select((LemonbeatTimer timer) => new executeType
			{
				timer_id = (byte)timer.Id,
				after = timer.Delay,
				calculation_id = (byte)timer.CalculationId.GetValueOrDefault(),
				calculation_idSpecified = timer.CalculationId.HasValue,
				action_id = (byte)timer.ActionId.GetValueOrDefault(),
				action_idSpecified = timer.ActionId.HasValue
			}).ToArray();
			if (array.Length > 0)
			{
				list.Add(new KeyValuePair<ItemsChoiceType, object>(ItemsChoiceType.timer_set, new timerSetType
				{
					execute = array
				}));
			}
		}
		if (timersToDelete != null)
		{
			foreach (uint item in timersToDelete)
			{
				list.Add(new KeyValuePair<ItemsChoiceType, object>(ItemsChoiceType.timer_delete, new timerDeleteType
				{
					timer_id = (byte)item,
					timer_idSpecified = true
				}));
			}
		}
		if (list.Count <= 0)
		{
			return;
		}
		network network = CreateNetworkMessage();
		network.device[0].Items = new object[list.Count];
		network.device[0].ItemsElementName = new ItemsChoiceType[list.Count];
		int num = 0;
		foreach (KeyValuePair<ItemsChoiceType, object> item2 in list)
		{
			network.device[0].ItemsElementName[num] = item2.Key;
			network.device[0].Items[num] = item2.Value;
			num++;
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

	private IEnumerable<LemonbeatTimer> ParseTimers(network message, uint subDeviceId)
	{
		List<LemonbeatTimer> list = new List<LemonbeatTimer>();
		if (message != null && message.device != null)
		{
			networkDevice networkDevice = message.device.Where((networkDevice d) => d.device_id == subDeviceId).FirstOrDefault();
			if (networkDevice != null && networkDevice.Items != null)
			{
				foreach (timerReportType item in networkDevice.Items.OfType<timerReportType>())
				{
					if (item.execute != null)
					{
						executeType[] execute = item.execute;
						foreach (executeType executeType in execute)
						{
							list.Add(new LemonbeatTimer
							{
								Id = executeType.timer_id,
								Delay = executeType.after,
								CalculationId = (executeType.calculation_idSpecified ? new uint?(executeType.calculation_id) : ((uint?)null)),
								ActionId = (executeType.action_idSpecified ? new uint?(executeType.action_id) : ((uint?)null))
							});
						}
					}
				}
			}
		}
		return list;
	}
}
