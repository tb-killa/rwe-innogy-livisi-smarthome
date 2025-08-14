using System.Collections.Generic;
using System.Linq;
using JsonLite;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;
using SmartHome.Common.API.Entities.Extensions;

namespace WebServerHost.Helpers;

internal class TrackDataJsonSerializer
{
	public static string Serialize(IEnumerable<TrackData> data)
	{
		JsonArrayBuilder jsonArrayBuilder = new JsonArrayBuilder(JsonType.Object);
		foreach (TrackData datum in data)
		{
			jsonArrayBuilder.Add(CreateJsonObject(datum));
		}
		return jsonArrayBuilder.Build();
	}

	private static JsonBuilder CreateJsonObject(TrackData data)
	{
		JsonObjectBuilder jsonObjectBuilder = new JsonObjectBuilder().Add("eventType", data.EventType).Add("eventTime", data.Timestamp).Add("entityId", data.EntityId);
		if (data.Properties.Any())
		{
			Property property = data.Properties.First();
			jsonObjectBuilder.Add("dataName", property.Name.FirstToLower()).Add("dataValue", property.Value.ToString());
		}
		return jsonObjectBuilder;
	}
}
