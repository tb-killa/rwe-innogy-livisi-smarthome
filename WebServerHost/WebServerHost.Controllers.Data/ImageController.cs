using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using SmartHome.Common.API.Entities.Data;
using SmartHome.Common.API.Entities.ErrorHandling;
using SmartHome.Common.API.Entities.Extensions;
using WebServerHost.Helpers;
using WebServerHost.Web;

namespace WebServerHost.Controllers.Data;

[Route("image")]
public class ImageController : Controller
{
	private readonly IImageService imageService;

	private readonly IShcClient shcClient;

	private readonly IRepository repository;

	public ImageController(IImageService imageService, IShcClient shcClient, IRepository repository)
	{
		this.imageService = imageService;
		this.shcClient = shcClient;
		this.repository = repository;
	}

	[Route("metadata")]
	[Route("thumbnail")]
	[HttpGet]
	[Route("")]
	[Route("raw")]
	public List<Image> GetAllImages(string camera, string cameraid, DateTime? start, DateTime? end, int? page, int? pagesize, string size)
	{
		IEnumerable<Image> source = ((!cameraid.IsNotEmptyOrNull()) ? (from i in imageService.GetAllImages()
			where !i.IsLiveImage
			select i) : (from i in imageService.GetAllImages(cameraid)
			where !i.IsLiveImage
			select i));
		if (start.HasValue && end.HasValue)
		{
			source = source.Where(delegate(Image i)
			{
				DateTime value = i.Timestamp.ToUniversalTime();
				DateTime? dateTime = start;
				if (value >= dateTime)
				{
					DateTime value2 = i.Timestamp.ToUniversalTime();
					DateTime? dateTime2 = end;
					return value2 <= dateTime2;
				}
				return false;
			});
		}
		if (Validator.ValidatePageParameters(page, pagesize))
		{
			if (!pagesize.HasValue)
			{
				pagesize = 100;
			}
			if (page.HasValue)
			{
				source = source.Skip((page.Value - 1) * pagesize.Value).Take(pagesize.Value);
			}
		}
		return source.ToList();
	}

	[HttpGet]
	[Route("{id}/thumbnail")]
	[Route("{id}/raw")]
	[Route("{id}")]
	public IResult GetImage(string id, string size)
	{
		if (id.IsNotEmptyOrNull())
		{
			Image image = imageService.GetAllImages().FirstOrDefault((Image i) => i.Id == id);
			if (image != null)
			{
				return Ok(image);
			}
			return NotFound(ApiException.CreateErrorModel(ErrorCode.EntityDoesNotExist));
		}
		return BadRequest(ApiException.CreateErrorModel(ErrorCode.InvalidArgument, "Parameter <id> is not valid"));
	}

	[HttpDelete]
	[Route("")]
	public IResult DeleteMultipleImages(string camera, string cameraid, DateTime? start, DateTime? end)
	{
		IEnumerable<Image> enumerable = (cameraid.IsNotEmptyOrNull() ? imageService.GetAllImages(cameraid) : imageService.GetAllImages());
		if (start.HasValue && end.HasValue)
		{
			enumerable = enumerable.Where(delegate(Image i)
			{
				DateTime value = i.Timestamp.ToUniversalTime();
				DateTime? dateTime = start;
				if (value >= dateTime)
				{
					DateTime value2 = i.Timestamp.ToUniversalTime();
					DateTime? dateTime2 = end;
					return value2 <= dateTime2;
				}
				return false;
			});
		}
		foreach (Image item in enumerable)
		{
			imageService.DeleteImage(item.Id);
		}
		return Ok();
	}

	[Route("{id}")]
	[HttpDelete]
	public IResult DeleteImage(string id)
	{
		if (id.IsNotEmptyOrNull())
		{
			imageService.DeleteImage(id);
			return Ok();
		}
		return BadRequest(ApiException.CreateErrorModel(ErrorCode.InvalidArgument, "parmameter <id> is invalid"));
	}

	[HttpGet]
	[Route("{id}/survey")]
	public IResult GetSurvey(string id)
	{
		IEnumerable<Image> source = from i in imageService.GetAllImages()
			where !i.IsLiveImage
			select i;
		DataSurvey dataSurvey = new DataSurvey();
		if (source.Any())
		{
			dataSurvey.MaxDate = source.Max((Image i) => i.Timestamp);
			dataSurvey.MinDate = source.Min((Image i) => i.Timestamp);
			dataSurvey.Count = source.Count();
		}
		else
		{
			dataSurvey.Count = 0;
		}
		return Ok(dataSurvey);
	}
}
