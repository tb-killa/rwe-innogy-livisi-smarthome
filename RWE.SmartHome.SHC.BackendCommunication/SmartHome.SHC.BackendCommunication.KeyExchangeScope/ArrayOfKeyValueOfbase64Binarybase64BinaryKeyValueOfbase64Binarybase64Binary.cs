using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace SmartHome.SHC.BackendCommunication.KeyExchangeScope;

[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays")]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
public class ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary
{
	private byte[] keyField;

	private byte[] valueField;

	[XmlElement(DataType = "base64Binary", IsNullable = true, Order = 0)]
	public byte[] Key
	{
		get
		{
			return keyField;
		}
		set
		{
			keyField = value;
		}
	}

	[XmlElement(DataType = "base64Binary", IsNullable = true, Order = 1)]
	public byte[] Value
	{
		get
		{
			return valueField;
		}
		set
		{
			valueField = value;
		}
	}
}
