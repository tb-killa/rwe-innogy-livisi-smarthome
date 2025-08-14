using System;

namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

public class ShcRef
{
	public Guid RefId { get; set; }

	public override string ToString()
	{
		return "RefId: " + RefId;
	}
}
