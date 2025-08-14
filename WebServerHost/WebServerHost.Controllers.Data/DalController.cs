using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.DataAccessInterfaces.DeviceActivityLogging;
using SmartHome.Common.API.Entities.Data;
using SmartHome.Common.API.Entities.Extensions;
using WebServerHost.Helpers;
using WebServerHost.Web;

namespace WebServerHost.Controllers.Data;

[Route("")]
public class DalController : Controller
{
	private ITrackDataPersistence trackDataStorage;

	private IEventManager eventManager;

	public DalController(ITrackDataPersistence trackDataStorage, IEventManager eventManager)
	{
		this.trackDataStorage = trackDataStorage;
		this.eventManager = eventManager;
	}

	[Route("data")]
	[HttpDelete]
	public IResult Delete()
	{
		trackDataStorage.DeleteAll();
		return Ok();
	}

	private string GetEntityType()
	{
		string[] array = base.Request.RequestUri.Split('/');
		return array[array.Length - 1];
	}

	[HttpDelete]
	[Route("all")]
	public IResult DeleteAllDalData()
	{
		eventManager.GetEvent<DeleteAllLocalDataEvent>().Publish(new DeleteAllLocalDataEventArgs());
		return Ok();
	}

	[Route("device")]
	[HttpGet]
	[Route("capability")]
	public IResult GetEntityData(string entityId, DateTime? start, DateTime? end, string eventType, string dataName, int? page, int? pageSize)
	{
		IEnumerable<TrackData> enumerable = new List<TrackData>();
		string entityType = GetEntityType();
		SetDefaultWhereMissing(ref start, ref end, ref page, ref pageSize);
		enumerable = ((!entityId.IsNotEmptyOrNull()) ? trackDataStorage.GetEvents(entityType, eventType, start.Value, end.Value, pageSize.Value * (page.Value - 1), pageSize.Value, dataName) : trackDataStorage.GetEvents(entityType, entityId, eventType, start.Value, end.Value, pageSize.Value * (page.Value - 1), pageSize.Value, dataName));
		return Ok(TrackDataJsonSerializer.Serialize(enumerable));
	}

	private static void SetDefaultWhereMissing(ref DateTime? start, ref DateTime? end, ref int? page, ref int? pageSize)
	{
		if (!start.HasValue)
		{
			start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		}
		if (!end.HasValue)
		{
			end = DateTime.UtcNow;
		}
		if (!page.HasValue || page.Value < 1)
		{
			page = 1;
		}
		if (!pageSize.HasValue)
		{
			pageSize = 100;
		}
	}

	[Route("survey/device")]
	[Route("survey/capability")]
	[HttpGet]
	public IResult GetEntitySurvey(string entityId, DateTime? start, DateTime? end, string dataName, int? page, int? pageSize, bool? includeData)
	{
		string entityType = GetEntityType();
		SetDefaultWhereMissing(ref start, ref end, ref page, ref pageSize);
		IEnumerable<TrackData> enumerable = new List<TrackData>();
		enumerable = ((!entityId.IsNotEmptyOrNull()) ? trackDataStorage.GetEventsForEntity(entityType, start.Value, end.Value, pageSize.Value * (page.Value - 1), pageSize.Value, dataName) : trackDataStorage.GetEventsForEntity(entityType, entityId, start.Value, end.Value, pageSize.Value * (page.Value - 1), pageSize.Value, dataName));
		DataSurvey data;
		if (enumerable.Any())
		{
			DataSurvey dataSurvey = new DataSurvey();
			dataSurvey.Count = enumerable.Count();
			dataSurvey.MaxDate = enumerable.Max((TrackData d) => d.Timestamp).Date;
			dataSurvey.MinDate = enumerable.Min((TrackData d) => d.Timestamp).Date;
			dataSurvey.Records = ((includeData.HasValue && includeData.Value) ? new List<DateTime>(enumerable.Select((TrackData d) => d.Timestamp)) : null);
			data = dataSurvey;
		}
		else
		{
			DataSurvey dataSurvey2 = new DataSurvey();
			dataSurvey2.Count = 0;
			data = dataSurvey2;
		}
		return Ok(data);
	}
}
