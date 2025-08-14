using System.Collections.Generic;
using WebServerHost.Web;

namespace WebServerHost.Controllers.Account;

[Route("friend")]
public class FriendController : Controller
{
	[HttpGet]
	[Route("")]
	public IResult RetrieveFriends()
	{
		return Ok(new List<string>());
	}
}
