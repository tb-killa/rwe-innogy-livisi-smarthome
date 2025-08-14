using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;

namespace RWE.SmartHome.SHC.ChannelInterfaces;

public class NotificationValidator
{
	private XmlSerializer serializer;

	public NotificationValidator(XmlSerializer serializer)
	{
		this.serializer = serializer;
	}

	public void ValidateNotification(BaseNotification notification)
	{
		using XmlWriter xmlWriter = XmlWriter.Create(TextWriter.Null, new XmlWriterSettings
		{
			Indent = false,
			OmitXmlDeclaration = true
		});
		serializer.Serialize(xmlWriter, new NotificationList
		{
			Notifications = new List<BaseNotification>(1) { notification }
		});
		xmlWriter.Close();
	}
}
