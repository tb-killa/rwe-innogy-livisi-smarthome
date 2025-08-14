using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities;

public class UserInfo
{
	[XmlAttribute]
	public string Username { get; set; }

	[XmlAttribute]
	public string Channel { get; set; }

	public UserInfo()
	{
	}

	public UserInfo(string username, string channel)
	{
		Username = username;
		Channel = channel;
	}
}
