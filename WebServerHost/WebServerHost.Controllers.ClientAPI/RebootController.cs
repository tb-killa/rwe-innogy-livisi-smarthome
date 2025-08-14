using WebServerHost.Web;

namespace WebServerHost.Controllers.ClientAPI;

[Route("reboot")]
public class RebootController : Controller
{
	private readonly IRebootService rebootService;

	public RebootController(IRebootService rebootService)
	{
		this.rebootService = rebootService;
	}

	[Route("")]
	[HttpGet]
	public IResult Reboot()
	{
		rebootService.Reboot();
		return Ok();
	}
}
