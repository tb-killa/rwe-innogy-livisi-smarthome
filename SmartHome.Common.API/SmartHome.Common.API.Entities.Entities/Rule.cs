using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class Rule
{
	[JsonProperty("id")]
	public string Id { get; set; }

	[JsonProperty("triggers")]
	public List<Trigger> Triggers { get; set; }

	[JsonProperty("customTriggers")]
	public List<CustomTrigger> CustomTriggers { get; set; }

	[JsonProperty("constraints")]
	public List<Condition> Constraints { get; set; }

	[JsonProperty("actions")]
	public List<Action> Actions { get; set; }

	[JsonProperty("conditionEvaluationDelay")]
	public int? ConditionEvaluationDelay { get; set; }

	[JsonProperty("tags")]
	public List<Property> Tags { get; set; }
}
