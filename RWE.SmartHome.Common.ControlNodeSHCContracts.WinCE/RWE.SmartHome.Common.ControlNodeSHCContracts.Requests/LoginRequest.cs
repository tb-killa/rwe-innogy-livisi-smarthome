using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;

public class LoginRequest : BaseRequest, IPreserializedRequest
{
	private const string REQUEST_WITH_PLACEHOLDERS_AND_ACCOUNTNAME = "<BaseRequest xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"LoginRequest\" Version=\"{0}\" RequestId=\"{1}\" UserName=\"{2}\" Password=\"{3}\" AccountName=\"{4}\"/>";

	private const string REQUEST_WITH_PLACEHOLDERS_WITHOUT_ACCOUNTNAME = "<BaseRequest xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"LoginRequest\" Version=\"{0}\" RequestId=\"{1}\" UserName=\"{2}\" Password=\"{3}\"/>";

	[XmlAttribute]
	public string UserName { get; set; }

	[XmlAttribute]
	public string Password { get; set; }

	[XmlAttribute]
	public string AccountName { get; set; }

	public string SerializeToXml()
	{
		return (AccountName != null) ? $"<BaseRequest xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"LoginRequest\" Version=\"{base.Version}\" RequestId=\"{base.RequestId}\" UserName=\"{UserName}\" Password=\"{Password}\" AccountName=\"{AccountName}\"/>" : $"<BaseRequest xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"LoginRequest\" Version=\"{base.Version}\" RequestId=\"{base.RequestId}\" UserName=\"{UserName}\" Password=\"{Password}\"/>";
	}
}
