using System;
using System.Text.RegularExpressions;
using SmartHome.Common.API.Entities;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.Extensions;

public static class LinkExtensions
{
	private static readonly ILogger logger = LogManager.Instance.GetLogger(typeof(LinkExtensions));

	private static readonly Regex LinkTypeRegex = new Regex("^(?:\\/)([\\w]+)", RegexOptions.IgnoreCase);

	public static string GetId(this string aLink)
	{
		Match match = RegexExpressions.LastComponentOccurrenceRegex.Match(aLink);
		try
		{
			if (match.Success)
			{
				return match.Value;
			}
		}
		catch
		{
		}
		logger.LogAndThrow<ArgumentException>($"Invalid Link object. Cannot retrieve identity from link: {aLink}");
		return null;
	}

	public static Guid GetGuid(this string link)
	{
		if (!string.IsNullOrEmpty(link))
		{
			string uniqueIdentifier = link.GetUniqueIdentifier();
			if (uniqueIdentifier.GuidTryParse(out var output))
			{
				return output;
			}
		}
		logger.LogAndThrow<ArgumentException>(string.Format("Invalid link GUID representation: {0}. GUID should contain 32 digits.", link ?? "null"));
		return Guid.Empty;
	}

	public static string GetUniqueIdentifier(this string link, EntityType entityType)
	{
		if (entityType == EntityType.Product)
		{
			Match match = RegexExpressions.ProductSpecificTypeRegex.Match(link);
			if (match.Groups.Count < 3)
			{
				logger.LogAndThrow<ArgumentException>(string.Format("Invalid link representation : {0}. Link for product should have the product, followed by appId and by appVersion", link ?? "null"));
			}
			if (match.Groups.Count > 1)
			{
				return match.Groups[1].Value;
			}
			return string.Empty;
		}
		return link.GetUniqueIdentifier();
	}

	public static string GetUniqueIdentifier(this string link)
	{
		MatchCollection matchCollection = RegexExpressions.LastComponentOccurrenceRegex.Matches(link);
		if (matchCollection.Count > 0)
		{
			return matchCollection[0].Value;
		}
		return string.Empty;
	}

	public static EntityType GetEntityType(this string link)
	{
		Match match = LinkTypeRegex.Match(link);
		if (match.Success && match.Groups[1].Value.TryParse<EntityType>(ignoreCase: true, out var enumValue))
		{
			return enumValue;
		}
		logger.LogAndThrow<ArgumentException>(string.Format("Invalid Link object. Cannot retrieve entity type from link: {0}", link ?? "null"));
		return EntityType.Unknown;
	}
}
