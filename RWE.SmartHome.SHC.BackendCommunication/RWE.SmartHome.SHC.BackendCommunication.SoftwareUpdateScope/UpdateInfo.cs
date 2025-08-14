using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope;

[Serializable]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
[DesignerCategory("code")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://rwe.com/SmartHome/2010/09/08/Common")]
public class UpdateInfo
{
	private UpdateCategory categoryField;

	private bool categoryFieldSpecified;

	private string downloadLocationField;

	private string downloadPasswordField;

	private string downloadUserField;

	private UpdateType typeField;

	private bool typeFieldSpecified;

	private DateTime updateDeadlineField;

	private bool updateDeadlineFieldSpecified;

	private string versionField;

	[XmlElement(Order = 0)]
	public UpdateCategory Category
	{
		get
		{
			return categoryField;
		}
		set
		{
			categoryField = value;
		}
	}

	[XmlIgnore]
	public bool CategorySpecified
	{
		get
		{
			return categoryFieldSpecified;
		}
		set
		{
			categoryFieldSpecified = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 1)]
	public string DownloadLocation
	{
		get
		{
			return downloadLocationField;
		}
		set
		{
			downloadLocationField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 2)]
	public string DownloadPassword
	{
		get
		{
			return downloadPasswordField;
		}
		set
		{
			downloadPasswordField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 3)]
	public string DownloadUser
	{
		get
		{
			return downloadUserField;
		}
		set
		{
			downloadUserField = value;
		}
	}

	[XmlElement(Order = 4)]
	public UpdateType Type
	{
		get
		{
			return typeField;
		}
		set
		{
			typeField = value;
		}
	}

	[XmlIgnore]
	public bool TypeSpecified
	{
		get
		{
			return typeFieldSpecified;
		}
		set
		{
			typeFieldSpecified = value;
		}
	}

	[XmlElement(Order = 5)]
	public DateTime UpdateDeadline
	{
		get
		{
			return updateDeadlineField;
		}
		set
		{
			updateDeadlineField = value;
		}
	}

	[XmlIgnore]
	public bool UpdateDeadlineSpecified
	{
		get
		{
			return updateDeadlineFieldSpecified;
		}
		set
		{
			updateDeadlineFieldSpecified = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 6)]
	public string Version
	{
		get
		{
			return versionField;
		}
		set
		{
			versionField = value;
		}
	}
}
