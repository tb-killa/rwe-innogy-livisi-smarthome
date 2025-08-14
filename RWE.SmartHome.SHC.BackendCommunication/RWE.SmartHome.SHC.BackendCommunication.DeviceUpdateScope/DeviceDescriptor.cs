using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.DeviceUpdateScope;

[Serializable]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
[DesignerCategory("code")]
[XmlType(Namespace = "http://rwe.com/SmartHome/2010/09/08/Common")]
[DebuggerStepThrough]
public class DeviceDescriptor
{
	private string addInVersionField;

	private string currentFirmwareVersionField;

	private string hardwareVersionField;

	private short manufacturerField;

	private bool manufacturerFieldSpecified;

	private int productIdField;

	private bool productIdFieldSpecified;

	[XmlElement(IsNullable = true, Order = 0)]
	public string AddInVersion
	{
		get
		{
			return addInVersionField;
		}
		set
		{
			addInVersionField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 1)]
	public string CurrentFirmwareVersion
	{
		get
		{
			return currentFirmwareVersionField;
		}
		set
		{
			currentFirmwareVersionField = value;
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

	[XmlElement(Order = 3)]
	public short Manufacturer
	{
		get
		{
			return manufacturerField;
		}
		set
		{
			manufacturerField = value;
		}
	}

	[XmlIgnore]
	public bool ManufacturerSpecified
	{
		get
		{
			return manufacturerFieldSpecified;
		}
		set
		{
			manufacturerFieldSpecified = value;
		}
	}

	[XmlElement(Order = 4)]
	public int ProductId
	{
		get
		{
			return productIdField;
		}
		set
		{
			productIdField = value;
		}
	}

	[XmlIgnore]
	public bool ProductIdSpecified
	{
		get
		{
			return productIdFieldSpecified;
		}
		set
		{
			productIdFieldSpecified = value;
		}
	}
}
