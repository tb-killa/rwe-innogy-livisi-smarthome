using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.FirmwareUpdate;

[Serializable]
[DesignerCategory("code")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[XmlType(Namespace = "urn:firmware_updatexsd")]
public class firmwareReportType
{
	private uint statusField;

	private uint expected_offsetField;

	private bool expected_offsetFieldSpecified;

	[XmlAttribute]
	public uint status
	{
		get
		{
			return statusField;
		}
		set
		{
			statusField = value;
		}
	}

	[XmlAttribute]
	public uint expected_offset
	{
		get
		{
			return expected_offsetField;
		}
		set
		{
			expected_offsetField = value;
		}
	}

	[XmlIgnore]
	public bool expected_offsetSpecified
	{
		get
		{
			return expected_offsetFieldSpecified;
		}
		set
		{
			expected_offsetFieldSpecified = value;
		}
	}
}
