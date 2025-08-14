using System;
using System.IO;
using System.ServiceModel.Channels;
using System.Xml;

namespace SmartHome.SHC.SCommAdapter;

public class WcfMessage : Message
{
	private readonly MessageHeaders headers;

	private readonly MessageProperties properties;

	private readonly MessageVersion version;

	private readonly string content;

	private readonly bool isFault;

	private readonly string faultReason;

	public override bool IsEmpty => string.IsNullOrEmpty(content);

	public string FaultReason => faultReason;

	public override bool IsFault => isFault;

	public override MessageHeaders Headers => headers;

	public override MessageProperties Properties => properties;

	public override MessageVersion Version => version;

	private WcfMessage(MessageVersion version)
	{
		headers = new MessageHeaders(version);
		properties = new MessageProperties();
		this.version = version;
	}

	public WcfMessage(MessageVersion version, Stream contentStream)
		: this(version)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(contentStream);
		ParseContentAndHeaders(xmlDocument, out content);
		if (!IsEmpty)
		{
			xmlDocument.LoadXml(content);
		}
		isFault = xmlDocument.DocumentElement != null && xmlDocument.DocumentElement.LocalName.Equals("Fault", StringComparison.OrdinalIgnoreCase);
		if (isFault)
		{
			faultReason = GetInnerXML(xmlDocument, "//faultstring");
			if (string.IsNullOrEmpty(faultReason))
			{
				faultReason = content;
			}
		}
	}

	public override string ToString()
	{
		return content;
	}

	protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
	{
		using StringReader input = new StringReader(content);
		using XmlReader reader = XmlReader.Create(input);
		writer.WriteNode(reader, defattr: true);
	}

	private void ParseContentAndHeaders(XmlDocument xml, out string content)
	{
		content = null;
		XmlNode xmlNode = null;
		if (string.IsNullOrEmpty(xml.DocumentElement.NamespaceURI))
		{
			content = GetInnerXML(xml, "//Body");
			xmlNode = GetFirstNodeExist(xml, "//Header", "//Headers");
		}
		else
		{
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xml.NameTable);
			xmlNamespaceManager.AddNamespace("s", xml.DocumentElement.NamespaceURI);
			content = GetInnerXML(xml, "//s:Body", xmlNamespaceManager);
			xmlNode = GetFirstNodeExist(xml, "//s:Header", "//s:Headers", xmlNamespaceManager);
		}
		if (string.IsNullOrEmpty(content))
		{
			content = GetFirstChildTwoLevelInnerXml(xml);
		}
		if (xmlNode == null)
		{
			return;
		}
		foreach (XmlNode item in xmlNode)
		{
			headers.Add(new WcfMessageHeader(item.LocalName, item.NamespaceURI, item.InnerXml));
		}
	}

	private string GetFirstChildTwoLevelInnerXml(XmlDocument xml)
	{
		if (xml.FirstChild != null && xml.FirstChild.FirstChild != null)
		{
			return xml.FirstChild.FirstChild.InnerXml;
		}
		if (xml.FirstChild != null)
		{
			return xml.FirstChild.InnerXml;
		}
		return xml.InnerXml;
	}

	private string GetInnerXML(XmlDocument xml, string xpath)
	{
		return xml.SelectSingleNode(xpath)?.InnerXml;
	}

	private string GetInnerXML(XmlDocument xml, string xpath, XmlNamespaceManager nameSpace)
	{
		return xml.SelectSingleNode(xpath, nameSpace)?.InnerXml;
	}

	private XmlNode GetFirstNodeExist(XmlDocument xml, string xpath1, string xpath2)
	{
		XmlNode xmlNode = xml.SelectSingleNode(xpath1);
		if (xmlNode == null)
		{
			xmlNode = xml.SelectSingleNode(xpath2);
		}
		return xmlNode;
	}

	private XmlNode GetFirstNodeExist(XmlDocument xml, string xpath1, string xpath2, XmlNamespaceManager nameSpace)
	{
		XmlNode xmlNode = xml.SelectSingleNode(xpath1, nameSpace);
		if (xmlNode == null)
		{
			xmlNode = xml.SelectSingleNode(xpath2, nameSpace);
		}
		return xmlNode;
	}
}
