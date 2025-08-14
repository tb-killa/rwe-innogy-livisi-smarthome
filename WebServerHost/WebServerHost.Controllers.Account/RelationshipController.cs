using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core.Authentication;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Entities.Account;
using WebServerHost.Helpers;
using WebServerHost.Web;

namespace WebServerHost.Controllers.Account;

[Route("relationship")]
public class RelationshipController : Controller
{
	private ILocalUserManager userManager;

	private IShcClient shcClient;

	public RelationshipController(ILocalUserManager userManager, IShcClient shcClient)
	{
		this.userManager = userManager;
		this.shcClient = shcClient;
	}

	[HttpGet]
	[Route("")]
	public IResult Get()
	{
		return Ok(new List<Relationship>
		{
			new Relationship
			{
				AccountName = userManager.User.Name,
				SerialNumber = SHCSerialNumber.SerialNumber(),
				Config = new List<Property>
				{
					new Property
					{
						Name = "permission",
						Value = "All"
					},
					new Property
					{
						Name = "name",
						Value = "SmartHome"
					}
				}
			}
		});
	}

	[HttpDelete]
	[Route("")]
	public IResult Delete()
	{
		FactoryResetRequest request = ShcRequestHelper.NewRequest<FactoryResetRequest>();
		shcClient.GetResponse(request);
		return Ok();
	}
}
