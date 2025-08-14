using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ApplicationTokenScope;

[Serializable]
[DesignerCategory("code")]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
[XmlType(Namespace = "http://rwe.com/SmartHome/2011/11/15/ApplicationManagement")]
[DebuggerStepThrough]
public class ApplicationsToken
{
	private ApplicationTokenEntry[] entriesField;

	private string hashField;

	private long shcTypeField;

	private bool shcTypeFieldSpecified;

	[XmlArray(IsNullable = true, Order = 0)]
	public ApplicationTokenEntry[] Entries
	{
		get
		{
			return entriesField;
		}
		set
		{
			entriesField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 1)]
	public string Hash
	{
		get
		{
			return hashField;
		}
		set
		{
			hashField = value;
		}
	}

	[XmlElement(Order = 2)]
	public long ShcType
	{
		get
		{
			return shcTypeField;
		}
		set
		{
			shcTypeField = value;
		}
	}

	[XmlIgnore]
	public bool ShcTypeSpecified
	{
		get
		{
			return shcTypeFieldSpecified;
		}
		set
		{
			shcTypeFieldSpecified = value;
		}
	}
}
