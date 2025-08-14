using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Authentication;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.DataAccessInterfaces.TechnicalConfiguration;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Calibrators;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Persistence;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.SwitchDelegate;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.TaskList;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.DeviceFirmwareUpdate;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.RuleEngineCommunication;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter;

internal class SipCosProtocolSpecificProtocolAdapter : ISipCosProtocolAdapter, IProtocolAdapter
{
	private readonly IProtocolSpecificLogicalStateRequestor logicalStateRequestor;

	private readonly IProtocolSpecificPhysicalStateHandler physicalStateHandler;

	private readonly IProtocolSpecificDataBackup dataBackup;

	private readonly SipCosDeviceController deviceController;

	private readonly IDeviceManager deviceManager;

	private readonly ISipCosPersistence sipCosPersistence;

	private readonly IProtocolSpecificTransformation transformation;

	private readonly TechnicalConfigurationManager technicalConfigurationManager;

	private readonly XmlSerializer systemInfoSerializer = new XmlSerializer(typeof(SipCosSpecificInformation));

	public ProtocolIdentifier ProtocolId => ProtocolIdentifier.Cosip;

	public IProtocolSpecificLogicalStateRequestor LogicalState => logicalStateRequestor;

	public IProtocolSpecificPhysicalStateHandler PhysicalState => physicalStateHandler;

	public IProtocolSpecificDeviceController DeviceController => deviceController;

	public IProtocolSpecificTransformation Transformation => transformation;

	public IProtocolSpecificDataBackup DataBackup => dataBackup;

	public SipCosProtocolSpecificProtocolAdapter(IRepository configurationRepository, ILogicalDeviceStateRepository logicalDeviceStateRepository, IEventManager eventManager, IDeviceManager deviceManager, IUserManager userManager, LogicalDeviceHandlerCollection logicalDeviceHandlerCollection, ITriggerCapableDeviceHandlerCollection triggerCapableDeviceHandlerCollection, ISipCosPersistence sipCosPersistence, IConfigurationManager configurationManager, ICoprocessorAccess coprocessorAccess, ITechnicalConfigurationPersistence techConfigPersistence, ISwitchDelegate switchDelegate, IRollerShutterCalibrator rollerShutterCalibrator, IScheduler scheduler, ICosIPFirmwareUpdateController updateController, IDeviceFirmwareManager firmwareManager, IBidCosConfigurator bidCosConfigurator)
	{
		this.sipCosPersistence = sipCosPersistence;
		this.deviceManager = deviceManager;
		logicalStateRequestor = new SipCosLogicalStateRequestor(deviceManager, logicalDeviceHandlerCollection, configurationRepository);
		deviceController = new SipCosDeviceController(configurationRepository, eventManager, deviceManager, userManager, logicalDeviceHandlerCollection, triggerCapableDeviceHandlerCollection);
		physicalStateHandler = new SipCosPhysicalStateHandler(deviceManager, configurationRepository);
		dataBackup = new SipCosDataBackup(sipCosPersistence, configurationManager, coprocessorAccess);
		technicalConfigurationManager = new TechnicalConfigurationManager(deviceManager, eventManager, techConfigPersistence);
		IDeviceList deviceList = deviceManager.DeviceList;
		Func<Guid, LogicalDeviceState> getLogicalDeviceState = logicalDeviceStateRepository.GetLogicalDeviceState;
		Func<IList<byte[]>> shcAddresses = () => deviceManager.ShcAddresses;
		transformation = new SipCosTransformation(deviceList, getLogicalDeviceState, shcAddresses, this, switchDelegate, technicalConfigurationManager, rollerShutterCalibrator, ProtocolId, configurationRepository, bidCosConfigurator, techConfigPersistence, deviceManager, eventManager);
		Configuration configuration = new Configuration(configurationManager);
		CosIPDeviceFirmwareUpdater updater = new CosIPDeviceFirmwareUpdater(scheduler, eventManager, sipCosPersistence, updateController, deviceManager.DeviceList, configuration.OTAUPackageSendDelay, configuration.OTAUPackageSendDelayEventListeners);
		firmwareManager.RegisterUpdater(updater);
	}

	public IEnumerable<Guid> GetHandledDevices()
	{
		return deviceManager.DeviceList.Select((IDeviceInformation device) => device.DeviceId);
	}

	public string GetDeviceDescription(Guid deviceId)
	{
		return deviceManager.DeviceList.LogInfoByGuid(deviceId);
	}

	public void ResetDeviceInclusionState(Guid deviceId)
	{
		deviceManager.ResetDeviceInclusionState(deviceId);
	}

	public void DropDiscoveredDevices(BaseDevice[] devices)
	{
		deviceManager.DropDiscoveredDevices(devices);
	}

	public ProtocolSpecificInformation GetProtocolSpecificInformation()
	{
		ProtocolSpecificInformation protocolSpecificInformation = new ProtocolSpecificInformation(ProtocolId);
		protocolSpecificInformation.XmlInformation = CreateProtocolSpecificInformation();
		return protocolSpecificInformation;
	}

	private string CreateProtocolSpecificInformation()
	{
		SipCosSpecificInformation sipCosSpecificInformation = new SipCosSpecificInformation();
		sipCosSpecificInformation.Entities = sipCosPersistence.LoadAllEntities().ToList();
		sipCosSpecificInformation.NetworkParameter = sipCosPersistence.LoadSIPCosNetworkParameter();
		SipCosSpecificInformation sipCosSpecificInformation2 = sipCosSpecificInformation;
		sipCosSpecificInformation2.NetworkParameter.NetworkKey = null;
		string empty = string.Empty;
		XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces(new XmlQualifiedName[1] { XmlQualifiedName.Empty });
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.OmitXmlDeclaration = true;
		xmlWriterSettings.Indent = true;
		using StringWriter stringWriter = new StringWriter();
		using XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings);
		systemInfoSerializer.Serialize(xmlWriter, sipCosSpecificInformation2, namespaces);
		empty = stringWriter.ToString();
		xmlWriter.Close();
		return empty;
	}
}
