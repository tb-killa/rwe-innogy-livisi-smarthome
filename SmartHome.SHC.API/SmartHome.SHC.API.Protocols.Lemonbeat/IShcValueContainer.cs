using System.Collections.Generic;

namespace SmartHome.SHC.API.Protocols.Lemonbeat;

public interface IShcValueContainer
{
	uint RegisterValue(string name);

	bool TryGetValueId(string name, out uint valueId);

	uint GetValueId(string name);

	void RemoveValue(string name);

	Dictionary<string, uint> GetUsedValues();
}
