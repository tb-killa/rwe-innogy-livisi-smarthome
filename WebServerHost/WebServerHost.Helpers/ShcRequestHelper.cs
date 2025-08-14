using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;

namespace WebServerHost.Helpers;

internal static class ShcRequestHelper
{
	public static T NewRequest<T>() where T : BaseRequest, new()
	{
		T result = new T();
		Guid requestId = Guid.NewGuid();
		result.RequestId = requestId;
		result.Version = "3.00";
		return result;
	}
}
