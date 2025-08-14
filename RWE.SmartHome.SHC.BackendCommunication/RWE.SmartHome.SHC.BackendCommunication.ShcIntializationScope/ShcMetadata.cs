using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope;

[Serializable]
[XmlType(Namespace = "http://rwe.com/SmartHome/2010/11/08/Common")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
public class ShcMetadata
{
	private string applicationVersionField;

	private string firmwareVersionField;

	private string hardwareVersionField;

	private string ipField;

	private string nameField;

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

	[XmlElement(IsNullable = true, Order = 3)]
	public string Ip
	{
		get
		{
			return ipField;
		}
		set
		{
			ipField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 4)]
	public string Name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}
}
