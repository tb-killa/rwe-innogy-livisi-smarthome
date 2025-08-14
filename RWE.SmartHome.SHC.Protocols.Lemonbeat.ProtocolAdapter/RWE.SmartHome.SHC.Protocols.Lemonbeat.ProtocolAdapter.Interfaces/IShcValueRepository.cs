using System.Collections.Generic;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Interfaces;

internal interface IShcValueRepository
{
	uint RegisterValue(string appId, string valueName);

	uint GetValueId(string appId, string valueName);

	Dictionary<string, uint> GetUsedValues(string appId);

	bool TryGetValueId(string appId, string name, out uint valueId);

	void RemoveValue(string appId, string name);

	void BeginUpdate();

	void CommitChanges();

	void RollbackChanges();
}
