namespace Rebex.Security.Certificates;

public class CertificateValidationParameters
{
	private ValidationOptions tnrki;

	private string umawv;

	public ValidationOptions Options
	{
		get
		{
			return tnrki;
		}
		set
		{
			tnrki = value;
		}
	}

	public string ServerName
	{
		get
		{
			return umawv;
		}
		set
		{
			umawv = value;
		}
	}

	internal CertificateValidationParameters(ValidationOptions options, string serverName)
	{
		Options = options;
		ServerName = serverName;
	}

	public CertificateValidationParameters()
		: this(ValidationOptions.None, null)
	{
	}
}
