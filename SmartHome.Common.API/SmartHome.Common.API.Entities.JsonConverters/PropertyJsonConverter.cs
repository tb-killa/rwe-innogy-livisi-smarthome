using System;
using System.Collections.Generic;
using JsonLite;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Extensions;

namespace SmartHome.Common.API.Entities.JsonConverters;

public class PropertyJsonConverter : JsonConverter
{
	public PropertyJsonConverter()
		: base(typeof(List<Property>))
	{
	}

	public override JsonBuilder ToJson(object obj)
	{
		List<Property> list = obj as List<Property>;
		if (list.Count == 0)
		{
			return new JsonArrayBuilder(JsonType.Object);
		}
		JsonObjectBuilder jsonObjectBuilder = new JsonObjectBuilder();
		foreach (Property item in list)
		{
			string name = JsonNameResolver.ResolveName(item.Name);
			if (item.LastChanged.HasValue)
			{
				JsonObjectBuilder jsonObjectBuilder2 = new JsonObjectBuilder();
				if (item.Value != null)
				{
					jsonObjectBuilder2.Add("value", (IConvertible)item.Value);
				}
				jsonObjectBuilder2.Add("lastChanged", (IConvertible)(object)item.LastChanged);
				jsonObjectBuilder.Add(name, (JsonBuilder)jsonObjectBuilder2);
			}
			else if (item.Value != null)
			{
				jsonObjectBuilder.Add(name, (IConvertible)item.Value);
			}
		}
		return jsonObjectBuilder;
	}

	public override object ToObject(JsonParser json)
	{
		List<Property> list = new List<Property>();
		foreach (JsonParser allField in json.GetAllFields())
		{
			object value = null;
			if (allField.IsBoolean)
			{
				value = allField.GetValue<bool>();
			}
			if (allField.IsNumber)
			{
				value = allField.GetValue<decimal>();
			}
			if (allField.IsString)
			{
				value = allField.GetValue<string>();
			}
			list.Add(new Property
			{
				Name = allField.Name.FirstToUpper(),
				Value = value
			});
		}
		return list;
	}
}
