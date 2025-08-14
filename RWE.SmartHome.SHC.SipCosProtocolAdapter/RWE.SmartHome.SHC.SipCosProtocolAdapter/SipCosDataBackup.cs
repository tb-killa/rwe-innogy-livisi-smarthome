using System;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Persistence;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter;

public class SipCosDataBackup : IProtocolSpecificDataBackup
{
	private const string LoggingSource = "SipCosProtocolSpecificDataBackup";

	private readonly Configuration configuration;

	private readonly ISipCosPersistence sipCosPersistence;

	private readonly ICoprocessorAccess coprocessorAccess;

	public SipCosDataBackup(ISipCosPersistence sipCosPersistence, IConfigurationManager configurationManager, ICoprocessorAccess coprocessorAccess)
	{
		this.coprocessorAccess = coprocessorAccess;
		this.sipCosPersistence = sipCosPersistence;
		configuration = new Configuration(configurationManager);
	}

	public void Backup()
	{
		PersistSequenceCounter();
	}

	public void Restore(bool setDefaults)
	{
		RetrieveSequenceCounter(setDefaults);
	}

	public void PersistSequenceCounter()
	{
		uint num;
		try
		{
			string currentVersion = GetCurrentVersion();
			if (currentVersion != configuration.V1CoprocessorVersion)
			{
				num = GetSequenceCounter();
				Log.Information(Module.BusinessLogic, "SipCosProtocolSpecificDataBackup", $"Coprocessor version (i.e. {currentVersion}) different than V1. Current sequence counter (i.e. {num}) will be persisted");
			}
			else
			{
				num = GetSequenceCounterIncrement(configuration.SequenceCounterReferenceDate.Value);
				Log.Information(Module.BusinessLogic, "SipCosProtocolSpecificDataBackup", $"Coprocessor V1 detected (i.e. {currentVersion}). Calculated value (i.e. {num}) will be persisted");
			}
		}
		catch
		{
			Log.Warning(Module.BusinessLogic, "SipCosProtocolSpecificDataBackup", "Sequence counter could not be retrieved from the coprocessor. The persisted value will not be changed.");
			return;
		}
		SIPCosNetworkParameter sIPCosNetworkParameter = sipCosPersistence.LoadSIPCosNetworkParameter();
		if (sIPCosNetworkParameter == null)
		{
			SIPCosNetworkParameter sIPCosNetworkParameter2 = new SIPCosNetworkParameter();
			sIPCosNetworkParameter2.SequenceCounter = num;
			sIPCosNetworkParameter2.SequenceCounterSaveTime = DateTime.Now;
			sIPCosNetworkParameter = sIPCosNetworkParameter2;
			sipCosPersistence.SaveSIPCosNetworkParameterInTransaction(sIPCosNetworkParameter, suppressEvent: true);
		}
		else if (!sIPCosNetworkParameter.SequenceCounter.HasValue || num > sIPCosNetworkParameter.SequenceCounter)
		{
			sIPCosNetworkParameter.SequenceCounter = num;
			sIPCosNetworkParameter.SequenceCounterSaveTime = DateTime.Now;
			sipCosPersistence.SaveSIPCosNetworkParameterInTransaction(sIPCosNetworkParameter, suppressEvent: true);
		}
		else
		{
			Log.Warning(Module.BusinessLogic, "SipCosProtocolSpecificDataBackup", "Existing sequence counter is greater than he value attempted to be saved. The persisted value will not be changed.");
		}
	}

	internal void RetrieveSequenceCounter(bool forceRetrieve)
	{
		SIPCosNetworkParameter sIPCosNetworkParameter = sipCosPersistence.LoadSIPCosNetworkParameter();
		if (sIPCosNetworkParameter != null)
		{
			if (sIPCosNetworkParameter.SequenceCounter.HasValue)
			{
				uint num;
				if (!sIPCosNetworkParameter.SequenceCounterSaveTime.HasValue)
				{
					num = GetSequenceCounterIncrement(configuration.SequenceCounterReferenceDate.Value);
					Log.Warning(Module.BusinessLogic, "SipCosProtocolSpecificDataBackup", $"SequenceCounterSaveTime does not have a value even if a SequenceCounter was saved. Calculated V1 default value will be applied: {num}");
				}
				else
				{
					num = sIPCosNetworkParameter.SequenceCounter.Value + GetSequenceCounterIncrement(sIPCosNetworkParameter.SequenceCounterSaveTime.Value);
					Log.Information(Module.BusinessLogic, "SipCosProtocolSpecificDataBackup", $"Found sequence counter value: {sIPCosNetworkParameter.SequenceCounter.Value}, last persistence date: {sIPCosNetworkParameter.SequenceCounterSaveTime}, new value applied: {num}.");
				}
				SetSequenceCount(num);
				PersistSequenceCounter();
			}
			else
			{
				Log.Information(Module.BusinessLogic, "SipCosProtocolSpecificDataBackup", "No sequence counter value found into the database although there is a configuration there.Calculated V1 value will be applied.");
				SetCalculatedDefaultCounter();
			}
		}
		else if (forceRetrieve)
		{
			SetCalculatedDefaultCounter();
		}
		else
		{
			Log.Information(Module.SipCosProtocolAdapter, "SipCosProtocolSpecificDataBackup", "No sequence counter value found into the database. No change will be done.");
		}
	}

	private void SetCalculatedDefaultCounter()
	{
		uint sequenceCounterIncrement = GetSequenceCounterIncrement(configuration.SequenceCounterReferenceDate.Value);
		Log.Warning(Module.BusinessLogic, "SipCosProtocolSpecificDataBackup", $"No sequence counter value found into the database. Calculated V1 value will be applied: {sequenceCounterIncrement}");
		SetSequenceCount(sequenceCounterIncrement);
		PersistSequenceCounter();
	}

	private string GetCurrentVersion()
	{
		return coprocessorAccess.Version.ToReadable();
	}

	private uint GetSequenceCounter()
	{
		return coprocessorAccess.SecuritySequenceCounter;
	}

	private void SetSequenceCount(uint sequenceCount)
	{
		coprocessorAccess.SecuritySequenceCounter = sequenceCount;
	}

	private uint GetSequenceCounterIncrement(DateTime referernceDate)
	{
		return (uint)((DateTime.Now.Subtract(referernceDate).Days + 1) * configuration.SequenceCounterDailyIncrement).Value;
	}
}
