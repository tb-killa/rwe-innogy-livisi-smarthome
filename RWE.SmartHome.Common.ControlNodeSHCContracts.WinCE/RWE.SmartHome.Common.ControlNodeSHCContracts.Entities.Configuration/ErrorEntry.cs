using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

public class ErrorEntry
{
	public EntityMetadata AffectedEntity { get; set; }

	[XmlAttribute]
	public ValidationErrorCode ErrorCode { get; set; }

	[XmlArray(ElementName = "EPS")]
	[XmlArrayItem(ElementName = "EP")]
	public List<ErrorParameter> ErrorParameters { get; set; }

	public ErrorEntry()
	{
		ErrorParameters = new List<ErrorParameter>();
	}

	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();
		builder.AppendFormat("ErrCode: {0}", new object[1] { ErrorCode });
		if (AffectedEntity != null)
		{
			builder.AppendFormat(" Entity: [{0}]-[{1}]", new object[2] { AffectedEntity.Id, AffectedEntity.EntityType });
		}
		if (ErrorParameters != null)
		{
			ErrorParameters.ForEach(delegate(ErrorParameter t)
			{
				builder.AppendFormat("\n  ErrorParam[{0}] = {1}", new object[2] { t.Key, t.Value });
			});
		}
		return builder.ToString();
	}
}
