using System.IO;
using System.Xml;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public class NamespaceIgnorantXmlTextReader : XmlTextReader
{
	public override string NamespaceURI => "";

	public NamespaceIgnorantXmlTextReader(TextReader input)
		: base(input)
	{
	}
}
