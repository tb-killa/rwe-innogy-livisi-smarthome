using System;
using System.Globalization;

namespace RWE.SmartHome.Common.GlobalContracts;

public class ShcRole
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "Id: {0}, Name: {1}", new object[2] { Id, Name });
	}
}
