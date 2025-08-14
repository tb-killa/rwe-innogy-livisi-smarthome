using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public static class XmlSerializerExtension
{
	public static string Serialize<T>(this XmlSerializer serializer, T obj)
	{
		using MemoryStream memoryStream = new MemoryStream();
		serializer.Serialize(memoryStream, obj);
		using StreamReader streamReader = new StreamReader(memoryStream);
		memoryStream.Position = 0L;
		string result = streamReader.ReadToEnd();
		streamReader.Close();
		return result;
	}

	public static string Serialize<T>(this XmlSerializer serializer, T obj, XmlSerializerNamespaces namespaces)
	{
		using MemoryStream memoryStream = new MemoryStream();
		serializer.Serialize(memoryStream, obj, namespaces);
		using StreamReader streamReader = new StreamReader(memoryStream);
		memoryStream.Position = 0L;
		return streamReader.ReadToEnd();
	}

	public static T Deserialize<T>(this XmlSerializer serializer, string xml)
	{
		using StringReader input = new StringReader(xml);
		using XmlTextReader xmlReader = new XmlTextReader(input);
		return (T)serializer.Deserialize(xmlReader);
	}

	public static T DeserializeWithoutNamespace<T>(this XmlSerializer serializer, string xml)
	{
		using StringReader input = new StringReader(xml);
		using NamespaceIgnorantXmlTextReader xmlReader = new NamespaceIgnorantXmlTextReader(input);
		return (T)serializer.Deserialize(xmlReader);
	}
}
