using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope;

[Serializable]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
[XmlType(Namespace = "http://rwe.com/SmartHome/2010/11/08/Common")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class ShcUser
{
	private DateTime createDateField;

	private bool createDateFieldSpecified;

	private string idField;

	private string nameField;

	private string passwordHashField;

	private ulong permissionField;

	private bool permissionFieldSpecified;

	private ShcRef[] rolesField;

	[XmlElement(Order = 0)]
	public DateTime CreateDate
	{
		get
		{
			return createDateField;
		}
		set
		{
			createDateField = value;
		}
	}

	[XmlIgnore]
	public bool CreateDateSpecified
	{
		get
		{
			return createDateFieldSpecified;
		}
		set
		{
			createDateFieldSpecified = value;
		}
	}

	[XmlElement(Order = 1)]
	public string Id
	{
		get
		{
			return idField;
		}
		set
		{
			idField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 2)]
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

	[XmlElement(IsNullable = true, Order = 3)]
	public string PasswordHash
	{
		get
		{
			return passwordHashField;
		}
		set
		{
			passwordHashField = value;
		}
	}

	[XmlElement(Order = 4)]
	public ulong Permission
	{
		get
		{
			return permissionField;
		}
		set
		{
			permissionField = value;
		}
	}

	[XmlIgnore]
	public bool PermissionSpecified
	{
		get
		{
			return permissionFieldSpecified;
		}
		set
		{
			permissionFieldSpecified = value;
		}
	}

	[XmlArray(IsNullable = true, Order = 5)]
	public ShcRef[] Roles
	{
		get
		{
			return rolesField;
		}
		set
		{
			rolesField = value;
		}
	}
}
