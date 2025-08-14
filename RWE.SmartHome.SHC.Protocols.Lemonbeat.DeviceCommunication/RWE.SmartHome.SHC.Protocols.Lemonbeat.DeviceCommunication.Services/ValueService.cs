using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Value;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Events;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Services;

public class ValueService : DuplexService<network>, IValueService
{
	private const string DefaultNamespace = "urn:valuexsd";

	public event EventHandler<ValueReportReceivedArgs> ValueReportReceived;

	public event EventHandler<RequestStatusCompletedEventArgs> RequestStatusCompleted;

	public event EventHandler<AsyncCompletedEventArgs> SetValueCompleted;

	public event EventHandler<AsyncCompletedEventArgs> ReportValueCompleted;

	public ValueService(ILemonbeatCommunication aggregator)
		: base(aggregator, ServiceType.Value, "urn:valuexsd")
	{
	}

	public void RequestStatusAsync(DeviceIdentifier identifier, object userState)
	{
		ThreadPool.QueueUserWorkItem(delegate
		{
			RequestStatusAsyncImpl(identifier, userState, null);
		});
	}

	public void RequestStatusAsync(DeviceIdentifier identifier, uint valueIndex, object userState)
	{
		ThreadPool.QueueUserWorkItem(delegate
		{
			RequestStatusAsyncImpl(identifier, userState, valueIndex);
		});
	}

	public void RequestStatusAsync(DeviceIdentifier identifier, uint[] valueIndexes, object userState)
	{
		ThreadPool.QueueUserWorkItem(delegate
		{
			RequestStatusForMultipleIdsAsyncImpl(identifier, userState, valueIndexes);
		});
	}

	public ValueCollection RequestStatus(DeviceIdentifier identifier)
	{
		return RequestStatusImpl(identifier, null);
	}

	public ValueCollection RequestStatus(DeviceIdentifier identifier, uint valueIndex)
	{
		return RequestStatus(identifier, valueIndex);
	}

	public void SetValueAsync(DeviceIdentifier identifier, uint valueId, string state, Transport transport, object userState)
	{
		ThreadPool.QueueUserWorkItem(delegate
		{
			SetValueImpl(identifier, new object[1]
			{
				new valueSetType
				{
					value_id = (byte)valueId,
					@string = state
				}
			}, transport, userState);
		});
	}

	public void SetValueAsync(DeviceIdentifier identifier, uint valueId, double state, Transport transport, object userState)
	{
		ThreadPool.QueueUserWorkItem(delegate
		{
			SetValueImpl(identifier, new object[1]
			{
				new valueSetType
				{
					value_id = (byte)valueId,
					number = state,
					numberSpecified = true
				}
			}, transport, userState);
		});
	}

	public void SetValueAsync(DeviceIdentifier identifier, uint valueId, byte[] state, Transport transport, object userState)
	{
		ThreadPool.QueueUserWorkItem(delegate
		{
			SetValueImpl(identifier, new object[1]
			{
				new valueSetType
				{
					value_id = (byte)valueId,
					hexBinary = state,
					numberSpecified = false
				}
			}, transport, userState);
		});
	}

	public void SetValueAsync(DeviceIdentifier identifier, IEnumerable<CoreNumberValue> numberValues, IEnumerable<CoreStringValue> stringValues, IEnumerable<CoreHexBinaryValue> hexBinaryValues, Transport transport, object userState)
	{
		List<object> values = new List<object>();
		if (numberValues != null)
		{
			values.AddRange(numberValues.Select((CoreNumberValue pair) => new valueSetType
			{
				value_id = (byte)pair.Id,
				number = (double)pair.Value,
				numberSpecified = true
			}).Cast<object>());
		}
		if (stringValues != null)
		{
			values.AddRange(stringValues.Select((CoreStringValue pair) => new valueSetType
			{
				value_id = (byte)pair.Id,
				@string = pair.Value
			}).Cast<object>());
		}
		if (hexBinaryValues != null)
		{
			values.AddRange(hexBinaryValues.Select((CoreHexBinaryValue pair) => new valueSetType
			{
				value_id = (byte)pair.Id,
				hexBinary = pair.Value
			}).Cast<object>());
		}
		if (values.Count > 0)
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				SetValueImpl(identifier, values.ToArray(), transport, userState);
			});
		}
	}

	public void ReportValueAsync(DeviceIdentifier identifier, uint valueId, double state, Transport transport, object userState)
	{
		ThreadPool.QueueUserWorkItem(delegate
		{
			ReportValueImpl(identifier, new object[1]
			{
				new valueReportType
				{
					value_id = (byte)valueId,
					number = state,
					numberSpecified = true,
					value_idSpecified = true,
					timestamp = (ulong)DateTime.Now.Subtract(CalendarService.EpochTime).TotalMilliseconds
				}
			}, transport, userState);
		});
	}

	protected override void Handle(int gatewayId, IPAddress remoteAddress, network message)
	{
		Dictionary<DeviceIdentifier, ValueCollection> dictionary = ProcessAllValues(gatewayId, remoteAddress, message);
		foreach (KeyValuePair<DeviceIdentifier, ValueCollection> item in dictionary)
		{
			OnValueReportReceived(new ValueReportReceivedArgs(item.Key, item.Value.NumberValues, item.Value.StringValues, item.Value.HexBinaryValues));
		}
	}

	private Dictionary<DeviceIdentifier, ValueCollection> ProcessAllValues(int gatewayId, IPAddress remoteAddress, network response)
	{
		Dictionary<DeviceIdentifier, ValueCollection> dictionary = new Dictionary<DeviceIdentifier, ValueCollection>();
		if (response == null || response.device == null)
		{
			return dictionary;
		}
		networkDevice[] device = response.device;
		foreach (networkDevice networkDevice in device)
		{
			if (networkDevice.Items == null)
			{
				continue;
			}
			DeviceIdentifier key = new DeviceIdentifier(remoteAddress, networkDevice.device_idSpecified ? new uint?(networkDevice.device_id) : ((uint?)null), gatewayId);
			List<CoreNumberValue> list = new List<CoreNumberValue>();
			List<CoreStringValue> list2 = new List<CoreStringValue>();
			List<CoreHexBinaryValue> list3 = new List<CoreHexBinaryValue>();
			foreach (valueReportType item in networkDevice.Items.OfType<valueReportType>())
			{
				if (item.numberSpecified)
				{
					list.Add(new CoreNumberValue
					{
						Id = item.value_id,
						Value = (decimal)item.number,
						TimeStamp = GetTimeStamp(item.timestamp)
					});
				}
				else if (item.@string != null)
				{
					list2.Add(new CoreStringValue
					{
						Id = item.value_id,
						Value = item.@string,
						TimeStamp = GetTimeStamp(item.timestamp)
					});
				}
				else
				{
					list3.Add(new CoreHexBinaryValue
					{
						Id = item.value_id,
						Value = item.hexBinary,
						TimeStamp = GetTimeStamp(item.timestamp)
					});
				}
			}
			dictionary.Add(key, new ValueCollection
			{
				NumberValues = list,
				StringValues = list2,
				HexBinaryValues = list3
			});
		}
		return dictionary;
	}

	private network CreateNetworkMessage(DeviceIdentifier identifier)
	{
		network network = new network();
		network.version = 1u;
		network.device = new networkDevice[1]
		{
			new networkDevice
			{
				version = 1u,
				device_id = (identifier.SubDeviceId.HasValue ? identifier.SubDeviceId.Value : 0u),
				device_idSpecified = identifier.SubDeviceId.HasValue
			}
		};
		return network;
	}

	private void SetValueImpl(DeviceIdentifier identifier, object[] valueSets, Transport transport, object userState)
	{
		try
		{
			network network = CreateNetworkMessage(identifier);
			network.device[0].Items = valueSets;
			SendMessage(identifier, network, (transport != Transport.Tcp) ? RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.TransportType.Datagram : RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.TransportType.Connection);
			RaiseSetValueCompletedEvent(new AsyncCompletedEventArgs(null, cancelled: false, userState));
		}
		catch (Exception error)
		{
			RaiseSetValueCompletedEvent(new AsyncCompletedEventArgs(error, cancelled: false, userState));
		}
	}

	private void ReportValueImpl(DeviceIdentifier identifier, object[] valueReports, Transport transport, object userState)
	{
		try
		{
			network network = CreateNetworkMessage(identifier);
			network.device[0].Items = valueReports;
			SendMessage(identifier, network, (transport != Transport.Tcp) ? RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.TransportType.Datagram : RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.TransportType.Connection);
			RaiseReportValueCompletedEvent(new AsyncCompletedEventArgs(null, cancelled: false, userState));
		}
		catch (Exception error)
		{
			RaiseReportValueCompletedEvent(new AsyncCompletedEventArgs(error, cancelled: false, userState));
		}
	}

	private void OnValueReportReceived(ValueReportReceivedArgs e)
	{
		this.ValueReportReceived?.Invoke(this, e);
	}

	private static DateTime GetTimeStamp(ulong timestamp)
	{
		return new DateTime((long)timestamp);
	}

	private void RequestStatusAsyncImpl(DeviceIdentifier identifier, object userState, uint? valueIndex)
	{
		try
		{
			ValueCollection valueCollection = RequestStatusImpl(identifier, valueIndex);
			RaiseRequestStatusCompletedEvent(new RequestStatusCompletedEventArgs(valueCollection ?? new ValueCollection(), null, cancelled: false, userState));
		}
		catch (Exception error)
		{
			RaiseRequestStatusCompletedEvent(new RequestStatusCompletedEventArgs(null, error, cancelled: false, userState));
		}
	}

	private void RequestStatusForMultipleIdsAsyncImpl(DeviceIdentifier identifier, object userState, uint[] valueIndexes)
	{
		try
		{
			ValueCollection valueCollection = RequestStatusForMultipleIdsImpl(identifier, valueIndexes);
			RaiseRequestStatusCompletedEvent(new RequestStatusCompletedEventArgs(valueCollection ?? new ValueCollection(), null, cancelled: false, userState));
		}
		catch (Exception error)
		{
			RaiseRequestStatusCompletedEvent(new RequestStatusCompletedEventArgs(null, error, cancelled: false, userState));
		}
	}

	private ValueCollection RequestStatusImpl(DeviceIdentifier identifier, uint? valueIndex)
	{
		network valueRequestMessage = GetValueRequestMessage(identifier, valueIndex);
		network response = SendRequest(identifier, valueRequestMessage);
		ProcessAllValues(identifier.GatewayId, identifier.IPAddress, response).TryGetValue(identifier, out var value);
		return value ?? new ValueCollection();
	}

	private ValueCollection RequestStatusForMultipleIdsImpl(DeviceIdentifier identifier, uint[] valueIndexes)
	{
		network valueRequestMessage = GetValueRequestMessage(identifier, valueIndexes);
		network response = SendRequest(identifier, valueRequestMessage);
		ProcessAllValues(identifier.GatewayId, identifier.IPAddress, response).TryGetValue(identifier, out var value);
		return value ?? new ValueCollection();
	}

	private void RaiseRequestStatusCompletedEvent(RequestStatusCompletedEventArgs args)
	{
		this.RequestStatusCompleted?.Invoke(this, args);
	}

	private void RaiseSetValueCompletedEvent(AsyncCompletedEventArgs args)
	{
		this.SetValueCompleted?.Invoke(this, args);
	}

	private void RaiseReportValueCompletedEvent(AsyncCompletedEventArgs args)
	{
		this.ReportValueCompleted?.Invoke(this, args);
	}

	private network GetValueRequestMessage(DeviceIdentifier identifier, uint? valueIndex)
	{
		valueGetType valueGetType2;
		if (valueIndex.HasValue)
		{
			valueGetType valueGetType = new valueGetType();
			valueGetType.value_id = (byte)valueIndex.Value;
			valueGetType.value_idSpecified = true;
			valueGetType2 = valueGetType;
		}
		else
		{
			valueGetType2 = new valueGetType();
		}
		network network = CreateNetworkMessage(identifier);
		network.device[0].Items = new object[1] { valueGetType2 };
		return network;
	}

	private network GetValueRequestMessage(DeviceIdentifier identifier, uint[] valueIds)
	{
		network network = CreateNetworkMessage(identifier);
		if (valueIds == null)
		{
			network.device[0].Items = new object[1]
			{
				new valueGetType()
			};
		}
		else
		{
			network.device[0].Items = valueIds.Select((uint m) => new valueGetType
			{
				value_id = (byte)m,
				value_idSpecified = true
			}).ToArray();
		}
		return network;
	}
}
