using System.Collections.Generic;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;

public interface IValueDescriptionService
{
	List<ValueDescription> GetValueDescriptions(DeviceIdentifier identifier);

	void AddAndDeleteValueDescriptions(DeviceIdentifier identifier, IEnumerable<ValueDescription> valueDescriptionsToSet, IEnumerable<uint> valueDescriptionsToDelete);

	void AddValueDescription(DeviceIdentifier identifier, ValueDescription valueDescription);

	void AddValueDescriptions(DeviceIdentifier identifier, IEnumerable<ValueDescription> valueDescriptions);

	void DeleteValueDescription(DeviceIdentifier identifier, uint index);
}
