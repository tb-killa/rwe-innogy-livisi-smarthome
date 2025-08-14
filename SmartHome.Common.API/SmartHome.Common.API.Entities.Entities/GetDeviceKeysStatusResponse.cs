using System;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class GetDeviceKeysStatusResponse
{
	[JsonProperty("master_key")]
	public bool MasterKey { get; set; }

	[JsonProperty("stored_keys")]
	public int NumberOfStoredKeys { get; set; }

	[JsonProperty("export_needed")]
	public bool ExportNeeded { get; set; }
}
