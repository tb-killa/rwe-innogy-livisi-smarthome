using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope;

[Serializable]
[DesignerCategory("code")]
[XmlType(Namespace = "http://rwe.com/SmartHome/2010/11/08/Common")]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
[DebuggerStepThrough]
public class ShcSyncRecord
{
	private ShcRole[] rolesField;

	private ShcUser[] usersField;

	[XmlArray(IsNullable = true, Order = 0)]
	[XmlArrayItem(Namespace = "http://schemas.datacontract.org/2004/07/RWE.SmartHome.Common.GlobalContracts")]
	public ShcRole[] Roles
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

	[XmlArray(IsNullable = true, Order = 1)]
	public ShcUser[] Users
	{
		get
		{
			return usersField;
		}
		set
		{
			usersField = value;
		}
	}
}
