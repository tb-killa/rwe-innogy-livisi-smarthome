using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities.Account;

[Serializable]
public class Partner
{
	[JsonProperty("name")]
	public string Name { get; set; }

	[JsonProperty("displayName")]
	public string DisplayName { get; set; }

	[JsonProperty("logoUrl")]
	public string LogoUrl { get; set; }

	[JsonProperty("optins")]
	public List<OptIn2> OptIns { get; set; }

	[JsonProperty("legalInfo")]
	public List<OptInTextWithLinks> LegalInfo { get; set; }

	[JsonProperty("objectionInfo")]
	public List<OptInTextWithLinks> ObjectionInfo { get; set; }

	[JsonProperty("optoutInfo")]
	public List<OptInTextWithLinks> OptOutInfo { get; set; }
}
