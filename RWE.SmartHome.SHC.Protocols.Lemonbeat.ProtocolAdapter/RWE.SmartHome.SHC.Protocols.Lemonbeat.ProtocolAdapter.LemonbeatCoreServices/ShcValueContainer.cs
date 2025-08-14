using System.Collections.Generic;
using SmartHome.SHC.API.Protocols.Lemonbeat;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.LemonbeatCoreServices;

internal class ShcValueContainer : IShcValueContainer
{
	private string appId;

	public static readonly uint SHC_SWITCH_VALUE = 200u;

	private ShcValueRepository repository;

	public ShcValueContainer(string appId, ShcValueRepository repository)
	{
		this.appId = appId;
		this.repository = repository;
	}

	public uint GetValueId(string name)
	{
		return repository.GetValueId(appId, name);
	}

	public uint RegisterValue(string name)
	{
		return repository.RegisterValue(appId, name);
	}

	public void RemoveValue(string name)
	{
		repository.RemoveValue(appId, name);
	}

	public bool TryGetValueId(string name, out uint valueId)
	{
		return repository.TryGetValueId(appId, name, out valueId);
	}

	public Dictionary<string, uint> GetUsedValues()
	{
		return repository.GetUsedValues(appId);
	}
}
