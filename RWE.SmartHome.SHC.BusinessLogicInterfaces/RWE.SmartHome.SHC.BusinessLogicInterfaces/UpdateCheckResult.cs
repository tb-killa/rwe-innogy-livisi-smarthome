namespace RWE.SmartHome.SHC.BusinessLogicInterfaces;

public class UpdateCheckResult
{
	public UpdateCheckResultCode ResultCode { get; private set; }

	public string UpdateVersion { get; private set; }

	public UpdateCheckResult(UpdateCheckResultCode resultCode, string newVersion)
	{
		ResultCode = resultCode;
		UpdateVersion = newVersion;
	}
}
