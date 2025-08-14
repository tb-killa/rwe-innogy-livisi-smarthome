using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.DataAccessInterfaces.DeviceActivityLogging;
using SmartHome.Common.API.Entities.Data;
using SmartHome.Common.API.Entities.ErrorHandling;
using SmartHome.Common.API.Entities.Extensions;
using WebServerHost.Helpers;
using WebServerHost.Web;

namespace WebServerHost.Controllers.Data;

[Route("utility")]
public class UtilityController : Controller
{
	private const int DefaultPageSize = 100;

	private static DateTime DefaultStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	private IUtilityDataService utilityDataService;

	public UtilityController(IUtilityDataService utilityDataService)
	{
		this.utilityDataService = utilityDataService;
	}

	[HttpGet]
	[Route("energy/{type}")]
	public IResult GetEnergy([FromRoute] string type, string meterId, string agregation, DateTime? start, DateTime? end, int? page, int? pageSize)
	{
		Error error = CheckParameter(type, ref start, ref end, ref page, ref pageSize);
		if (error != null)
		{
			return BadRequest(error);
		}
		string energyType = ResolveEnergyType(type);
		Utility energyData = utilityDataService.GetEnergyData(meterId, energyType, start.Value, end.Value, page.Value, pageSize.Value);
		return Ok(new List<Utility> { energyData });
	}

	private Error CheckParameter(string type, ref DateTime? start, ref DateTime? end, ref int? page, ref int? pageSize)
	{
		if (type.IsNullOrEmpty())
		{
			return ApiException.CreateErrorModel(ErrorCode.InvalidArgument, "<type> parameter is required");
		}
		return CheckParameter(ref start, ref end, ref page, ref pageSize);
	}

	private Error CheckParameter(ref DateTime? start, ref DateTime? end, ref int? page, ref int? pageSize)
	{
		if (Validator.ValidatePageParameters(page, pageSize))
		{
			if (!pageSize.HasValue)
			{
				pageSize = 100;
			}
			if (!page.HasValue)
			{
				page = 0;
			}
			if (!start.HasValue)
			{
				start = DefaultStart;
			}
			if (!end.HasValue)
			{
				end = DateTime.MaxValue;
			}
			return null;
		}
		return ApiException.CreateErrorModel(ErrorCode.InvalidArgument, "<page> and/or <pagesize> parameters can't be less then one.");
	}

	[Route("power/{type}")]
	[HttpGet]
	public IResult GetPower([FromRoute] string type, string meterId, string agregation, DateTime? start, DateTime? end, int? page, int? pageSize)
	{
		Error error = CheckParameter(type, ref start, ref end, ref page, ref pageSize);
		if (error != null)
		{
			return BadRequest(error);
		}
		string powerType = ResolvePowerType(type);
		Utility powerData = utilityDataService.GetPowerData(meterId, powerType, start.Value, end.Value, page.Value, pageSize.Value);
		return Ok(new List<Utility> { powerData });
	}

	[HttpGet]
	[Route("storage")]
	public IResult GetStorage(string meterId, string agregation, DateTime? start, DateTime? end, int? page, int? pageSize)
	{
		Error error = CheckParameter(ref start, ref end, ref page, ref pageSize);
		if (error != null)
		{
			return BadRequest(error);
		}
		Utility storageData = utilityDataService.GetStorageData(meterId, start.Value, end.Value, page.Value, pageSize.Value);
		return Ok(new List<Utility> { storageData });
	}

	[HttpDelete]
	[Route("")]
	public IResult DeleteAllUtility()
	{
		utilityDataService.Delete();
		return Ok();
	}

	[Route("energy/{meterid}")]
	[HttpDelete]
	public IResult DeleteEnergy([FromRoute] string meterid)
	{
		if (meterid.IsNullOrEmpty())
		{
			return BadRequest(ApiException.CreateErrorModel(ErrorCode.InvalidArgument, "<meterid> parameter is required"));
		}
		utilityDataService.Delete(UtilityType.Energy, meterid);
		return Ok();
	}

	[Route("power/{meterid}")]
	[HttpDelete]
	public IResult DeletePower([FromRoute] string meterid)
	{
		if (meterid.IsNullOrEmpty())
		{
			return BadRequest(ApiException.CreateErrorModel(ErrorCode.InvalidArgument, "<meterid> parameter is required"));
		}
		utilityDataService.Delete(UtilityType.Power, meterid);
		return Ok();
	}

	[Route("storage/{meterid}")]
	[HttpDelete]
	public IResult DeleteStorage([FromRoute] string meterid)
	{
		if (meterid.IsNullOrEmpty())
		{
			return BadRequest(ApiException.CreateErrorModel(ErrorCode.InvalidArgument, "<meterid> parameter is required"));
		}
		utilityDataService.Delete(UtilityType.Storage, meterid);
		return Ok();
	}

	[Route("energy/{type}/survey")]
	[HttpGet]
	public IResult GetEnergySurvey([FromRoute] string type, string meterId, string aggregation)
	{
		if (type.IsNullOrEmpty())
		{
			return BadRequest(ApiException.CreateErrorModel(ErrorCode.InvalidArgument, "<type> parameter is required"));
		}
		string energyType = ResolveEnergyType(type);
		Survey energySurvey = utilityDataService.GetEnergySurvey(meterId, energyType);
		if (energySurvey == null)
		{
			return Ok(new DataSurvey
			{
				Count = 0
			});
		}
		return Ok(new DataSurvey
		{
			Count = energySurvey.Count,
			MinDate = energySurvey.Max,
			MaxDate = energySurvey.Min
		});
	}

	[HttpGet]
	[Route("power/{type}/survey")]
	public IResult GetPowerSurvey([FromRoute] string type, string meterId, string aggregation)
	{
		if (type.IsNullOrEmpty())
		{
			return BadRequest(ApiException.CreateErrorModel(ErrorCode.InvalidArgument, "<type> parameter is required"));
		}
		string powerType = ResolvePowerType(type);
		Survey powerSurvey = utilityDataService.GetPowerSurvey(meterId, powerType);
		if (powerSurvey == null)
		{
			return Ok(new DataSurvey
			{
				Count = 0
			});
		}
		return Ok(new DataSurvey
		{
			Count = powerSurvey.Count,
			MinDate = powerSurvey.Min,
			MaxDate = powerSurvey.Max
		});
	}

	[HttpGet]
	[Route("storage/survey")]
	public IResult GetPowerSurvey(string meterId, string aggregation)
	{
		Survey storageSurvey = utilityDataService.GetStorageSurvey(meterId);
		if (storageSurvey == null)
		{
			return Ok(new DataSurvey
			{
				Count = 0
			});
		}
		return Ok(new DataSurvey
		{
			Count = storageSurvey.Count,
			MinDate = storageSurvey.Min,
			MaxDate = storageSurvey.Max
		});
	}

	private static string ResolveEnergyType(string type)
	{
		return type switch
		{
			"generation" => "GenerationEnergy", 
			"supply" => "SupplyEnergy", 
			"feedin" => "FeedEnergy", 
			_ => string.Empty, 
		};
	}

	private static string ResolvePowerType(string type)
	{
		switch (type)
		{
		case "generation":
			return "GenerationPower";
		case "supply":
			return "SupplyPower";
		case "feedin":
		case "twoway":
			return "TwoWayPower";
		case "chargedischarge":
			return "BatteryPower";
		default:
			return string.Empty;
		}
	}
}
