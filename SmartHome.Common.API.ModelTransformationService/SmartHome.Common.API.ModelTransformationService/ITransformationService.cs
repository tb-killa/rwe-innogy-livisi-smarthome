namespace SmartHome.Common.API.ModelTransformationService;

public interface ITransformationService
{
	IDeviceConverterService DeviceConverter { get; }

	ICapabilityConverterService CapabilityConverter { get; }

	IActionConverterService ActionConverter { get; }

	IEventConverterService EventConverter { get; }

	ILocationConverterService LocationConverter { get; }

	IMessageConverterService MessageConverter { get; }

	IInteractionConverterService InteractionConverter { get; }

	IHomeConverterService HomeConverter { get; }

	IMemberConverterService MemberConverter { get; }

	IHomeSetupConverterService HomeSetupConverter { get; }

	IStatusConverterService StatusConverter { get; }
}
