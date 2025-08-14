using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.MemoryInformation;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlType(Namespace = "urn:memory_informationxsd")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class memoryInformationType
{
	private uint memory_idField;

	private uint countField;

	private uint free_countField;

	[XmlAttribute]
	public uint memory_id
	{
		get
		{
			return memory_idField;
		}
		set
		{
			memory_idField = value;
		}
	}

	[XmlAttribute]
	public uint count
	{
		get
		{
			return countField;
		}
		set
		{
			countField = value;
		}
	}

	[XmlAttribute]
	public uint free_count
	{
		get
		{
			return free_countField;
		}
		set
		{
			free_countField = value;
		}
	}
}
