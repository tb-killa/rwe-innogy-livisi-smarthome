using System.Collections.Generic;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using SmartHome.Common.API.Entities.Entities;
using WebServerHost.Web;

namespace WebServerHost.Controllers.ClientAPI;

[Route("product")]
public class ProductController : Controller
{
	private readonly IProductsService products;

	private readonly IRegistrationService registrationService;

	public ProductController(IProductsService products, IRegistrationService registrationService)
	{
		this.products = products;
		this.registrationService = registrationService;
	}

	[HttpGet]
	[Route("")]
	public IResult GetAvailableProducts(string provisioned, string isPremiumService, string isService, string productKind)
	{
		return Ok(products.GetAvailableProducts());
	}

	[HttpGet]
	[Route("{type}")]
	public IResult GetProduct([FromRoute] string type, [FromQuery] string culture)
	{
		Product product = products.GetProduct(type);
		if (product != null)
		{
			return Ok(product);
		}
		return NotFound();
	}

	[Route("hash")]
	[HttpGet]
	public IResult GetProductHash()
	{
		return Ok($"{{\"hash\":\"{products.GetHash()}\"}}");
	}

	[HttpGet]
	[Route("buyable")]
	public IResult GetBuyableProducts()
	{
		return Ok(new List<Product>());
	}

	[Route("add/{type}")]
	[HttpGet]
	public IResult AddProduct([FromRoute] string type)
	{
		if (!registrationService.IsShcLocalOnly)
		{
			return PreconditionFailed();
		}
		products.InstallProduct(type);
		return Ok();
	}

	[Route("update/{type}")]
	[HttpGet]
	public IResult UpdateProduct([FromRoute] string type)
	{
		return Ok();
	}

	[Route("activate/{type}")]
	[HttpGet]
	public IResult ActivateProduct([FromRoute] string type)
	{
		products.ActivateProduct(type);
		return Ok();
	}

	[Route("deactivate/{type}")]
	[HttpGet]
	public IResult DeactivateProduct([FromRoute] string type)
	{
		products.DeactivateProduct(type);
		return Ok();
	}

	[HttpDelete]
	[Route("remove/{type}")]
	public IResult RemoveProduct([FromRoute] string type)
	{
		if (!registrationService.IsShcLocalOnly)
		{
			return PreconditionFailed();
		}
		products.UninstallProduct(type);
		return Ok();
	}
}
