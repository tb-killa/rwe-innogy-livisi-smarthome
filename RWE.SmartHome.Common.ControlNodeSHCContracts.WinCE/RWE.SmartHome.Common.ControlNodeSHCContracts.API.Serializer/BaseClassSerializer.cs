using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.API.Serializer;

public class BaseClassSerializer<T>
{
	private XmlSerializer serializer;

	private XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

	private Type baseType;

	private Type[] types;

	private XmlSerializer Serializer => serializer ?? (serializer = new XmlSerializer(baseType, types.Where(baseType.IsAssignableFrom).ToArray()));

	public BaseClassSerializer(AssemblyName assemblyName)
		: this(Assembly.Load(assemblyName.FullName))
	{
	}

	public BaseClassSerializer(Assembly assembly)
	{
		namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
		baseType = typeof(T);
		types = assembly.GetTypes();
	}

	public BaseClassSerializer()
		: this(Assembly.GetExecutingAssembly())
	{
	}

	public bool CanDeserialize(string input)
	{
		using TextReader input2 = new StringReader(input);
		using XmlReader reader = XmlReader.Create(input2);
		return CanDeserialize(reader);
	}

	public bool CanDeserialize(XmlReader reader)
	{
		return Serializer.CanDeserialize(reader);
	}

	public T Deserialize(Stream stream)
	{
		return (T)Serializer.Deserialize(stream);
	}

	public T Deserialize(string input)
	{
		using TextReader textReader = new StringReader(input);
		return (T)Serializer.Deserialize(textReader);
	}

	public T Deserialize(XmlReader reader)
	{
		return (T)Serializer.Deserialize(reader);
	}

	public void Serialize(Stream stream, T obj)
	{
		Serializer.Serialize(stream, obj);
	}

	public void Serialize(TextWriter textWriter, T obj)
	{
		Serializer.Serialize(textWriter, obj);
	}

	public string Serialize(T obj)
	{
		if (obj is IPreserializedRequest)
		{
			return (obj as IPreserializedRequest).SerializeToXml();
		}
		bool flag = false;
		flag = true;
		using StringWriter stringWriter = new StringWriter();
		using XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
		{
			Indent = flag,
			OmitXmlDeclaration = true
		});
		Serializer.Serialize(xmlWriter, obj, namespaces);
		return stringWriter.ToString();
	}
}
