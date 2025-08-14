using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.DeviceUpdateScope;

[Serializable]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://rwe.com/SmartHome/2015/02/09/Common")]
public class DeviceUpdateInfo
{
	private string imageChecksumField;

	private string imageUrlField;

	private string releaseNotesLocationField;

	private DeviceUpdateType updateTypeField;

	private bool updateTypeFieldSpecified;

	private string versionNumberField;

	[XmlElement(IsNullable = true, Order = 0)]
	public string ImageChecksum
	{
		get
		{
			return imageChecksumField;
		}
		set
		{
			imageChecksumField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 1)]
	public string ImageUrl
	{
		get
		{
			return imageUrlField;
		}
		set
		{
			imageUrlField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 2)]
	public string ReleaseNotesLocation
	{
		get
		{
			return releaseNotesLocationField;
		}
		set
		{
			releaseNotesLocationField = value;
		}
	}

	[XmlElement(Order = 3)]
	public DeviceUpdateType UpdateType
	{
		get
		{
			return updateTypeField;
		}
		set
		{
			updateTypeField = value;
		}
	}

	[XmlIgnore]
	public bool UpdateTypeSpecified
	{
		get
		{
			return updateTypeFieldSpecified;
		}
		set
		{
			updateTypeFieldSpecified = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 4)]
	public string VersionNumber
	{
		get
		{
			return versionNumberField;
		}
		set
		{
			versionNumberField = value;
		}
	}
}
