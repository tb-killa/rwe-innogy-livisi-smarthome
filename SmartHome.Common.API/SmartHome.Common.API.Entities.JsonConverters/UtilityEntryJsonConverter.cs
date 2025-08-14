using System;
using JsonLite;
using SmartHome.Common.API.Entities.Data;

namespace SmartHome.Common.API.Entities.JsonConverters;

internal class UtilityEntryJsonConverter : JsonConverter
{
	public UtilityEntryJsonConverter()
		: base(typeof(UtilityEntry))
	{
	}

	public override JsonBuilder ToJson(object obj)
	{
		JsonObjectBuilder jsonObjectBuilder = new JsonObjectBuilder();
		if (obj is UtilityEntry utilityEntry)
		{
			jsonObjectBuilder.Add("vl", utilityEntry.value).Add("ti", utilityEntry.timestamp);
			if (utilityEntry.calculatedValue.HasValue)
			{
				jsonObjectBuilder.Add("cv", utilityEntry.calculatedValue.Value);
			}
			if (utilityEntry.tariff.HasValue)
			{
				jsonObjectBuilder.Add("tt", utilityEntry.tariff.Value);
			}
			if (utilityEntry.percentage.HasValue)
			{
				jsonObjectBuilder.Add("p", utilityEntry.percentage.Value);
			}
		}
		return jsonObjectBuilder;
	}

	public override object ToObject(JsonParser json)
	{
		UtilityEntry utilityEntry = new UtilityEntry();
		utilityEntry.value = json["vl"].GetValue<int>();
		utilityEntry.timestamp = json["ti"].GetValue<DateTime>();
		JsonParser jsonParser = json["cv"];
		if (jsonParser != null)
		{
			utilityEntry.calculatedValue = jsonParser.GetValue<double>();
		}
		JsonParser jsonParser2 = json["tt"];
		if (jsonParser2 != null)
		{
			utilityEntry.tariff = jsonParser2.GetValue<int>();
		}
		JsonParser jsonParser3 = json["p"];
		if (jsonParser3 != null)
		{
			utilityEntry.percentage = jsonParser3.GetValue<int>();
		}
		return utilityEntry;
	}
}
