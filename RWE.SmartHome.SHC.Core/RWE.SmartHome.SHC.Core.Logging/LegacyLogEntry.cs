using System;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Core.Logging;

internal class LegacyLogEntry
{
	[XmlAttribute]
	public byte Severity { get; set; }

	[XmlAttribute]
	public DateTime Timestamp { get; set; }

	[XmlAttribute]
	public string Source { get; set; }

	[XmlAttribute]
	public string Message { get; set; }
}
