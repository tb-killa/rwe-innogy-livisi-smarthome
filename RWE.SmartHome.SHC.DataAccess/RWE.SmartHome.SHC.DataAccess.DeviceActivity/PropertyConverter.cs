using System;
using System.Collections.Generic;
using JsonLite;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

namespace RWE.SmartHome.SHC.DataAccess.DeviceActivity;

internal class PropertyConverter : JsonConverter
{
	public PropertyConverter()
		: base(typeof(List<Property>))
	{
	}

	public override JsonBuilder ToJson(object obj)
	{
		List<Property> list = obj as List<Property>;
		if (list.Count == 0)
		{
			return new JsonObjectBuilder();
		}
		JsonObjectBuilder jsonObjectBuilder = new JsonObjectBuilder();
		foreach (Property item in list)
		{
			jsonObjectBuilder.Add(item.Name, (IConvertible)item.Value);
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
				Name = allField.Name,
				Value = value
			});
		}
		return list;
	}
}
