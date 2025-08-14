using System.Collections.Generic;
using JsonLite;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Serializers;

namespace SmartHome.Common.API.Entities.JsonConverters;

public class ConditionJsonConverter : JsonConverter
{
	public ConditionJsonConverter()
		: base(typeof(Condition))
	{
	}

	public override JsonBuilder ToJson(object obj)
	{
		Condition condition = obj as Condition;
		JsonObjectBuilder jsonObjectBuilder = new JsonObjectBuilder();
		jsonObjectBuilder.Add("type", (string)condition.Operator.Constant.Value);
		if (condition.Tags != null && condition.Tags.Count > 0)
		{
			jsonObjectBuilder.AddJson("tags", new JsonParser(ApiJsonSerializer.Serialize(condition.Tags)));
		}
		JsonObjectBuilder jsonObjectBuilder2 = new JsonObjectBuilder();
		new ParameterJsonConverter();
		jsonObjectBuilder2.AddJson("leftOp", new JsonParser(ApiJsonSerializer.Serialize(condition.Leqp)));
		jsonObjectBuilder2.AddJson("rightOp", new JsonParser(ApiJsonSerializer.Serialize(condition.Reqp)));
		return jsonObjectBuilder.Add("params", (JsonBuilder)jsonObjectBuilder2);
	}

	public override object ToObject(JsonParser json)
	{
		Condition condition = new Condition();
		JsonParser field = json.GetField("tags");
		if (field != null)
		{
			condition.Tags = ApiJsonSerializer.Deserialize<List<Property>>(field);
		}
		condition.Operator = new Parameter
		{
			Constant = new Constant
			{
				Value = json.GetField("type").GetValue<string>()
			}
		};
		new ParameterJsonConverter();
		JsonParser field2 = json.GetField("params");
		condition.Leqp = ApiJsonSerializer.Deserialize<Parameter>(field2.GetField("leftOp"));
		condition.Reqp = ApiJsonSerializer.Deserialize<Parameter>(field2.GetField("rightOp"));
		return condition;
	}
}
