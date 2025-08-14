using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.API.Serializer;

public static class Serializer
{
	public static byte[] ToByteArray(BaseRequest command)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(command.GetType());
		using MemoryStream memoryStream = new MemoryStream();
		XmlWriter xmlWriter = XmlWriter.Create(memoryStream, new XmlWriterSettings
		{
			Encoding = new UTF8Encoding()
		});
		if (xmlWriter != null)
		{
			xmlSerializer.Serialize(xmlWriter, command);
		}
		return memoryStream.ToArray();
	}

	public static T ToCommand<T>(Stream stream)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		return (T)xmlSerializer.Deserialize(stream);
	}

	public static T ToCommand<T>(string utf8String)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		using (StringReader input = new StringReader(utf8String))
		{
			using XmlReader xmlReader = XmlReader.Create(input);
			if (xmlSerializer.CanDeserialize(xmlReader))
			{
				return (T)xmlSerializer.Deserialize(xmlReader);
			}
		}
		return default(T);
	}

	public static string ReadBody(Stream input, int length)
	{
		byte[] array = new byte[length];
		int num = 0;
		while (num < length)
		{
			int num2 = input.ReadByte();
			if (num2 != -1)
			{
				array[num] = (byte)num2;
				num++;
			}
		}
		return Encoding.UTF8.GetString(array, 0, length);
	}
}
