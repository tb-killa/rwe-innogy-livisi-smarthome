using System.Collections.Generic;
using JsonLite;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Serializers;

namespace SmartHome.Common.API.Entities.JsonConverters;

public class ParameterListJsonConverter : JsonConverter
{
	public ParameterListJsonConverter()
		: base(typeof(List<Parameter>))
	{
	}

	public override JsonBuilder ToJson(object obj)
	{
		List<Parameter> list = obj as List<Parameter>;
		JsonObjectBuilder jsonObjectBuilder = new JsonObjectBuilder();
		foreach (Parameter item in list)
		{
			jsonObjectBuilder.AddJson(JsonNameResolver.ResolveName(item.Name), new JsonParser(ApiJsonSerializer.Serialize(item)));
		}
		return jsonObjectBuilder;
	}

	public override object ToObject(JsonParser json)
	{
		List<Parameter> list = new List<Parameter>();
		foreach (JsonParser allField in json.GetAllFields())
		{
			list.Add(ApiJsonSerializer.Deserialize<Parameter>(allField));
		}
		return list;
	}
}
