using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope;

[Serializable]
[DesignerCategory("code")]
[XmlType(Namespace = "http://rwe.com/SmartHome/2010/09/08/Common")]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
[DebuggerStepThrough]
public class ShcVersionInfo
{
	private string applicationVersionField;

	private string firmwareVersionField;

	private string hardwareVersionField;

	[XmlElement(IsNullable = true, Order = 0)]
	public string ApplicationVersion
	{
		get
		{
			return applicationVersionField;
		}
		set
		{
			applicationVersionField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 1)]
	public string FirmwareVersion
	{
		get
		{
			return firmwareVersionField;
		}
		set
		{
			firmwareVersionField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 2)]
	public string HardwareVersion
	{
		get
		{
			return hardwareVersionField;
		}
		set
		{
			hardwareVersionField = value;
		}
	}
}
