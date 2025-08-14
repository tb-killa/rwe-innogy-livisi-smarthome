using System;
using System.Net;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

public sealed class DeviceIdentifier : IEquatable<DeviceIdentifier>
{
	public IPAddress IPAddress { get; private set; }

	public uint? SubDeviceId { get; private set; }

	public int GatewayId { get; private set; }

	public DeviceIdentifier(IPAddress address, uint? subdeviceId, int gatewayId)
	{
		IPAddress = address;
		SubDeviceId = subdeviceId;
		GatewayId = gatewayId;
	}

	public bool Equals(DeviceIdentifier other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (!IPAddress.Equals(other.IPAddress))
		{
			return false;
		}
		if (SubDeviceId != other.SubDeviceId)
		{
			return false;
		}
		if (GatewayId != other.GatewayId)
		{
			return false;
		}
		return true;
	}

	public override bool Equals(object other)
	{
		if (this == other)
		{
			return true;
		}
		return Equals(other as DeviceIdentifier);
	}

	public override int GetHashCode()
	{
		return (IPAddress.GetHashCode() * 937) ^ (SubDeviceId.HasValue ? SubDeviceId.Value.GetHashCode() : 0) ^ (GatewayId * 937);
	}

	public override string ToString()
	{
		if (SubDeviceId.HasValue)
		{
			return $"[Gw: {GatewayId} | SubDeviceId: {SubDeviceId.Value} IP:{IPAddress.ToString()}]";
		}
		return $"[Gw: {GatewayId}, IP: {IPAddress.ToString()}]";
	}
}
