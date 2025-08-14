using System;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService;

public class TransformationService : ITransformationService
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(TransformationService));

	private readonly IDeviceConverterService deviceConverter;

	private readonly ICapabilityConverterService capabilityConverter;

	private readonly IEventConverterService eventsConverter;

	private readonly IActionConverterService actionConverter;

	private readonly ILocationConverterService locationConverter;

	private readonly IInteractionConverterService interactionConverter;

	private readonly IMessageConverterService messageConverter;

	private readonly IHomeConverterService homeConverter;

	private readonly IMemberConverterService memberConverter;

	private readonly IHomeSetupConverterService homeSetupConverter;

	private readonly IStatusConverterService statusConverterService;

	private static readonly IConstantsConverter constantsConverter = new ConstantsConverter();

	public IDeviceConverterService DeviceConverter => deviceConverter;

	public ICapabilityConverterService CapabilityConverter => capabilityConverter;

	public IEventConverterService EventConverter => eventsConverter;

	public IActionConverterService ActionConverter => actionConverter;

	public ILocationConverterService LocationConverter => locationConverter;

	public IInteractionConverterService InteractionConverter => interactionConverter;

	public IMessageConverterService MessageConverter => messageConverter;

	public static IConstantsConverter ConstantsConverter => constantsConverter;

	public IHomeConverterService HomeConverter => homeConverter;

	public IMemberConverterService MemberConverter => memberConverter;

	public IHomeSetupConverterService HomeSetupConverter => homeSetupConverter;

	public IStatusConverterService StatusConverter => statusConverterService;

	public TransformationService(IConversionContext context)
	{
		if (context == null)
		{
			logger.LogAndThrow<ArgumentNullException>("context");
		}
		deviceConverter = new DeviceConverterService();
		capabilityConverter = new CapabilityConverterService(context);
		eventsConverter = new EventConverterService();
		actionConverter = new ActionConverterService(context);
		locationConverter = new LocationConverterService();
		interactionConverter = new InteractionConverterService();
		messageConverter = new MessageConverterService();
		homeConverter = new HomeConverterService();
		memberConverter = new MemberConverterService();
		homeSetupConverter = new HomeSetupConverterService();
		statusConverterService = new StatusConverterService();
	}
}
