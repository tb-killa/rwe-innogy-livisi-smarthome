using System.ServiceModel;

namespace Microsoft.Tools.ServiceModel;

public class CFFaultException : CommunicationException
{
	private string _faultMessage;

	public string FaultMessage => _faultMessage;

	public CFFaultException(string faultMessage)
	{
		_faultMessage = faultMessage;
	}
}
