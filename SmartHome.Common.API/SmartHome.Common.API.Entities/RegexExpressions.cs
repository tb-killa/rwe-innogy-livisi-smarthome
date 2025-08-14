using System.Text.RegularExpressions;

namespace SmartHome.Common.API.Entities;

public class RegexExpressions
{
	public static Regex LastComponentOccurrenceRegex = new Regex("[^/]*$", RegexOptions.IgnoreCase);

	public static Regex ActionGenericTypeRegex = new Regex("^action\\/([\\d\\w]+)", RegexOptions.IgnoreCase);

	public static Regex ActionMemberSpecificTypeRegex = new Regex("^member\\/([\\w\\d\\._-]+)\\/action\\/([\\d\\w]+)", RegexOptions.IgnoreCase);

	public static Regex ActionHomeSpecificTypeRegex = new Regex("^home\\/([\\w\\d\\._-]+)\\/action\\/([\\d\\w]+)", RegexOptions.IgnoreCase);

	public static Regex ActionSHCSpecificTypeRegex = new Regex("^device\\/SHC.RWE\\/([\\w\\d\\._-]+)\\/action\\/([\\d\\w]+)", RegexOptions.IgnoreCase);

	public static Regex DeviceSpecificTypeRegex = new Regex("^device\\/([\\w\\d\\._-]+)\\/([\\w\\d\\._-]+)", RegexOptions.IgnoreCase);

	public static Regex CapabilitySpecificTypeRegex = new Regex("^device\\/([\\w\\d\\._-]+)\\/([\\w\\d\\._-]+)\\/capability\\/([\\w\\d]+)", RegexOptions.IgnoreCase);

	public static Regex ProductSpecificTypeRegex = new Regex("^\\/product\\/([\\w\\d\\._-]+)\\/([\\w\\d\\._-]+)", RegexOptions.IgnoreCase);

	public static Regex LinkSpecificTypeRegex = new Regex("^(?:\\/)([\\w]+)", RegexOptions.IgnoreCase);
}
