using System;
using System.Linq;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.FirmwareUpdate;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Services;

public class FirmwareUpdateService : SenderService<network>, IFirmwareUpdateService
{
	private enum DeviceUpdateStatusCodes
	{
		OK = 1,
		NotInitialized,
		SizeTooLarge,
		ChecksumError,
		DataOverflow,
		WrongOffset,
		ChunkTooLarge,
		DataMissing,
		ChunkSizeTooSmall
	}

	private const string DefaultNamespace = "urn:firmware_updatexsd";

	public FirmwareUpdateService(ILemonbeatCommunication aggregator)
		: base(aggregator, ServiceType.FirmwareUpdate, "urn:firmware_updatexsd")
	{
	}

	public FirmwareInformation GetFirmwareInformation(DeviceIdentifier identifier)
	{
		network network = CreateNetworkMessage(identifier);
		network.device[0].Items = new object[1]
		{
			new firmwareInformationGetType()
		};
		network.device[0].device_idSpecified = identifier.SubDeviceId.HasValue;
		network answer = SendRequest(identifier, network);
		return ConvertFirmwareInformation(answer);
	}

	public FirmwareUpdateStatus FirmwareUpdateInit(DeviceIdentifier identifier, uint firmwareID, byte[] firmwareChecksum, uint firmwareSize)
	{
		network network = CreateNetworkMessage(identifier);
		network.device[0].Items = new object[1]
		{
			new firmwareInitType
			{
				checksum = firmwareChecksum,
				firmware_id = firmwareID,
				size = firmwareSize
			}
		};
		network.device[0].device_idSpecified = identifier.SubDeviceId.HasValue;
		network answer = SendRequest(identifier, network);
		return ConvertUpdateStatus(answer);
	}

	public FirmwareUpdateStatus TransferUpdate(DeviceIdentifier identifier, byte[] chunkData, uint desiredOffset)
	{
		network network = CreateNetworkMessage(identifier);
		network.device[0].Items = new object[1]
		{
			new firmwareDataType
			{
				chunk = chunkData,
				offset = desiredOffset
			}
		};
		network.device[0].device_idSpecified = identifier.SubDeviceId.HasValue;
		network answer = SendRequestDatagram(identifier, network);
		return ConvertUpdateStatus(answer);
	}

	public FirmwareUpdateStatus DoUpdate(DeviceIdentifier identifier)
	{
		network network = CreateNetworkMessage(identifier);
		network.device[0].Items = new object[1]
		{
			new firmwareUpdateStartType()
		};
		network.device[0].device_idSpecified = identifier.SubDeviceId.HasValue;
		network answer = SendRequest(identifier, network);
		return ConvertUpdateStatus(answer);
	}

	private FirmwareUpdateStatus ConvertUpdateStatus(network answer)
	{
		FirmwareUpdateStatus firmwareUpdateStatus = new FirmwareUpdateStatus();
		firmwareUpdateStatus.StatusCode = UpdateStatusCode.OK;
		firmwareUpdateStatus.ExpectedOffset = 0u;
		FirmwareUpdateStatus firmwareUpdateStatus2 = firmwareUpdateStatus;
		networkDevice networkDevice = answer.device.FirstOrDefault();
		if (networkDevice != null && networkDevice.Items != null)
		{
			firmwareReportType firmwareReportType = networkDevice.Items.OfType<firmwareReportType>().FirstOrDefault();
			if (firmwareReportType != null)
			{
				firmwareUpdateStatus2.StatusCode = ConvertStatus((DeviceUpdateStatusCodes)firmwareReportType.status);
				firmwareUpdateStatus2.ExpectedOffset = firmwareReportType.expected_offset;
			}
		}
		return firmwareUpdateStatus2;
	}

	private network CreateNetworkMessage(DeviceIdentifier identifier)
	{
		network network = new network();
		network.version = 1u;
		network.device = new networkDevice[1]
		{
			new networkDevice
			{
				version = 1u,
				device_id = (identifier.SubDeviceId ?? 0)
			}
		};
		return network;
	}

	private FirmwareInformation ConvertFirmwareInformation(network answer)
	{
		if (answer == null)
		{
			return null;
		}
		if (!(answer.device[0].Items[0] is firmwareInformationReportType firmwareInformationReportType))
		{
			return null;
		}
		FirmwareInformation firmwareInformation = new FirmwareInformation();
		firmwareInformation.Size = firmwareInformationReportType.size;
		firmwareInformation.ReceivedSize = firmwareInformationReportType.received_size;
		firmwareInformation.ID = firmwareInformationReportType.firmware_id;
		firmwareInformation.ChunkSize = firmwareInformationReportType.chunk_size;
		return firmwareInformation;
	}

	private UpdateStatusCode ConvertStatus(DeviceUpdateStatusCodes status)
	{
		return status switch
		{
			DeviceUpdateStatusCodes.NotInitialized => UpdateStatusCode.NotInitialized, 
			DeviceUpdateStatusCodes.OK => UpdateStatusCode.OK, 
			DeviceUpdateStatusCodes.WrongOffset => UpdateStatusCode.WrongOffset, 
			DeviceUpdateStatusCodes.ChecksumError => UpdateStatusCode.ChecksumError, 
			DeviceUpdateStatusCodes.ChunkSizeTooSmall => UpdateStatusCode.ChunkSizeTooSmall, 
			DeviceUpdateStatusCodes.ChunkTooLarge => UpdateStatusCode.ChunkTooLarge, 
			DeviceUpdateStatusCodes.DataMissing => UpdateStatusCode.DataMissing, 
			DeviceUpdateStatusCodes.DataOverflow => UpdateStatusCode.DataOverflow, 
			DeviceUpdateStatusCodes.SizeTooLarge => UpdateStatusCode.SizeTooLarge, 
			_ => throw new ArgumentOutOfRangeException("Unknown firmware update status code [" + status.ToString() + "] received."), 
		};
	}
}
