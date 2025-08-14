using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Interfaces;
using SmartHome.SHC.API.Protocols.Lemonbeat;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.LemonbeatCoreServices;

internal class ValueHandler : IValueHandler
{
	private readonly IValueService valueService;

	private readonly IDeviceList deviceList;

	private readonly string appId;

	internal ValueHandler(string applicationId, IValueService valueService, IDeviceList deviceList)
	{
		this.valueService = valueService;
		this.deviceList = deviceList;
		appId = applicationId;
	}

	public void SetValues(Guid baseDeviceId, IEnumerable<NumberValue> numberValues, IEnumerable<StringValue> stringValues)
	{
		SetValues(baseDeviceId, numberValues, stringValues, Transport.Udp);
	}

	public void SetValues(Guid baseDeviceId, IEnumerable<NumberValue> numberValues, IEnumerable<StringValue> stringValues, Transport transport)
	{
		DeviceInformation deviceInformation = deviceList[baseDeviceId];
		IEnumerable<CoreNumberValue> enumerable = numberValues?.Select((NumberValue v) => v.ToCoreNumberValue());
		IEnumerable<CoreStringValue> enumerable2 = stringValues?.Select((StringValue v) => v.ToCoreStringValue());
		if (enumerable == null && enumerable2 == null)
		{
			Log.Debug(Module.LemonbeatProtocolAdapter, $"LemonbeatValueHandler: No values to set for addIn with id: {appId}");
		}
		else if (deviceInformation != null && deviceInformation.Identifier != null)
		{
			valueService.SetValueAsync(deviceInformation.Identifier, enumerable, enumerable2, null, transport.ToCoreTransport(), null);
		}
		else
		{
			Log.Warning(Module.LemonbeatProtocolAdapter, $"LemonbeatValueHandler: AddIn with id: {appId} requested to set the values of an inexistent device");
		}
	}

	public void SetValues(Guid baseDeviceId, IEnumerable<NumberValue> numberValues, IEnumerable<StringValue> stringValues, IEnumerable<HexBinaryValue> hexBinaryValues)
	{
		SetValues(baseDeviceId, numberValues, stringValues, hexBinaryValues, Transport.Udp);
	}

	public void SetValues(Guid baseDeviceId, IEnumerable<NumberValue> numberValues, IEnumerable<StringValue> stringValues, IEnumerable<HexBinaryValue> hexBinaryValues, Transport transport)
	{
		DeviceInformation deviceInformation = deviceList[baseDeviceId];
		IEnumerable<CoreNumberValue> enumerable = numberValues?.Select((NumberValue v) => v.ToCoreNumberValue());
		IEnumerable<CoreStringValue> enumerable2 = stringValues?.Select((StringValue v) => v.ToCoreStringValue());
		IEnumerable<CoreHexBinaryValue> enumerable3 = hexBinaryValues?.Select((HexBinaryValue v) => v.ToCoreHexBinaryValue());
		if (enumerable == null && enumerable2 == null && enumerable3 == null)
		{
			Log.Debug(Module.LemonbeatProtocolAdapter, $"LemonbeatValueHandler: No values to set for addIn with id: {appId}");
		}
		else if (deviceInformation != null && deviceInformation.Identifier != null)
		{
			valueService.SetValueAsync(deviceInformation.Identifier, enumerable, enumerable2, enumerable3, transport.ToCoreTransport(), null);
		}
		else
		{
			Log.Warning(Module.LemonbeatProtocolAdapter, $"LemonbeatValueHandler: AddIn with id: {appId} requested to set the values of an inexistent device");
		}
	}

	public void RequestValueAsync(Guid deviceId)
	{
		DeviceInformation deviceInformation = deviceList[deviceId];
		if (deviceInformation != null && deviceInformation.Identifier != null)
		{
			valueService.RequestStatusAsync(deviceInformation.Identifier, null);
		}
		else
		{
			Log.Warning(Module.LemonbeatProtocolAdapter, $"LemonbeatValueHandler: AddIn with id: {appId} requested to get the values of an inexistent device");
		}
	}

	public void RequestValueAsync(Guid deviceId, uint[] valueIds)
	{
		DeviceInformation deviceInformation = deviceList[deviceId];
		if (deviceInformation != null && deviceInformation.Identifier != null)
		{
			valueService.RequestStatusAsync(deviceInformation.Identifier, valueIds, null);
		}
		else
		{
			Log.Warning(Module.LemonbeatProtocolAdapter, $"LemonbeatValueHandler: AddIn with id: {appId} requested to get the values of an inexistent device");
		}
	}
}
