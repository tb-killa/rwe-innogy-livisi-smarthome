using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.PublicKey;

[Serializable]
[XmlType(Namespace = "urn:public_keyxsd")]
[DebuggerStepThrough]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DesignerCategory("code")]
public class publicKeyReportType
{
	private uint key_typeField;

	private bool key_typeFieldSpecified;

	private byte[] valueField;

	[XmlAttribute]
	public uint key_type
	{
		get
		{
			return key_typeField;
		}
		set
		{
			key_typeField = value;
		}
	}

	[XmlIgnore]
	public bool key_typeSpecified
	{
		get
		{
			return key_typeFieldSpecified;
		}
		set
		{
			key_typeFieldSpecified = value;
		}
	}

	[XmlText(DataType = "hexBinary")]
	public byte[] Value
	{
		get
		{
			return valueField;
		}
		set
		{
			valueField = value;
		}
	}
}
