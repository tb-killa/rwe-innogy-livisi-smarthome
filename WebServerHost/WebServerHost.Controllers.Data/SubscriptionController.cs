using System.Collections.Generic;
using WebServerHost.Web;

namespace WebServerHost.Controllers.Data;

[Route("subscription")]
internal class SubscriptionController : Controller
{
	[Route("{type}/{subtype}")]
	[Route("")]
	[HttpGet]
	public IResult GetSubscription([FromRoute] string type, [FromRoute] string subtype)
	{
		return Ok(new List<string>());
	}
}
