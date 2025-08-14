using SmartHome.Common.API.Entities.Entities;
using WebServerHost.Web;

namespace WebServerHost.Controllers.ClientAPI;

[Route("device_keys")]
public class DeviceKeysController : Controller
{
	private readonly IDeviceKeysService deviceKeysService;

	public DeviceKeysController(IDeviceKeysService deviceKeysService)
	{
		this.deviceKeysService = deviceKeysService;
	}

	[Route("")]
	[HttpGet]
	public GetDeviceKeysStatusResponse GetDeviceKeysStatus()
	{
		GetDeviceKeysStatusResponse getDeviceKeysStatusResponse = new GetDeviceKeysStatusResponse();
		getDeviceKeysStatusResponse.MasterKey = deviceKeysService.GetMasterKeyStatus();
		getDeviceKeysStatusResponse.NumberOfStoredKeys = deviceKeysService.GetNumberOfDeviceKeys();
		getDeviceKeysStatusResponse.ExportNeeded = deviceKeysService.IsExportOfDeviceKeysNeeded();
		return getDeviceKeysStatusResponse;
	}
}
