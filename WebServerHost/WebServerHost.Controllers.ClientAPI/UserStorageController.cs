using System.Collections.Generic;
using System.Linq;
using SmartHome.Common.API.Entities.ClientData;
using SmartHome.Common.API.Entities.ErrorHandling;
using SmartHome.Common.API.Entities.Extensions;
using WebServerHost.Web;

namespace WebServerHost.Controllers.ClientAPI;

[Route("userstorage")]
public class UserStorageController : Controller
{
	private IUserStorageService userStorage;

	public UserStorageController(IUserStorageService userStorage)
	{
		this.userStorage = userStorage;
	}

	[HttpGet]
	[Route("{partition}/{key}")]
	public IResult Get([FromRoute] string partition, [FromRoute] string key)
	{
		List<UserData> list = userStorage.Get();
		if (partition.IsNotEmptyOrNull() && key.IsNotEmptyOrNull())
		{
			list = list.Where((UserData d) => d.Partition == partition && d.Key == key).ToList();
		}
		return Ok(list);
	}

	[Route("")]
	[HttpGet]
	public IResult GetAll()
	{
		List<UserData> data = userStorage.Get();
		return Ok(data);
	}

	[Route("")]
	[HttpPost]
	public IResult Post([FromBody] List<UserData> data)
	{
		if (data == null || !data.Any())
		{
			throw new ApiException(ErrorCode.EntityMalformedContent);
		}
		userStorage.Save(data);
		return Ok();
	}

	[Route("")]
	[HttpPut]
	public IResult Put([FromBody] List<UserData> data)
	{
		if (data == null || !data.Any())
		{
			throw new ApiException(ErrorCode.EntityMalformedContent);
		}
		userStorage.Update(data);
		return Ok();
	}

	[Route("{partition}/{key}")]
	[HttpDelete]
	public IResult Delete([FromRoute] string partition, [FromRoute] string key)
	{
		if (partition.IsNotEmptyOrNull() && key.IsNotEmptyOrNull())
		{
			userStorage.Delete(partition, key);
		}
		else
		{
			BadRequest("missing parameters");
		}
		return Ok();
	}

	[HttpDelete]
	[Route("")]
	public IResult Delete()
	{
		userStorage.DeletaAll();
		return Ok();
	}
}
