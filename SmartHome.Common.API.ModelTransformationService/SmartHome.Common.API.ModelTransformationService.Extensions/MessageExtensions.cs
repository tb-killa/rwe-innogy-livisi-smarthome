using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.Extensions;

public static class MessageExtensions
{
	private static readonly ILogger logger = LogManager.Instance.GetLogger(typeof(MessageExtensions));

	public static string GetApplicationName(this Message message)
	{
		if (string.IsNullOrEmpty(message.AppId))
		{
			logger.Warn($"Message [Id={message.Type}; Type={message.Id}] with NULL or empty AppId detected.");
			return string.Empty;
		}
		return message.AppId.Replace("sh://", string.Empty);
	}

	public static string GetVersion(this Message message)
	{
		if (message.AppId == CoreConstants.CoreAppId)
		{
			return "2.0".ToMajorDotMinorVersion();
		}
		if (message.AddinVersion == null)
		{
			logger.Warn($"Message [Id={message.Type}; Type={message.Id}] does not contain an add-in version.");
			return string.Empty;
		}
		return message.AddinVersion.ToMajorDotMinorVersion();
	}
}
