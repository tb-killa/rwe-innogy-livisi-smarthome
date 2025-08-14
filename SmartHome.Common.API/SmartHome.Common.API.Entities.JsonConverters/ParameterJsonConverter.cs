using System;
using System.Collections.Generic;
using JsonLite;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.API.Entities.Serializers;

namespace SmartHome.Common.API.Entities.JsonConverters;

public class ParameterJsonConverter : JsonConverter
{
	public ParameterJsonConverter()
		: base(typeof(Parameter))
	{
	}

	public override JsonBuilder ToJson(object obj)
	{
		Parameter parameter = obj as Parameter;
		JsonObjectBuilder jsonObjectBuilder = new JsonObjectBuilder();
		if (parameter.Constant != null)
		{
			jsonObjectBuilder.Add("type", "Constant");
			jsonObjectBuilder.Add("value", (IConvertible)parameter.Constant.Value);
		}
		else
		{
			jsonObjectBuilder.Add("type", parameter.Function.Type);
			JsonParser json = new JsonParser(ApiJsonSerializer.Serialize(parameter.Function.Parameters));
			jsonObjectBuilder.AddJson("params", json);
		}
		return jsonObjectBuilder;
	}

	public override object ToObject(JsonParser json)
	{
		Parameter parameter = new Parameter();
		parameter.Name = json.Name.FirstToUpper();
		parameter.Type = json.GetField("type").GetValue<string>();
		Parameter parameter2 = parameter;
		if (parameter2.Type.Equals("Constant", StringComparison.InvariantCultureIgnoreCase))
		{
			JsonParser field = json.GetField("value");
			object value = null;
			if (field.IsBoolean)
			{
				value = field.GetValue<bool>();
			}
			if (field.IsString)
			{
				value = ((!field.GetValue<string>().DateTimeTryParse(out var result)) ? field.GetValue<string>() : ((object)result));
			}
			if (field.IsNumber)
			{
				value = field.GetValue<decimal>();
			}
			parameter2.Constant = new Constant
			{
				Value = value
			};
		}
		else
		{
			new ParameterListJsonConverter();
			parameter2.Function = new Function
			{
				Type = parameter2.Type,
				Parameters = ApiJsonSerializer.Deserialize<List<Parameter>>(json.GetField("params"))
			};
		}
		return parameter2;
	}
}
