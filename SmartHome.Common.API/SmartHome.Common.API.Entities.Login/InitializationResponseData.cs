using System;
using System.Collections.Generic;

namespace SmartHome.Common.API.Entities.Login;

[Serializable]
public class InitializationResponseData
{
	public int CurrentConfigurationVersion { get; set; }

	public List<object> Data { get; set; }
}
