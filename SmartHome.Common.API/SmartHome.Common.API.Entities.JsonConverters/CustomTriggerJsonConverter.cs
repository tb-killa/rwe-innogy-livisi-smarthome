using System.Collections.Generic;
using JsonLite;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Serializers;

namespace SmartHome.Common.API.Entities.JsonConverters;

public class CustomTriggerJsonConverter : JsonConverter
{
	public CustomTriggerJsonConverter()
		: base(typeof(CustomTrigger))
	{
	}

	public override JsonBuilder ToJson(object obj)
	{
		CustomTrigger customTrigger = obj as CustomTrigger;
		JsonObjectBuilder jsonObjectBuilder = new JsonObjectBuilder();
		jsonObjectBuilder.Add("type", "Custom");
		jsonObjectBuilder.Add("namespace", customTrigger.Namespace);
		jsonObjectBuilder.Add("subtype", customTrigger.Type);
		jsonObjectBuilder.Add("source", customTrigger.Link);
		jsonObjectBuilder.AddJson("properties", new JsonParser(ApiJsonSerializer.Serialize(customTrigger.Properties)));
		if (customTrigger.Tags != null)
		{
			jsonObjectBuilder.AddJson("tags", new JsonParser(ApiJsonSerializer.Serialize(customTrigger.Tags)));
		}
		return jsonObjectBuilder;
	}

	public override object ToObject(JsonParser json)
	{
		CustomTrigger customTrigger = new CustomTrigger();
		customTrigger.Type = json.GetField("subtype").GetValue<string>();
		customTrigger.Namespace = json.GetField("namespace").GetValue<string>();
		customTrigger.Link = json.GetField("source").GetValue<string>();
		customTrigger.Properties = ApiJsonSerializer.Deserialize<List<Property>>(json.GetField("properties"));
		JsonParser field = json.GetField("tags");
		if (field != null)
		{
			customTrigger.Tags = ApiJsonSerializer.Deserialize<List<Property>>(field);
		}
		return customTrigger;
	}
}
