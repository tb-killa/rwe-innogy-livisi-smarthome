using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.ShcType;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;

public class GetShcTypeResponse : BaseResponse
{
	[XmlElement]
	public List<RestrictionState> CurrentLicensingState { get; set; }
}
