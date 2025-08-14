using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Phy;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "urn:phyxsd")]
[XmlRoot(Namespace = "urn:phyxsd", IsNullable = false)]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class phy
{
	private byte[] payloadField;

	private uint phy_layer_versionField;

	private uint securityField;

	private uint foward_error_correctionField;

	private uint foward_error_correction_lengthField;

	private bool foward_error_correction_lengthFieldSpecified;

	[XmlAttribute(DataType = "hexBinary")]
	public byte[] payload
	{
		get
		{
			return payloadField;
		}
		set
		{
			payloadField = value;
		}
	}

	[XmlAttribute]
	public uint phy_layer_version
	{
		get
		{
			return phy_layer_versionField;
		}
		set
		{
			phy_layer_versionField = value;
		}
	}

	[XmlAttribute]
	public uint security
	{
		get
		{
			return securityField;
		}
		set
		{
			securityField = value;
		}
	}

	[XmlAttribute]
	public uint foward_error_correction
	{
		get
		{
			return foward_error_correctionField;
		}
		set
		{
			foward_error_correctionField = value;
		}
	}

	[XmlAttribute]
	public uint foward_error_correction_length
	{
		get
		{
			return foward_error_correction_lengthField;
		}
		set
		{
			foward_error_correction_lengthField = value;
		}
	}

	[XmlIgnore]
	public bool foward_error_correction_lengthSpecified
	{
		get
		{
			return foward_error_correction_lengthFieldSpecified;
		}
		set
		{
			foward_error_correction_lengthFieldSpecified = value;
		}
	}
}
