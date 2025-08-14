using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.SmsScope;

[Serializable]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
[XmlType(Namespace = "http://rwe.com/SmartHome/2011/11/15/SmsObjects")]
public enum SmsTemplates
{
	ConfirmationCodeToMaster,
	ConfirmationCode,
	SmsLimitReachedSoon,
	SmsLimitExceeded,
	SmsLeft,
	LastSms
}
