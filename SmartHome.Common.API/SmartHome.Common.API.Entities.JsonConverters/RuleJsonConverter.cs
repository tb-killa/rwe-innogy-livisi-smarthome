using System.Collections.Generic;
using JsonLite;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Serializers;

namespace SmartHome.Common.API.Entities.JsonConverters;

public class RuleJsonConverter : JsonConverter
{
	public RuleJsonConverter()
		: base(typeof(Rule))
	{
	}

	public override JsonBuilder ToJson(object obj)
	{
		Rule rule = obj as Rule;
		JsonObjectBuilder jsonObjectBuilder = new JsonObjectBuilder();
		jsonObjectBuilder.Add("id", rule.Id);
		JsonArrayBuilder jsonArrayBuilder = new JsonArrayBuilder(JsonType.Object);
		if (rule.Triggers != null)
		{
			foreach (Trigger trigger in rule.Triggers)
			{
				jsonArrayBuilder.AddJson(new JsonParser(ApiJsonSerializer.Serialize(trigger)));
			}
		}
		if (rule.CustomTriggers != null)
		{
			foreach (CustomTrigger customTrigger in rule.CustomTriggers)
			{
				jsonArrayBuilder.AddJson(new JsonParser(ApiJsonSerializer.Serialize(customTrigger)));
			}
		}
		jsonObjectBuilder.Add("triggers", (JsonBuilder)jsonArrayBuilder);
		if (rule.Constraints != null)
		{
			jsonObjectBuilder.AddJson("constraints", new JsonParser(ApiJsonSerializer.Serialize(rule.Constraints)));
		}
		if (rule.Actions != null)
		{
			jsonObjectBuilder.AddJson("actions", new JsonParser(ApiJsonSerializer.Serialize(rule.Actions)));
		}
		if (rule.ConditionEvaluationDelay.HasValue)
		{
			jsonObjectBuilder.Add("conditionsEvaluationDelay", rule.ConditionEvaluationDelay.Value);
		}
		if (rule.Tags != null)
		{
			jsonObjectBuilder.AddJson("tags", new JsonParser(ApiJsonSerializer.Serialize(rule.Tags)));
		}
		return jsonObjectBuilder;
	}

	public override object ToObject(JsonParser json)
	{
		Rule rule = new Rule();
		rule.Id = json.GetField("id").GetValue<string>();
		rule.Triggers = new List<Trigger>();
		rule.CustomTriggers = new List<CustomTrigger>();
		foreach (JsonParser allField in json.GetField("triggers").GetAllFields())
		{
			if (allField.GetField("type").GetValue<string>() == "Custom")
			{
				rule.CustomTriggers.Add(ApiJsonSerializer.Deserialize<CustomTrigger>(allField.GetRawValue()));
			}
			else
			{
				rule.Triggers.Add(ApiJsonSerializer.Deserialize<Trigger>(allField.GetRawValue()));
			}
		}
		JsonParser field = json.GetField("constraints");
		if (field != null)
		{
			rule.Constraints = ApiJsonSerializer.Deserialize<List<Condition>>(field.GetRawValue());
		}
		JsonParser field2 = json.GetField("actions");
		if (field2 != null)
		{
			rule.Actions = ApiJsonSerializer.Deserialize<List<Action>>(field2.GetRawValue());
		}
		JsonParser field3 = json.GetField("tags");
		if (field3 != null)
		{
			rule.Tags = ApiJsonSerializer.Deserialize<List<Property>>(field3.GetRawValue());
		}
		JsonParser field4 = json.GetField("conditionsEvaluationDelay");
		if (field4 != null)
		{
			rule.ConditionEvaluationDelay = field4.GetValue<int>();
		}
		return rule;
	}
}
