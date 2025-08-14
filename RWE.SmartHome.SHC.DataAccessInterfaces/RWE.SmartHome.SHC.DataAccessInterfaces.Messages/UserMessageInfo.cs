using System;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;

namespace RWE.SmartHome.SHC.DataAccessInterfaces.Messages;

public class UserMessageInfo
{
	[XmlAttribute]
	public Guid UserId { get; set; }

	[XmlAttribute]
	public MessageState State { get; set; }
}
